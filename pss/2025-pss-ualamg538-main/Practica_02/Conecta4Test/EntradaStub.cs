using System;
using System.Collections.Generic;
using System.Text;
using PSS.amg538.Practica_02;

namespace Conecta4Test
{
    internal class EntradaStub : IEntrada
    {
        private readonly Queue<string?> _respuestas;
        public EntradaStub(params string?[] respuestas)
            => _respuestas = new Queue<string?>(respuestas);

        public string? LeerLinea()
            => _respuestas.Count > 0 ? _respuestas.Dequeue() : null;
    }
}
