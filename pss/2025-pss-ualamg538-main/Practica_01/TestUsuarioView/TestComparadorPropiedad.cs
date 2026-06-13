using PSS.amg538.Practica_02;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUsuarioView
{
    [TestClass]
    public class TestComparadorPropiedad
    {
        private UsuarioView usu1;
        private UsuarioView usu2;

        [TestInitialize]
        public void TestInitialize()
        {
            usu1 = new UsuarioView(1, "Adrian", "PSS", "Practica", true);
            usu2 = new UsuarioView(2, "Alex", "PSS", "Practica", true);
        }

        [TestMethod]
        public void TestComparadorPropiedadIdFalse()
        {
            ComparadorPropiedad<UsuarioView> comp = new ComparadorPropiedad<UsuarioView>("Id");
            Assert.IsFalse(comp.Compare(usu1, usu2) == 0);
        }

        [TestMethod]
        public void TestComparadorPropiedadNombreFalse()
        {
            ComparadorPropiedad<UsuarioView> comp = new ComparadorPropiedad<UsuarioView>("Nombre");
            Assert.IsFalse(comp.Compare(usu1, usu2) == 0);
        }

        [TestMethod]
        public void TestComparadorPropiedadPalabraPasoFalse()
        {
            ComparadorPropiedad<UsuarioView> comp = new ComparadorPropiedad<UsuarioView>("PalabraPaso");
            Assert.IsFalse(comp.Compare(usu1, usu2) != 0);
        }

        [TestMethod]
        public void TestComparadorPropiedadCategoriaFalse()
        {
            ComparadorPropiedad<UsuarioView> comp = new ComparadorPropiedad<UsuarioView>("Categoria");
            Assert.IsFalse(comp.Compare(usu1, usu2) != 0);
        }

        [TestMethod]
        public void TestComparadorPropiedadEsValidoFalse()
        {
            ComparadorPropiedad<UsuarioView> comp = new ComparadorPropiedad<UsuarioView>("EsValido");
            Assert.IsFalse(comp.Compare(usu1, usu2) != 0);
        }

        [TestMethod]
        public void TestComparadorPropiedadIdTrue()
        {
            ComparadorPropiedad<UsuarioView> comp = new ComparadorPropiedad<UsuarioView>("Id");
            Assert.IsTrue(comp.Compare(usu1, usu2) < 0);
        }

        [TestMethod]
        public void TestComparadorPropiedadNombreTrue()
        {
            ComparadorPropiedad<UsuarioView> comp = new ComparadorPropiedad<UsuarioView>("Nombre");
            Assert.IsTrue(comp.Compare(usu1, usu2) < 0);
        }

        [TestMethod]
        public void TestComparadorPropiedadPalabraPasoTrue()
        {
            ComparadorPropiedad<UsuarioView> comp = new ComparadorPropiedad<UsuarioView>("PalabraPaso");
            Assert.IsTrue(comp.Compare(usu1, usu2) == 0);
        }

        [TestMethod]
        public void TestComparadorPropiedadCategoriaTrue()
        {
            ComparadorPropiedad<UsuarioView> comp = new ComparadorPropiedad<UsuarioView>("Categoria");
            Assert.IsTrue(comp.Compare(usu1, usu2) == 0);
        }

        [TestMethod]
        public void TestComparadorPropiedadEsValidoTrue()
        {
            ComparadorPropiedad<UsuarioView> comp = new ComparadorPropiedad<UsuarioView>("EsValido");
            Assert.IsTrue(comp.Compare(usu1, usu2) == 0);
        }
    }
}
