using Microsoft.Extensions.Configuration;
using PSS.amg538.Practica_04;
using System.Text;

namespace PSS.amg538.Practica_04
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            // 1) Leer la configuración.
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            // 2) Construir la implementación elegida con la factoría.
            IAutentificacion auth;
            try
            {
                auth = FactoriaAutentificacion.Crear(config);
            }
            catch (AutentificacionExcepcion ex)
            {
                Console.WriteLine("No se pudo iniciar el sistema de autentificación:");
                Console.WriteLine("  " + ex.Message);
                return;
            }

            // 3) Pedir credenciales.
            Console.Write("Id de usuario: ");
            string id = Console.ReadLine() ?? "";
            string password = LeerPassword("Palabra de paso: ");

            // 4) Autentificar y tratar el código devuelto.
            CodigoAutentificacion codigo;
            try
            {
                codigo = auth.EsUsuarioAutentificado(id, password);
            }
            catch (AutentificacionExcepcion ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error de acceso a datos: " + ex.Message);
                return;
            }

            Console.WriteLine();
            if (codigo == CodigoAutentificacion.AccesoCorrecto)
            {
                var u = auth.ObtenerUsuario(id);
                Console.WriteLine($"Bienvenido, {u.Nombre} ({u.Categoria})");
            }
            else
            {
                Console.WriteLine("Acceso denegado. Causas:");
                if (codigo.HasFlag(CodigoAutentificacion.ErrorIdUsuario))
                    Console.WriteLine("  - El usuario no existe.");
                if (codigo.HasFlag(CodigoAutentificacion.ErrorPalabraPaso))
                    Console.WriteLine("  - La palabra de paso es incorrecta.");
                if (codigo.HasFlag(CodigoAutentificacion.AccesoInvalido))
                    Console.WriteLine("  - El usuario no está autorizado a acceder.");
                if (codigo.HasFlag(CodigoAutentificacion.ErrorDatos))
                    Console.WriteLine("  - Error de acceso a los datos.");
            }
        }

        // Lee la contraseña mostrando asteriscos en lugar de los caracteres.
        static string LeerPassword(string prompt)
        {
            Console.Write(prompt);
            try
            {
                var sb = new StringBuilder();
                ConsoleKeyInfo key;
                do
                {
                    key = Console.ReadKey(intercept: true);
                    if (key.Key == ConsoleKey.Backspace && sb.Length > 0)
                    {
                        sb.Length--;
                        Console.Write("\b \b");
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        sb.Append(key.KeyChar);
                        Console.Write('*');
                    }
                } while (key.Key != ConsoleKey.Enter);
                Console.WriteLine();
                return sb.ToString();
            }
            catch (InvalidOperationException)
            {
                // En algunas terminales integradas la entrada está redirigida:
                // leemos sin ocultar para que no falle.
                return Console.ReadLine() ?? "";
            }
        }
    }
}
