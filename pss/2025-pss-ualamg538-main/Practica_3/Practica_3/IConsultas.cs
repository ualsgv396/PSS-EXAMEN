namespace Practica_3
{
    public interface IConsultas
    {
        // ======================== CONSULTAS DE USUARIOS ========================

        /// Usuarios que pertenecen a una categoría (rol) determinada
        /// Nombre de usuario en mayúsculas
        IEnumerable<vmNombre> UsuariosEnCategoria(string nombreCategoria);


        /// Usuarios cuyo nombre comienza con la cadena proporcionada
        /// Nombre de usuario en mayúsculas
        IEnumerable<vmNombre> UsuariosConNombreComienza(string cadenaComienzo);


        /// Usuarios cuyo nombre comienza por cadenaComienzo que pertenecen a una categoría dada
        /// Nombre de usuario en mayúsculas
         IEnumerable<vmNombre> UsuariosConNombreComienzaEnCategoria(string cadenaComienzo, string categoria);




        /// Usuarios conectados desde una IP determinada
        /// Nombre de usuario en mayúsculas
        IEnumerable<vmNombre> UsuariosConectadosIP(string ip);


        /// Encuentra el nombre del usuario de una aplicación dada a través de su e-mail
        /// Nombre de usuario en mayúsculas
        IEnumerable<vmNombre> EncontrarUsuarioAppEmail(string aplicacion, string email);


        /// Usuarios no anónimos de una aplicación
        /// Nombre de usuario en mayúsculas
        IEnumerable<vmNombre> UsuariosNoAnonimosPorApp(string nombreApp);


        /// Usuarios anónimos
        /// Nombre de usuario en mayúsculas
         IEnumerable<vmNombre> UsuariosAnonimos();


        /// Usuarios bloqueados
        /// Nombre de usuario en mayúsculas
        IEnumerable<vmNombre> UsuariosBloqueados();


        /// Usuarios de una aplicación específica
        /// Nombre de usuario en mayúsculas
        IEnumerable<vmNombre> UsuariosPorAplicacion(string nombreApp);


        // ======================== CATEGORÍAS / ROLES ========================

        /// Lista de pares (Categoría, Usuario) para una aplicación dada 
        /// Nombre categoría, Nombre usuario
        IEnumerable<vmCategoriaNombre> ListaParCategoriaUsuarioParaApp(string aplicacion);


        /// <summary>
        /// Usuarios agrupados por categoría (rol)
        /// Nombre categoría, Nombre usuario
        /// </summary>
        IEnumerable<IGrouping<string, vmCategoriaNombre>> AgrupacionUsuariosCategorias();


        /// <summary>
        /// Usuarios agrupados por categorías ordenadas descendentemente
        /// Nombre categoría, Nombre usuario
        /// </summary>
        IEnumerable<vmCategoriaNombre> AgrupacionUsuariosCategoriasOrdenadas();


        /// <summary>
        /// Categoría con mayor número de usuarios
        /// Nombre categoría, Número de usuarios
        /// </summary>
        IEnumerable<vmCategoriaNombre> CategoriaMaximoNumeroUsuarios();


        /// <summary>
        /// Todas las categorías de usuarios para una aplicación dada
        /// Nombre categoría
        /// </summary>
        IEnumerable<vmCategoriaNombre> TodasCategoriasApp(string aplicacion);


        /// <summary>
        /// Categorías y aplicaciones para un usuario dado
        /// Nombre categoría, Nombre aplicación
        /// </summary>
        IEnumerable<vmCategoriaNombre> CategoriasAplicacionParaUsuario(string usuario);


        /// <summary>
        /// Roles ordenados por cantidad de usuarios (descendente)
        /// Nombre Rol, Cantidad de usuarios
        /// </summary>
        IEnumerable<vmNombreCantidad> RolesPorCantidadUsuarios();


        // ======================== CONEXIONES / ESTADÍSTICAS ========================

        /// <summary>
        /// IP con mayor número de conexiones según una categoría (rol)
        /// Ip , Cantidad de conexiones
        /// </summary>
        IEnumerable<vmNombreCantidad> IPconMasConexionesSegunCategoria(string nombreCategoria);


        /// <summary>
        /// Suma total de duración de conexiones por usuario (solo usuarios con conexiones)
        /// nombre Usuario, Suma duración
        /// </summary>
        IEnumerable<vmNombreCantidad> UsuarioSumaDuracionConexiones();


        /// <summary>
        /// LEFT OUTER JOIN - Suma total de duración de conexiones por usuario
        /// Incluye usuarios sin conexiones con 0
        /// Nombre Usuario, Suma duración
        /// </summary>
        IEnumerable<vmNombreCantidad> UsuarioSumaDuracionConexionesNulos();


        /// <summary>
        /// Usuarios cuya suma total de duración de conexión sea superior a la media
        /// Nombre Usuario, Suma duración
        /// </summary>
        IEnumerable<vmNombreCantidad> UsuariosSumaDuracionMayorMedia();


        /// <summary>
        /// Aplicaciones más usadas (suma de duración de conexiones)
        /// </summary>
        IEnumerable<vmNombreCantidad> AplicacionesMasUsadas();

        /// <summary>
        /// Aplicaciones más usadas, ordenadas de mayor a menor duración
        /// </summary>
        IEnumerable<vmNombreCantidad> AplicacionesMasUsadasOrdenadas();


        /// <summary>
        /// Número total de conexiones por usuario
        /// </summary>
        IEnumerable<vmNombreCantidad> UsuariosTotalConexiones();


        /// <summary>
        /// IPs más frecuentes (global)
        /// </summary>
        IEnumerable<vmNombreCantidad> IPsMasUsadas();


        /// <summary>
        /// Duración promedio de conexión por usuario
        /// </summary>
        IEnumerable<vmNombreCantidad> UsuarioDuracionPromedio();


        /// <summary>
        /// Duración promedio de conexión por aplicación
        /// </summary>
        IEnumerable<vmNombreCantidad> AplicacionDuracionPromedio();


        // ======================== CONSULTAS COMPLEJAS / STATS AVANZADAS ========================

        /// <summary>
        /// Estadísticas detalladas por usuario
        /// Nombre usuario, total conexiones, duración total, duración promedio, IP más usada por el usuario
        /// </summary>
        IEnumerable<vmEstadisticasUsuario> EstadisticasDetaladasUsuario();


        /// <summary>
        /// Estadísticas detalladas por aplicación
        /// NOmbre Aplicacion, total usuarios, total conexiones, duración total, duración promedio
        /// </summary>
        IEnumerable<vmEstadisticasApp> EstadisticasDetalladasAplicacion();


        /// <summary>
        /// Usuarios por rol con estadísticas de conexión
        /// Nombre Usuario, Rol, Aplicación
        /// </summary>
         IEnumerable<vmUsuarioRolAplicacion> UsuariosRolesAplicaciones();


        /// <summary>
        /// Conexiones por fecha (agrupadas)
        /// Fecha, Cantidad de conexiones
        /// </summary>
        IEnumerable<vmNombreCantidad> ConexionesPorFecha();


        /// <summary>
        /// Conexiones por hora del día
        /// Entre cada hora, Cantidad de conexiones
        /// </summary>
        IEnumerable<vmNombreCantidad> ConexionesPorHora();


        /// <summary>
        /// Usuarios que han intentado conectarse desde múltiples IPs
        /// Nombre Usuario, Cantidad de IPs distintas
        /// </summary>
        IEnumerable<vmNombreCantidad> UsuariosConMultiplesIPs();


        /// <summary>
        /// Usuarios por estado (activo/bloqueado/anónimo)
        /// Estado, Cantidad de usuarios
        /// </summary>
        IEnumerable<vmNombreCantidad> EstadoUsuarios();


        /// <summary>
        /// Roles más populares (por cantidad de usuarios)
        /// Nombre Rol, Cantidad de usuarios
        /// </summary>
        IEnumerable<vmNombreCantidad> RolesMasPopulares();


        /// <summary>
        /// Top N usuarios más activos (por duración total de conexiones)
        /// Nombre Usuario, Duración total
        /// </summary>
        IEnumerable<vmNombreCantidad> TopUsuariosActivos(int top = 10);


        /// <summary>
        /// Aplicaciones con mayor cantidad de usuarios únicos
        /// Nombre Aplicación, Cantidad de usuarios
        /// </summary>
        IEnumerable<vmNombreCantidad> AplicacionesConMasUsuarios();


        /// <summary>
        /// Usuarios que nunca se han conectado
        /// Nombre Usuario
        /// </summary>
        IEnumerable<vmNombre> UsuariosSinConexiones();


        /// <summary>
        /// Usuarios que se han conectado desde una IP sospechosa (solo N=1 conexiones)
        /// Nombre Usuario, Cantidad de conexiones
        /// </summary>
        IEnumerable<vmNombreCantidad> UsuariosPocasConexiones(int minConexiones = 1);

    }
}