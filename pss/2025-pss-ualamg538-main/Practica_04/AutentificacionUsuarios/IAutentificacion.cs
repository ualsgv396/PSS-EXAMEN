using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_04
{
    public interface IAutentificacion
    {
        CodigoAutentificacion EsUsuarioAutentificado(string id, string palabraPaso);
        bool ModificarUsuario(string id, IUsuarioView user);
        bool InsertarUsuario(IUsuarioView user);
        bool EliminarUsuario(string id);
        IUsuarioView ObtenerUsuario(string id);
        void GuardarDatos();
    }
}
