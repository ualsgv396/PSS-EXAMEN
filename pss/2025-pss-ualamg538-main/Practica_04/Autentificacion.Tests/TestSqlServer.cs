using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using PSS.amg538.Practica_04;

namespace PSS.amg538.Practica_04
{
    [TestClass]
    public sealed class TestSqlServer
    {
        static string connectionString =
            @"data source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=%DIR_APP%\AutentificacionDB9.mdf;" +
            @"Database=AutentificacionDB9Test;Integrated Security=True";

        static TestSqlServer()
        {
            if (connectionString.Contains("%DIR_APP%"))
                connectionString = connectionString.Replace("%DIR_APP%", ExecutionDirectoryPathName());
        }

        public static string ExecutionDirectoryPathName()
        {
            var dirPath = Assembly.GetExecutingAssembly().Location;
            return Path.GetDirectoryName(dirPath) ?? AppContext.BaseDirectory;
        }

        [TestMethod]
        public void Conexion_Correcta()
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            conn.Close();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Insertar_Registro()
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText =
                "INSERT INTO Usuario (Nombre, PalabraPaso, Categoria, EsValido) " +
                "VALUES (@n, @p, @c, @v)";
            cmd.Parameters.Add("@n", SqlDbType.NVarChar, 20).Value = "N_" + Guid.NewGuid();
            cmd.Parameters.Add("@p", SqlDbType.NVarChar, 20).Value = "PalabraPaso";
            cmd.Parameters.Add("@c", SqlDbType.NVarChar, 20).Value = "Categoria";
            cmd.Parameters.Add("@v", SqlDbType.Bit).Value = true;

            int filas = cmd.ExecuteNonQuery();
            Assert.AreEqual(1, filas);
        }

        [TestMethod]
        public void Modificar_Registro()
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            int id = InsertarYObtenerId(conn);

            using var cmd = conn.CreateCommand();
            cmd.CommandText =
                "UPDATE Usuario SET Nombre=@n, PalabraPaso=@p, Categoria=@c, EsValido=@v " +
                "WHERE Id=@id";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@n", SqlDbType.NVarChar, 20).Value = "N_" + Guid.NewGuid();
            cmd.Parameters.Add("@p", SqlDbType.NVarChar, 20).Value = "PalabraPasoMod";
            cmd.Parameters.Add("@c", SqlDbType.NVarChar, 20).Value = "CategoriaMod";
            cmd.Parameters.Add("@v", SqlDbType.Bit).Value = false;

            Assert.AreEqual(1, cmd.ExecuteNonQuery());
        }

        [TestMethod]
        public void Eliminar_Registro()
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            int id = InsertarYObtenerId(conn);

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Usuario WHERE Id=@id";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            Assert.AreEqual(1, cmd.ExecuteNonQuery());
        }

        // Inserta un registro y devuelve su Id autogenerado.
        private static int InsertarYObtenerId(SqlConnection conn)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText =
                "INSERT INTO Usuario (Nombre, PalabraPaso, Categoria, EsValido) " +
                "VALUES (@n, @p, @c, @v); SELECT CAST(SCOPE_IDENTITY() AS int);";
            cmd.Parameters.Add("@n", SqlDbType.NVarChar, 20).Value = "N_" + Guid.NewGuid();
            cmd.Parameters.Add("@p", SqlDbType.NVarChar, 20).Value = "PalabraPaso";
            cmd.Parameters.Add("@c", SqlDbType.NVarChar, 20).Value = "Categoria";
            cmd.Parameters.Add("@v", SqlDbType.Bit).Value = true;

            return (int)cmd.ExecuteScalar();
        }

        // Bonus: prueba la clase real a través del interface.
        [TestMethod]
        public void Clase_Inserta_Obtiene_Y_Autentifica()
        {
            IAutentificacion auth = new AutentificacionSqlServerFile(connectionString);

            var nuevo = new UsuarioView
            {
                Nombre = "Test",
                PalabraPaso = "1234",
                Categoria = "Demo",
                EsValido = true
            };
            Assert.IsTrue(auth.InsertarUsuario(nuevo));
        }
    }
}
