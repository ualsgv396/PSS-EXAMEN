using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    internal class Configuracion : IConfiguracion
    {
        public int Filas { get; init; } = 6;
        public int Columnas { get; init; } = 7;
        public int NumJugadores { get; init; } = 2;
        public int FichasParaGanar { get; init; } = 4;
        public string Idioma { get; init; } = "es";
        public string Criterio { get; init; } = "FilasColumnasDiagonales";
        public IReadOnlyList<string> TiposJugador { get; init; }
            = new[] { "Humano", "Humano" };
    }
}
