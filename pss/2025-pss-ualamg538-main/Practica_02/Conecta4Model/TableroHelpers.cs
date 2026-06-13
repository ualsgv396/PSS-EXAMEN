using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    internal static class TableroHelpers
    {
        public static bool HayLinea(ITablero t, IFicha ficha, int filaInicio, int colInicio, int dFila, int dCol, int longitud)
        {
            for (int k = 0; k < longitud; k++)
            {
                int f = filaInicio + k * dFila;
                int c = colInicio + k * dCol;

                if (f < 0 || f >= t.Filas || c < 0 || c >= t.Columnas) return false;

                var celda = t[f, c];
                if (celda == null) return false;
                if (celda.Color != ficha.Color) return false;
            }
            return true;
        }
    }
}
