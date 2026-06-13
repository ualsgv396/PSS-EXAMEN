namespace Practica_3
{
    public class DatosColecciones:IContexto
    {
        public List<Aplicacion> Aplicaciones { get; set; }
        public List<Role> Roles { get; set; }
        public List<Usuario> Usuarios  { get; set; }
        public List<Conexion> Conexiones { get; set; }
        public List<UsuarioRole> UsuariosRoles {  get; set; }


        public DatosColecciones()
        {
            Aplicaciones = new List<Aplicacion>();
            Roles = new List<Role>();
            Usuarios = new List<Usuario>();
            Conexiones = new List<Conexion>();
            UsuariosRoles = new List<UsuarioRole>();

            // === CREAR APLICACIONES ===
            var apps = new List<Aplicacion>
            {
                new Aplicacion { Id = 1, NombreAplicacion = "Word", Path = @"c:\word" },
                new Aplicacion { Id = 2, NombreAplicacion = "Excel", Path = @"c:\excel" },
                new Aplicacion { Id = 3, NombreAplicacion = "GestionUsuarios", Path = @"c:\gestion" }, 
                new Aplicacion { Id = 4, NombreAplicacion = "Explorer", Path = @"c:\explorer" }
            };
            Aplicaciones.AddRange(apps);

            // === CREAR ROLES ===
            var roles = new List<Role>
            {
                new Role { Id = 1, Name = "Alumno", AplicacionId = 1 },
                new Role { Id = 2, Name = "Alumno", AplicacionId = 2 },
                new Role { Id = 3, Name = "Profesor", AplicacionId = 1 },
                new Role { Id = 4, Name = "Profesor", AplicacionId = 2 },
                new Role { Id = 5, Name = "Administrador", AplicacionId = 3 },
                new Role { Id = 6, Name = "Invitado", AplicacionId = 4 }
            };
            Roles.AddRange(roles);

            // === CREAR USUARIOS ===
            var usuarios = new List<Usuario>
            {
                new Usuario
                {
                    Id = 1,
                    UserName = "Diana",
                    Email = "diana@example.com",
                    EsAnonimo = false,
                    AplicacionId = 1,
                    FechaAlta = new DateTime(1980, 1, 9, 3, 0, 0),
                    EstaBloqueado = false,
                    EmailConfirmed = true
                },
                new Usuario
                {
                    Id = 2,
                    UserName = "Juan",
                    Email = "Juan.pss-2@ual.es",
                    EsAnonimo = false,
                    AplicacionId = 2,
                    FechaAlta = new DateTime(1979, 4, 6, 5, 0, 0),
                    EstaBloqueado = false,
                    EmailConfirmed = true
                },
                new Usuario
                {
                    Id = 3,
                    UserName = "Antonio",
                    Email = "Antonio.pss-3@hotmail.es",
                    EsAnonimo = false,
                    AplicacionId = 1,
                    FechaAlta = new DateTime(1980, 2, 6, 7, 0, 0),
                    EstaBloqueado = false,
                    EmailConfirmed = true
                },
                new Usuario
                {
                    Id = 4,
                    UserName = "Ana",
                    Email = "ana@example.com",
                    EsAnonimo = false,
                    AplicacionId = 1,
                    FechaAlta = new DateTime(1982, 4, 7, 9, 0, 0),
                    EstaBloqueado = false,
                    EmailConfirmed = true
                },
                new Usuario
                {
                    Id = 5,
                    UserName = "Jose",
                    Email = "Jose.pss-5@ual.es",
                    EsAnonimo = false,
                    AplicacionId = 1,
                    FechaAlta = new DateTime(1960, 5, 5, 6, 0, 0),
                    EstaBloqueado = false,
                    EmailConfirmed = true
                },
                new Usuario
                {
                    Id = 6,
                    UserName = "Julio",
                    Email = "julio@example.com",
                    EsAnonimo = false,
                    AplicacionId = 2,
                    FechaAlta = new DateTime(1961, 3, 6, 8, 0, 0),
                    EstaBloqueado = false,
                    EmailConfirmed = true
                },
                new Usuario
                {
                    Id = 7,
                    UserName = "Mercedes",
                    Email = "mercedes@example.com",
                    EsAnonimo = false,
                    AplicacionId = 2,
                    FechaAlta = new DateTime(1962, 5, 2, 9, 0, 0),
                    EstaBloqueado = false,
                    EmailConfirmed = true
                },
                new Usuario
                {
                    Id = 8,
                    UserName = "Anonimo",
                    Email = "anonimo@example.com",
                    EsAnonimo = true,
                    AplicacionId = 4,
                    FechaAlta = new DateTime(2000, 1, 1, 0, 0, 0),
                    EstaBloqueado = false,
                    EmailConfirmed = true
                },
                new Usuario
                {
                    Id = 9,
                    UserName = "Jose",
                    Email = "jose.otro@example.com",
                    EsAnonimo = false,
                    AplicacionId = 3,
                    FechaAlta = new DateTime(2000, 5, 5, 6, 0, 0),
                    EstaBloqueado = false,
                    EmailConfirmed = true
                }
            };
            Usuarios.AddRange(usuarios);

            // === ASIGNAR USUARIOS A ROLES ===
            // Diana -> Alumno-1
            // Juan -> Alumno-2
            // Antonio -> Alumno-1
            // Ana -> Alumno-1
            // Jose -> Profesor-1
            // Julio -> Profesor-2
            // Mercedes -> Profesor-2
            // Anonimo -> Invitado-4
            // Jose -> Administrador-3

            var UserRoles = new List<UsuarioRole>
                {
                  new UsuarioRole { Id=1, UserId=1, RoleId=1},
                  new UsuarioRole { Id=2, UserId=2, RoleId=2},
                  new UsuarioRole { Id=3, UserId=3, RoleId=1},
                 new UsuarioRole { Id=4, UserId=4, RoleId=1},
                new UsuarioRole { Id=5, UserId=5, RoleId=3},
                new UsuarioRole { Id=6, UserId=6, RoleId=4},
                new UsuarioRole { Id=7, UserId=7, RoleId=4},
            new UsuarioRole { Id=8, UserId=8, RoleId=6},
            new UsuarioRole { Id=9, UserId=9, RoleId=5}
            };
            UsuariosRoles.AddRange(UserRoles);


            // === CREAR CONEXIONES ===
            var conexiones = new List<Conexion>
            {
                new Conexion { Id = 1, IP = "192.168.134.23", FechaInicio = new DateTime(2012, 3, 21, 1, 40, 12), Duracion = 1214, UserId = 1 },
                new Conexion { Id = 3, IP = "192.168.134.28", FechaInicio = new DateTime(2011, 4, 22, 2, 30, 22), Duracion = 1874, UserId = 1 },
                new Conexion { Id = 4, IP = "192.168.134.28", FechaInicio = new DateTime(2011, 5, 23, 3, 20, 32), Duracion = 167, UserId = 1 },
                new Conexion { Id = 6, IP = "192.168.134.123", FechaInicio = new DateTime(2011, 4, 20, 2, 50, 11), Duracion = 114, UserId = 2 },
                new Conexion { Id = 7, IP = "192.168.134.128", FechaInicio = new DateTime(2011, 5, 24, 1, 10, 21), Duracion = 1678, UserId = 2 },
                new Conexion { Id = 8, IP = "192.168.134.18", FechaInicio = new DateTime(2011, 3, 11, 0, 10, 2), Duracion = 14, UserId = 3 },
                new Conexion { Id = 9, IP = "192.168.134.13", FechaInicio = new DateTime(2012, 4, 21, 1, 35, 12), Duracion = 11, UserId = 3 },
                new Conexion { Id = 10, IP = "192.168.134.18", FechaInicio = new DateTime(2011, 5, 1, 0, 37, 22), Duracion = 84, UserId = 3 },
                new Conexion { Id = 11, IP = "192.168.134.18", FechaInicio = new DateTime(2012, 5, 20, 1, 12, 32), Duracion = 168, UserId = 3 },
                new Conexion { Id = 12, IP = "192.168.134.108", FechaInicio = new DateTime(2012, 1, 1, 1, 11, 12), Duracion = 141, UserId = 4 },
                new Conexion { Id = 14, IP = "192.168.134.103", FechaInicio = new DateTime(2011, 2, 2, 1, 12, 2), Duracion = 111, UserId = 4 },
                new Conexion { Id = 16, IP = "192.168.134.108", FechaInicio = new DateTime(2011, 3, 12, 2, 45, 51), Duracion = 84, UserId = 4 },
                new Conexion { Id = 17, IP = "192.168.134.108", FechaInicio = new DateTime(2011, 5, 13, 1, 32, 22), Duracion = 568, UserId = 4 },
                new Conexion { Id = 18, IP = "192.168.134.108", FechaInicio = new DateTime(2011, 5, 19, 0, 55, 33), Duracion = 2, UserId = 4 },
                new Conexion { Id = 19, IP = "192.168.134.103", FechaInicio = new DateTime(2011, 6, 21, 1, 30, 44), Duracion = 11, UserId = 4 },
                new Conexion { Id = 20, IP = "192.168.134.108", FechaInicio = new DateTime(2011, 7, 22, 0, 44, 55), Duracion = 84, UserId = 4 },
                new Conexion { Id = 21, IP = "192.168.134.108", FechaInicio = new DateTime(2011, 9, 29, 1, 16, 3), Duracion = 368, UserId = 4 },
                new Conexion { Id = 22, IP = "193.161.134.18", FechaInicio = new DateTime(2011, 3, 1, 2, 31, 57), Duracion = 14, UserId = 5 },
                new Conexion { Id = 24, IP = "193.161.134.18", FechaInicio = new DateTime(2011, 5, 11, 4, 32, 56), Duracion = 11, UserId = 9 },
                new Conexion { Id = 27, IP = "193.161.134.18", FechaInicio = new DateTime(2012, 1, 21, 6, 33, 55), Duracion = 18, UserId = 6 },
                new Conexion { Id = 28, IP = "193.161.134.18", FechaInicio = new DateTime(2011, 6, 28, 8, 34, 54), Duracion = 16, UserId = 6 },
                new Conexion { Id = 31, IP = "193.161.134.15", FechaInicio = new DateTime(2011, 2, 12, 18, 10, 25), Duracion = 38, UserId = 8 },
                new Conexion { Id = 32, IP = "193.162.134.15", FechaInicio = new DateTime(2011, 5, 13, 18, 10, 27), Duracion = 162, UserId = 6 }
            };

            Conexiones.AddRange(conexiones);

        }
    }
}





