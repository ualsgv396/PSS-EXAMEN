# Practica_3 - Consultas LINQ

Este proyecto demuestra el uso de LINQ tanto con objetos en memoria (LINQ to Objects) como con XML (LINQ to XML).

## Estructura del Proyecto

### Archivos de Datos

- **`Datos.cs`**: Define las clases de modelo (User, Role, Aplicacion, Conexion, UserRole) y ViewModels
- **`Contexto.cs`**: Proporciona datos en memoria para las consultas LINQ to Objects
- **`Datos.xml`**: Archivo XML con los mismos datos para consultas LINQ to XML

### Archivos de Consultas

- **`IConsultas.cs`**: Interfaz que define todas las consultas disponibles
- **`Consultas.cs`**: Implementación usando LINQ to Objects (consulta datos del Contexto)
- **`ConsultasXML.cs`**: Implementación usando LINQ to XML (consulta el archivo Datos.xml)

### Archivos de Tests

- **`TestConsultas.cs`**: Tests unitarios para verificar las consultas con LINQ to Objects
- **`TestConsultasXML.cs`**: Tests unitarios para verificar las consultas con LINQ to XML

## Uso

### LINQ to Objects

```csharp
var consultas = new Consultas();
var usuarios = consultas.UsuariosEnCategoria("Alumno");
```

### LINQ to XML

```csharp
var consultasXML = new ConsultasXML("Datos.xml");
var usuarios = consultasXML.UsuariosEnCategoria("Alumno");
```

## Estructura del XML

El archivo `Datos.xml` contiene la siguiente estructura:

```xml
<Datos>
  <Aplicaciones>
    <Aplicacion>
      <Id>1</Id>
      <NombreAplicacion>Word</NombreAplicacion>
      <Path>c:\word</Path>
    </Aplicacion>
    <!-- Más aplicaciones -->
  </Aplicaciones>

  <Roles>
    <Role>
      <Id>1</Id>
      <Name>Alumno</Name>
      <AplicacionId>1</AplicacionId>
    </Role>
    <!-- Más roles -->
  </Roles>

  <Usuarios>
    <User>
      <Id>1</Id>
      <UserName>Diana</UserName>
      <Email>diana@example.com</Email>
      <EsAnonimo>false</EsAnonimo>
      <AplicacionId>1</AplicacionId>
      <FechaAlta>1980-01-09T03:00:00</FechaAlta>
      <EstaBloqueado>false</EstaBloqueado>
      <EmailConfirmed>true</EmailConfirmed>
    </User>
    <!-- Más usuarios -->
  </Usuarios>

  <UsuariosRoles>
    <UserRole>
      <Id>1</Id>
      <UserId>1</UserId>
      <RoleId>1</RoleId>
    </UserRole>
    <!-- Más relaciones usuario-rol -->
  </UsuariosRoles>

  <Conexiones>
    <Conexion>
      <Id>1</Id>
      <IP>192.168.134.23</IP>
      <FechaInicio>2012-03-21T01:40:12</FechaInicio>
      <Duracion>1214</Duracion>
      <UserId>1</UserId>
    </Conexion>
    <!-- Más conexiones -->
  </Conexiones>
</Datos>
```

## Consultas Disponibles

### Consultas de Usuarios
- `UsuariosEnCategoria(string nombreCategoria)`: Usuarios de un rol específico
- `UsuariosConNombreComienza(string cadenaComienzo)`: Usuarios cuyo nombre comienza con una cadena
- `UsuariosConNombreComienzaEnCategoria(string cadenaComienzo, string categoria)`: Combinación de filtros
- `UsuariosConectadosIP(string ip)`: Usuarios conectados desde una IP
- `EncontrarUsuarioAppEmail(string aplicacion, string email)`: Buscar usuario por email en una aplicación
- `UsuariosNoAnonimosPorApp(string nombreApp)`: Usuarios no anónimos de una aplicación
- `UsuariosAnonimos()`: Todos los usuarios anónimos
- `UsuariosBloqueados()`: Usuarios bloqueados
- `UsuariosPorAplicacion(string nombreApp)`: Usuarios de una aplicación

### Consultas de Categorías/Roles
- `ListaParCategoriaUsuarioParaApp(string aplicacion)`: Pares categoría-usuario por aplicación
- `AgrupacionUsuariosCategorias()`: Usuarios agrupados por categoría
- `AgrupacionUsuariosCategoriasOrdenadas()`: Igual que el anterior pero ordenado
- `CategoriaMaximoNumeroUsuarios()`: Categoría con más usuarios
- `TodasCategoriasApp(string aplicacion)`: Categorías de una aplicación
- `CategoriasAplicacionParaUsuario(string usuario)`: Categorías y apps de un usuario
- `RolesPorCantidadUsuarios()`: Roles ordenados por popularidad

### Consultas de Conexiones/Estadísticas
- `IPconMasConexionesSegunCategoria(string nombreCategoria)`: IPs más usadas por rol
- `UsuarioSumaDuracionConexiones()`: Suma de duración de conexiones por usuario
- `UsuarioSumaDuracionConexionesNulos()`: Incluye usuarios sin conexiones
- `UsuariosSumaDuracionMayorMedia()`: Usuarios sobre la media
- `AplicacionesMasUsadas()`: Aplicaciones por tiempo de uso
- `AplicacionesMasUsadasOrdenadas()`: Igual pero ordenado
- `UsuariosTotalConexiones()`: Número de conexiones por usuario
- `IPsMasUsadas()`: IPs más frecuentes
- `UsuarioDuracionPromedio()`: Duración promedio por usuario
- `AplicacionDuracionPromedio()`: Duración promedio por aplicación

### Consultas Complejas/Estadísticas Avanzadas
- `EstadisticasDetaladasUsuario()`: Estadísticas completas por usuario
- `EstadisticasDetalladasAplicacion()`: Estadísticas completas por aplicación
- `UsuariosRolesAplicaciones()`: Relaciones usuario-rol-aplicación
- `ConexionesPorFecha()`: Conexiones agrupadas por fecha
- `ConexionesPorHora()`: Conexiones agrupadas por hora
- `UsuariosConMultiplesIPs()`: Usuarios con varias IPs
- `EstadoUsuarios()`: Distribución por estado (activo/bloqueado/anónimo)
- `RolesMasPopulares()`: Roles por popularidad
- `TopUsuariosActivos(int top)`: Top N usuarios más activos
- `AplicacionesConMasUsuarios()`: Aplicaciones por cantidad de usuarios
- `UsuariosSinConexiones()`: Usuarios que nunca se conectaron
- `UsuariosPocasConexiones(int minConexiones)`: Usuarios con pocas conexiones

## ViewModels

El proyecto utiliza los siguientes ViewModels para retornar datos:

- **`vmNombre`**: Contiene solo un nombre
- **`vmCategoriaNombre`**: Contiene categoría y nombre
- **`vmNombreCantidad`**: Contiene nombre y cantidad (double)
- **`vmUsuarioRolAplicacion`**: Usuario, rol y aplicación
- **`vmEstadisticasUsuario`**: Estadísticas detalladas de usuario
- **`vmEstadisticasApp`**: Estadísticas detalladas de aplicación

## Datos de Ejemplo

El sistema incluye los siguientes datos de ejemplo:

### Aplicaciones
- Word, Excel, GestionUsuarios, Explorer

### Roles
- Alumno, Profesor, Administrador, Invitado

### Usuarios
- Diana, Juan, Antonio, Ana (Alumnos)
- Jose, Julio, Mercedes (Profesores)
- Jose (Administrador)
- Anonimo (Invitado)

### Conexiones
- 23 conexiones de diferentes usuarios desde varias IPs

## Ejecutar Tests

Para ejecutar los tests:

```bash
dotnet test
```

Los tests verifican que ambas implementaciones (LINQ to Objects y LINQ to XML) produzcan los mismos resultados.

## Ventajas de cada enfoque

### LINQ to Objects
- ✅ Más rápido (datos en memoria)
- ✅ Más fácil de depurar
- ✅ Mejor rendimiento para consultas complejas
- ❌ Datos no persistentes

### LINQ to XML
- ✅ Datos persistentes
- ✅ Fácil de compartir/exportar
- ✅ Formato estándar
- ✅ Puede editarse externamente
- ❌ Más lento que en memoria
- ❌ Requiere parsing

## Notas

- Ambas implementaciones producen los mismos resultados
- El archivo XML debe estar accesible para `ConsultasXML`
- Los ViewModels son compartidos entre ambas implementaciones
- Todos los tests verifican los valores exactos esperados
