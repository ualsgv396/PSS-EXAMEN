using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PSS.amg538.Practica_02
{
    public class Conecta4Factory
    {
        public static IConfiguracion CargarConfiguracion(string ruta)
        {
            if (!File.Exists(ruta))
                throw new ConfiguracionInvalidaException(
                    $"No se encontró el fichero de configuración: {ruta}");

            string contenido = File.ReadAllText(ruta);
            Configuracion? cfg;
            try
            {
                cfg = JsonSerializer.Deserialize<Configuracion>(
                    contenido,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true
                    });
            }
            catch (JsonException ex)
            {
                throw new ConfiguracionInvalidaException(
                    $"JSON malformado en {ruta}: {ex.Message}");
            }

            if (cfg is null)
                throw new ConfiguracionInvalidaException(
                    $"El fichero {ruta} está vacío o no es válido.");

            Validar(cfg);
            return cfg;
        }



        public static IConfiguracion CrearConfiguracionPorDefecto()
        {
            return new Configuracion();
        }

        public static ICriterioVictoria CrearCriterioCruz(int fichasEnLinea)
        {
            return new CriterioCruz(fichasEnLinea);
        }

        public static ICriterioVictoria CrearCriterioFilasColumnas(int fichasEnLinea)
        {
            return new CriterioFilasColumnas(fichasEnLinea);
        }

        public static ICriterioVictoria CrearCriterioFilasColumnasDiagonales(int fichasEnLinea)
        {
            return new CriterioFilasColumnasDiagonales(fichasEnLinea);
        }

        public static IFicha CrearFicha(string color)
        {
            return new Ficha(color);
        }

        public static IJuego CrearJuego(
            int filas = 6,
            int columnas = 7,
            int fichasParaGanar = 4,
            int numJugadores = 2,
            string[]? nombres = null)
        {
            if (numJugadores < 2)
                throw new ArgumentException("Mínimo 2 jugadores", nameof(numJugadores));

            var tablero = CrearTablero(filas, columnas);
            var criterio = CrearCriterioFilasColumnasDiagonales(fichasParaGanar);

            var coloresPorDefecto = new[] { "Rojo", "Amarillo", "Verde", "Azul" };
            var jugadores = new List<IJugador>();
            for (int i = 0; i < numJugadores; i++)
            {
                string nombre = (nombres is not null && i < nombres.Length)
                    ? nombres[i]
                    : $"Jugador{i + 1}";
                var color = coloresPorDefecto[i % coloresPorDefecto.Length];
                jugadores.Add(CrearJugador(nombre, CrearFicha(color)));
            }

            return new Juego(tablero, jugadores, criterio);
        }

        public static IJuego CrearJuego(IConfiguracion cfg)
        {
            if (cfg is null) throw new ArgumentNullException(nameof(cfg));

            var tablero = CrearTablero(cfg.Filas, cfg.Columnas);
            var criterio = CrearCriterioPorNombre(cfg.Criterio, cfg.FichasParaGanar);

            var coloresPorDefecto = new[] { "Rojo", "Amarillo", "Verde", "Azul" };
            var jugadores = new List<IJugador>();

            for (int i = 0; i < cfg.NumJugadores; i++)
            {
                string color = coloresPorDefecto[i % coloresPorDefecto.Length];
                string nombre = $"Jugador{i + 1}";
                var ficha = CrearFicha(color);

                string tipo = (i < cfg.TiposJugador.Count) ? cfg.TiposJugador[i] : "Humano";
                IJugador jugador = tipo switch
                {
                    "Humano" => CrearJugador(nombre, ficha),
                    "IA-Aleatoria" => CrearJugadorIA(nombre, ficha, CrearEstrategiaAleatoria()),
                    "IA-Minimax" => CrearJugadorIA(nombre, ficha, CrearEstrategiaMinimax(profundidad: 5)),
                    _ => throw new ConfiguracionInvalidaException(
                            $"Tipo de jugador desconocido: '{tipo}'")
                };
                jugadores.Add(jugador);
            }

            return new Juego(tablero, jugadores, criterio);
        }

        public static IJugador CrearJugador(IFicha ficha)
        {
            return new Jugador("Jugador1", ficha);
        }

        public static IJugador CrearJugador(string nombre, IFicha ficha)
        {
            return new Jugador(nombre, ficha);
        }

        public static ITablero CrearTablero(int filas = 6, int columnas = 7)
        {
            return new Tablero(filas, columnas);
        }

        public static IEstrategia CrearEstrategiaAleatoria(int? semilla = null)
            => new EstrategiaAleatoria(semilla);

        public static IJugador CrearJugadorIA(string nombre, IFicha ficha, IEstrategia estrategia)
            => new JugadorIA(nombre, ficha, estrategia);

        public static IEstrategia CrearEstrategiaMinimax(int profundidad)
        {
            return new EstrategiaMinimax(profundidad);
        }

        private static void Validar(IConfiguracion cfg)
        {
            if (cfg.Filas < 4 || cfg.Columnas < 4)
                throw new ConfiguracionInvalidaException(
                    "El tablero debe ser al menos 4x4.");
            if (cfg.NumJugadores < 2)
                throw new ConfiguracionInvalidaException(
                    "Se requieren al menos 2 jugadores.");
            if (cfg.FichasParaGanar < 3)
                throw new ConfiguracionInvalidaException(
                    "Se requieren al menos 3 fichas en línea para ganar.");
        }

        private static ICriterioVictoria CrearCriterioPorNombre(string nombre, int x) =>
        nombre switch
        {
            "FilasColumnas" => CrearCriterioFilasColumnas(x),
            "FilasColumnasDiagonales" => CrearCriterioFilasColumnasDiagonales(x),
            "Cruz" => CrearCriterioCruz(x),
            _ => throw new ConfiguracionInvalidaException(
                    $"Criterio de victoria desconocido: '{nombre}'")
        };

    }
}
