using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    public interface IEstrategia
    {
        int ElegirColumna(IJuego juego, IJugador jugador);
    }
}
