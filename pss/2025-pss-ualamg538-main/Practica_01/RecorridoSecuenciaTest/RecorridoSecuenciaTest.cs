using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSS.amg538.Practica_02;

namespace RecorridoSecuenciaTest
{
    [TestClass]
    public sealed class RecorridoSecuenciaTest
    {
        private Secuencia<UsuarioView> secuencia;
        private UsuarioView usu1;
        private UsuarioView usu2;
        private UsuarioView usu3;
        private UsuarioView usu4;

        [TestInitialize]
        public void Inicializar()
        {
            usu1 = new UsuarioView(1, "Loli", "PSS", "Practica", true);
            usu2 = new UsuarioView(2, "Alex", "PSS", "Practica", true);
            usu3 = new UsuarioView(3, "Adrian", "PSS", "Practica", true);
            usu4 = new UsuarioView(4, "Pedro", "InSo", "Trabajo", false);
            secuencia = new Secuencia<UsuarioView>();
            secuencia.Añadir(usu1);
            secuencia.Añadir(usu2);
            secuencia.Añadir(usu3);
            secuencia.Añadir(usu4);
        }

        [TestMethod]
        public void TestRecorridoDefecto_EsOrdenInverso()
        {
            List<UsuarioView> esperado = new List<UsuarioView> { usu4, usu3, usu2, usu1 };
            List<UsuarioView> obtenido = new List<UsuarioView>();

            foreach (UsuarioView u in secuencia)
                obtenido.Add(u);

            CollectionAssert.AreEqual(esperado, obtenido);
        }

        [TestMethod]
        public void TestRecorridoDefecto_NoEsOrdenEntrada()
        {
            List<UsuarioView> ordenEntrada = new List<UsuarioView> { usu1, usu2, usu3, usu4 };
            List<UsuarioView> obtenido = new List<UsuarioView>();

            foreach (UsuarioView u in secuencia)
                obtenido.Add(u);

            CollectionAssert.AreNotEqual(ordenEntrada, obtenido);
        }

        [TestMethod]
        public void TestRecorridoDefecto_ContieneTosdosLosElementos()
        {
            List<UsuarioView> obtenido = new List<UsuarioView>();

            foreach (UsuarioView u in secuencia)
                obtenido.Add(u);

            CollectionAssert.AreEquivalent(
                new List<UsuarioView> { usu1, usu2, usu3, usu4 },
                obtenido);
        }

        [TestMethod]
        public void TestRecorridoAdelante_EsOrdenEntrada()
        {
            List<UsuarioView> esperado = new List<UsuarioView> { usu1, usu2, usu3, usu4 };
            List<UsuarioView> obtenido = new List<UsuarioView>();

            foreach (UsuarioView u in secuencia.RecorridoAdelante)
                obtenido.Add(u);

            CollectionAssert.AreEqual(esperado, obtenido);
        }

        [TestMethod]
        public void TestRecorridoAdelante_EsOpuestoAlDefecto()
        {
            List<UsuarioView> adelante = new List<UsuarioView>();
            List<UsuarioView> defecto = new List<UsuarioView>();

            foreach (UsuarioView u in secuencia.RecorridoAdelante)
                adelante.Add(u);

            foreach (UsuarioView u in secuencia)
                defecto.Add(u);

            defecto.Reverse();
            CollectionAssert.AreEqual(defecto, adelante);
        }

        [TestMethod]
        public void TestRecorridoAtras_EsOrdenInverso()
        {
            List<UsuarioView> esperado = new List<UsuarioView> { usu4, usu3, usu2, usu1 };
            List<UsuarioView> obtenido = new List<UsuarioView>();

            foreach (UsuarioView u in secuencia.RecorridoAtras)
                obtenido.Add(u);

            CollectionAssert.AreEqual(esperado, obtenido);
        }

        [TestMethod]
        public void TestRecorridoAtras_EsIgualAlDefecto()
        {
            List<UsuarioView> atras = new List<UsuarioView>();
            List<UsuarioView> defecto = new List<UsuarioView>();

            foreach (UsuarioView u in secuencia.RecorridoAtras)
                atras.Add(u);

            foreach (UsuarioView u in secuencia)
                defecto.Add(u);

            CollectionAssert.AreEqual(defecto, atras);
        }

        [TestMethod]
        public void TestRecorridoAscendente_PorId_OrdenCorrecto()
        {
            secuencia.Ordenar(new ComparadorPropiedad<UsuarioView>("Id"));

            List<UsuarioView> esperado = new List<UsuarioView> { usu1, usu2, usu3, usu4 };
            List<UsuarioView> obtenido = new List<UsuarioView>();

            foreach (UsuarioView u in secuencia.RecorridoAscendente)
                obtenido.Add(u);

            CollectionAssert.AreEqual(esperado, obtenido);
        }

        [TestMethod]
        public void TestRecorridoAscendente_PorNombre_OrdenCorrecto()
        {
            secuencia.Ordenar(new ComparadorPropiedad<UsuarioView>("Nombre"));

            List<UsuarioView> esperado = new List<UsuarioView> { usu3, usu2, usu1, usu4 };
            List<UsuarioView> obtenido = new List<UsuarioView>();

            foreach (UsuarioView u in secuencia.RecorridoAscendente)
                obtenido.Add(u);

            CollectionAssert.AreEqual(esperado, obtenido);
        }

        [TestMethod]
        public void TestRecorridoDescendente_PorId_OrdenCorrecto()
        {
            secuencia.Ordenar(new ComparadorPropiedad<UsuarioView>("Id"));

            List<UsuarioView> esperado = new List<UsuarioView> { usu4, usu3, usu2, usu1 };
            List<UsuarioView> obtenido = new List<UsuarioView>();

            foreach (UsuarioView u in secuencia.RecorridoDescendente)
                obtenido.Add(u);

            CollectionAssert.AreEqual(esperado, obtenido);
        }

        [TestMethod]
        public void TestRecorridoDescendente_PorNombre_OrdenCorrecto()
        {
            secuencia.Ordenar(new ComparadorPropiedad<UsuarioView>("Nombre"));

            List<UsuarioView> esperado = new List<UsuarioView> { usu4, usu1, usu2, usu3 };
            List<UsuarioView> obtenido = new List<UsuarioView>();

            foreach (UsuarioView u in secuencia.RecorridoDescendente)
                obtenido.Add(u);

            CollectionAssert.AreEqual(esperado, obtenido);
        }

        [TestMethod]
        public void TestAscendenteYDescendente_SonOpuestos()
        {
            secuencia.Ordenar(new ComparadorPropiedad<UsuarioView>("Id"));

            List<UsuarioView> ascendente = new List<UsuarioView>();
            List<UsuarioView> descendente = new List<UsuarioView>();

            foreach (UsuarioView u in secuencia.RecorridoAscendente)
                ascendente.Add(u);

            foreach (UsuarioView u in secuencia.RecorridoDescendente)
                descendente.Add(u);

            descendente.Reverse();
            CollectionAssert.AreEqual(ascendente, descendente);
        }

        [TestMethod]
        public void TestTodosLosRecorridos_MismosElementos()
        {
            secuencia.Ordenar(new ComparadorPropiedad<UsuarioView>("Id"));

            List<UsuarioView> referencia = new List<UsuarioView> { usu1, usu2, usu3, usu4 };
            List<UsuarioView> defecto = new List<UsuarioView>();
            List<UsuarioView> adelante = new List<UsuarioView>();
            List<UsuarioView> atras = new List<UsuarioView>();
            List<UsuarioView> ascendente = new List<UsuarioView>();
            List<UsuarioView> descendente = new List<UsuarioView>();

            foreach (UsuarioView u in secuencia) defecto.Add(u);
            foreach (UsuarioView u in secuencia.RecorridoAdelante) adelante.Add(u);
            foreach (UsuarioView u in secuencia.RecorridoAtras) atras.Add(u);
            foreach (UsuarioView u in secuencia.RecorridoAscendente) ascendente.Add(u);
            foreach (UsuarioView u in secuencia.RecorridoDescendente) descendente.Add(u);

            CollectionAssert.AreEquivalent(referencia, defecto);
            CollectionAssert.AreEquivalent(referencia, adelante);
            CollectionAssert.AreEquivalent(referencia, atras);
            CollectionAssert.AreEquivalent(referencia, ascendente);
            CollectionAssert.AreEquivalent(referencia, descendente);
        }
    }
}
