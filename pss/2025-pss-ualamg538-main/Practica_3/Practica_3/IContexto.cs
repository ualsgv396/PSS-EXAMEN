using System;
using System.Collections.Generic;
using System.Text;

namespace Practica_3
{
    public interface IContexto
    {

         List<Aplicacion> Aplicaciones { get; set; }
         List<Role> Roles { get; set; }
         List<Usuario> Usuarios { get; set; }
        List<Conexion> Conexiones { get; set; }
         List<UsuarioRole> UsuariosRoles { get; set; }
    }
}
