using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_04;
/// <summary>
/// Códigos de autentificación. Es [Flags], así que se pueden COMBINAR
/// (p. ej. usuario no válido + contraseña mal = AccesoInvalido | ErrorPalabraPaso).
/// </summary>
[Flags]
public enum CodigoAutentificacion
{
    AccesoCorrecto = 0,
    ErrorDatos = 1,
    AccesoInvalido = 2,
    ErrorIdUsuario = 4,
    ErrorPalabraPaso = 8
}