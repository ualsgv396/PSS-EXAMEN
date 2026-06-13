using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    public class JugadorIA : IJugador
    {

        private readonly IJugador _interno; 
        public IEstrategia Estrategia { get; }

        public JugadorIA(string nombre, IFicha ficha, IEstrategia estrategia)
        {
            _interno = new Jugador(nombre, ficha);
            Estrategia = estrategia ?? throw new ArgumentNullException(nameof(estrategia));
        }

        public string Nombre => _interno.Nombre;
        public IFicha Ficha => _interno.Ficha;

        public bool ColocarFichaColumna(ITablero tablero, int columna, out Posicion pos)
            => _interno.ColocarFichaColumna(tablero, columna, out pos);

        public bool QuitaFichaPosicion(ITablero tablero, Posicion pos)
            => _interno.QuitaFichaPosicion(tablero, pos);

        public int ElegirColumna(IJuego juego)
            => Estrategia.ElegirColumna(juego, this);

    }

}
