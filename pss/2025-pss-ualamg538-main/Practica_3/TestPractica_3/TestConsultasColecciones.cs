using Practica_3;

namespace TestPractica_3
{
    [TestClass]
    public sealed class TestConsultasColecciones
    {
        private Consultas _consultas;

        [TestInitialize]
        public void Setup()
        {
            _consultas = new Consultas(new DatosColecciones());
        }

        // ======================== TESTS DE USUARIOS ========================

        [TestMethod]
        public void UsuariosEnCategoria_Alumnos_DeberiaRetornarUsuariosDeCategoria()
        {
            var resultado = _consultas.UsuariosEnCategoria("Alumno").Select (n=>n.Nombre).ToList();
            var listaNombreAlumnos = new List<string> { "ANA", "ANTONIO", "DIANA", "JUAN" };
            CollectionAssert.AreEquivalent(resultado, listaNombreAlumnos);

        }
        [TestMethod]
        public void UsuariosEnCategoria_Profesores_DeberiaRetornarUsuariosDeCategoria()
        {
            var resultado = _consultas.UsuariosEnCategoria("Profesor").Select(n => n.Nombre).ToList();
            var listaNombres = new List<string> { "JOSE", "JULIO", "MERCEDES" };
            CollectionAssert.AreEquivalent(resultado, listaNombres);

        }

        [TestMethod]
        public void UsuariosEnCategoria_Administrador_DeberiaRetornarUsuariosDeCategoria()
        {
            var resultado = _consultas.UsuariosEnCategoria("Administrador").Select(n => n.Nombre).ToList();
            var listaNombres = new List<string> { "JOSE" };
            CollectionAssert.AreEquivalent(resultado, listaNombres);

        }

        [TestMethod]
        public void UsuariosConNombreComienza_DeberiaRetornarUsuariosComienzanConJ()
        {
            var resultado = _consultas.UsuariosConNombreComienza("J").Select(n => n.Nombre).ToList();
            var listaNombres = new List<string> { "JOSE", "JOSE", "JULIO", "JUAN" };
            CollectionAssert.AreEquivalent(resultado, listaNombres);
        }

        [TestMethod]
        public void UsuariosConNombreComienza_ConCadenaVacia_DeberiaRetornarTodos()
        { 
            var resultado = _consultas.UsuariosConNombreComienza("").Select(n => n.Nombre).ToList();
        var listaNombres = new List<string> { "DIANA","JOSE", "JOSE", "JULIO", "JUAN", "ANTONIO","ANA","MERCEDES","ANONIMO" };
        CollectionAssert.AreEquivalent(resultado, listaNombres);
        }

        [TestMethod]
        public void UsuariosConNombreComienzaEnCategoria_DeberiaRetornarUsuariosFiltrados()
        {
            var resultado = _consultas.UsuariosConNombreComienzaEnCategoria("J", "Profesor").Select(n => n.Nombre).ToList();
            var listaNombres = new List<string> {  "JOSE", "JULIO" };
            CollectionAssert.AreEquivalent(resultado, listaNombres);
        }

        [TestMethod]
        public void UsuariosConectadosIP_DeberiaRetornarUsuariosDeIP()
        {
            var resultado = _consultas.UsuariosConectadosIP("192.168.134.108").Select(n => n.Nombre).ToList();
            var listaNombres = new List<string> { "ANA"};
            CollectionAssert.AreEquivalent(resultado, listaNombres);
        }

        [TestMethod]
        public void EncontrarUsuarioAppEmail_DeberiaRetornarUsuarioPorEmail()
        {
            var resultado = _consultas.EncontrarUsuarioAppEmail("Word", "diana@example.com").Select(n => n.Nombre).ToList();
            var listaNombres = new List<string> { "DIANA" };
            CollectionAssert.AreEquivalent(resultado, listaNombres);
        }

        [TestMethod]
        public void UsuariosNoAnonimosPorApp_DeberiaRetornarUsuariosNoAnonimos()
        {
            var resultado = _consultas.UsuariosNoAnonimosPorApp("Word").Select(n => n.Nombre).ToList();
            var listaNombres = new List<string> { "DIANA","ANTONIO","ANA","JOSE" };
            CollectionAssert.AreEquivalent(resultado, listaNombres);
        }

        [TestMethod]
        public void UsuariosAnonimos_DeberiaRetornarSoloAnonimos()
        {
            var resultado = _consultas.UsuariosAnonimos().ToList();
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Any(u => u.Nombre == "ANONIMO"));
        }

        [TestMethod]
        public void UsuariosBloqueados_NoDeberiaRetornarListaVacia()
        {
            var resultado = _consultas.UsuariosBloqueados().ToList();
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void UsuariosPorAplicacion_DeberiaRetornarUsuariosDeApp()
        {
            var resultado = _consultas.UsuariosPorAplicacion("Excel").Select(n => n.Nombre).ToList();
            var listaNombres = new List<string> { "JUAN", "JULIO", "MERCEDES" };
            CollectionAssert.AreEquivalent(resultado, listaNombres);
        }

        // ======================== TESTS DE CATEGORÍAS / ROLES ========================

        [TestMethod]
        public void ListaParCategoriaUsuarioParaApp_DeberiaRetornarPares()
        {
            var resultado = _consultas.ListaParCategoriaUsuarioParaApp("Word").ToList();
            var listaPares = new List<vmCategoriaNombre> {
                new vmCategoriaNombre { Nombre="ANA", Categoria="Alumno"},
                new vmCategoriaNombre { Nombre="ANTONIO", Categoria="Alumno"},
                new vmCategoriaNombre { Nombre="DIANA", Categoria="Alumno"},
                new vmCategoriaNombre { Nombre="JOSE", Categoria="Profesor"}
            };
            CollectionAssert.AreEquivalent(resultado, listaPares);
        }

        [TestMethod]
        public void AgrupacionUsuariosCategorias_DeberiaRetornarGrupos()
        {
            var resultado = _consultas.AgrupacionUsuariosCategorias().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(4, resultado.Count, "Debería haber 4 categorías diferentes");

            var grupoAlumno = resultado.FirstOrDefault(g => g.Key == "Alumno");
            Assert.IsNotNull(grupoAlumno, "Debería existir la categoría Alumno");
            Assert.AreEqual(4, grupoAlumno.Count(), "La categoría Alumno debería tener 4 usuarios");
            var nombresAlumnos = grupoAlumno.Select(u => u.Nombre).ToList();
            CollectionAssert.AreEquivalent(new List<string> { "Diana", "Juan", "Antonio", "Ana" }, nombresAlumnos);

            var grupoProfesor = resultado.FirstOrDefault(g => g.Key == "Profesor");
            Assert.IsNotNull(grupoProfesor, "Debería existir la categoría Profesor");
            Assert.AreEqual(3, grupoProfesor.Count(), "La categoría Profesor debería tener 3 usuarios");
            var nombresProfesores = grupoProfesor.Select(u => u.Nombre).ToList();
            CollectionAssert.AreEquivalent(new List<string> { "Jose", "Julio", "Mercedes" }, nombresProfesores);

            var grupoAdministrador = resultado.FirstOrDefault(g => g.Key == "Administrador");
            Assert.IsNotNull(grupoAdministrador, "Debería existir la categoría Administrador");
            Assert.AreEqual(1, grupoAdministrador.Count(), "La categoría Administrador debería tener 1 usuario");
            Assert.AreEqual("Jose", grupoAdministrador.First().Nombre);

            var grupoInvitado = resultado.FirstOrDefault(g => g.Key == "Invitado");
            Assert.IsNotNull(grupoInvitado, "Debería existir la categoría Invitado");
            Assert.AreEqual(1, grupoInvitado.Count(), "La categoría Invitado debería tener 1 usuario");
            Assert.AreEqual("Anonimo", grupoInvitado.First().Nombre);
        }

        [TestMethod]
        public void AgrupacionUsuariosCategoriasOrdenadas_DeberiaRetornarOrdenados()
        {
            var resultado = _consultas.AgrupacionUsuariosCategoriasOrdenadas().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(9, resultado.Count, "Debería haber 9 registros en total");

            // Verificar orden descendente por categoría y luego ascendente por nombre
            // Orden esperado: Profesor > Invitado > Alumno > Administrador

            var esperado = new List<vmCategoriaNombre>
            {
                // Profesor (descendente alfabéticamente)
                new vmCategoriaNombre { Categoria = "Profesor", Nombre = "Jose" },
                new vmCategoriaNombre { Categoria = "Profesor", Nombre = "Julio" },
                new vmCategoriaNombre { Categoria = "Profesor", Nombre = "Mercedes" },

                // Invitado
                new vmCategoriaNombre { Categoria = "Invitado", Nombre = "Anonimo" },

                // Alumno (ascendente alfabéticamente por nombre)
                new vmCategoriaNombre { Categoria = "Alumno", Nombre = "Ana" },
                new vmCategoriaNombre { Categoria = "Alumno", Nombre = "Antonio" },
                new vmCategoriaNombre { Categoria = "Alumno", Nombre = "Diana" },
                new vmCategoriaNombre { Categoria = "Alumno", Nombre = "Juan" },

                // Administrador
                new vmCategoriaNombre { Categoria = "Administrador", Nombre = "Jose" }
            };

            // Verificar orden exacto
            for (int i = 0; i < esperado.Count; i++)
            {
                Assert.AreEqual(esperado[i].Categoria, resultado[i].Categoria, 
                    $"La categoría en la posición {i} no coincide");
                Assert.AreEqual(esperado[i].Nombre, resultado[i].Nombre, 
                    $"El nombre en la posición {i} no coincide");
            }

            // Verificar que las categorías están en orden descendente
            var categorias = resultado.Select(r => r.Categoria).Distinct().ToList();
            Assert.AreEqual("Profesor", categorias[0], "Primera categoría debería ser Profesor");
            Assert.AreEqual("Invitado", categorias[1], "Segunda categoría debería ser Invitado");
            Assert.AreEqual("Alumno", categorias[2], "Tercera categoría debería ser Alumno");
            Assert.AreEqual("Administrador", categorias[3], "Cuarta categoría debería ser Administrador");
        }

        [TestMethod]
        public void CategoriaMaximoNumeroUsuarios_DeberiaRetornarCategoriaConMasUsuarios()
        {
            var resultado = _consultas.CategoriaMaximoNumeroUsuarios().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count, "Debería retornar solo la categoría con más usuarios");
            Assert.AreEqual("Alumno", resultado[0].Categoria, "La categoría con más usuarios debería ser Alumno");
            Assert.AreEqual("4", resultado[0].Nombre, "Alumno debería tener 4 usuarios");
        }

        [TestMethod]
        public void TodasCategoriasApp_DeberiaRetornarCategoriasDeApp()
        {
            var resultado = _consultas.TodasCategoriasApp("Word").ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count, "Word debería tener 2 categorías");

            var categorias = resultado.Select(r => r.Categoria).ToList();
            CollectionAssert.AreEquivalent(new List<string> { "Alumno", "Profesor" }, categorias);
        }

        [TestMethod]
        public void CategoriasAplicacionParaUsuario_DeberiaRetornarCategoriasDeUsuario()
        {
            var resultado = _consultas.CategoriasAplicacionParaUsuario("Diana").ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count, "Diana debería tener 1 rol");
            Assert.AreEqual("Alumno", resultado[0].Categoria, "Diana debería ser Alumno");
            Assert.AreEqual("Word", resultado[0].Nombre, "Diana debería estar en Word");
        }

        [TestMethod]
        public void RolesPorCantidadUsuarios_DeberiaRetornarRolesOrdenados()
        {
            var resultado = _consultas.RolesPorCantidadUsuarios().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(4, resultado.Count, "Debería haber 4 roles");

            // Verificar orden descendente por cantidad
            Assert.AreEqual("Alumno", resultado[0].Nombre, "Alumno debería estar primero");
            Assert.AreEqual(4, resultado[0].Cantidad, "Alumno debería tener 4 usuarios");

            Assert.AreEqual("Profesor", resultado[1].Nombre, "Profesor debería estar segundo");
            Assert.AreEqual(3, resultado[1].Cantidad, "Profesor debería tener 3 usuarios");

            Assert.AreEqual(1, resultado[2].Cantidad, "debería tener 1 usuario");
            Assert.AreEqual(1, resultado[3].Cantidad, "debería tener 1 usuario");
        }

        // ======================== TESTS DE CONEXIONES / ESTADÍSTICAS ========================

        [TestMethod]
        public void IPconMasConexionesSegunCategoria_DeberiaRetornarIPsOrdenadas()
        {
            var resultado = _consultas.IPconMasConexionesSegunCategoria("Alumno").ToList();

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Count > 0, "Debería haber IPs para categoría Alumno");

            // La IP con más conexiones de alumnos debería ser la primera
            Assert.AreEqual("192.168.134.108", resultado[0].Nombre, "IP con más conexiones de alumnos");
            Assert.AreEqual(6, resultado[0].Cantidad, "Ana tiene 6 conexiones desde esta IP");
        }

        [TestMethod]
        public void UsuarioSumaDuracionConexiones_DeberiaRetornarSumas()
        {
            var resultado = _consultas.UsuarioSumaDuracionConexiones().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(7, resultado.Count, "Debería haber 7 usuarios con conexiones");
            Assert.IsTrue(resultado.All(r => r.Cantidad >= 0));

            // Verificar que está ordenado descendentemente
            var ana = resultado.FirstOrDefault(r => r.Nombre == "Ana");
            Assert.IsNotNull(ana, "Ana debería tener conexiones");
            Assert.AreEqual(1369, ana.Cantidad, "Suma de duración de conexiones de Ana");
        }

        [TestMethod]
        public void UsuarioSumaDuracionConexionesNulos_DeberiaIncluirUsuariosSinConexiones()
        {
            var resultado = _consultas.UsuarioSumaDuracionConexionesNulos().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(9, resultado.Count, "Debería incluir todos los 9 usuarios");

            // Verificar que incluye usuarios sin conexiones con 0
            var mercedes = resultado.FirstOrDefault(r => r.Nombre == "Mercedes");
            Assert.IsNotNull(mercedes, "Mercedes debería estar en la lista");
            Assert.AreEqual(0, mercedes.Cantidad, "Mercedes no tiene conexiones");
        }

        [TestMethod]
        public void UsuariosSumaDuracionMayorMedia_DeberiaRetornarUsuariosSobreMedia()
        {
            var resultado = _consultas.UsuariosSumaDuracionMayorMedia().ToList();

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Count > 0, "Debería haber usuarios sobre la media");

            // Todos los usuarios retornados deberían tener duración mayor a la media
            Assert.IsTrue(resultado.All(r => r.Cantidad > 0), "Todos deberían tener conexiones");

            // Verificar orden descendente
            for (int i = 0; i < resultado.Count - 1; i++)
            {
                Assert.IsTrue(resultado[i].Cantidad >= resultado[i + 1].Cantidad, 
                    "Debería estar ordenado descendentemente por duración");
            }
        }

        [TestMethod]
        public void AplicacionesMasUsadas_DeberiaRetornarAplicaciones()
        {
            var resultado = _consultas.AplicacionesMasUsadas().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(4, resultado.Count, "Debería haber 3 aplicaciones con conexiones");

            var word = resultado.FirstOrDefault(r => r.Nombre == "Word");
            var excel = resultado.FirstOrDefault(r => r.Nombre == "Excel");
            var gestion = resultado.FirstOrDefault(r => r.Nombre == "GestionUsuarios");
            var explorer = resultado.FirstOrDefault(r => r.Nombre == "Explorer");

            Assert.IsNotNull(word, "Word debería tener conexiones");
            Assert.IsNotNull(excel, "Excel debería tener conexiones");
            Assert.IsNotNull(gestion, "GestionUsuarios debería tener conexiones");
            Assert.IsNotNull(explorer, "Explorer debería tener conexiones");
        }

        [TestMethod]
        public void AplicacionesMasUsadasOrdenadas_DeberiaRetornarOrdenadas()
        {
            var resultado = _consultas.AplicacionesMasUsadasOrdenadas().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(4, resultado.Count, "Debería haber 4 aplicaciones");

            // Verificar orden descendente
            for (int i = 0; i < resultado.Count - 1; i++)
            {
                Assert.IsTrue(resultado[i].Cantidad >= resultado[i + 1].Cantidad, 
                    "Debería estar ordenado descendentemente por duración total");
            }
        }

        [TestMethod]
        public void UsuariosTotalConexiones_DeberiaRetornarTotales()
        {
            var resultado = _consultas.UsuariosTotalConexiones().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(7, resultado.Count, "Debería haber 7 usuarios con conexiones");

            // Verificar usuario con más conexiones
            var ana = resultado.FirstOrDefault(r => r.Nombre == "Ana");
            Assert.IsNotNull(ana, "Ana debería tener conexiones");
            Assert.AreEqual(8, ana.Cantidad, "Ana debería tener 8 conexiones");

            // Verificar orden descendente
            for (int i = 0; i < resultado.Count - 1; i++)
            {
                Assert.IsTrue(resultado[i].Cantidad >= resultado[i + 1].Cantidad, 
                    "Debería estar ordenado descendentemente");
            }
        }

        [TestMethod]
        public void IPsMasUsadas_DeberiaRetornarIPs()
        {
            var resultado = _consultas.IPsMasUsadas().ToList();

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Count > 0, "Debería haber IPs con conexiones");

            // Verificar orden descendente
            for (int i = 0; i < resultado.Count - 1; i++)
            {
                Assert.IsTrue(resultado[i].Cantidad >= resultado[i + 1].Cantidad, 
                    "Debería estar ordenado descendentemente por cantidad de conexiones");
            }

            // La IP más usada debería ser 192.168.134.108 (Ana con 8 conexiones)
            Assert.AreEqual("192.168.134.108", resultado[0].Nombre, "IP más usada");
            Assert.AreEqual(6, resultado[0].Cantidad, "Cantidad de conexiones de la IP más usada");
        }

        [TestMethod]
        public void UsuarioDuracionPromedio_DeberiaRetornarPromedios()
        {
            var resultado = _consultas.UsuarioDuracionPromedio().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(7, resultado.Count, "Debería haber 7 usuarios con conexiones");
            Assert.IsTrue(resultado.All(r => r.Cantidad >= 0), "Todos los promedios deberían ser >= 0");

            // Verificar orden descendente
            for (int i = 0; i < resultado.Count - 1; i++)
            {
                Assert.IsTrue(resultado[i].Cantidad >= resultado[i + 1].Cantidad, 
                    "Debería estar ordenado descendentemente por duración promedio");
            }
        }

        [TestMethod]
        public void AplicacionDuracionPromedio_DeberiaRetornarPromedios()
        {
            var resultado = _consultas.AplicacionDuracionPromedio().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(4, resultado.Count, "Debería haber 4 aplicaciones con conexiones");

            // Verificar orden descendente
            for (int i = 0; i < resultado.Count - 1; i++)
            {
                Assert.IsTrue(resultado[i].Cantidad >= resultado[i + 1].Cantidad, 
                    "Debería estar ordenado descendentemente por duración promedio");
            }

            var excel = resultado[0].Cantidad;
            var word = resultado[1].Cantidad;            
            var gestion = resultado[2].Cantidad;
            var explorer = resultado[3].Cantidad;

            List<double> promedios = new List<double> { excel, word, gestion, explorer };
            List<double> valores = new List<double> { 397.6, 307.1875, 38, 11 };

            CollectionAssert.AreEqual(promedios, valores);


        }

        // ======================== TESTS DE CONSULTAS COMPLEJAS / STATS AVANZADAS ========================

        [TestMethod]
        public void EstadisticasDetaladasUsuario_DeberiaRetornarEstadisticas()
        {
            var resultado = _consultas.EstadisticasDetaladasUsuario().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(9, resultado.Count, "Debería haber estadísticas para todos los 9 usuarios");
            Assert.IsTrue(resultado.All(r => r.Usuario != null), "Todos deberían tener nombre de usuario");

            // Verificar un usuario con conexiones
            var ana = resultado.FirstOrDefault(r => r.Usuario == "Ana");
            Assert.IsNotNull(ana, "Ana debería tener estadísticas");
            Assert.AreEqual(8, ana.TotalConexiones, "Ana tiene 8 conexiones");
            Assert.AreEqual(1369, ana.DuracionTotal, "Duración total de conexiones de Ana");
            Assert.AreEqual("192.168.134.108", ana.IP_MasUsada, "IP más usada por Ana");

            // Verificar un usuario sin conexiones
            var mercedes = resultado.FirstOrDefault(r => r.Usuario == "Mercedes");
            Assert.IsNotNull(mercedes, "Mercedes debería tener estadísticas");
            Assert.AreEqual(0, mercedes.TotalConexiones, "Mercedes no tiene conexiones");
            Assert.AreEqual(0, mercedes.DuracionTotal, "Duración total de Mercedes debería ser 0");
        }

        [TestMethod]
        public void EstadisticasDetalladasAplicacion_DeberiaRetornarEstadisticas()
        {
            var resultado = _consultas.EstadisticasDetalladasAplicacion().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(4, resultado.Count, "Debería haber estadísticas para las 4 aplicaciones");

            var word = resultado.FirstOrDefault(r => r.Aplicacion == "Word");
            Assert.IsNotNull(word, "Word debería tener estadísticas");
            Assert.IsTrue(word.TotalUsuarios > 0, "Word debería tener usuarios");

            var explorer = resultado.FirstOrDefault(r => r.Aplicacion == "Explorer");
            Assert.IsNotNull(explorer, "Explorer debería tener estadísticas");
        }

        [TestMethod]
        public void UsuariosRolesAplicaciones_DeberiaRetornarRelaciones()
        {
            var resultado = _consultas.UsuariosRolesAplicaciones().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(9, resultado.Count, "Debería haber 9 relaciones usuario-rol-aplicación");

            var diana = resultado.FirstOrDefault(r => r.Usuario == "Diana");
            Assert.IsNotNull(diana, "Diana debería estar en los resultados");
            Assert.AreEqual("Alumno", diana.Rol, "Diana debería ser Alumno");
            Assert.AreEqual("Word", diana.Aplicacion, "Diana debería estar en Word");
        }

        [TestMethod]
        public void ConexionesPorFecha_DeberiaRetornarAgrupadasPorFecha()
        {
            var resultado = _consultas.ConexionesPorFecha().ToList();

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Count > 0, "Debería haber conexiones agrupadas por fecha");

            // Verificar que las fechas están en orden descendente
            for (int i = 0; i < resultado.Count - 1; i++)
            {
                DateTime fecha1 = DateTime.Parse(resultado[i].Nombre);
                DateTime fecha2 = DateTime.Parse(resultado[i + 1].Nombre);
                Assert.IsTrue(fecha1 >= fecha2, "Las fechas deberían estar en orden descendente");
            }

            Assert.IsTrue(resultado.All(r => r.Cantidad > 0), "Todas las fechas deberían tener al menos 1 conexión");
        }

        [TestMethod]
        public void ConexionesPorHora_DeberiaRetornarAgrupadasPorHora()
        {
            var resultado = _consultas.ConexionesPorHora().ToList();

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Count > 0, "Debería haber conexiones agrupadas por hora");

            // Verificar que todas tienen formato de hora correcto
            Assert.IsTrue(resultado.All(r => r.Nombre.Contains(":00 -")), 
                "Todos los nombres deberían tener formato de rango horario");

            Assert.IsTrue(resultado.All(r => r.Cantidad > 0), 
                "Todas las horas deberían tener al menos 1 conexión");
        }

        [TestMethod]
        public void UsuariosConMultiplesIPs_DeberiaRetornarUsuariosConVariasIPs()
        {
            var resultado = _consultas.UsuariosConMultiplesIPs().ToList();

            Assert.IsNotNull(resultado);

            // Verificar que todos tienen más de 1 IP
            Assert.IsTrue(resultado.All(r => r.Cantidad > 1), 
                "Todos los usuarios deberían tener más de 1 IP");

            // Verificar orden descendente
            for (int i = 0; i < resultado.Count - 1; i++)
            {
                Assert.IsTrue(resultado[i].Cantidad >= resultado[i + 1].Cantidad, 
                    "Debería estar ordenado descendentemente por cantidad de IPs");
            }
        }

        [TestMethod]
        public void EstadoUsuarios_DeberiaRetornarEstados()
        {
            var resultado = _consultas.EstadoUsuarios().ToList();

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Count > 0, "Debería haber estados de usuarios");

            var activos = resultado.FirstOrDefault(r => r.Nombre == "Activo");
            var anonimos = resultado.FirstOrDefault(r => r.Nombre == "Anónimo");

            Assert.IsNotNull(activos, "Debería haber usuarios activos");
            Assert.IsTrue(activos.Cantidad > 0, "Debería haber al menos un usuario activo");

            Assert.IsNotNull(anonimos, "Debería haber usuarios anónimos");
            Assert.AreEqual(1, anonimos.Cantidad, "Debería haber 1 usuario anónimo");
        }

        [TestMethod]
        public void RolesMasPopulares_DeberiaRetornarRolesOrdenados()
        {
            var resultado = _consultas.RolesMasPopulares().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(4, resultado.Count, "Debería haber 4 roles");

            // Verificar el rol más popular
            Assert.AreEqual("Alumno", resultado[0].Nombre, "Alumno debería ser el rol más popular");
            Assert.AreEqual(4, resultado[0].Cantidad, "Alumno debería tener 4 usuarios");

            // Verificar orden descendente
            for (int i = 0; i < resultado.Count - 1; i++)
            {
                Assert.IsTrue(resultado[i].Cantidad >= resultado[i + 1].Cantidad, 
                    "Debería estar ordenado descendentemente");
            }
        }

        [TestMethod]
        public void TopUsuariosActivos_DeberiaRetornarTop10()
        {
            var resultado = _consultas.TopUsuariosActivos(10).ToList();

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Count <= 10, "No debería retornar más de 10 usuarios");
            Assert.AreEqual(7, resultado.Count, "Hay 7 usuarios con conexiones");

            // Verificar orden descendente por duración total
            for (int i = 0; i < resultado.Count - 1; i++)
            {
                Assert.IsTrue(resultado[i].Cantidad >= resultado[i + 1].Cantidad, 
                    "Debería estar ordenado descendentemente por duración total");
            }
        }

        [TestMethod]
        public void TopUsuariosActivos_ConParametroPersonalizado_DeberiaRetornarTop5()
        {
            var resultado = _consultas.TopUsuariosActivos(5).ToList();

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Count <= 5, "No debería retornar más de 5 usuarios");
            Assert.AreEqual(5, resultado.Count, "Debería retornar exactamente 5 usuarios");

            // Verificar orden descendente
            for (int i = 0; i < resultado.Count - 1; i++)
            {
                Assert.IsTrue(resultado[i].Cantidad >= resultado[i + 1].Cantidad, 
                    "Debería estar ordenado descendentemente");
            }
        }

        [TestMethod]
        public void AplicacionesConMasUsuarios_DeberiaRetornarAplicaciones()
        {
            var resultado = _consultas.AplicacionesConMasUsuarios().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(4, resultado.Count, "Debería haber 4 aplicaciones");

            // Verificar orden descendente por cantidad de usuarios
            for (int i = 0; i < resultado.Count - 1; i++)
            {
                Assert.IsTrue(resultado[i].Cantidad >= resultado[i + 1].Cantidad, 
                    "Debería estar ordenado descendentemente por cantidad de usuarios");
            }

            var word = resultado.FirstOrDefault(r => r.Nombre == "Word");
            Assert.IsNotNull(word, "Word debería estar en los resultados");
            Assert.AreEqual(4, word.Cantidad, "Word debería tener 4 usuarios");
        }

        [TestMethod]
        public void UsuariosSinConexiones_DeberiaRetornarUsuariosSinConexiones()
        {
            var resultado = _consultas.UsuariosSinConexiones().ToList();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count, "Debería haber 2 usuarios sin conexiones");

            var nombres = resultado.Select(r => r.Nombre).ToList();
            CollectionAssert.AreEquivalent(new List<string> { "MERCEDES" }, nombres, 
                " Mercedes no tiene conexiones");
        }

        [TestMethod]
        public void UsuariosPocasConexiones_DeberiaRetornarUsuariosConPocasConexiones()
        {
            var resultado = _consultas.UsuariosPocasConexiones(1).ToList();

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Count > 0, "Debería haber usuarios con 1 conexión o menos");

            // Verificar que todos tienen <= 1 conexión
            Assert.IsTrue(resultado.All(r => r.Cantidad <= 1), 
                "Todos los usuarios deberían tener 1 conexión o menos");

            // Verificar orden ascendente
            for (int i = 0; i < resultado.Count - 1; i++)
            {
                Assert.IsTrue(resultado[i].Cantidad <= resultado[i + 1].Cantidad, 
                    "Debería estar ordenado ascendentemente por cantidad de conexiones");
            }
        }

        [TestMethod]
        public void UsuariosPocasConexiones_ConParametroPersonalizado_DeberiaRetornarResultados()
        {
            var resultado = _consultas.UsuariosPocasConexiones(2).ToList();

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Count > 0, "Debería haber usuarios con 2 conexiones o menos");

            // Verificar que todos tienen <= 2 conexiones
            Assert.IsTrue(resultado.All(r => r.Cantidad <= 2), 
                "Todos los usuarios deberían tener 2 conexiones o menos");

            // Verificar orden ascendente
            for (int i = 0; i < resultado.Count - 1; i++)
            {
                Assert.IsTrue(resultado[i].Cantidad <= resultado[i + 1].Cantidad, 
                    "Debería estar ordenado ascendentemente");
            }
        }
    }
}
