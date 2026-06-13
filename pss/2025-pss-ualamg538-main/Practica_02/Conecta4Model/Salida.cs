using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    public class Salida : ISalida
    {
        public void Escribir(string texto) => Console.Write(texto);
        public void EscribirLinea(string texto) => Console.WriteLine(texto);
    }
}
