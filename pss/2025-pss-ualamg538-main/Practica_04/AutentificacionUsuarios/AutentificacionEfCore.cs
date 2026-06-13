using Microsoft.EntityFrameworkCore;

namespace PSS.amg538.Practica_04
{
    public class AutentificacionEfCore : IAutentificacion
    {
        private readonly string _cs;

        public AutentificacionEfCore(string cadenaConexion)
        {
            _cs = (cadenaConexion ?? "").Contains("%DIR_APP%")
                ? cadenaConexion.Replace("%DIR_APP%", AutentificacionSqlServerFile.ExecutionDirectoryPathName())
                : cadenaConexion;

            try
            {
                using var ctx = new AutentificacionDbContext(_cs);
                ctx.Database.OpenConnection();
                ctx.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                throw new AutentificacionExcepcion(
                    "No se puede acceder a la base de datos: " + ex.Message,
                    CodigoAutentificacion.ErrorDatos);
            }
        }

        public CodigoAutentificacion EsUsuarioAutentificado(string id, string palabraPaso)
        {
            if (!int.TryParse(id, out int idInt)) return CodigoAutentificacion.ErrorIdUsuario;
            using var ctx = new AutentificacionDbContext(_cs);
            var u = ctx.Usuarios.FirstOrDefault(x => x.Id == idInt);
            if (u == null) return CodigoAutentificacion.ErrorIdUsuario;

            var r = CodigoAutentificacion.AccesoCorrecto;
            if (u.PalabraPaso != palabraPaso) r |= CodigoAutentificacion.ErrorPalabraPaso;
            if (!u.EsValido) r |= CodigoAutentificacion.AccesoInvalido;
            return r;
        }

        public IUsuarioView ObtenerUsuario(string id)
        {
            if (!int.TryParse(id, out int idInt)) return null;
            using var ctx = new AutentificacionDbContext(_cs);
            var u = ctx.Usuarios.FirstOrDefault(x => x.Id == idInt);
            return u == null ? null : new UsuarioView
            {
                Id = u.Id.ToString(),
                Nombre = u.Nombre,
                PalabraPaso = u.PalabraPaso,
                Categoria = u.Categoria,
                EsValido = u.EsValido
            };
        }

        public bool InsertarUsuario(IUsuarioView user)
        {
            if (user == null) return false;
            using var ctx = new AutentificacionDbContext(_cs);
            ctx.Usuarios.Add(new UsuarioEntity
            {
                Nombre = user.Nombre,
                PalabraPaso = user.PalabraPaso,
                Categoria = user.Categoria,
                EsValido = user.EsValido
            });
            return ctx.SaveChanges() == 1;
        }

        public bool ModificarUsuario(string id, IUsuarioView user)
        {
            if (user == null || !int.TryParse(id, out int idInt)) return false;
            using var ctx = new AutentificacionDbContext(_cs);
            var u = ctx.Usuarios.FirstOrDefault(x => x.Id == idInt);
            if (u == null) return false;
            u.Nombre = user.Nombre; u.PalabraPaso = user.PalabraPaso;
            u.Categoria = user.Categoria; u.EsValido = user.EsValido;
            return ctx.SaveChanges() == 1;
        }

        public bool EliminarUsuario(string id)
        {
            if (!int.TryParse(id, out int idInt)) return false;
            using var ctx = new AutentificacionDbContext(_cs);
            var u = ctx.Usuarios.FirstOrDefault(x => x.Id == idInt);
            if (u == null) return false;
            ctx.Usuarios.Remove(u);
            return ctx.SaveChanges() == 1;
        }

        public void GuardarDatos() { } // EF persiste con SaveChanges()
    }
}
