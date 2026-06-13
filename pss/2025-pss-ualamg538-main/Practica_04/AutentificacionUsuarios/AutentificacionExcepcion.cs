using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_04
{
    public class AutentificacionExcepcion : Exception
    {
        public CodigoAutentificacion Codigo { get; }

        public AutentificacionExcepcion(string mensaje, CodigoAutentificacion codigo)
            : base(mensaje)
        {
            Codigo = codigo;
        }
    }
}
