using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    public interface ICriterioVictoria
    {
        int FichasEnLinea { get; }
        bool hayVictoria(ITablero tablero, IFicha ficha);

    }
}
