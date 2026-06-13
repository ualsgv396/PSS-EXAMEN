using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    internal class EstrategiaMinimax : IEstrategia
    {
        private const int WIN_SCORE = 10000;
        private const int LOSS_SCORE = -10000;

        private readonly int _profundidad;

        public EstrategiaMinimax(int profundidad)
        {
            if (profundidad < 1)
                throw new ArgumentException("La profundidad debe ser >= 1.", nameof(profundidad));
            _profundidad = profundidad;
        }

        public int ElegirColumna(IJuego juego, IJugador yo)
        {
            IJugador rival = juego.Jugadores.First(j => !ReferenceEquals(j, yo));

            int mejorCol = -1;
            double mejor = double.NegativeInfinity;
            double dummy = 0;

            for (int col = 0; col < juego.Tablero.Columnas; col++)
            {
                if (!yo.ColocarFichaColumna(juego.Tablero, col, out Posicion pos))
                    continue;

                double valor = Min(juego, rival, yo, juego.Tablero,
                                   depth: 1,
                                   alpha: double.NegativeInfinity,
                                   beta: double.PositiveInfinity,
                                   depthLimit: _profundidad,
                                   numVeces: ref dummy);

                yo.QuitaFichaPosicion(juego.Tablero, pos);

                if (valor > mejor)
                {
                    mejor = valor;
                    mejorCol = col;
                }
            }

            if (mejorCol == -1)
                throw new InvalidOperationException("No hay movimientos legales.");

            return mejorCol;
        }

        // --- Núcleo: pegamos el algoritmo del PDF, adaptándolo a las interfaces ---

        private static double Max(IJuego juego, IJugador rival, IJugador ia,
                                  ITablero tab, int depth, double alpha, double beta,
                                  int depthLimit, ref double numVeces)
        {
            if (juego.ObtenerGanador(ia))
            {
                numVeces += 10.0 / depth;
                return WIN_SCORE - depth;
            }
            if (juego.ObtenerGanador(rival))
            {
                numVeces -= 10.0 / depth;
                return LOSS_SCORE + depth;
            }
            if (juego.FinJuego() || depth >= depthLimit)
                return EvaluarTablero(tab, ia, rival, juego.CriterioVictoria.FichasEnLinea);

            double best = double.NegativeInfinity;
            for (int col = 0; col < juego.Tablero.Columnas; col++)
            {
                if (!ia.ColocarFichaColumna(juego.Tablero, col, out Posicion pos))
                    continue;

                double score = Min(juego, rival, ia, tab,
                                   depth + 1, alpha, beta, depthLimit, ref numVeces);

                ia.QuitaFichaPosicion(juego.Tablero, pos);

                best = Math.Max(best, score);
                alpha = Math.Max(alpha, best);
                if (beta <= alpha) break; // poda
            }
            return best;
        }

        private static double Min(IJuego juego, IJugador rival, IJugador ia,
                                  ITablero tab, int depth, double alpha, double beta,
                                  int depthLimit, ref double numVeces)
        {
            if (juego.ObtenerGanador(ia))
            {
                numVeces += 10.0 / depth;
                return WIN_SCORE - depth;
            }
            if (juego.ObtenerGanador(rival))
            {
                numVeces -= 10.0 / depth;
                return LOSS_SCORE + depth;
            }
            if (juego.FinJuego() || depth >= depthLimit)
                return EvaluarTablero(tab, ia, rival, juego.CriterioVictoria.FichasEnLinea);

            double worst = double.PositiveInfinity;
            for (int col = 0; col < juego.Tablero.Columnas; col++)
            {
                if (!rival.ColocarFichaColumna(juego.Tablero, col, out Posicion pos))
                    continue;

                double score = Max(juego, rival, ia, tab,
                                   depth + 1, alpha, beta, depthLimit, ref numVeces);

                rival.QuitaFichaPosicion(juego.Tablero, pos);

                worst = Math.Min(worst, score);
                beta = Math.Min(beta, worst);
                if (beta <= alpha) break; // poda
            }
            return worst;
        }

        // --- Heurística (versión configurable con FichasEnLinea = X) ---

        private static int EvaluarTablero(ITablero tab, IJugador ia, IJugador rival, int x)
        {
            int p = 0;

            // Horizontales
            for (int f = 0; f < tab.Filas; f++)
                for (int c = 0; c <= tab.Columnas - x; c++)
                    p += EvaluarVentana(tab, ia, rival, f, c, 0, 1, x);

            // Verticales
            for (int c = 0; c < tab.Columnas; c++)
                for (int f = 0; f <= tab.Filas - x; f++)
                    p += EvaluarVentana(tab, ia, rival, f, c, 1, 0, x);

            // Diagonales descendentes
            for (int f = 0; f <= tab.Filas - x; f++)
                for (int c = 0; c <= tab.Columnas - x; c++)
                    p += EvaluarVentana(tab, ia, rival, f, c, 1, 1, x);

            // Diagonales ascendentes
            for (int f = x - 1; f < tab.Filas; f++)
                for (int c = 0; c <= tab.Columnas - x; c++)
                    p += EvaluarVentana(tab, ia, rival, f, c, -1, 1, x);

            return p;
        }

        private static int EvaluarVentana(ITablero tab, IJugador ia, IJugador rival,
                                          int f0, int c0, int df, int dc, int x)
        {
            int mias = 0, suyas = 0, huecos = 0;

            for (int k = 0; k < x; k++)
            {
                var f = tab[f0 + k * df, c0 + k * dc];
                if (f is null) huecos++;
                else if (f.Color == ia.Ficha.Color) mias++;
                else if (f.Color == rival.Ficha.Color) suyas++;
            }

            int s = 0;
            if (mias == x) s += 1000;
            else if (mias == x - 1 && huecos == 1) s += 50;
            else if (mias == x - 2 && huecos == 2) s += 10;

            if (suyas == x - 1 && huecos == 1) s -= 80;
            else if (suyas == x - 2 && huecos == 2) s -= 15;

            return s;
        }
    }
}
