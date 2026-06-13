using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    public interface IJuego
    {
        ITablero Tablero { get; }
        IReadOnlyList<IJugador> Jugadores { get; }
        IJugador JugadorActual { get; }
        ICriterioVictoria CriterioVictoria { get; }

        bool FinJuego();
        bool JugarColumna(int columna);
        bool ObtenerGanador(IJugador jugador);
    }
}
