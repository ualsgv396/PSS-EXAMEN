using Microsoft.Testing.Platform.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSS.amg538.Practica_02;
using static System.Net.Mime.MediaTypeNames;

namespace Conecta4Test
{
    [TestClass]
    public class Conecta4Test
    {
        [TestMethod]
        public void TestCrearFichaNoEsNula()
        {
            IFicha ficha = Conecta4Factory.CrearFicha("Roja");
            Assert.IsNotNull(ficha);
        }

        [TestMethod]
        public void TestCrearFichaColorCorrecto()
        {
            IFicha ficha = Conecta4Factory.CrearFicha("Amarilla");
            Assert.AreEqual("Amarilla", ficha.Color);
        }

        [TestMethod]
        public void TestFichaMismoColorEquivalentes()
        {
            IFicha ficha1 = Conecta4Factory.CrearFicha("Roja");
            IFicha ficha2 = Conecta4Factory.CrearFicha("Roja");
            Assert.AreEqual(ficha1.Color, ficha2.Color);
        }

        [TestMethod]
        public void TestTableroPorDefecto()
        {
            ITablero t = Conecta4Factory.CrearTablero();
            Assert.AreEqual(6, t.Filas);
            Assert.AreEqual(7, t.Columnas);
        }

        [TestMethod]
        public void TestTableroPersonalizado()
        {
            ITablero t = Conecta4Factory.CrearTablero(9, 9);
            Assert.AreEqual(9, t.Filas);
            Assert.AreEqual(9, t.Columnas);
        }

        [TestMethod]
        public void TestTableroInicialVacio()
        {
            ITablero t = Conecta4Factory.CrearTablero();
            for (int i = 0; i < t.Filas; i++)
            {
                for (int j = 0; j < t.Columnas; j++)
                {
                    Assert.IsNull(t[i, j]);
                }
            }
        }

        [TestMethod]
        public void TestTableroPequeno()
        {
            Assert.ThrowsExactly<TableroTamanoException>(
                () => Conecta4Factory.CrearTablero(2, 2)
            );
        }

        [TestMethod]
        public void TestJugadorNombreDefecto()
        {
            IJugador jugador = Conecta4Factory.CrearJugador(ficha: Conecta4Factory.CrearFicha("Rojo"));
            Assert.AreEqual("Jugador1", jugador.Nombre);
        }

        [TestMethod]
        public void TestJugadorNombrePersonalizado()
        {
            IJugador jugador = Conecta4Factory.CrearJugador("Ana", Conecta4Factory.CrearFicha("Amarillo"));
            Assert.AreEqual("Ana", jugador.Nombre);
        }

        [TestMethod]
        public void TestColocarFichaColumnaVaciaCaeFilaInferior()
        {
            ITablero t = Conecta4Factory.CrearTablero();
            IFicha ficha = Conecta4Factory.CrearFicha("Roja");
            IJugador jugador = Conecta4Factory.CrearJugador("Adrian", ficha);

            bool colocada = jugador.ColocarFichaColumna(t, 3, out Posicion pos);

            Assert.IsTrue(colocada);
            Assert.AreEqual(5, pos.Fila);
            Assert.AreEqual(3, pos.Columna);
            Assert.IsNotNull(t[5, 3]);
            Assert.AreEqual("Roja", t[5, 3].Color);
        }

        [TestMethod]
        public void TestColocarSegundaFichaColumna()
        {
            ITablero t = Conecta4Factory.CrearTablero();
            IFicha ficha = Conecta4Factory.CrearFicha("Roja");
            IJugador jugador = Conecta4Factory.CrearJugador("Adrian", ficha);

            jugador.ColocarFichaColumna(t, 3, out Posicion pos1);
            bool colocada = jugador.ColocarFichaColumna(t, 3, out Posicion pos2);

            Assert.IsTrue(colocada);
            Assert.AreEqual(4, pos2.Fila);
            Assert.AreEqual(3, pos2.Columna);
        }

        [TestMethod]
        public void TestColocarFichaColumnaLlena()
        {
            ITablero t = Conecta4Factory.CrearTablero();
            IFicha ficha = Conecta4Factory.CrearFicha("Roja");
            IJugador jugador = Conecta4Factory.CrearJugador("Adrian", ficha);

            for (int i = 0; i < t.Filas; i++)
            {
                jugador.ColocarFichaColumna(t, 3, out _);
            }
            bool colocada = jugador.ColocarFichaColumna(t, 3, out Posicion pos);
            Assert.IsFalse(colocada);
        }

        [TestMethod]
        public void TestColocarFichaColumnaInvalida()
        {
            ITablero t = Conecta4Factory.CrearTablero();
            IFicha ficha = Conecta4Factory.CrearFicha("Roja");
            IJugador jugador = Conecta4Factory.CrearJugador("Adrian", ficha);

            Assert.ThrowsExactly<ColumnaFueraRangoException>(
                () => jugador.ColocarFichaColumna(t, -1, out _)
            );
            Assert.ThrowsExactly<ColumnaFueraRangoException>(
                () => jugador.ColocarFichaColumna(t, t.Columnas, out _)
            );
        }

        [TestMethod]
        public void TestQuitarFicha()
        {
            ITablero t = Conecta4Factory.CrearTablero();
            IFicha ficha = Conecta4Factory.CrearFicha("Roja");
            IJugador jugador = Conecta4Factory.CrearJugador("Adrian", ficha);

            jugador.ColocarFichaColumna(t, 3, out Posicion pos);
            bool quitada = jugador.QuitaFichaPosicion(t, pos);

            Assert.IsTrue(quitada);
            Assert.IsNull(t[pos.Fila, pos.Columna]);
        }

        [TestMethod]
        public void TestCuatroEnHorizontalVictoria()
        {
            ITablero t = Conecta4Factory.CrearTablero();
            IFicha ficha = Conecta4Factory.CrearFicha("Roja");
            IJugador jugador = Conecta4Factory.CrearJugador("Adrian", ficha);
            ICriterioVictoria criterio = Conecta4Factory.CrearCriterioFilasColumnasDiagonales(4);

            jugador.ColocarFichaColumna(t, 0, out _);
            jugador.ColocarFichaColumna(t, 1, out _);
            jugador.ColocarFichaColumna(t, 2, out _);
            jugador.ColocarFichaColumna(t, 3, out _);

            Assert.IsTrue(criterio.hayVictoria(t, ficha));
        }

        [TestMethod]
        public void TestCuatroEnVerticalVictoria()
        {
            ITablero t = Conecta4Factory.CrearTablero();
            IFicha ficha = Conecta4Factory.CrearFicha("Roja");
            IJugador jugador = Conecta4Factory.CrearJugador("Adrian", ficha);
            ICriterioVictoria criterio = Conecta4Factory.CrearCriterioFilasColumnasDiagonales(4);

            jugador.ColocarFichaColumna(t, 0, out _);
            jugador.ColocarFichaColumna(t, 0, out _);
            jugador.ColocarFichaColumna(t, 0, out _);
            jugador.ColocarFichaColumna(t, 0, out _);

            Assert.IsTrue(criterio.hayVictoria(t, ficha));
        }

        [TestMethod]
        public void TestCuatroEnDiagonalAscendenteVictoria()
        {
            ITablero t = Conecta4Factory.CrearTablero();
            IFicha ficha = Conecta4Factory.CrearFicha("Roja");
            IFicha ficha2 = Conecta4Factory.CrearFicha("Azul");
            IJugador jugador = Conecta4Factory.CrearJugador("Adrian", ficha);
            IJugador jugador2 = Conecta4Factory.CrearJugador("Maria", ficha2);

            ICriterioVictoria criterio = Conecta4Factory.CrearCriterioFilasColumnasDiagonales(4);

            jugador.ColocarFichaColumna(t, 0, out _);
            jugador2.ColocarFichaColumna(t, 1, out _);
            jugador.ColocarFichaColumna(t, 1, out _);
            jugador2.ColocarFichaColumna(t, 2, out _);
            jugador2.ColocarFichaColumna(t, 2, out _);
            jugador.ColocarFichaColumna(t, 2, out _);
            jugador2.ColocarFichaColumna(t, 3, out _);
            jugador2.ColocarFichaColumna(t, 3, out _);
            jugador2.ColocarFichaColumna(t, 3, out _);
            jugador.ColocarFichaColumna(t, 3, out _);

            /*
             *       o
             *     o v
             *   o v v
             * o v v v
             * 
             */


            Assert.IsTrue(criterio.hayVictoria(t, ficha));
            Assert.IsFalse(criterio.hayVictoria(t, ficha2));
        }

        [TestMethod]
        public void TestCuatroEnDiagonalDescendenteVictoria()
        {
            ITablero t = Conecta4Factory.CrearTablero();
            IFicha ficha = Conecta4Factory.CrearFicha("Roja");
            IFicha ficha2 = Conecta4Factory.CrearFicha("Azul");
            IJugador jugador = Conecta4Factory.CrearJugador("Adrian", ficha);
            IJugador jugador2 = Conecta4Factory.CrearJugador("Maria", ficha2);

            ICriterioVictoria criterio = Conecta4Factory.CrearCriterioFilasColumnasDiagonales(4);

            jugador2.ColocarFichaColumna(t, 0, out _);
            jugador2.ColocarFichaColumna(t, 0, out _);
            jugador2.ColocarFichaColumna(t, 0, out _);
            jugador.ColocarFichaColumna(t, 0, out _);
            jugador2.ColocarFichaColumna(t, 1, out _);
            jugador2.ColocarFichaColumna(t, 1, out _);
            jugador.ColocarFichaColumna(t, 1, out _);
            jugador2.ColocarFichaColumna(t, 2, out _);
            jugador.ColocarFichaColumna(t, 2, out _);
            jugador.ColocarFichaColumna(t, 3, out _);


            /*
             * o
             * v o
             * v v o
             * v v v o
             * 
             */


            Assert.IsTrue(criterio.hayVictoria(t, ficha));
            Assert.IsFalse(criterio.hayVictoria(t, ficha2));
        }

        [TestMethod]
        public void Test3EnLineaNoVictoria()
        {
            ITablero t = Conecta4Factory.CrearTablero();
            IFicha ficha = Conecta4Factory.CrearFicha("Roja");
            IJugador jugador = Conecta4Factory.CrearJugador("Adrian", ficha);
            ICriterioVictoria criterio = Conecta4Factory.CrearCriterioFilasColumnasDiagonales(4);

            jugador.ColocarFichaColumna(t, 0, out _);
            jugador.ColocarFichaColumna(t, 1, out _);
            jugador.ColocarFichaColumna(t, 2, out _);

            Assert.IsFalse(criterio.hayVictoria(t, ficha));
        }

        [TestMethod]
        public void TestVictoriaSoloHorizontalYVertical()
        {
            ITablero t = Conecta4Factory.CrearTablero();
            IFicha ficha = Conecta4Factory.CrearFicha("Roja");
            IFicha ficha2 = Conecta4Factory.CrearFicha("Azul");
            IJugador jugador = Conecta4Factory.CrearJugador("Adrian", ficha);
            IJugador jugador2 = Conecta4Factory.CrearJugador("Maria", ficha2);

            ICriterioVictoria criterio = Conecta4Factory.CrearCriterioFilasColumnas(4);

            jugador2.ColocarFichaColumna(t, 0, out _);
            jugador2.ColocarFichaColumna(t, 0, out _);
            jugador2.ColocarFichaColumna(t, 0, out _);
            jugador.ColocarFichaColumna(t, 0, out _);
            jugador2.ColocarFichaColumna(t, 1, out _);
            jugador2.ColocarFichaColumna(t, 1, out _);
            jugador.ColocarFichaColumna(t, 1, out _);
            jugador2.ColocarFichaColumna(t, 2, out _);
            jugador.ColocarFichaColumna(t, 2, out _);
            jugador.ColocarFichaColumna(t, 3, out _);


            /*
             * o
             * v o
             * v v o
             * v v v o
             * 
             */


            Assert.IsFalse(criterio.hayVictoria(t, ficha));
        }

        [TestMethod]
        public void TestVictoriaEnCruz()
        {
            ITablero t = Conecta4Factory.CrearTablero();
            IFicha ficha = Conecta4Factory.CrearFicha("Roja");
            IFicha ficha2 = Conecta4Factory.CrearFicha("Azul");
            IJugador jugador = Conecta4Factory.CrearJugador("Adrian", ficha);
            IJugador jugador2 = Conecta4Factory.CrearJugador("Maria", ficha2);

            ICriterioVictoria criterio = Conecta4Factory.CrearCriterioCruz(3);

            jugador2.ColocarFichaColumna(t, 0, out _);
            jugador.ColocarFichaColumna(t, 0, out _);
            jugador.ColocarFichaColumna(t, 1, out _);
            jugador.ColocarFichaColumna(t, 1, out _);
            jugador.ColocarFichaColumna(t, 1, out _);
            jugador2.ColocarFichaColumna(t, 2, out _);
            jugador.ColocarFichaColumna(t, 2, out _);


            /*
             *   o
             * o o o
             * v o v
             * 
             */


            Assert.IsTrue(criterio.hayVictoria(t, ficha));
            Assert.IsFalse(criterio.hayVictoria(t, ficha2));
        }

        [TestMethod]
        public void TestCrearJuegoTiene2JugadoresPorDefecto()
        {
            IJuego juego = Conecta4Factory.CrearJuego();
            Assert.HasCount(2, juego.Jugadores);
        }

        [TestMethod]
        public void TestCrearJuego3Jugadores()
        {
            IJuego juego = Conecta4Factory.CrearJuego(numJugadores: 3);
            Assert.HasCount(3, juego.Jugadores);
        }

        [TestMethod]
        public void TestTurnoInicialJugador1()
        {
            IJuego juego = Conecta4Factory.CrearJuego();
            Assert.AreSame(juego.Jugadores[0], juego.JugadorActual);
        }

        [TestMethod]
        public void TestCambiaTurnoTrasMovimientoValido()
        {
            IJuego juego = Conecta4Factory.CrearJuego();
            var primero = juego.JugadorActual;
            bool valido = juego.JugarColumna(0);

            Assert.IsTrue(valido);
            Assert.AreNotSame(primero, juego.JugadorActual);
        }

        [TestMethod]
        public void TestObtenerGanadorFalse()
        {
            IJuego juego = Conecta4Factory.CrearJuego();
            Assert.IsFalse(juego.ObtenerGanador(juego.Jugadores[0]));
            Assert.IsFalse(juego.ObtenerGanador(juego.Jugadores[1]));
        }

        [TestMethod]
        public void TestObtenerGanadorTrue()
        {
            IJuego juego = Conecta4Factory.CrearJuego();
            var jug1 = juego.Jugadores[0];
            var jug2 = juego.Jugadores[1];

            juego.JugarColumna(0); // jug1
            juego.JugarColumna(0); // jug2
            juego.JugarColumna(1); // jug1
            juego.JugarColumna(1); // jug2
            juego.JugarColumna(2); // jug1
            juego.JugarColumna(2); // jug2
            juego.JugarColumna(3); // jug1  → 4 en línea fila inferior

            Assert.IsTrue(juego.ObtenerGanador(jug1));
            Assert.IsFalse(juego.ObtenerGanador(jug2));
        }

        [TestMethod]
        public void TestFinJuego_TableroVacio_EsFalse()
        {
            IJuego juego = Conecta4Factory.CrearJuego();
            Assert.IsFalse(juego.FinJuego());
        }

        [TestMethod]
        public void TestFinJuego_TrasVictoria_EsTrue()
        {
            IJuego juego = Conecta4Factory.CrearJuego();

            juego.JugarColumna(0);
            juego.JugarColumna(0);
            juego.JugarColumna(1);
            juego.JugarColumna(1);
            juego.JugarColumna(2);
            juego.JugarColumna(2);
            juego.JugarColumna(3); // 4 en línea de jug1

            Assert.IsTrue(juego.FinJuego());
        }

        [TestMethod]
        public void TestFinJuego_PartidaAMedias_EsFalse()
        {
            IJuego juego = Conecta4Factory.CrearJuego();

            juego.JugarColumna(3);
            juego.JugarColumna(2);
            juego.JugarColumna(4);

            Assert.IsFalse(juego.FinJuego());
        }

        [TestMethod]
        public void TestFinJuego_TableroLlenoSinGanador_EsTrue()
        {
            // 4x4 con "5 en línea" para ganar → imposible que alguien gane.
            IJuego juego = Conecta4Factory.CrearJuego(
                filas: 4, columnas: 4, fichasParaGanar: 5);

            for (int c = 0; c < 4; c++)
                for (int i = 0; i < 4; i++)
                    juego.JugarColumna(c);

            Assert.IsFalse(juego.ObtenerGanador(juego.Jugadores[0]));
            Assert.IsFalse(juego.ObtenerGanador(juego.Jugadores[1]));
            Assert.IsTrue(juego.FinJuego());
        }

        [TestMethod]
        public void TestConfiguracionPorDefecto()
        {
            IConfiguracion cfg = Conecta4Factory.CrearConfiguracionPorDefecto();
            Assert.AreEqual(6, cfg.Filas);
            Assert.AreEqual(7, cfg.Columnas);
            Assert.AreEqual(2, cfg.NumJugadores);
            Assert.AreEqual(4, cfg.FichasParaGanar);
            Assert.AreEqual("es", cfg.Idioma);
            Assert.AreEqual("FilasColumnasDiagonales", cfg.Criterio);
        }

        [TestMethod]
        public void TestConfiguracionDesdeJson()
        {
            IConfiguracion cfg = Conecta4Factory.CargarConfiguracion("config_test.json");

            Assert.AreEqual(9, cfg.Filas);
            Assert.AreEqual(9, cfg.Columnas);
            Assert.AreEqual(3, cfg.NumJugadores);
            Assert.AreEqual(5, cfg.FichasParaGanar);
            Assert.AreEqual("en", cfg.Idioma);
            Assert.AreEqual("FilasColumnas", cfg.Criterio);
            CollectionAssert.Contains(cfg.TiposJugador.ToList(), "IA-Minimax");
        }

        [TestMethod]
        public void TestCargarConfiguracionFicheroInexistente()
        {
            Assert.ThrowsExactly<ConfiguracionInvalidaException>(() => Conecta4Factory.CargarConfiguracion("no_existe.json"));
        }

        [TestMethod]
        public void TestCargarConfiguracionTableroDemasiadoPequeno()
        {
            Assert.ThrowsExactly<ConfiguracionInvalidaException>(() => Conecta4Factory.CargarConfiguracion("cfg_pequena.json"));
        }

        [TestMethod]
        public void TestCargarConfiguracionJsonMalFormado()
        {
            Assert.ThrowsExactly<ConfiguracionInvalidaException>(() => Conecta4Factory.CargarConfiguracion("cfg_rota.json"));
        }

        [TestMethod]
        public void CrearJuegoDesdeConfiguracion()
        {
            IConfiguracion cfg = Conecta4Factory.CargarConfiguracion("config_test.json");
            IJuego juego = Conecta4Factory.CrearJuego(cfg);

            Assert.AreEqual(9, juego.Tablero.Filas);
            Assert.AreEqual(9, juego.Tablero.Columnas);
            Assert.HasCount(3, juego.Jugadores);
            Assert.AreEqual(5, juego.CriterioVictoria.FichasEnLinea);
        }

        [TestMethod]
        public void TestEstrategiaAleatoria()
        {
            IJuego juego = Conecta4Factory.CrearJuego();
            IEstrategia est = Conecta4Factory.CrearEstrategiaAleatoria(semilla: 42);

            int col = est.ElegirColumna(juego, juego.JugadorActual);

            Assert.IsTrue(col >= 0 && col < juego.Tablero.Columnas);
        }

        [TestMethod]
        public void TestEstrategiaAleatoriaNoDevuelveColumnaLlena()
        {
            IJuego juego = Conecta4Factory.CrearJuego();
            var jug = juego.Jugadores[0];

            for (int i = 0; i < juego.Tablero.Filas; i++)
                jug.ColocarFichaColumna(juego.Tablero, 0, out _);

            IEstrategia est = Conecta4Factory.CrearEstrategiaAleatoria(semilla: 42);

            for (int i = 0; i < 100; i++)
            {
                int col = est.ElegirColumna(juego, juego.JugadorActual);
                Assert.AreNotEqual(0, col, "La estrategia eligió una columna llena");
            }
        }

        [TestMethod]
        public void TestEstrategiaAleatoriaSemilla()
        {
            IJuego j1 = Conecta4Factory.CrearJuego();
            IJuego j2 = Conecta4Factory.CrearJuego();

            var est1 = Conecta4Factory.CrearEstrategiaAleatoria(semilla: 7);
            var est2 = Conecta4Factory.CrearEstrategiaAleatoria(semilla: 7);

            for (int i = 0; i < 10; i++)
            {
                int c1 = est1.ElegirColumna(j1, j1.JugadorActual);
                int c2 = est2.ElegirColumna(j2, j2.JugadorActual);
                Assert.AreEqual(c1, c2);
            }
        }

        [TestMethod]
        public void TestJugadorIA()
        {
            IFicha ficha = Conecta4Factory.CrearFicha("Verde");
            IEstrategia est = Conecta4Factory.CrearEstrategiaAleatoria(semilla: 1);
            IJugador ia = Conecta4Factory.CrearJugadorIA("HAL", ficha, est);

            Assert.AreEqual("HAL", ia.Nombre);
            Assert.AreEqual("Verde", ia.Ficha.Color);
        }

        [TestMethod]
        public void TestMinimax()
        {
            IJuego juego = Conecta4Factory.CrearJuego();
            IEstrategia est = Conecta4Factory.CrearEstrategiaMinimax(profundidad: 3);

            int col = est.ElegirColumna(juego, juego.JugadorActual);

            Assert.IsTrue(col >= 0 && col < juego.Tablero.Columnas);
            Assert.IsNull(juego.Tablero[0, col]);
        }

        [TestMethod]
        public void TestMinimaxBloqueaPerder()
        {
            IJuego juego = Conecta4Factory.CrearJuego();
            IJugador ai = juego.Jugadores[0];
            IJugador rival = juego.Jugadores[1];

            rival.ColocarFichaColumna(juego.Tablero, 0, out _);
            rival.ColocarFichaColumna(juego.Tablero, 1, out _);
            rival.ColocarFichaColumna(juego.Tablero, 3, out _);

            IEstrategia minimax = Conecta4Factory.CrearEstrategiaMinimax(profundidad: 3);
            int col = minimax.ElegirColumna(juego, ai);

            Assert.AreEqual(2, col);
        }

        [TestMethod]
        public void TestMinimaxGana()
        {
            IJuego juego = Conecta4Factory.CrearJuego();
            IJugador ai = juego.Jugadores[0];
            IJugador rival = juego.Jugadores[1];

            ai.ColocarFichaColumna(juego.Tablero, 0, out _);
            ai.ColocarFichaColumna(juego.Tablero, 1, out _);
            ai.ColocarFichaColumna(juego.Tablero, 3, out _);

            IEstrategia minimax = Conecta4Factory.CrearEstrategiaMinimax(profundidad: 3);
            int col = minimax.ElegirColumna(juego, ai);

            Assert.AreEqual(2, col);
        }

        [TestMethod]
        public void TestMinimaxProfundidad1()
        {
            IJuego juego = Conecta4Factory.CrearJuego();
            IEstrategia est = Conecta4Factory.CrearEstrategiaMinimax(profundidad: 1);

            int col = est.ElegirColumna(juego, juego.JugadorActual);

            Assert.IsTrue(col >= 0 && col < juego.Tablero.Columnas);
        }

        [TestMethod]

        public void Minimax_ProfundidadCero_Excepcion()
        {
            Assert.Throws<ArgumentException>(() => Conecta4Factory.CrearEstrategiaMinimax(profundidad: 0));
        }

        [TestMethod]
        public void CrearJuegoDesdeConfiguracion_RespetaTiposJugador()
        {
            IConfiguracion cfg = Conecta4Factory.CargarConfiguracion("config_test.json");
            IJuego juego = Conecta4Factory.CrearJuego(cfg);

            Assert.IsNotInstanceOfType(juego.Jugadores[0], typeof(JugadorIA));
            Assert.IsInstanceOfType(juego.Jugadores[1], typeof(JugadorIA));
            Assert.IsInstanceOfType(juego.Jugadores[2], typeof(JugadorIA));
        }

        [TestMethod]
        public void TestPedirNombreVacioUsaValorPorDefecto()
        {
            var entrada = new EntradaStub("");
            var salida = new SalidaStub();

            string nombre = MotorConsola.PedirNombre(entrada, salida, defecto: "Jugador1");

            Assert.AreEqual("Jugador1", nombre);
        }

        [TestMethod]
        public void TestPedirNombreExplicito()
        {
            var entrada = new EntradaStub("Ana");
            var salida = new SalidaStub();

            string nombre = MotorConsola.PedirNombre(entrada, salida, defecto: "Jugador1");

            Assert.AreEqual("Ana", nombre);
        }

        [TestMethod]
        public void TestPedirColumna()
        {
            var entrada = new EntradaStub("3");
            var salida = new SalidaStub();

            int col = MotorConsola.PedirColumna(entrada, salida, columnasTotales: 7);

            Assert.AreEqual(3, col);
        }

        [TestMethod]
        public void TestPedirColumnaEntradaNoNumerica()
        {
            var entrada = new EntradaStub("abc", "3");
            var salida = new SalidaStub();

            int col = MotorConsola.PedirColumna(entrada, salida, columnasTotales: 7);

            Assert.AreEqual(3, col);
            StringAssert.Contains(salida.Todo, "número");
        }

        [TestMethod]
        public void TestPedirColumnaFueraRango()
        {
            var entrada = new EntradaStub("99", "-1", "3");
            var salida = new SalidaStub();

            int col = MotorConsola.PedirColumna(entrada, salida, columnasTotales: 7);

            Assert.AreEqual(3, col);
            StringAssert.Contains(salida.Todo, "rango");
        }

        [TestMethod]
        public void PedirColumna_ColumnaLlena_PideDeNuevo()
        {
            IJuego juego = Conecta4Factory.CrearJuego();
            for (int i = 0; i < juego.Tablero.Filas; i++)
                juego.Jugadores[0].ColocarFichaColumna(juego.Tablero, 0, out _);

            var entrada = new EntradaStub("0", "3");
            var salida = new SalidaStub();

            int col = MotorConsola.PedirColumnaJugable(entrada, salida, juego.Tablero);

            Assert.AreEqual(3, col);
            StringAssert.Contains(salida.Todo, "llena");
        }

        [TestMethod]
        public void RenderTablero_VacioMuestraPuntos()
        {
            ITablero t = Conecta4Factory.CrearTablero(4, 4);
            string render = MotorConsola.RenderTablero(t);

            string esperado =
                ". . . ." + Environment.NewLine +
                ". . . ." + Environment.NewLine +
                ". . . ." + Environment.NewLine +
                ". . . ." + Environment.NewLine +
                "0 1 2 3" + Environment.NewLine;

            Assert.AreEqual(esperado, render);
        }

        [TestMethod]
        public void RenderTablero_MuestraFichasPorInicial()
        {
            ITablero t = Conecta4Factory.CrearTablero(4, 4);
            var jug = Conecta4Factory.CrearJugador("Ana", Conecta4Factory.CrearFicha("Rojo"));
            jug.ColocarFichaColumna(t, 1, out _);

            string render = MotorConsola.RenderTablero(t);

            StringAssert.Contains(render, "R");
        }

        [TestMethod]
        public void TestEjecutarCuandoHayGanadorAnunciaFinYTermina()
        {
            IJuego juego = Conecta4Factory.CrearJuego(); 

            var entrada = new EntradaStub(
                "0",  
                "0",  
                "1",  
                "1",  
                "2",
                "2",  
                "3"  
            );
            var salida = new SalidaStub();

            var motor = new MotorConsola(juego, entrada, salida);
            motor.Ejecutar();

            Assert.IsTrue(juego.ObtenerGanador(juego.Jugadores[0]));
            StringAssert.Contains(salida.Todo, "gana");
            StringAssert.Contains(salida.Todo, juego.Jugadores[0].Nombre);
        }

        [TestMethod]
        public void Bienvenida_RespetaIdiomaIngles()
        {
            IJuego juego = Conecta4Factory.CrearJuego();

            var entrada = new EntradaStub("0", "0", "1", "1", "2", "2", "3");
            var salida = new SalidaStub();

            var motor = new MotorConsola(juego, entrada, salida, idioma: "en");
            motor.Ejecutar();

            StringAssert.Contains(salida.Todo, "Welcome");
            StringAssert.Contains(salida.Todo, "wins");

            Assert.DoesNotContain("Bienvenido", salida.Todo);
            Assert.DoesNotContain("gana la partida", salida.Todo);
        }

        [TestMethod]
        public void Textos_IdiomaDesconocido_CaeAlEspanol()
        {
            string s = Textos.Get("klingon", "ganador", "Worf");
            StringAssert.Contains(s, "gana");
        }

        [TestMethod]
        public void Textos_ClaveDesconocida_DevuelveMarcador()
        {
            string s = Textos.Get("es", "clave_que_no_existe");
            StringAssert.Contains(s, "??");
        }

    }
}
