using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    internal class Tablero : ITablero
    {
        private readonly IFicha?[,] _celdas;
        public int Filas { get; }
        public int Columnas {  get; }

        public Tablero (int filas, int columnas)
        {
            if (filas < 4 || columnas < 4) throw new TableroTamanoException("El tablero debe tener al menos 4 filas y 4 columnas.");
            Filas = filas;
            Columnas = columnas;
            _celdas = new IFicha?[filas, columnas];
        }
        public IFicha? this[int fila, int columna] => _celdas[fila, columna];

        public void colocarEn(Posicion pos, IFicha ficha)
            => _celdas[pos.Fila, pos.Columna] = ficha;


        public void quitarDe(Posicion pos)
            => _celdas[pos.Fila, pos.Columna] = null;
    }
}
