using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSS.amg538.Practica_02;
namespace TestColeccionUsuarioView
{
    [TestClass]
    public class TestColeccionUsuarioView
    {
        private List<UsuarioView> listaUsu;
        private UsuarioView usu1;
        private UsuarioView usu2;
        private UsuarioView usu3;
        private UsuarioView usu4;
        private UsuarioView usu5;
        private Dictionary<UsuarioView, string> diccionarioUsu;


        [TestInitialize]
        public void Inicializar()
        {
            usu1 = new UsuarioView(1, "Adrian", "PSS", "Practica", true);
            usu2 = new UsuarioView(2, "Alex", "PSS", "Practica", true);
            usu3 = new UsuarioView(3, "Adrian", "PSS", "Practica", true);
            usu4 = new UsuarioView(4, "Pedro", "InSo", "Trabajo", false);
            usu5 = new UsuarioView(1, "Pedro", "InSo", "Trabajo", false);
            listaUsu = new List<UsuarioView>();
            listaUsu.Add(usu1);
            listaUsu.Add(usu2);
            listaUsu.Add(usu3);
            listaUsu.Add(usu1);
            diccionarioUsu = new Dictionary<UsuarioView, string>();
        }

        [TestMethod]
        public void TestListContainsIsTrue()
        {
            Assert.IsTrue(listaUsu.Contains(usu1));
        }

        [TestMethod]
        public void TestListContainsIsFalse()
        {
            Assert.IsFalse(listaUsu.Contains(usu4));
        }

        [TestMethod]
        public void TestListIndexOfIsTrue()
        {
            Assert.IsTrue(listaUsu.IndexOf(usu1) == 0);
        }

        [TestMethod]
        public void TestListIndexOfIsFalse()
        {
            Assert.IsFalse(listaUsu.IndexOf(usu1) == 1);
        }

        [TestMethod]
        public void TestListLastIndexOfIsTrue()
        {
            Assert.IsTrue(listaUsu.LastIndexOf(usu1) == 3);
        }

        [TestMethod]
        public void TestListLastIndexOfIsFalse()
        {
            Assert.IsFalse(listaUsu.LastIndexOf(usu1) == 1);
        }
        
        [TestMethod]
        public void TestListRemoveIsTrue()
        {
            Assert.IsTrue(listaUsu.Remove(usu1));
        }

        [TestMethod]
        public void TestListRemoveIsFalse()
        {
            Assert.IsFalse(listaUsu.Remove(usu4));
        }

        [TestMethod]
        public void TestDiccionarioObtenerIdCorrespondiente()
        {
            diccionarioUsu.Add(usu1, usu1.Id);
            diccionarioUsu.Add(usu2, usu2.Id);
            diccionarioUsu.Add(usu3, usu3.Id);

            string idObtenido = diccionarioUsu[usu2];

            Assert.AreEqual(usu2.Id, idObtenido);
        }

        [TestMethod]
        public void TestDiccionarioNoPermiteDosUsuariosConMismoId()
        {
            diccionarioUsu.Add(usu1, usu1.Id);

            bool lanzaExcepcion = false;
            try
            {
                diccionarioUsu.Add(usu5, usu5.Id);
            }
            catch (System.ArgumentException)
            {
                lanzaExcepcion = true;
            }

            Assert.IsTrue(lanzaExcepcion);
            Assert.AreEqual(1, diccionarioUsu.Count);
        }

        [TestMethod]
        public void TestSortCorrecto()
        {
            listaUsu.Sort();
            Assert.AreEqual(usu1, listaUsu[0]);
            Assert.AreEqual(usu1, listaUsu[1]);
            Assert.AreEqual(usu2, listaUsu[2]);
            Assert.AreEqual(usu3, listaUsu[3]);
        }

        [TestMethod]
        public void TestSortIncorrecto()
        {
            listaUsu.Sort();
            Assert.AreNotEqual(usu2, listaUsu[0]);
            Assert.AreNotEqual(usu3, listaUsu[1]);
            Assert.AreNotEqual(usu1, listaUsu[2]);
            Assert.AreNotEqual(usu4, listaUsu[3]);
        }
    }
}
