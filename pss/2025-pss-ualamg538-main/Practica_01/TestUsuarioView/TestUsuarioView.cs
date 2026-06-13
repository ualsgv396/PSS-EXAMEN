using PSS.amg538.Practica_02;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TestUsuarioView
{
    [TestClass]
    public class TestUsuarioView
    {
        private UsuarioView usu1;
        private UsuarioView usu2;
        private UsuarioView usu3;
        private UsuarioView usu4;
        private UsuarioView usu5;

        [TestInitialize]
        public void TestInitialize()
        {
            usu1 = new UsuarioView(1, "Adrian", "PSS", "Practica", true);
            usu2 = new UsuarioView(2, "Alex", "PSS", "Practica", true);
            usu3 = new UsuarioView(3, "Adrian", "PSS", "Practica", true);
            usu4 = new UsuarioView(1, "Pedro", "InSo", "Trabajo", false);
            usu5 = new UsuarioView(1, "Loli", "PSS", "Practica", true);
        }

        [TestMethod]
        public void TestEqualsIsFalseIdDifferent()
        {
            Assert.IsFalse(usu1.Equals(usu2));
        }

        [TestMethod]
        public void TestEqualsIsTrueIdSame()
        {
            Assert.IsTrue(usu1.Equals(usu1));
        }

        [TestMethod]
        public void TestEqualsIsFalseIdDifferentSameValues()
        {
            Assert.IsFalse(usu1.Equals(usu3));
        }

        [TestMethod]
        public void TestEqualsIsTrueIdSameDifferentValues()
        {
            Assert.IsTrue(usu1.Equals(usu4));
        }

        [TestMethod]
        public void TestOperadorIsFalseIdDifferent()
        {
            Assert.IsTrue(usu1 != usu2);
        }

        [TestMethod]
        public void TestOperadorIsTrueIdSame()
        {
            Assert.IsTrue(usu1 == usu1);
            Assert.IsTrue(usu1 == usu4);
            Assert.IsTrue(usu4 == usu1);
        }

        [TestMethod]
        public void TestOperadorIsFalseIdDifferentSameValues()
        {
            Assert.IsTrue(usu1 != usu3);
        }

        [TestMethod]
        public void TestOperadorIsTrueIdSameDifferentValues()
        {
            Assert.IsTrue(usu1 == usu4);
        }

        [TestMethod]
        public void TestCompareToIguales()
        {
            Assert.AreEqual(0, usu1.CompareTo(usu1));
        }

        [TestMethod]
        public void TestCompareToIgualesDistintoOrden()
        {
            Assert.AreEqual(usu1.CompareTo(usu4), 0);
            Assert.AreEqual(usu4.CompareTo(usu1), 0);
        }

        [TestMethod]
        public void TestCompareTo3Iguales()
        {
            Assert.AreEqual(usu1.CompareTo(usu4), 0);
            Assert.AreEqual(usu4.CompareTo(usu5), 0);
            Assert.AreEqual(usu1.CompareTo(usu5), 0);
        }

        [TestMethod]
        public void TestCompareToSignoDistintoOrden()
        {
            int cmp = usu1.CompareTo(usu2);
            Assert.AreEqual(Math.Sign(-cmp), Math.Sign(usu2.CompareTo(usu1)));
        }

        [TestMethod]
        public void TestCompareToTransitividadOrdenEstricto()
        {
            int cmp1 = usu1.CompareTo(usu2);
            int cmp2 = usu2.CompareTo(usu3);
            int cmp3 = usu1.CompareTo(usu3);

            Assert.AreEqual(Math.Sign(cmp1), Math.Sign(cmp2));
            Assert.AreEqual(Math.Sign(cmp1), Math.Sign(cmp3));
        }

        [TestMethod]
        public void TestOperador3Iguales()
        {
            Assert.IsTrue(usu1 == usu4);
            Assert.IsTrue(usu4 == usu5);
            Assert.IsTrue(usu1 == usu5);
        }

        [TestMethod]
        public void TestOperadorSignoDistintoOrden()
        {
            Assert.IsTrue(usu1 <= usu2);
            Assert.IsTrue(usu2 >= usu1);
        }

        [TestMethod]
        public void TestOperadorTransitividadOrdenEstricto()
        {
            Assert.IsTrue(usu1 <= usu2);
            Assert.IsTrue(usu2 <= usu3);
            Assert.IsTrue(usu1 <= usu3);
        }

    }
}
