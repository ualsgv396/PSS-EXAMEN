using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    public interface IJugador
    {
        string Nombre { get; }
        IFicha Ficha { get; }

        bool ColocarFichaColumna(ITablero t, int columna, out Posicion pos);
        bool QuitaFichaPosicion(ITablero t, Posicion pos);
    }
}
