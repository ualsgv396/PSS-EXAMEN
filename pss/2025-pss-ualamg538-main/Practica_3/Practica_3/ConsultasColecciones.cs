using System;
using System.Collections.Generic;
using System.Text;

namespace Practica_3
{
    public class Consultas : IConsultas
    {
        private readonly IContexto _ctx;
        public Consultas(IContexto ctx)
        {
            _ctx = ctx;
        }
        // ======================== CONSULTAS DE USUARIOS ========================

        /// Usuarios que pertenecen a una categoría (rol) determinada
        /// Nombre de usuario en mayúsculas
        public IEnumerable<vmNombre> UsuariosEnCategoria(string nombreCategoria)
        {
            return (from u in _ctx.Usuarios
                    join ur in _ctx.UsuariosRoles on u.Id equals ur.UserId
                    join r in _ctx.Roles on ur.RoleId equals r.Id
                    where r.Name == nombreCategoria
                    select new vmNombre { Nombre = u.UserName.ToUpper() })
                   .Distinct();
        }

        /// Usuarios cuyo nombre comienza con la cadena proporcionada
        /// Nombre de usuario en mayúsculas
        public IEnumerable<vmNombre> UsuariosConNombreComienza(string cadenaComienzo)
        {
            return _ctx.Usuarios
                .Where(u => u.UserName.StartsWith(cadenaComienzo))
                .Select(u => new vmNombre { Nombre = u.UserName.ToUpper() });
        }

        /// Usuarios cuyo nombre comienza por cadenaComienzo que pertenecen a una categoría dada
        /// Nombre de usuario en mayúsculas
        public IEnumerable<vmNombre> UsuariosConNombreComienzaEnCategoria(
            string cadenaComienzo,
            string categoria)
        {
            return (from u in _ctx.Usuarios
                    join ur in _ctx.UsuariosRoles on u.Id equals ur.UserId
                    join r in _ctx.Roles on ur.RoleId equals r.Id
                    where u.UserName.StartsWith(cadenaComienzo) && r.Name == categoria
                    select new vmNombre { Nombre = u.UserName.ToUpper() })
                    .Distinct();
        }



        /// Usuarios conectados desde una IP determinada
        /// Nombre de usuario en mayúsculas
        public IEnumerable<vmNombre> UsuariosConectadosIP(string ip)
        {
            return _ctx.Conexiones
                .Where(c => c.IP == ip)
                .Join(_ctx.Usuarios,
                      c => c.UserId,
                      u => u.Id,
                      (c, u) => new vmNombre { Nombre = u.UserName.ToUpper() })
                .Distinct();
        }

        /// Encuentra el nombre del usuario de una aplicación dada a través de su e-mail
        /// Nombre de usuario en mayúsculas
        public IEnumerable<vmNombre> EncontrarUsuarioAppEmail(string aplicacion, string email)
        {
            return (from u in _ctx.Usuarios
                    join a in _ctx.Aplicaciones on u.AplicacionId equals a.Id
                    where a.NombreAplicacion == aplicacion && u.Email == email
                    select new vmNombre { Nombre = u.UserName.ToUpper() });
        }

        /// Usuarios no anónimos de una aplicación
        /// Nombre de usuario en mayúsculas
        public IEnumerable<vmNombre> UsuariosNoAnonimosPorApp(string nombreApp)
        {
            return (from u in _ctx.Usuarios
                    join a in _ctx.Aplicaciones on u.AplicacionId equals a.Id
                    where !u.EsAnonimo && a.NombreAplicacion == nombreApp
                    select new vmNombre { Nombre = u.UserName.ToUpper() });
        }

        /// Usuarios anónimos
        /// Nombre de usuario en mayúsculas
        public IEnumerable<vmNombre> UsuariosAnonimos()
        {
            return _ctx.Usuarios
                .Where(u => u.EsAnonimo)
                .Select(u => new vmNombre { Nombre = u.UserName.ToUpper() });
        }

        /// Usuarios bloqueados
        /// Nombre de usuario en mayúsculas
        public IEnumerable<vmNombre> UsuariosBloqueados()
        {
            return _ctx.Usuarios
                .Where(u => u.EstaBloqueado)
                .Select(u => new vmNombre { Nombre = u.UserName.ToUpper() });
        }

        /// Usuarios de una aplicación específica
        /// Nombre de usuario en mayúsculas
        public IEnumerable<vmNombre> UsuariosPorAplicacion(string nombreApp)
        {
            return (from u in _ctx.Usuarios
                    join a in _ctx.Aplicaciones on u.AplicacionId equals a.Id
                    where a.NombreAplicacion == nombreApp
                    select new vmNombre { Nombre = u.UserName.ToUpper() });
        }

        // ======================== CATEGORÍAS / ROLES ========================

        /// Lista de pares (Categoría, Usuario) para una aplicación dada 
        /// Nombre categoría, Nombre usuario
        public IEnumerable<vmCategoriaNombre> ListaParCategoriaUsuarioParaApp(string aplicacion)
        {
            return (from u in _ctx.Usuarios
                    join a in _ctx.Aplicaciones on u.AplicacionId equals a.Id
                    join ur in _ctx.UsuariosRoles on u.Id equals ur.UserId
                    join r in _ctx.Roles on ur.RoleId equals r.Id
                    where a.NombreAplicacion == aplicacion
                    select new vmCategoriaNombre
                    {
                        Categoria = r.Name,
                        Nombre = u.UserName
                    });
        }

        /// <summary>
        /// Usuarios agrupados por categoría (rol)
        /// Nombre categoría, Nombre usuario
        /// </summary>
        public IEnumerable<IGrouping<string, vmCategoriaNombre>> AgrupacionUsuariosCategorias()
        {
            return (from u in _ctx.Usuarios
                    join ur in _ctx.UsuariosRoles on u.Id equals ur.UserId
                    join r in _ctx.Roles on ur.RoleId equals r.Id
                    select new vmCategoriaNombre
                    {
                        Nombre = u.UserName,
                        Categoria = r.Name
                    })
                   .GroupBy(x => x.Categoria);
        }

        /// <summary>
        /// Usuarios agrupados por categorías ordenadas descendentemente
        /// Nombre categoría, Nombre usuario
        /// </summary>
        public IEnumerable<vmCategoriaNombre> AgrupacionUsuariosCategoriasOrdenadas()
        {
            return (from u in _ctx.Usuarios
                    join ur in _ctx.UsuariosRoles on u.Id equals ur.UserId
                    join r in _ctx.Roles on ur.RoleId equals r.Id
                    select new vmCategoriaNombre
                    {
                        Categoria = r.Name,
                        Nombre = u.UserName
                    })
                   .OrderByDescending(x => x.Categoria)
                   .ThenBy(x => x.Nombre);
        }

        /// <summary>
        /// Categoría con mayor número de usuarios
        /// Nombre categoría, Número de usuarios
        /// </summary>
        public IEnumerable<vmCategoriaNombre> CategoriaMaximoNumeroUsuarios()
        {
            return (from u in _ctx.Usuarios
                    join ur in _ctx.UsuariosRoles on u.Id equals ur.UserId
                    join r in _ctx.Roles on ur.RoleId equals r.Id
                    group u by r.Name into g
                    orderby g.Count() descending
                    select new vmCategoriaNombre
                    {
                        Categoria = g.Key,
                        Nombre = g.Count().ToString()
                    }).Take(1);
        }

        /// <summary>
        /// Todas las categorías de usuarios para una aplicación dada
        /// Nombre categoría
        /// </summary>
        public IEnumerable<vmCategoriaNombre> TodasCategoriasApp(string aplicacion)
        {
            return (from u in _ctx.Usuarios
                    join ur in _ctx.UsuariosRoles on u.Id equals ur.UserId
                    join r in _ctx.Roles on ur.RoleId equals r.Id
                    join a in _ctx.Aplicaciones on u.AplicacionId equals a.Id
                    where a.NombreAplicacion == aplicacion
                    select new vmCategoriaNombre
                    {
                        Categoria = r.Name,
                        Nombre = u.UserName
                    }).Distinct();
        }

        /// <summary>
        /// Categorías y aplicaciones para un usuario dado
        /// Nombre categoría, Nombre aplicación
        /// </summary>
        public IEnumerable<vmCategoriaNombre> CategoriasAplicacionParaUsuario(string usuario)
        {
            return (from u in _ctx.Usuarios
                    join ur in _ctx.UsuariosRoles on u.Id equals ur.UserId
                    join r in _ctx.Roles on ur.RoleId equals r.Id
                    join a in _ctx.Aplicaciones on u.AplicacionId equals a.Id
                    where u.UserName == usuario
                    select new vmCategoriaNombre
                    {
                        Categoria = r.Name,
                        Nombre = a.NombreAplicacion
                    });
        }

        /// <summary>
        /// Roles ordenados por cantidad de usuarios (descendente)
        /// Nombre Rol, Cantidad de usuarios
        /// </summary>
        public IEnumerable<vmNombreCantidad> RolesPorCantidadUsuarios()
        {
            return _ctx.Roles
                .GroupBy(r => r.Name)
                .Select(g => new vmNombreCantidad
                {
                    Nombre = g.Key,
                    Cantidad = g.SelectMany(role =>
                        _ctx.UsuariosRoles.Where(ur => ur.RoleId == role.Id)).Count()
                })
                .OrderByDescending(x => x.Cantidad);
        }

        // ======================== CONEXIONES / ESTADÍSTICAS ========================

        /// <summary>
        /// IP con mayor número de conexiones según una categoría (rol)
        /// Ip , Cantidad de conexiones
        /// </summary>
        public IEnumerable<vmNombreCantidad> IPconMasConexionesSegunCategoria(string nombreCategoria)
        {
            return (from u in _ctx.Usuarios
                    join ur in _ctx.UsuariosRoles on u.Id equals ur.UserId
                    join r in _ctx.Roles on ur.RoleId equals r.Id
                    join c in _ctx.Conexiones on u.Id equals c.UserId
                    where r.Name == nombreCategoria
                    group u by c.IP into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Count()
                    }).OrderByDescending(x => x.Cantidad);
        }

        /// <summary>
        /// Suma total de duración de conexiones por usuario (solo usuarios con conexiones)
        /// nombre Usuario, Suma duración
        /// </summary>
        public IEnumerable<vmNombreCantidad> UsuarioSumaDuracionConexiones()
        {
            return (from c in _ctx.Conexiones
                    join u in _ctx.Usuarios on c.UserId equals u.Id
                    group c by u.UserName into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Sum(c => c.Duracion)
                    })
                    .OrderByDescending(x => x.Cantidad);
        }

        /// <summary>
        /// LEFT OUTER JOIN - Suma total de duración de conexiones por usuario
        /// Incluye usuarios sin conexiones con 0
        /// Nombre Usuario, Suma duración
        /// </summary>
        public IEnumerable<vmNombreCantidad> UsuarioSumaDuracionConexionesNulos()
        {
            return from u in _ctx.Usuarios
                   join c in _ctx.Conexiones on u.Id equals c.UserId into conexionesUsuario
                   select new vmNombreCantidad
                   {
                       Nombre = u.UserName,
                       Cantidad = conexionesUsuario.Sum(c => c.Duracion) // 0 si no hay
                   };
        }

        /// <summary>
        /// Usuarios cuya suma total de duración de conexión sea superior a la media
        /// Nombre Usuario, Suma duración
        /// </summary>
        public IEnumerable<vmNombreCantidad> UsuariosSumaDuracionMayorMedia()
        {
            var sumas = UsuarioSumaDuracionConexiones().ToList();

            return from s in sumas
                   where s.Cantidad > sumas.Average(x => x.Cantidad)
                   orderby s.Cantidad descending
                   select s;
        }

        /// <summary>
        /// Aplicaciones más usadas (suma de duración de conexiones)
        /// </summary>
        public IEnumerable<vmNombreCantidad> AplicacionesMasUsadas()
        {
            return (from u in _ctx.Usuarios
                    join a in _ctx.Aplicaciones on u.AplicacionId equals a.Id
                    join c in _ctx.Conexiones on u.Id equals c.UserId
                    group c by a.NombreAplicacion into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Sum(c => c.Duracion)
                    });
        }

        /// <summary>
        /// Aplicaciones más usadas, ordenadas de mayor a menor duración
        /// </summary>
        public IEnumerable<vmNombreCantidad> AplicacionesMasUsadasOrdenadas()
        {
            return AplicacionesMasUsadas().OrderByDescending(x => x.Cantidad);

        }

        /// <summary>
        /// Número total de conexiones por usuario
        /// </summary>
        public IEnumerable<vmNombreCantidad> UsuariosTotalConexiones()
        {
            return (from u in _ctx.Usuarios
                    join c in _ctx.Conexiones on u.Id equals c.UserId
                    group c by u.UserName into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Count()
                    }).OrderByDescending(x => x.Cantidad);
        }

        /// <summary>
        /// IPs más frecuentes (global)
        /// </summary>
        public IEnumerable<vmNombreCantidad> IPsMasUsadas()
        {
            return (from u in _ctx.Usuarios
                    join c in _ctx.Conexiones on u.Id equals c.UserId
                    group u by c.IP into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Count()
                    }
                    ).OrderByDescending(x => x.Cantidad);
        }

        /// <summary>
        /// Duración promedio de conexión por usuario
        /// </summary>
        public IEnumerable<vmNombreCantidad> UsuarioDuracionPromedio()
        {
            return (from u in _ctx.Usuarios
                    join c in _ctx.Conexiones on u.Id equals c.UserId
                    group c by u.UserName into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Sum(c => c.Duracion)/g.Count()
                    }
                    ).OrderByDescending(x => x.Cantidad);
        }

        /// <summary>
        /// Duración promedio de conexión por aplicación
        /// </summary>
        public IEnumerable<vmNombreCantidad> AplicacionDuracionPromedio()
        {
            return (from a in _ctx.Aplicaciones
                    join u in _ctx.Usuarios on a.Id equals u.AplicacionId
                    join c in _ctx.Conexiones on u.Id equals c.UserId
                    group c by a.NombreAplicacion into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Average(c => c.Duracion)
                    }
                    ).OrderByDescending(x => x.Cantidad);
        }

        // ======================== CONSULTAS COMPLEJAS / STATS AVANZADAS ========================

        /// <summary>
        /// Estadísticas detalladas por usuario
        /// Nombre usuario, total conexiones, duración total, duración promedio, IP más usada por el usuario
        /// </summary>
        public IEnumerable<vmEstadisticasUsuario> EstadisticasDetaladasUsuario()
        {
            return _ctx.Usuarios.Select(u =>
            {
                var conexionesUsuario = _ctx.Conexiones.Where(c => c.UserId == u.Id).ToList();
#pragma warning disable CS8601 // Posible asignación de referencia nula
                return new vmEstadisticasUsuario
                {
                    Usuario = u.UserName,
                    TotalConexiones = conexionesUsuario.Count,
                    DuracionTotal = conexionesUsuario.Sum(c => c.Duracion),
                    DuracionPromedio = conexionesUsuario.Any()
                        ? conexionesUsuario.Average(c => c.Duracion)
                        : 0,
                    IP_MasUsada = conexionesUsuario
                        .GroupBy(c => c.IP)
                        .OrderByDescending(g => g.Count())
                        .Select(g => g.Key)
                        .FirstOrDefault()
                };
#pragma warning restore CS8601 // Posible asignación de referencia nula
            });
        }

        /// <summary>
        /// Estadísticas detalladas por aplicación
        /// NOmbre Aplicacion, total usuarios, total conexiones, duración total, duración promedio
        /// </summary>
        public IEnumerable<vmEstadisticasApp> EstadisticasDetalladasAplicacion()
        {
            return (from a in _ctx.Aplicaciones
                    join u in _ctx.Usuarios on a.Id equals u.AplicacionId
                    join c in _ctx.Conexiones on u.Id equals c.UserId
                    group new {c.Duracion, u.Id} by a.NombreAplicacion into g
                    select new vmEstadisticasApp
                    {
                        Aplicacion = g.Key,
                        DuracionPromedio = g.Average(c => c.Duracion),
                        DuracionTotal = g.Sum(c => c.Duracion),
                        TotalConexiones = g.Count(),
                        TotalUsuarios = g.Select(x => x.Id).Distinct().Count()
                    });
        }

        /// <summary>
        /// Usuarios por rol con estadísticas de conexión
        /// Nombre Usuario, Rol, Aplicación
        /// </summary>
        public IEnumerable<vmUsuarioRolAplicacion> UsuariosRolesAplicaciones()
        {
            return (from u in _ctx.Usuarios
                    join ur in _ctx.UsuariosRoles on u.Id equals ur.UserId
                    join r in _ctx.Roles on ur.RoleId equals r.Id
                    join a in _ctx.Aplicaciones on r.AplicacionId equals a.Id
                    select new vmUsuarioRolAplicacion
                    {
                        Aplicacion = a.NombreAplicacion,
                        Rol = r.Name,
                        Usuario = u.UserName
                    });
        }

        /// <summary>
        /// Conexiones por fecha (agrupadas)
        /// Fecha, Cantidad de conexiones
        /// </summary>
        public IEnumerable<vmNombreCantidad> ConexionesPorFecha()
        {
            return (from c in _ctx.Conexiones
                    group c by c.FechaInicio.Date into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key.ToString("yyyy-MM-dd"),
                        Cantidad = g.Count()
                    }).OrderByDescending(x => DateTime.Parse(x.Nombre));
        }

        /// <summary>
        /// Conexiones por hora del día
        /// Entre cada hora, Cantidad de conexiones
        /// </summary>
        public IEnumerable<vmNombreCantidad> ConexionesPorHora()
        {
            return (from c in _ctx.Conexiones
                    group c by c.FechaInicio.Hour into g
                    select new vmNombreCantidad
                    {
                        Nombre = $"{g.Key}:00 - {g.Key + 1}:00",
                        Cantidad = g.Count()
                    }).OrderByDescending(x => x.Nombre);
        }

        /// <summary>
        /// Usuarios que han intentado conectarse desde múltiples IPs
        /// Nombre Usuario, Cantidad de IPs distintas
        /// </summary>
        public IEnumerable<vmNombreCantidad> UsuariosConMultiplesIPs()
        {
            return (from u in _ctx.Usuarios
                    join c in _ctx.Conexiones on u.Id equals c.UserId
                    group c.IP by u.UserName into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Distinct().Count(),
                    }).Where(x => x.Cantidad > 1).OrderByDescending(x => x.Cantidad);
        }

        /// <summary>
        /// Usuarios por estado (activo/bloqueado/anónimo)
        /// Estado, Cantidad de usuarios
        /// </summary>
        public IEnumerable<vmNombreCantidad> EstadoUsuarios()
        {
            return _ctx.Usuarios
                .GroupBy(u => u.EsAnonimo ? "Anónimo" :
                              u.EstaBloqueado ? "Bloqueado" :
                                                "Activo")
                .Select(g => new vmNombreCantidad
                {
                    Nombre = g.Key,
                    Cantidad = g.Count()
                });
        }

        /// <summary>
        /// Roles más populares (por cantidad de usuarios)
        /// Nombre Rol, Cantidad de usuarios
        /// </summary>
        public IEnumerable<vmNombreCantidad> RolesMasPopulares()
        {
            return (from u in _ctx.Usuarios
                    join ur in _ctx.UsuariosRoles on u.Id equals ur.UserId
                    join r in _ctx.Roles on ur.RoleId equals r.Id
                    group ur by r.Name into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Count(),
                    }).OrderByDescending(x => x.Cantidad);
        }

        /// <summary>
        /// Top N usuarios más activos (por duración total de conexiones)
        /// Nombre Usuario, Duración total
        /// </summary>
        public IEnumerable<vmNombreCantidad> TopUsuariosActivos(int top = 10)
        {
            return UsuarioSumaDuracionConexiones().Take(top);
        }

        /// <summary>
        /// Aplicaciones con mayor cantidad de usuarios únicos
        /// Nombre Aplicación, Cantidad de usuarios
        /// </summary>
        public IEnumerable<vmNombreCantidad> AplicacionesConMasUsuarios()
        {
            return (from a in _ctx.Aplicaciones
                    join u in _ctx.Usuarios on a.Id equals u.AplicacionId
                    group u by a.NombreAplicacion into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Count()
                    }).OrderByDescending(x => x.Cantidad);
        }

        /// <summary>
        /// Usuarios que nunca se han conectado
        /// Nombre Usuario
        /// </summary>
        public IEnumerable<vmNombre> UsuariosSinConexiones()
        {
            return (from u in _ctx.Usuarios
                    where !_ctx.Conexiones.Any(c => c.UserId == u.Id)
                    select new vmNombre
                    {
                        Nombre = u.UserName.ToUpper()
                    });
        }

        /// <summary>
        /// Usuarios que se han conectado desde una IP sospechosa (solo N=1 conexiones)
        /// Nombre Usuario, Cantidad de conexiones
        /// </summary>
        public IEnumerable<vmNombreCantidad> UsuariosPocasConexiones(int minConexiones = 1)
        {
            return (from u in _ctx.Usuarios
                    join c in _ctx.Conexiones on u.Id equals c.UserId
                    group c by u.UserName into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Count()
                    })
                   .Where(x => x.Cantidad <= minConexiones)
                   .OrderBy(x => x.Cantidad);
        }
    }


}
