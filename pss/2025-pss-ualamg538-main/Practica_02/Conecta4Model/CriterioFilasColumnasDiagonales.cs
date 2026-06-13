using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    internal class CriterioFilasColumnasDiagonales : ICriterioVictoria
    {
        public int FichasEnLinea { get; }

        public CriterioFilasColumnasDiagonales(int fichasEnLinea)
        {
            if (fichasEnLinea < 2) throw new ArgumentException("FichasEnLinea debe ser >= 2", nameof(fichasEnLinea));
            FichasEnLinea = fichasEnLinea;
        }

        public bool hayVictoria(ITablero tablero, IFicha ficha)
        {
            int x = FichasEnLinea;

            for(int i = 0; i < tablero.Filas; i++)
                for(int j = 0; j <= tablero.Columnas - x; j++)
                    if(TableroHelpers.HayLinea(tablero, ficha, i, j, 0, 1, x)) return true;

            for (int j = 0; j < tablero.Columnas; j++)
                for (int i = 0; i <= tablero.Filas - x; i++)
                    if (TableroHelpers.HayLinea(tablero, ficha, i, j, 1, 0, x)) return true;

            for (int i = 0; i <= tablero.Filas - x; i++)
                for (int j = 0; j <= tablero.Columnas - x; j++)
                    if (TableroHelpers.HayLinea(tablero, ficha, i, j, 1, 1, x)) return true;

            for (int i = x - 1; i < tablero.Filas; i++)
                for (int j = 0; j <= tablero.Columnas - x; j++)
                    if (TableroHelpers.HayLinea(tablero, ficha, i, j, -1, 1, x))    return true;

            return false;

        }
    }
}
