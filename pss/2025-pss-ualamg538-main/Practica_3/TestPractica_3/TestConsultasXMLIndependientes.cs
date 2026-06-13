using Practica_3;

namespace TestPractica_3
{
    [TestClass]
    public sealed class TestConsultasXMLIndependientes
    {
        private ConsultasXMLIndependiente _consultasXML;

        [TestInitialize]
        public void Setup()
        {
            // El archivo XML debe estar en la carpeta de salida del proyecto de pruebas
            // o proporcionar la ruta completa

            _consultasXML = new ConsultasXMLIndependiente("DatosIndependientes.xml");
        }

        // ======================== TESTS DE USUARIOS ========================

        [TestMethod]
        public void XML_UsuariosEnCategoria_Alumnos_DeberiaRetornarUsuariosDeCategoria()
        {
            var resultado = _consultasXML.UsuariosEnCategoria("Alumno").Select(n => n.Nombre).ToList();
            var listaNombreAlumnos = new List<string> { "ANA", "ANTONIO", "DIANA", "JUAN" };
            CollectionAssert.AreEquivalent(resultado, listaNombreAlumnos);
        }

        [TestMethod]
        public void XML_UsuariosEnCategoria_Profesores_DeberiaRetornarUsuariosDeCategoria()
        {
            var resultado = _consultasXML.UsuariosEnCategoria("Profesor").Select(n => n.Nombre).ToList();
            var listaNombres = new List<string> { "JOSE", "JULIO", "MERCEDES" };
            CollectionAssert.AreEquivalent(resultado, listaNombres);
        }

        [TestMethod]
        public void XML_UsuariosConNombreComienza_DeberiaRetornarUsuariosComienzanConJ()
        {
            var resultado = _consultasXML.UsuariosConNombreComienza("J").Select(n => n.Nombre).ToList();
            var listaNombres = new List<string> { "JOSE", "JOSE", "JULIO", "JUAN" };
            CollectionAssert.AreEquivalent(resultado, listaNombres);
        }

        [TestMethod]
        public void XML_UsuariosConectadosIP_DeberiaRetornarUsuariosDeIP()
        {
            var resultado = _consultasXML.UsuariosConectadosIP("192.168.134.108").Select(n => n.Nombre).ToList();
            var listaNombres = new List<string> { "ANA" };
            CollectionAssert.AreEquivalent(resultado, listaNombres);
        }

        [TestMethod]
        public void XML_UsuariosNoAnonimosPorApp_DeberiaRetornarUsuariosNoAnonimos()
        {
            var resultado = _consultasXML.UsuariosNoAnonimosPorApp("Word").Select(n => n.Nombre).ToList();
            var listaNombres = new List<string> { "DIANA", "ANTONIO", "ANA", "JOSE" };
            CollectionAssert.AreEquivalent(resultado, listaNombres);
        }

        [TestMethod]
        public void XML_UsuariosPorAplicacion_DeberiaRetornarUsuariosDeApp()
        {
            var resultado = _consultasXML.UsuariosPorAplicacion("Excel").Select(n => n.Nombre).ToList();
            var listaNombres = new List<string> { "JUAN", "JULIO", "MERCEDES" };
            CollectionAssert.AreEquivalent(resultado, listaNombres);
        }

        // ======================== TESTS DE CATEGORÍAS / ROLES ========================

        [TestMethod]
        public void XML_ListaParCategoriaUsuarioParaApp_DeberiaRetornarPares()
        {
            var resultado = _consultasXML.ListaParCategoriaUsuarioParaApp("Word").ToList();
            
            Assert.IsNotNull(resultado);
            Assert.AreEqual(4, resultado.Count, "Debería haber 4 pares para Word");
        }

        [TestMethod]
        public void XML_AgrupacionUsuariosCategorias_DeberiaRetornarGrupos()
        {
            var resultado = _consultasXML.AgrupacionUsuariosCategorias().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(4, resultado.Count, "Debería haber 4 categorías diferentes");

            var grupoAlumno = resultado.FirstOrDefault(g => g.Key == "Alumno");
            Assert.IsNotNull(grupoAlumno, "Debería existir la categoría Alumno");
            Assert.AreEqual(4, grupoAlumno.Count(), "La categoría Alumno debería tener 4 usuarios");
        }

        [TestMethod]
        public void XML_CategoriaMaximoNumeroUsuarios_DeberiaRetornarCategoriaConMasUsuarios()
        {
            var resultado = _consultasXML.CategoriaMaximoNumeroUsuarios().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count, "Debería retornar solo la categoría con más usuarios");
            Assert.AreEqual("Alumno", resultado[0].Categoria, "La categoría con más usuarios debería ser Alumno");
            Assert.AreEqual("4", resultado[0].Nombre, "Alumno debería tener 4 usuarios");
        }

        [TestMethod]
        public void XML_RolesPorCantidadUsuarios_DeberiaRetornarRolesOrdenados()
        {
            var resultado = _consultasXML.RolesPorCantidadUsuarios().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(4, resultado.Count, "Debería haber 4 roles");

            Assert.AreEqual("Alumno", resultado[0].Nombre, "Alumno debería estar primero");
            Assert.AreEqual(4, resultado[0].Cantidad, "Alumno debería tener 4 usuarios");
        }

        // ======================== TESTS DE CONEXIONES / ESTADÍSTICAS ========================

        [TestMethod]
        public void XML_UsuarioSumaDuracionConexiones_DeberiaRetornarSumas()
        {
            var resultado = _consultasXML.UsuarioSumaDuracionConexiones().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(7, resultado.Count, "Debería haber 7 usuarios con conexiones");
            
            var ana = resultado.FirstOrDefault(r => r.Nombre == "Ana");
            Assert.IsNotNull(ana, "Ana debería tener conexiones");
        }

        [TestMethod]
        public void XML_UsuariosTotalConexiones_DeberiaRetornarTotales()
        {
            var resultado = _consultasXML.UsuariosTotalConexiones().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(7, resultado.Count, "Debería haber 7 usuarios con conexiones");

            var ana = resultado.FirstOrDefault(r => r.Nombre == "Ana");
            Assert.IsNotNull(ana, "Ana debería tener conexiones");
            Assert.AreEqual(8, ana.Cantidad, "Ana debería tener 8 conexiones");
        }

        [TestMethod]
        public void XML_AplicacionesMasUsadas_DeberiaRetornarAplicaciones()
        {
            var resultado = _consultasXML.AplicacionesMasUsadas().ToList();

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Count > 0, "Debería haber aplicaciones con conexiones");
        }

        [TestMethod]
        public void XML_TopUsuariosActivos_DeberiaRetornarTop10()
        {
            var resultado = _consultasXML.TopUsuariosActivos(10).ToList();

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Count <= 10, "No debería retornar más de 10 usuarios");
            Assert.AreEqual(7, resultado.Count, "Hay 7 usuarios con conexiones");
        }

        [TestMethod]
        public void XML_UsuariosSinConexiones_DeberiaRetornarUsuariosSinConexiones()
        {
            var resultado = _consultasXML.UsuariosSinConexiones().ToList();

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Count >= 1, "Debería haber al menos 1 usuario sin conexiones");
            
            var nombres = resultado.Select(r => r.Nombre).ToList();
            Assert.IsTrue(nombres.Contains("MERCEDES"), "Mercedes no tiene conexiones");
        }

        // ======================== TESTS DE CONSULTAS COMPLEJAS ========================

        [TestMethod]
        public void XML_EstadisticasDetaladasUsuario_DeberiaRetornarEstadisticas()
        {
            var resultado = _consultasXML.EstadisticasDetaladasUsuario().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(8, resultado.Count, "Debería haber estadísticas para todos los 9 usuarios");

            var ana = resultado.FirstOrDefault(r => r.Usuario == "Ana");
            Assert.IsNotNull(ana, "Ana debería tener estadísticas");
            Assert.AreEqual(8, ana.TotalConexiones, "Ana tiene 6 conexiones");
        }

        [TestMethod]
        public void XML_ConexionesPorFecha_DeberiaRetornarAgrupadasPorFecha()
        {
            var resultado = _consultasXML.ConexionesPorFecha().ToList();

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Count > 0, "Debería haber conexiones agrupadas por fecha");
        }

        [TestMethod]
        public void XML_UsuariosConMultiplesIPs_DeberiaRetornarUsuariosConVariasIPs()
        {
            var resultado = _consultasXML.UsuariosConMultiplesIPs().ToList();

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.All(r => r.Cantidad > 1), "Todos los usuarios deberían tener más de 1 IP");
        }
    }
}
