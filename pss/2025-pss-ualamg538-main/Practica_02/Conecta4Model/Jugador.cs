using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    internal class Jugador : IJugador
    {
        public IFicha Ficha { get; }
        public string Nombre { get; }

        public Jugador(string nombre, IFicha ficha)
        {
            Ficha = ficha;
            Nombre = nombre;
        }

        public bool ColocarFichaColumna(ITablero t, int columna, out Posicion pos)
        {
            if (columna < 0 || columna >= t.Columnas)
            {
                throw new ColumnaFueraRangoException($"Columna {columna} fuera de rango. El tablero tiene {t.Columnas} columnas.");
            }
            for (int i = t.Filas - 1; i >= 0; i-- ) {
                if (t[i, columna] == null)
                {
                    pos = new Posicion(i, columna);
                    t.colocarEn(pos, Ficha);
                    return true;
                }
            }
            pos = default;
            return false;
        }

        public bool QuitaFichaPosicion(ITablero t, Posicion pos)
        {
            if (t[pos.Fila, pos.Columna] == null)   return false;
            t.quitarDe(pos);
            return true;
        }
    }
}
