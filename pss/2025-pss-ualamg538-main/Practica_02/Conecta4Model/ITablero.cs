using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    public interface ITablero
    {
        int Filas { get; }
        int Columnas { get; }
        IFicha? this[int fila,  int columna] { get; }
        void colocarEn(Posicion pos, IFicha ficha);
        void quitarDe(Posicion pos);
    }
}
