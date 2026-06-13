using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Practica_3
{
    public class ConsultasXMLJerarquicas:IConsultas
    {
        private readonly string _rutaXml;
        private readonly XDocument _doc;

        public ConsultasXMLJerarquicas(string xmlFilePath)
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
            return (from u in _doc.Descendants("Usuario")
                    where u.Descendants("RoleAsignado")
                           .Any(r => (string)r.Attribute("RoleName") == nombreCategoria)
                    select new vmNombre
                    {
                        Nombre = ((string)u.Attribute("UserName")).ToUpper()
                    }).Distinct();
        }

        /// Usuarios cuyo nombre comienza con la cadena proporcionada
        public IEnumerable<vmNombre> UsuariosConNombreComienza(string cadenaComienzo)
        {
            return (from u in _doc.Descendants("Usuario")
                    where ((string)u.Attribute("UserName")).StartsWith(cadenaComienzo, StringComparison.OrdinalIgnoreCase)
                    select new vmNombre
                    {
                        Nombre = ((string)u.Attribute("UserName")).ToUpper()
                    });
        }

        /// Usuarios cuyo nombre comienza por cadenaComienzo que pertenecen a una categoría dada
        public IEnumerable<vmNombre> UsuariosConNombreComienzaEnCategoria(
            string cadenaComienzo,
            string categoria)
        {
            return (from u in _doc.Descendants("Usuario")
                    where ((string)u.Attribute("UserName")).StartsWith(cadenaComienzo, StringComparison.OrdinalIgnoreCase)
                    select new vmNombre
                    {
                        Nombre = ((string)u.Attribute("UserName")).ToUpper()
                    });
        }

        /// Usuarios conectados desde una IP determinada
        public IEnumerable<vmNombre> UsuariosConectadosIP(string ip)
        {
            return (from u in _doc.Descendants("Usuario")
                    where u.Descendants("Conexion")
                           .Any(c => (string)c.Attribute("IP") == ip)
                    select new vmNombre
                    {
                        Nombre = ((string)u.Attribute("UserName")).ToUpper()
                    }).Distinct();
        }

        /// Encuentra el nombre del usuario de una aplicación dada a través de su e-mail
        public IEnumerable<vmNombre> EncontrarUsuarioAppEmail(string aplicacion, string email)
        {
            return (from u in _doc.Descendants("Usuario")
                    where (string)u.Ancestors("Aplicacion").First().Attribute("Nombre") == aplicacion
                       && (string)u.Attribute("Email") == email
                    select new vmNombre
                    {
                        Nombre = ((string)u.Attribute("UserName")).ToUpper()
                    });
        }

        /// Usuarios no anónimos de una aplicación
        public IEnumerable<vmNombre> UsuariosNoAnonimosPorApp(string nombreApp)
        {
            return (from u in _doc.Descendants("Usuario")
                    where (string)u.Ancestors("Aplicacion").First().Attribute("Nombre") == nombreApp
                    where !(bool)u.Attribute("EsAnonimo")
                    select new vmNombre
                    {
                        Nombre = ((string)u.Attribute("UserName")).ToUpper()
                    });
        }

        /// Usuarios anónimos
        public IEnumerable<vmNombre> UsuariosAnonimos()
        {
            return (from u in _doc.Descendants("Usuario")
                    where !(bool)u.Attribute("EsAnonimo")
                    select new vmNombre
                    {
                        Nombre = ((string)u.Attribute("UserName")).ToUpper()
                    });
        }

        /// Usuarios bloqueados
        public IEnumerable<vmNombre> UsuariosBloqueados()
        {
            return (from u in _doc.Descendants("Usuario")
                    where !(bool)u.Attribute("EstaBloqueado")
                    select new vmNombre
                    {
                        Nombre = ((string)u.Attribute("UserName")).ToUpper()
                    });
        }

        /// Usuarios de una aplicación específica
        public IEnumerable<vmNombre> UsuariosPorAplicacion(string nombreApp)
        {
            return (from u in _doc.Descendants("Usuario")
                    where (string)u.Ancestors("Aplicacion").First().Attribute("Nombre") == nombreApp
                    select new vmNombre
                    {
                        Nombre = ((string)u.Attribute("UserName")).ToUpper()
                    });
        }

        // ======================== CATEGORÍAS / ROLES ========================

        /// Lista de pares (Categoría, Usuario) para una aplicación dada 
        public IEnumerable<vmCategoriaNombre> ListaParCategoriaUsuarioParaApp(string aplicacion)
        {
            return (from u in _doc.Descendants("Usuario")
                    where (string)u.Ancestors("Aplicacion").First().Attribute("Nombre") == aplicacion
                    from r in u.Descendants("RoleAsignado")
                    select new vmCategoriaNombre
                    {
                        Categoria = (string)r.Attribute("RoleName"),
                        Nombre = ((string)u.Attribute("UserName")).ToUpper()
                    });
        }

        /// Usuarios agrupados por categoría (rol)
        public IEnumerable<IGrouping<string, vmCategoriaNombre>> AgrupacionUsuariosCategorias()
        {
            return (from u in _doc.Descendants("Usuario")
                    from r in u.Descendants("RoleAsignado")
                    select new vmCategoriaNombre
                    {
                        Categoria = ((string)r.Attribute("RoleName")),
                        Nombre = ((string)u.Attribute("UserName")).ToUpper()
                    })
                   .GroupBy(x => x.Categoria);
        }

        /// Usuarios agrupados por categorías ordenadas descendentemente
        public IEnumerable<vmCategoriaNombre> AgrupacionUsuariosCategoriasOrdenadas()
        {
            return (from u in _doc.Descendants("Usuario")
                    from r in u.Descendants("RoleAsignado")
                    select new vmCategoriaNombre
                    {
                        Categoria = ((string)r.Attribute("RoleName")),
                        Nombre = ((string)u.Attribute("UserName")).ToUpper()
                    })
                   .OrderByDescending(x => x.Categoria)
                   .ThenBy(x => x.Nombre);
        }

        /// Categoría con mayor número de usuarios
        public IEnumerable<vmCategoriaNombre> CategoriaMaximoNumeroUsuarios()
        {
            return (from u in _doc.Descendants("Usuario")
                    from r in u.Descendants("RoleAsignado")
                    group u by ((string)r.Attribute("RoleName")) into g
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
            return (from u in _doc.Descendants("Usuario")
                    where (string)u.Ancestors("Aplicacion").First().Attribute("Nombre") == aplicacion
                    from r in u.Descendants("RoleAsignado")
                    select new vmCategoriaNombre
                    {
                        Categoria = ((string)r.Attribute("RoleName")),
                        Nombre = ((string)u.Attribute("UserName")).ToUpper()
                    }).Distinct();
        }

        /// Categorías y aplicaciones para un usuario dado
        public IEnumerable<vmCategoriaNombre> CategoriasAplicacionParaUsuario(string usuario)
        {
            return (from u in _doc.Descendants("Usuario")
                    where (string)u.Ancestors("Aplicacion").First().Attribute("Nombre") == usuario
                    from r in u.Descendants("RoleAsignado")
                    select new vmCategoriaNombre
                    {
                        Categoria = ((string)r.Attribute("RoleName")),
                        Nombre = ((string)u.Attribute("UserName")).ToUpper()
                    });
        }

        /// Roles ordenados por cantidad de usuarios (descendente)
        public IEnumerable<vmNombreCantidad> RolesPorCantidadUsuarios()
        {
            return (from u in _doc.Descendants("Usuario")
                    from r in u.Descendants("RoleAsignado")
                    group u by ((string)r.Attribute("RoleName")) into g
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
            return (from u in _doc.Descendants("Usuario")
                    where (string)u.Descendants("RoleAsignado").First().Attribute("RoleName") == nombreCategoria
                    from c in u.Descendants("Conexion")

                    group u by (string)c.Attribute("IP") into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Count()
                    }).OrderByDescending(x => x.Cantidad);
        }

        /// Suma total de duración de conexiones por usuario (solo usuarios con conexiones)
        public IEnumerable<vmNombreCantidad> UsuarioSumaDuracionConexiones()
        {
            return (from u in _doc.Descendants("Usuario")
                    from c in u.Descendants("Conexion")
                    group c by ((string)u.Attribute("UserName")) into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Sum(c => ((int)c.Attribute("Duracion")))
                    })
                    .OrderByDescending(x => x.Cantidad);
        }

        /// LEFT OUTER JOIN - Suma total de duración de conexiones por usuario
        /// Incluye usuarios sin conexiones con 0
        public IEnumerable<vmNombreCantidad> UsuarioSumaDuracionConexionesNulos()
        {
            return from u in _doc.Descendants("Usuario")
                   let conexionesUsuario = u.Descendants("Conexion")
                   select new vmNombreCantidad
                   {
                       Nombre = (string)u.Attribute("UserName"),
                       Cantidad = conexionesUsuario.Sum(c => (double)c.Attribute("Duracion"))
                   };
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
            return (from u in _doc.Descendants("Usuario")
                    from c in u.Descendants("Conexion")
                    from a in u.Ancestors("Aplicacion")
                    group c by ((string)a.Attribute("Nombre")) into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Sum(c => (int)c.Attribute("Duracion"))
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
            return (from u in _doc.Descendants("Usuario")
                    let conexiones = u.Descendants("Conexion")
                    where conexiones.Any()
                    select new vmNombreCantidad
                    {
                        Nombre = (string)u.Attribute("UserName"),
                        Cantidad = conexiones.Count()
                    }).OrderByDescending(x => x.Cantidad);
        }

        /// IPs más frecuentes (global)
        public IEnumerable<vmNombreCantidad> IPsMasUsadas()
        {
            return (from u in _doc.Descendants("Usuario")
                    from c in u.Descendants("Conexion")
                    group u by (string)c.Attribute("IP") into g
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
            return (from u in _doc.Descendants("Usuario")
                    from c in u.Descendants("Conexion")
                    group c by ((string)u.Attribute("UserName")) into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Sum(c => (int)c.Attribute("Duracion")) / g.Count()
                    }
                    ).OrderByDescending(x => x.Cantidad);
        }

        /// Duración promedio de conexión por aplicación
        public IEnumerable<vmNombreCantidad> AplicacionDuracionPromedio()
        {
            return (from u in _doc.Descendants("Usuario")
                    from c in u.Descendants("Conexion")
                    group c by ((string)u.Attribute("UserName")) into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Sum(c => (int)c.Attribute("Duracion")) / g.Count()
                    }
                    ).OrderByDescending(x => x.Cantidad);
        }

        // ======================== CONSULTAS COMPLEJAS / STATS AVANZADAS ========================

        /// Estadísticas detalladas por usuario
        public IEnumerable<vmEstadisticasUsuario> EstadisticasDetaladasUsuario()
        {
            return from u in _doc.Descendants("Usuario")
                   let conexiones = u.Descendants("Conexion").ToList()
                   select new vmEstadisticasUsuario
                   {
                       Usuario = (string)u.Attribute("UserName"),
                       TotalConexiones = conexiones.Count,
                       DuracionTotal = conexiones.Sum(c => (double)c.Attribute("Duracion")),
                       DuracionPromedio = conexiones.Any()
                                          ? conexiones.Average(c => (double)c.Attribute("Duracion"))
                                          : 0,
                       IP_MasUsada = conexiones
                                          .GroupBy(c => (string)c.Attribute("IP"))
                                          .OrderByDescending(g => g.Count())
                                          .Select(g => g.Key)
                                          .FirstOrDefault()
                   };
        }

        /// Estadísticas detalladas por aplicación
        public IEnumerable<vmEstadisticasApp> EstadisticasDetalladasAplicacion()
        {
            return (from u in _doc.Descendants("Usuario")
                    from c in u.Descendants("Conexion")
                    from a in u.Ancestors("Aplicacion")
                    group new { Duracion = (double)c.Attribute("Duracion"), UserId = (int)u.Attribute("Id") }
                        by (string)a.Attribute("Nombre") into g
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
            return (from u in _doc.Descendants("Usuario")
                    from r in u.Descendants("RoleAsignado")
                    from a in u.Ancestors("Aplicacion")
                    select new vmUsuarioRolAplicacion
                    {
                        Aplicacion = (string)a.Attribute("Nombre"),
                        Rol = (string)r.Attribute("RoleName"),
                        Usuario = (string)u.Attribute("UserName")
                    });
        }

        /// Conexiones por fecha (agrupadas)
        public IEnumerable<vmNombreCantidad> ConexionesPorFecha()
        {
            return (from c in _doc.Descendants("Conexion")
                    group c by ((DateTime)c.Attribute("FechaInicio")).Date into g
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
                    group c by ((DateTime)c.Attribute("FechaInicio")).Hour into g
                    select new vmNombreCantidad
                    {
                        Nombre = $"{g.Key}:00 - {g.Key + 1}:00",
                        Cantidad = g.Count()
                    }).OrderByDescending(x => x.Nombre);
        }

        /// Usuarios que han intentado conectarse desde múltiples IPs
        public IEnumerable<vmNombreCantidad> UsuariosConMultiplesIPs()
        {
            return (from u in _doc.Descendants("Usuario")
                    from c in u.Descendants("Conexion")
                    group ((string)c.Attribute("IP")) by ((string)u.Attribute("UserName")) into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Distinct().Count(),
                    }).Where(x => x.Cantidad > 1).OrderByDescending(x => x.Cantidad);
        }

        /// Usuarios por estado (activo/bloqueado/anónimo)
        public IEnumerable<vmNombreCantidad> EstadoUsuarios()
        {
            return (from u in _doc.Descendants("Usuario")
                    group u by ((bool)u.Attribute("EsAnonimo") ? "Anónimo" :
                                (bool)u.Attribute("EstaBloqueado") ? "Bloqueado" :
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
            return (from u in _doc.Descendants("Usuario")
                    let conexiones = u.Descendants("Conexion")
                    where conexiones.Any()
                    select new vmNombreCantidad
                    {
                        Nombre = (string)u.Attribute("UserName"),
                        Cantidad = conexiones.Sum(c => (double)c.Attribute("Duracion"))
                    })
                   .OrderByDescending(x => x.Cantidad);
        }

        /// Top N usuarios más activos (por duración total de conexiones)
        public IEnumerable<vmNombreCantidad> TopUsuariosActivos(int top = 10)
        {
            return (from u in _doc.Descendants("Usuario")
                    let conexiones = u.Descendants("Conexion")
                    where conexiones.Any()
                    select new vmNombreCantidad
                    {
                        Nombre = (string)u.Attribute("UserName"),
                        Cantidad = conexiones.Sum(c => (double)c.Attribute("Duracion"))
                    })
                   .OrderByDescending(x => x.Cantidad)
                   .Take(top);
        }

        /// Aplicaciones con mayor cantidad de usuarios únicos
        public IEnumerable<vmNombreCantidad> AplicacionesConMasUsuarios()
        {
            return (from u in _doc.Descendants("Usuario")
                    from a in u.Ancestors("Aplicacion")
                    group u by (string)a.Attribute("Nombre") into g
                    select new vmNombreCantidad
                    {
                        Nombre = g.Key,
                        Cantidad = g.Count()
                    }).OrderByDescending(x => x.Cantidad);
        }

        /// Usuarios que nunca se han conectado
        public IEnumerable<vmNombre> UsuariosSinConexiones()
        {
            return (from u in _doc.Descendants("Usuario")
                    where !u.Descendants("Conexion").Any()
                    select new vmNombre
                    {
                        Nombre = ((string)u.Attribute("UserName")).ToUpper()
                    });
        }

        /// Usuarios que se han conectado desde una IP sospechosa (solo N conexiones)
        public IEnumerable<vmNombreCantidad> UsuariosPocasConexiones(int minConexiones = 1)
        {
            return (from u in _doc.Descendants("Usuario")
                    from c in u.Descendants("Conexion")
                    group c by ((string)u.Attribute("UserName")) into g
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
