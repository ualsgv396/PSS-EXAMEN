using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace PSS.amg538.Practica_04
{
    public class AutentificacionSqlServerFile : IAutentificacion
    {
        private readonly string _connectionString;

        public AutentificacionSqlServerFile(string cadenaConexion)
        {
            _connectionString = cadenaConexion ?? "";

            if (_connectionString.Contains("%DIR_APP%"))
                _connectionString = _connectionString.Replace("%DIR_APP%", ExecutionDirectoryPathName());

            // Comprobamos que la BD es accesible (si no, ErrorDatos).
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
            }
            catch (Exception ex)
            {
                throw new AutentificacionExcepcion(
                    "No se puede acceder a la base de datos: " + ex.Message,
                    CodigoAutentificacion.ErrorDatos);
            }
        }

        // Sustituye %DIR_APP% por el directorio donde se ejecuta el ensamblado.
        public static string ExecutionDirectoryPathName()
        {
            var dirPath = Assembly.GetExecutingAssembly().Location;
            dirPath = Path.GetDirectoryName(dirPath);
            return dirPath ?? AppContext.BaseDirectory;
        }

        public CodigoAutentificacion EsUsuarioAutentificado(string id, string palabraPaso)
        {
            var u = (UsuarioView)ObtenerUsuario(id);
            if (u == null) return CodigoAutentificacion.ErrorIdUsuario;

            var r = CodigoAutentificacion.AccesoCorrecto;
            if (u.PalabraPaso != palabraPaso) r |= CodigoAutentificacion.ErrorPalabraPaso;
            if (!u.EsValido) r |= CodigoAutentificacion.AccesoInvalido;
            return r;
        }

        public IUsuarioView ObtenerUsuario(string id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText =
                    "SELECT Id, Nombre, PalabraPaso, Categoria, EsValido " +
                    "FROM Usuario WHERE Id = @Id";
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = int.Parse(id);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new UsuarioView
                    {
                        Id = reader.GetInt32(0).ToString(),
                        Nombre = reader.GetString(1),
                        PalabraPaso = reader.GetString(2),
                        Categoria = reader.IsDBNull(3) ? null : reader.GetString(3),
                        EsValido = reader.GetBoolean(4)
                    };
                }
                return null;
            }
            catch (FormatException)
            {
                return null; // el id no es numérico => lo tratamos como "no existe"
            }
            catch (Exception ex)
            {
                throw new AutentificacionExcepcion(
                    "Error al obtener el usuario: " + ex.Message,
                    CodigoAutentificacion.ErrorDatos);
            }
        }

        public bool InsertarUsuario(IUsuarioView user)
        {
            if (user == null) return false;
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                using var cmd = conn.CreateCommand();
                // El Id es identity: lo asigna la base de datos, no lo enviamos.
                cmd.CommandText =
                    "INSERT INTO Usuario (Nombre, PalabraPaso, Categoria, EsValido) " +
                    "VALUES (@Nombre, @PalabraPaso, @Categoria, @EsValido)";
                cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 20).Value = user.Nombre ?? "";
                cmd.Parameters.Add("@PalabraPaso", SqlDbType.NVarChar, 20).Value = user.PalabraPaso ?? "";
                cmd.Parameters.Add("@Categoria", SqlDbType.NVarChar, 20).Value =
                    (object)user.Categoria ?? DBNull.Value;
                cmd.Parameters.Add("@EsValido", SqlDbType.Bit).Value = user.EsValido;
                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {
                throw new AutentificacionExcepcion(
                    "Error al insertar el usuario: " + ex.Message,
                    CodigoAutentificacion.ErrorDatos);
            }
        }

        public bool ModificarUsuario(string id, IUsuarioView user)
        {
            if (user == null) return false;
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText =
                    "UPDATE Usuario SET Nombre=@Nombre, PalabraPaso=@PalabraPaso, " +
                    "Categoria=@Categoria, EsValido=@EsValido WHERE Id=@Id";
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = int.Parse(id);
                cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 20).Value = user.Nombre ?? "";
                cmd.Parameters.Add("@PalabraPaso", SqlDbType.NVarChar, 20).Value = user.PalabraPaso ?? "";
                cmd.Parameters.Add("@Categoria", SqlDbType.NVarChar, 20).Value =
                    (object)user.Categoria ?? DBNull.Value;
                cmd.Parameters.Add("@EsValido", SqlDbType.Bit).Value = user.EsValido;
                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {
                throw new AutentificacionExcepcion(
                    "Error al modificar el usuario: " + ex.Message,
                    CodigoAutentificacion.ErrorDatos);
            }
        }

        public bool EliminarUsuario(string id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Usuario WHERE Id=@Id";
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = int.Parse(id);
                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {
                throw new AutentificacionExcepcion(
                    "Error al eliminar el usuario: " + ex.Message,
                    CodigoAutentificacion.ErrorDatos);
            }
        }

        // En SQL Server cada operación ya persiste de inmediato: no hay nada que guardar.
        public void GuardarDatos() { }
    }
}
