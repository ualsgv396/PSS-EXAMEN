using Microsoft.Extensions.Configuration;
using PSS.amg538.Practica_04;

namespace PSS.amg538.Practica_04
{
    public static class FactoriaAutentificacion
    {
        public static IAutentificacion Crear(IConfiguration config)
        {
            var seccion = config.GetSection("Autentificacion");
            string tipo = seccion["Tipo"] ?? "Xml";

            switch (tipo.Trim().ToLower())
            {
                case "textfile":
                    {
                        var s = seccion.GetSection("TextFile");
                        string ruta = ResolverRuta(s["Ruta"]);
                        string separador = s["Separador"] ?? ";";

                        var nombres = s.GetSection("Formato").Get<string[]>()
                                      ?? new[] { "Id", "Nombre", "PalabraPaso", "Categoria", "EsValido" };
                        var campos = nombres
                            .Select(n => Enum.Parse<CamposRegistro>(n, ignoreCase: true))
                            .ToArray();

                        return new AutentificacionTextFile(ruta, new FormatoRegistro(campos), separador);
                    }

                case "xml":
                    {
                        string ruta = ResolverRuta(seccion.GetSection("Xml")["Ruta"]);
                        return new AutentificacionXml(ruta);
                    }

                case "sqlserverfile":
                    {
                        string cadena = seccion.GetSection("SqlServerFile")["CadenaConexion"];
                        return new AutentificacionSqlServerFile(cadena);
                    }

                case "efcore":
                    {
                        string cadena = seccion.GetSection("SqlServerFile")["CadenaConexion"];
                        return new AutentificacionEfCore(cadena);
                    }

                default:
                    throw new AutentificacionExcepcion(
                        "Tipo de autentificación no soportado: " + tipo,
                        CodigoAutentificacion.ErrorDatos);
            }
        }

        // Convierte rutas relativas (usuarios.txt) en absolutas respecto a la carpeta de ejecución.
        private static string ResolverRuta(string ruta)
        {
            if (string.IsNullOrEmpty(ruta) || Path.IsPathRooted(ruta)) return ruta;
            return Path.Combine(AppContext.BaseDirectory, ruta);
        }
    }
}
