namespace PSS.amg538.Practica_02
{
    [Serializable]
    internal class CriterioCruz : ICriterioVictoria
    {
        public int FichasEnLinea { get; }

        public CriterioCruz(int fichasEnLinea)
        {
            if (fichasEnLinea < 3)
                throw new ArgumentException("La cruz mínima es de tamaño 3", nameof(fichasEnLinea));
            if (fichasEnLinea % 2 == 0)
                throw new ArgumentException("La cruz requiere tamaño impar", nameof(fichasEnLinea));

            FichasEnLinea = fichasEnLinea;
        }

        public bool hayVictoria(ITablero tablero, IFicha ficha)
        {
            int x = FichasEnLinea;
            int brazo = (x - 1) / 2;

            for (int f = brazo; f < tablero.Filas - brazo; f++)
                for (int c = brazo; c < tablero.Columnas - brazo; c++)
                    if (EsCentroDeCruz(tablero, ficha, f, c, brazo))
                        return true;
            return false;
        }

        private static bool EsCentroDeCruz(ITablero t, IFicha ficha, int f, int c, int brazo)
        {
            int longitud = 2 * brazo + 1;

            if (!TableroHelpers.HayLinea(t, ficha, f - brazo, c, 1, 0, longitud))
                return false;

            if (!TableroHelpers.HayLinea(t, ficha, f, c - brazo, 0, 1, longitud))
                return false;

            return true;
        }
    }
}