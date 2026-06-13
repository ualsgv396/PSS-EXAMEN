using System;
using System.Collections.Generic;
using System.Text;
using PSS.amg538.Practica_02;

namespace Conecta4Test
{
    internal class SalidaStub : ISalida
    {
        public List<string> Lineas { get; } = new();
        private readonly StringBuilder _buffer = new();

        public void Escribir(string texto) => _buffer.Append(texto);

        public void EscribirLinea(string texto)
        {
            _buffer.Append(texto);
            Lineas.Add(_buffer.ToString());
            _buffer.Clear();
        }

        public string Todo => string.Join("\n", Lineas);
    }
}
