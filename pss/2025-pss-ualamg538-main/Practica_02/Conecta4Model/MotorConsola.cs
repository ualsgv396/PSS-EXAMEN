using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    public class MotorConsola
    {
        private readonly IJuego _juego;
        private readonly IEntrada _entrada;
        private readonly ISalida _salida;
        private readonly string _idioma;

        public MotorConsola(IJuego juego, IEntrada entrada, ISalida salida, string idioma = "es")
        {
            _juego = juego ?? throw new ArgumentNullException(nameof(juego));
            _entrada = entrada ?? throw new ArgumentNullException(nameof(entrada));
            _salida = salida ?? throw new ArgumentNullException(nameof(salida));
            _idioma = idioma;
        }

        public void Ejecutar()
        {
            _salida.EscribirLinea(Textos.Get(_idioma, "bienvenida", _juego.CriterioVictoria.FichasEnLinea));
            _salida.EscribirLinea(RenderTablero(_juego.Tablero));

            while (!_juego.FinJuego())
            {
                IJugador actual = _juego.JugadorActual;
                _salida.EscribirLinea(Textos.Get(_idioma, "turno", actual.Nombre, actual.Ficha.Color));

                int col = (actual is JugadorIA ia)
                    ? ia.ElegirColumna(_juego)
                    : PedirColumnaJugable(_entrada, _salida, _juego.Tablero, _idioma);

                bool ok = _juego.JugarColumna(col);
                if (!ok)
                {
                    _salida.EscribirLinea(Textos.Get(_idioma, "mov_invalido"));
                    continue;
                }

                _salida.EscribirLinea(RenderTablero(_juego.Tablero));
            }

            IJugador? ganador = _juego.Jugadores.FirstOrDefault(j => _juego.ObtenerGanador(j));
            if (ganador is not null)
                _salida.EscribirLinea(Textos.Get(_idioma, "ganador", ganador.Nombre));
            else
                _salida.EscribirLinea(Textos.Get(_idioma, "empate"));
        }

        public static int PedirColumna(IEntrada entrada, ISalida salida, int columnasTotales, string idioma = "es")
        {
            while (true)
            {
                salida.Escribir(Textos.Get(idioma, "pide_columna", columnasTotales - 1));
                string? linea = entrada.LeerLinea();

                if (!int.TryParse(linea, out int col))
                {
                    salida.EscribirLinea(Textos.Get(idioma, "col_invalida"));
                    continue;
                }

                if (col < 0 || col >= columnasTotales)
                {
                    salida.EscribirLinea(Textos.Get(idioma, "col_rango", columnasTotales - 1));
                    continue;
                }

                return col;
            }
        }

        public static int PedirColumnaJugable(IEntrada entrada, ISalida salida, ITablero tablero, string idioma = "es")
        {
            while (true)
            {
                int col = PedirColumna(entrada, salida, tablero.Columnas, idioma);
                if (tablero[0, col] is not null)
                {
                    salida.EscribirLinea(Textos.Get(idioma, "col_llena"));
                    continue;
                }
                return col;
            }
        }

        public static string PedirNombre(IEntrada entrada, ISalida salida, string defecto)
        {
            salida.Escribir($"Introduzca su nombre [{defecto}]: ");
            string? linea = entrada.LeerLinea();
            return string.IsNullOrWhiteSpace(linea) ? defecto : linea.Trim();
        }

        public static string RenderTablero(ITablero t)
        {
            var sb = new StringBuilder();

            for (int f = 0; f < t.Filas; f++)
            {
                for (int c = 0; c < t.Columnas; c++)
                {
                    if (c > 0) sb.Append(' ');
                    var ficha = t[f, c];
                    sb.Append(ficha is null ? '.' : ficha.Color[0]); // primera letra del color
                }
                sb.AppendLine();
            }

            // Índices de columnas en la última línea
            for (int c = 0; c < t.Columnas; c++)
            {
                if (c > 0) sb.Append(' ');
                sb.Append(c);
            }
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
