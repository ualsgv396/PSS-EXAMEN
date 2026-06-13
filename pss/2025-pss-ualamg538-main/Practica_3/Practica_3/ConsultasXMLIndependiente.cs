using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Practica_3
{
    public class ConsultasXMLIndependiente:IConsultas
    {
        private readonly string _rutaXml;
        private readonly XDocument _doc;

        public ConsultasXMLIndependiente(string xmlFilePath)
        {

            // Ruta relativa robusta para tests y ejecución normal
            _rutaXml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFilePath);

            // Verificar que existe
            if (!File.Exists(_rutaXml))
                throw new FileNotFoundException($"XML no encontrado: {_rutaXml}");

            _doc = XDocument.Load(_rutaXml);

        }

        // ======================== CONSULTAS DE USUARIOS ========================

        /// Usuarios que pertenecen a una categoría (rol) determinada
        public IEnumerable<vmNombre> UsuariosEnCategoria(string nombreCategoria)
        {
            return (from u in _doc.Descendants("User")
                    join ur in _doc.Descendants("UserRole") on (int)u.Element("Id") equals (int)ur.Element("UserId")
                    join r in _doc.Descendants("Role") on (int)ur.Element("RoleId") equals (int)r.Element("Id")
                    where (string)r.Element("Name") == nombreCategoria
                    select new vmNombre { Nombre = ((string)u.Element("UserName")).ToUpper() })
                   .Distinct();
        }

        /// Usuarios cuyo nombre comienza con la cadena proporcionada
        public IEnumerable<vmNombre> UsuariosConNombreComienza(string cadenaComienzo)
        {
            return (from u in _doc.Descendants("User")
                    where ((string)u.Element("UserName")).StartsWith(cadenaComienzo, StringComparison.OrdinalIgnoreCase)
                    select new vmNombre
                    {
                        Nombre = ((string)u.Element("UserName")).ToUpper()
                    });
        }

        /// Usuarios cuyo nombre comienza por cadenaComienzo que pertenecen a una categoría dada
        public IEnumerable<vmNombre> UsuariosConNombreComienzaEnCategoria(
            string cadenaComienzo,
            string categoria)
        {   
            return (from u in _doc.Descendants("User")

                    where ((string)u.Element("UserName")).StartsWith(cadenaComienzo, StringComparison.OrdinalIgnoreCase)
                    select new vmNombre
                    {
                        Nombre = ((string)u.Element("UserName")).ToUpper()
                    });
        }

        /// Usuarios conectados desde una IP determinada
        public IEnumerable<vmNombre> UsuariosConectadosIP(string ip)
        {
            return (from u in _doc.Descendants("User")
                    join c in _doc.Descendants("Conexion") on (int)u.Element("Id") equals (int)c.Element("UserId")
                    where ((string)c.Element("IP")).Equals(ip)
                    select new vmNombre
                    {
                        Nombre = ((string)u.Element("UserName")).ToUpper()
                    }).Distinct();
        }

        /// Encuentra el nombre del usuario de una aplicación dada a través de su e-mail
        public IEnumerable<vmNombre> EncontrarUsuarioAppEmail(string aplicacion, string email)
        {
            return (from u in _doc.Descendants("User")
                    join a in _doc.Descendants("Aplicacion") on (int)u.Element("AplicacionId") equals (int)a.Element("Id")
                    where ((string)a.Element("NombreAplicacion")).Equals(aplicacion) && ((string)u.Element("Email")).Equals(email)
                    select new vmNombre {
                        Nombre = ((string)u.Element("UserName")).ToUpper()
                    });
        }

        /// Usuarios no anónimos de una aplicación
        public IEnumerable<vmNombre> UsuariosNoAnonimosPorApp(string nombreApp)
        {
            return (from u in _doc.Descendants("User")
                    join a in _doc.Descendants("Aplicacion") on (int)u.Element("AplicacionId") equals (int)a.Element("Id")
                    where !(bool)u.Element("EsAnonimo") && ((string)a.Element("NombreAplicacion")).Equals(nombreApp)
                    select new vmNombre
                    {
                        Nombre = ((string)u.Element("UserName")).ToUpper()
                    });
        }

        /// Usuarios anónimos
        public IEnumerable<vmNombre> UsuariosAnonimos()
        {
            return (from u in _doc.Descendants("User")
                    where !(bool)u.Element("EsAnonimo")
                    select new vmNombre
                    {
                        Nombre = ((string)u.Element("UserName")).ToUpper()
                    });
        }

        /// Usuarios bloqueados
        public IEnumerable<vmNombre> UsuariosBloqueados()
        {
            return (from u in _doc.Descendants("User")
                    where !(bool)u.Element("EstaBloqueado")
                    select new vmNombre
                    {
                        Nombre = ((string)u.Element("UserName")).ToUpper()
                    });
        }

        /// Usuarios de una aplicación específica
        public IEnumerable<vmNombre> UsuariosPorAplicacion(string nombreApp)
        {
            return (from u in _doc.Descendants("User")
                    join a in _doc.Descendants("Aplicacion") on (int)u.Element("AplicacionId") equals (int)a.Element("Id")
                    where ((string)a.Element("NombreAplicacion")).Equals(nombreApp)
                    select new vmNombre
                    {
                        Nombre = ((string)u.Element("UserName")).ToUpper()
                    });
        }

        // ======================== CATEGORÍAS / ROLES ========================

        /// Lista de pares (Categoría, Usuario) para una aplicación dada 
        public IEnumerable<vmCategoriaNombre> ListaParCategoriaUsuarioParaApp(string aplicacion)
        {
            return (from u in _doc.Descendants("User")
                    join a in _doc.Descendants("Aplicacion") on (int)u.Element("AplicacionId") equals (int)a.Element("Id")
                    join ur in _doc.Descendants("UserRole") on (int)u.Element("Id") equals (int)ur.Element("UserId")
                    join r in _doc.Descendants("Role") on (int)ur.Element("RoleId") equals (int)r.Element("Id")
                    where ((string)a.Element("NombreAplicacion")).Equals(aplicacion)
                    select new vmCategoriaNombre
                    {
                        Categoria = ((string)r.Element("Name")).ToUpper(),
                        Nombre = ((string)u.Element("UserName")).ToUpper()
                    });
        }

        /// Usuarios agrupados por categoría (rol)
        public IEnumerable<IGrouping<string, vmCategoriaNombre>> AgrupacionUsuariosCategorias()
        {
            return (from u in _doc.Descendants("User")
                    join ur in _doc.Descendants("UserRole") on (int)u.Element("Id") equals (int)ur.Element("UserId")
                    join r in _doc.Descendants("Role") on (int)ur.Element("RoleId") equals (int)r.Element("Id")
                    select new vmCategoriaNombre
                    {
                        Categoria = ((string)r.Element("Name")),
                        Nombre = ((string)u.Element("UserName")).ToUpper()
                    })
                   .GroupBy(x => x.Categoria);
        }

        /// Usuarios agrupados por categorías ordenadas descendentemente
        public IEnumerable<vmCategoriaNombre> AgrupacionUsuariosCategoriasOrdenadas()
        {
            return (from u in _doc.Descendants("User")
                    join ur in _doc.Descendants("UserRole") on (int)u.Element("Id") equals (int)ur.Element("UserId")
                    join r in _doc.Descendants("Role") on (int)ur.Element("RoleId") equals (int)r.Element("Id")
                    select new vmCategoriaNombre
                    {
                        Categoria = ((string)r.Element("Name")),
                        Nombre = ((string)u.Element("UserName")).ToUpper()
                    })
                   .OrderByDescending(x => x.Categoria)
                   .ThenBy(x => x.Nombre);
        }

        /// Categoría con mayor número de usuarios
        public IEnumerable<vmCategoriaNombre> CategoriaMaximoNumeroUsuarios()
        {
            return (from u in _doc.Descendants("User")
                    join ur in _doc.Descendants("UserRole") on (int)u.Element("Id") equals (int)ur.Element("UserId")
                    join r in _doc.Descendants("Role") on (int)ur.Element("RoleId") equals (int)r.Element("Id")
                    group u by ((string)r.Element("Name")) into g
                    orderby g.Count() descending
                    select new vmCategoriaNombre
                    {
                        Categoria = g.Key,
                        Nombre = g.Count().ToString()
                    }).Take(1);
        }

        /// Todas las categorías de usuarios para una aplicación dada
        public IEnumerable<vmCategoriaNombre> TodasCategoriasApp(string aplicacion)
        {
            return (from u in _doc.Descendants("User")
                    join ur in _doc.Descendants("UserRole") on (int)u.Element("Id") equals (int)ur.Element("UserId")
                    join r in _doc.Descendants("Role") on (int)ur.Element("RoleId") equals (int)r.Element("Id")
                    join a in _doc.Descendants("Aplicacion") on (int)u.Element("AplicacionId") equals (int)a.Element("Id")
                    where ((string)a.Element("NombreAplicacion")).Equals(aplicacion)
                    select new vmCategoriaNombre
                    {
                        Categoria = ((string)r.Element("Name")),
                        Nombre = ((string)u.Element("UserName")).ToUpper()
                    }).Distinct();
        }

        /// Categorías y aplicaciones para un usuario dado
        public IEnumerable<vmCategoriaNombre> CategoriasAplicacionParaUsuario(string usuario)
        {
            return (from u in _doc.Descendants("User")
                    join ur in _doc.Descendants("UserRole") on (int)u.Element("Id") equals (int)ur.Element("UserId")
                    join r in _doc.Descendants("Role") on (int)ur.Element("RoleId") equals (int)r.Element("Id")
                    join a in _doc.Descendants("Aplicacion") on (int)u.Element("AplicacionId") equals (int)a.Element("Id")
                    where ((string)u.Element("UserName")).ToUpper().Equals(usuario)
                    select new vmCategoriaNombre
                    {
                        Categoria = ((string)r.Element("Name")),
                        Nombre = ((string)a.Element("NombreAplicacion"))
                    });
        }

        /// Roles ordenados por cantidad de usuarios (descendente)
        public IEnumerable<vmNombreCantidad> RolesPorCantidadUsuarios()
        {
            return (from u in _doc.Descendants("User")
                    join ur in _doc.Descendants("UserRole") on (int)u.Element("Id") equals (int)ur.Element("UserId")
                    join r in _doc.Descendants("Role") on (int)ur.Element("RoleId") equals (int)r.Element("Id")
                    group u by ((string)r.Element("Name")) into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Count()
                    }).OrderByDescending(x => x.Cantidad);
        }

        // ======================== CONEXIONES / ESTADÍSTICAS ========================

        /// IP con mayor número de conexiones según una categoría (rol)
        public IEnumerable<vmNombreCantidad> IPconMasConexionesSegunCategoria(string nombreCategoria)
        {
            return (from u in _doc.Descendants("User")
                    join ur in _doc.Descendants("UserRole") on (int)u.Element("Id") equals (int)ur.Element("UserId")
                    join r in _doc.Descendants("Role") on (int)ur.Element("RoleId") equals (int)r.Element("Id")
                    join c in _doc.Descendants("Conexion") on (int)u.Element("Id") equals (int)c.Element("UserId")
                    where ((string)r.Element("Name")).Equals(nombreCategoria)
                    group u by (string)c.Element("IP") into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Count()
                    }).OrderByDescending(x => x.Cantidad);
        }

        /// Suma total de duración de conexiones por usuario (solo usuarios con conexiones)
        public IEnumerable<vmNombreCantidad> UsuarioSumaDuracionConexiones()
        {
            return (from u in _doc.Descendants("User")
                    join c in _doc.Descendants("Conexion") on (int)u.Element("Id") equals (int)c.Element("UserId")
                    group c by ((string)u.Element("UserName")) into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Sum(c => ((int)c.Element("Duracion")))
                    })
                    .OrderByDescending(x => x.Cantidad);
        }

        /// LEFT OUTER JOIN - Suma total de duración de conexiones por usuario
        /// Incluye usuarios sin conexiones con 0
        public IEnumerable<vmNombreCantidad> UsuarioSumaDuracionConexionesNulos()
        {
            return (from u in _doc.Descendants("User")
                    join c in _doc.Descendants("Conexion") on (int)u.Element("Id") equals (int)c.Element("UserId") into conexionesUsuario
                    select new vmNombreCantidad
                    {
                        Nombre = ((string)u.Element("UserName")).ToUpper(),
                        Cantidad = conexionesUsuario.Sum(c => (int)c.Element("Duracion")) // 0 si no hay
                    });
        }

        /// Usuarios cuya suma total de duración de conexión sea superior a la media
        public IEnumerable<vmNombreCantidad> UsuariosSumaDuracionMayorMedia()
        {
            var sumas = UsuarioSumaDuracionConexiones().ToList();

            return from s in sumas
                   where s.Cantidad > sumas.Average(x => x.Cantidad)
                   orderby s.Cantidad descending
                   select s;
        }

        /// Aplicaciones más usadas (suma de duración de conexiones)
        public IEnumerable<vmNombreCantidad> AplicacionesMasUsadas()
        {
            return (from u in _doc.Descendants("User")
                    join c in _doc.Descendants("Conexion") on (int)u.Element("Id") equals (int)c.Element("UserId")
                    join a in _doc.Descendants("Aplicacion") on (int)u.Element("AplicacionId") equals (int)a.Element("Id")
                    group c by ((string)a.Element("NombreAplicacion")) into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Sum(c => (int)c.Element("Duracion"))
                    });
        }

        /// Aplicaciones más usadas, ordenadas de mayor a menor duración
        public IEnumerable<vmNombreCantidad> AplicacionesMasUsadasOrdenadas()
        {
            return AplicacionesMasUsadas().OrderByDescending(x => x.Cantidad);
        }

        /// Número total de conexiones por usuario
        public IEnumerable<vmNombreCantidad> UsuariosTotalConexiones()
        {
            return (from u in _doc.Descendants("User")
                    join c in _doc.Descendants("Conexion") on (int)u.Element("Id") equals (int)c.Element("UserId")
                    group c by ((string)u.Element("UserName")) into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Count()
                    }).OrderByDescending(x => x.Cantidad);
        }

        /// IPs más frecuentes (global)
        public IEnumerable<vmNombreCantidad> IPsMasUsadas()
        {
            return (from u in _doc.Descendants("User")
                    join c in _doc.Descendants("Conexion") on (int)u.Element("Id") equals (int)c.Element("UserId")
                    group u by (string)c.Element("IP") into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Count()
                    }
                    ).OrderByDescending(x => x.Cantidad);
        }

        /// Duración promedio de conexión por usuario
        public IEnumerable<vmNombreCantidad> UsuarioDuracionPromedio()
        {
            return (from u in _doc.Descendants("User")
                    join c in _doc.Descendants("Conexion") on (int)u.Element("Id") equals (int)c.Element("UserId")
                    group c by ((string)u.Element("UserName")) into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Sum(c => (int)c.Element("Duracion")) / g.Count()
                    }
                    ).OrderByDescending(x => x.Cantidad);
        }

        /// Duración promedio de conexión por aplicación
        public IEnumerable<vmNombreCantidad> AplicacionDuracionPromedio()
        {
            return (from u in _doc.Descendants("User")
                    join c in _doc.Descendants("Conexion") on (int)u.Element("Id") equals (int)c.Element("UserId")
                    group c by ((string)u.Element("UserName")) into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Sum(c => (int)c.Element("Duracion")) / g.Count()
                    }
                    ).OrderByDescending(x => x.Cantidad);
        }

        // ======================== CONSULTAS COMPLEJAS / STATS AVANZADAS ========================

        /// Estadísticas detalladas por usuario
        public IEnumerable<vmEstadisticasUsuario> EstadisticasDetaladasUsuario()
        {
            var conexiones = _doc.Descendants("Conexion");

            return _doc.Descendants("User")
                .Where(u => conexiones.Any(c => (int)c.Element("UserId") == (int)u.Element("Id")))
                .Select(u =>
                {
                    var conexionesUsuario = conexiones
                        .Where(c => (int)c.Element("UserId") == (int)u.Element("Id"))
                        .ToList();

                    return new vmEstadisticasUsuario
                    {
                        Usuario = (string)u.Element("UserName"),
                        TotalConexiones = conexionesUsuario.Count,
                        DuracionTotal = conexionesUsuario.Sum(c => (double)c.Element("Duracion")),
                        DuracionPromedio = conexionesUsuario.Average(c => (double)c.Element("Duracion")),
                        IP_MasUsada = conexionesUsuario
                                           .GroupBy(c => (string)c.Element("IP"))
                                           .OrderByDescending(g => g.Count())
                                           .Select(g => g.Key)
                                           .FirstOrDefault()
                    };
                });
        }

        /// Estadísticas detalladas por aplicación
        public IEnumerable<vmEstadisticasApp> EstadisticasDetalladasAplicacion()
        {
            return (from u in _doc.Descendants("User")
                    join c in _doc.Descendants("Conexion") on (int)u.Element("Id") equals (int)c.Element("UserId")
                    join a in _doc.Descendants("Aplicacion") on (int)u.Element("AplicacionId") equals (int)a.Element("Id")
                    group new { Duracion = (double)c.Element("Duracion"), UserId = (int)u.Element("Id") }
                        by (string)a.Element("NombreAplicacion") into g
                    select new vmEstadisticasApp
                    {
                        Aplicacion = g.Key,
                        TotalConexiones = g.Count(),
                        DuracionTotal = g.Sum(x => x.Duracion),
                        DuracionPromedio = g.Average(x => x.Duracion),
                        TotalUsuarios = g.Select(x => x.UserId).Distinct().Count()
                    });
        }

        /// Usuarios por rol con estadísticas de conexión
        public IEnumerable<vmUsuarioRolAplicacion> UsuariosRolesAplicaciones()
        {
            return (from u in _doc.Descendants("User")
                    join a in _doc.Descendants("Aplicacion") on (int)u.Element("AplicacionId") equals (int)a.Element("Id")
                    join ur in _doc.Descendants("UserRole") on (int)u.Element("Id") equals (int)ur.Element("UserId")
                    join r in _doc.Descendants("Role") on (int)ur.Element("RoleId") equals (int)r.Element("Id")
                    select new vmUsuarioRolAplicacion
                    {
                        Aplicacion = (string)a.Element("NombreAplicacion"),
                        Rol = (string)r.Element("Name"),
                        Usuario = (string)u.Element("UserName")
                    });
        }

        /// Conexiones por fecha (agrupadas)
        public IEnumerable<vmNombreCantidad> ConexionesPorFecha()
        {
            return (from c in _doc.Descendants("Conexion")
                    group c by ((DateTime)c.Element("FechaInicio")).Date into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key.ToString("yyyy-MM-dd"),
                        Cantidad = g.Count()
                    }).OrderByDescending(x => DateTime.Parse(x.Nombre));
        }

        /// Conexiones por hora del día
        public IEnumerable<vmNombreCantidad> ConexionesPorHora()
        {
            return (from c in _doc.Descendants("Conexion")
                    group c by ((DateTime)c.Element("FechaInicio")).Hour into g
                    select new vmNombreCantidad
                    {
                        Nombre = $"{g.Key}:00 - {g.Key + 1}:00",
                        Cantidad = g.Count()
                    }).OrderByDescending(x => x.Nombre);
        }

        /// Usuarios que han intentado conectarse desde múltiples IPs
        public IEnumerable<vmNombreCantidad> UsuariosConMultiplesIPs()
        {
            return (from u in _doc.Descendants("User")
                    join c in _doc.Descendants("Conexion") on (int)u.Element("Id") equals (int)c.Element("UserId")
                    group ((string)c.Element("IP")) by ((string)u.Element("UserName")) into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Distinct().Count(),
                    }).Where(x => x.Cantidad > 1).OrderByDescending(x => x.Cantidad);
        }

        /// Usuarios por estado (activo/bloqueado/anónimo)
        public IEnumerable<vmNombreCantidad> EstadoUsuarios()
        {
            return (from u in _doc.Descendants("User")
                    group u by ((bool)u.Element("EsAnonimo") ? "Anónimo" :
                                (bool)u.Element("EstaBloqueado") ? "Bloqueado" :
                                                                   "Activo") into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Count()
                    });
        }

        /// Roles más populares (por cantidad de usuarios)
        public IEnumerable<vmNombreCantidad> RolesMasPopulares()
        {
            return (from u in _doc.Descendants("User")
                    join ur in _doc.Descendants("UserRole") on (int)u.Element("Id") equals (int)ur.Element("UserId")
                    join r in _doc.Descendants("Role") on (int)ur.Element("RoleId") equals (int)r.Element("Id")
                    group ur by (string)r.Element("Name") into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Count(),
                    }).OrderByDescending(x => x.Cantidad);
        }

        /// Top N usuarios más activos (por duración total de conexiones)
        public IEnumerable<vmNombreCantidad> TopUsuariosActivos(int top = 10)
        {
            return UsuarioSumaDuracionConexiones().Take(top);
        }

        /// Aplicaciones con mayor cantidad de usuarios únicos
        public IEnumerable<vmNombreCantidad> AplicacionesConMasUsuarios()
        {
            return (from u in _doc.Descendants("User")
                    join a in _doc.Descendants("Aplicacion") on (int)u.Element("AplicacionId") equals (int)a.Element("Id")
                    group u by (string)a.Element("NombreAplicacion") into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Count()
                    }).OrderByDescending(x => x.Cantidad);
        }

        /// Usuarios que nunca se han conectado
        public IEnumerable<vmNombre> UsuariosSinConexiones()
        {
            return (from u in _doc.Descendants("User")
                    where !_doc.Descendants("Conexion").Any(c => (int)c.Element("UserId") == (int)u.Element("Id"))
                    select new vmNombre
                    {
                        Nombre = ((string)u.Element("UserName")).ToUpper()
                    });
        }

        /// Usuarios que se han conectado desde una IP sospechosa (solo N conexiones)
        public IEnumerable<vmNombreCantidad> UsuariosPocasConexiones(int minConexiones = 1)
        {
            return (from u in _doc.Descendants("User")
                    join c in _doc.Descendants("Conexion") on (int)u.Element("Id") equals (int)c.Element("UserId")
                    group c by ((string)u.Element("UserName")) into g
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
