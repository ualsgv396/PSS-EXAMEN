using System.Collections.Generic;

namespace PSS.amg538.Practica_02
{
    internal class Juego : IJuego
    {
        public ITablero Tablero { get; }
        public IReadOnlyList<IJugador> Jugadores { get; }
        public IJugador JugadorActual { get; private set; }
        public ICriterioVictoria CriterioVictoria { get; }
        private int indiceTurno = 0;

        public Juego(ITablero tablero, IReadOnlyList<IJugador> jugadores, ICriterioVictoria criterio)
        {
            if (jugadores == null || jugadores.Count < 2)
                throw new ArgumentException("Se requieren al menos 2 jugadores", nameof(jugadores));

            Tablero = tablero ?? throw new ArgumentNullException(nameof(tablero));
            Jugadores = jugadores;
            CriterioVictoria = criterio ?? throw new ArgumentNullException(nameof(criterio));
            JugadorActual = jugadores[0];
        }

        public bool JugarColumna(int columna)
        {
            if (FinJuego()) return false;

            bool valido = JugadorActual.ColocarFichaColumna(Tablero, columna, out _);
            if (valido) indiceTurno = (indiceTurno + 1) % Jugadores.Count;
            JugadorActual = Jugadores[indiceTurno];
            return valido;
        }

        public bool ObtenerGanador(IJugador jugador) => CriterioVictoria.hayVictoria(Tablero, jugador.Ficha);

        public bool FinJuego()
        {
            foreach (IJugador j in Jugadores)
                if (ObtenerGanador(j)) return true;

            return TableroLleno();
        }

        private bool TableroLleno()
        {
            for (int c = 0; c < Tablero.Columnas; c++)
                if (Tablero[0, c] is null) return false;
            return true;
        }
    }
}