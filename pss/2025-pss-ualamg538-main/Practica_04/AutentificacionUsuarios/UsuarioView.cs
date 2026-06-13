using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_04
{
    public class UsuarioView : IUsuarioView
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string PalabraPaso { get; set; }
        public string Categoria { get; set; }
        public bool EsValido { get; set; }

        public UsuarioView() { }

        public UsuarioView(int id, string nombre, string palabraPaso, string categoria, bool esValido)
        {
            Id = id.ToString();
            Nombre = nombre;
            PalabraPaso = palabraPaso;
            Categoria = categoria;
            EsValido = esValido;
        }

        // Constructor que recibe los campos como cadenas en este orden:
        // Id, Nombre, PalabraPaso, Categoria, EsValido
        public UsuarioView(params string[] campos)
        {
            if (campos.Length > 0) Id = campos[0];
            if (campos.Length > 1) Nombre = campos[1];
            if (campos.Length > 2) PalabraPaso = campos[2];
            if (campos.Length > 3) Categoria = campos[3];
            if (campos.Length > 4) EsValido = campos[4].Trim().ToUpper() == "TRUE";
        }
    }
}
