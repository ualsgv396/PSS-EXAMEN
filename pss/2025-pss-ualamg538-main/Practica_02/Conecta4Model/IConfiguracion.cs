using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    public interface IConfiguracion
    {
        int Filas { get; }
        int Columnas { get; }
        int NumJugadores { get; }
        int FichasParaGanar { get; }
        string Idioma { get; }
        string Criterio { get; }
        IReadOnlyList<string> TiposJugador { get; }

    }
}
