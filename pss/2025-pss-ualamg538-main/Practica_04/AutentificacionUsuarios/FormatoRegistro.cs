using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_04
{
    public class FormatoRegistro
    {
        public CamposRegistro[] CamposRegistro { get; set; }

        public FormatoRegistro(CamposRegistro[] camposRegistro)
        {
            CamposRegistro = camposRegistro;
        }
    }
}
