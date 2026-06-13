using System.Security.Cryptography.X509Certificates;

namespace PSS.amg538.Practica_02
{
    internal class Ficha : IFicha
    {
        public string Color { get;}
        public Ficha(string color)
        {
            Color = color;
        }
    }
}