using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_04
{
    public class UsuarioEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string PalabraPaso { get; set; }
        public string Categoria { get; set; }
        public bool EsValido { get; set; }
    }
}
