using System;
using System.Collections.Generic;
using System.Text;

namespace Practica_3
{
    public class Usuario
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool EsAnonimo { get; set; }
        public DateTime FechaAlta { get; set; }
        public bool EstaBloqueado { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }

        public int? AplicacionId { get; set; }

        public ICollection<UsuarioRole> UsuarioRoles { get; set; }
        public ICollection<Conexion> Conexiones { get; set; }
    }

    public class Role 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? AplicacionId { get; set; } // roles por app
    }

    public class UsuarioRole 
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

    }

    public class Aplicacion
    {
        public int Id { get; set; }
        public string NombreAplicacion { get; set; }
        public string Path { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }

    public class Conexion
    {
        public int Id { get; set; }
        public string IP { get; set; }
        public DateTime FechaInicio { get; set; }
        public double Duracion { get; set; }

        public int UserId { get; set; }
    }
 

public class vmNombre
    {
        public string Nombre { get; set; }
        public override bool Equals(object obj)
        {
            vmNombre vm = obj as vmNombre;
            if (ReferenceEquals(vm, null)) return false;
            return Nombre.Equals(vm.Nombre);
        }
        public override int GetHashCode() => Nombre.GetHashCode();
    }
    public class vmCategoriaNombre
    {
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public override bool Equals(object obj)
        {
            vmCategoriaNombre vm = obj as vmCategoriaNombre;
            if (ReferenceEquals(vm, null)) return false;
            return Categoria.Equals(vm.Categoria);
        }
        public override int GetHashCode() => Categoria.GetHashCode();
    }
    public class vmNombreCantidad
    {
        public string Nombre { get; set; }
        public double Cantidad { get; set; }
    }
    public class vmUsuarioRolAplicacion
    {
        public string Usuario { get; set; }
        public string Rol { get; set; }
        public string Aplicacion { get; set; }
    }
    public class vmEstadisticasUsuario
    {
        public string Usuario { get; set; }
        public int TotalConexiones { get; set; }
        public double DuracionTotal { get; set; }
        public double DuracionPromedio { get; set; }
        public string IP_MasUsada { get; set; }
    }
    public class vmEstadisticasApp
    {
        public string Aplicacion { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalConexiones { get; set; }
        public double DuracionTotal { get; set; }
        public double DuracionPromedio { get; set; }
    }

}
