using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    public static class Textos
    {
        private static readonly Dictionary<string, Dictionary<string, string>> _textos = new()
        {
            ["es"] = new()
            {
                ["bienvenida"] = "Bienvenido a Conecta {0}",
                ["turno"] = "Turno de {0} ({1})",
                ["ganador"] = "¡{0} gana la partida!",
                ["empate"] = "La partida termina en empate.",
                ["pide_columna"] = "Columna (0..{0}): ",
                ["col_invalida"] = "Entrada inválida: introduzca un número.",
                ["col_rango"] = "Columna fuera de rango (debe estar entre 0 y {0}).",
                ["col_llena"] = "Columna llena, elija otra.",
                ["mov_invalido"] = "Movimiento inválido, inténtalo de nuevo.",
            },
            ["en"] = new()
            {
                ["bienvenida"] = "Welcome to Connect {0}",
                ["turno"] = "{0}'s turn ({1})",
                ["ganador"] = "{0} wins!",
                ["empate"] = "The game ends in a draw.",
                ["pide_columna"] = "Column (0..{0}): ",
                ["col_invalida"] = "Invalid input: please enter a number.",
                ["col_rango"] = "Column out of range (must be between 0 and {0}).",
                ["col_llena"] = "Column is full, choose another one.",
                ["mov_invalido"] = "Invalid move, try again.",
            }
        };

        public static string Get(string idioma, string clave, params object[] args)
        {
            // Si el idioma pedido no existe, caemos al español como idioma por defecto.
            if (!_textos.TryGetValue(idioma, out var diccionario))
                diccionario = _textos["es"];

            if (!diccionario.TryGetValue(clave, out var plantilla))
                return $"[??{clave}??]"; // visible si te olvidas de una clave

            return args.Length == 0 ? plantilla : string.Format(plantilla, args);
        }
    }
}
