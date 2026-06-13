using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    internal class EstrategiaAleatoria : IEstrategia
    {
        private readonly Random _rng;

        public EstrategiaAleatoria(int? semilla = null)
            => _rng = semilla.HasValue ? new Random(semilla.Value) : new Random();

        public int ElegirColumna(IJuego juego, IJugador yo)
        {
            var legales = ColumnasLegales(juego.Tablero).ToList();
            if (legales.Count == 0)
                throw new InvalidOperationException("No hay columnas legales (tablero lleno).");
            return legales[_rng.Next(legales.Count)];
        }

        private static IEnumerable<int> ColumnasLegales(ITablero tablero)
        {
            for (int c = 0; c < tablero.Columnas; c++)
                if (tablero[0, c] is null) yield return c;
        }
    }
}
