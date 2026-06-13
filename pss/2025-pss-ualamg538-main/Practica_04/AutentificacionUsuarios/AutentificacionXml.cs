using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace PSS.amg538.Practica_04
{
    public class AutentificacionXml : IAutentificacion
    {
        private readonly string _xmlFilename;
        private XDocument _xmlDocument;
        private readonly Dictionary<string, UsuarioView> _usuarios = new();

        public AutentificacionXml(string xmlFile)
        {
            _xmlFilename = xmlFile;

            if (string.IsNullOrEmpty(_xmlFilename) || !File.Exists(_xmlFilename))
                throw new AutentificacionExcepcion(
                    "El fichero " + xmlFile + " no existe.",
                    CodigoAutentificacion.ErrorDatos);

            try
            {
                _xmlDocument = XDocument.Load(_xmlFilename);
            }
            catch
            {
                throw new AutentificacionExcepcion(
                    "Error al acceder al archivo Xml",
                    CodigoAutentificacion.ErrorDatos);
            }

            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            _usuarios.Clear();
            var raiz = _xmlDocument.Element("Usuarios");
            if (raiz == null) return;

            foreach (var e in raiz.Elements("Usuario"))
            {
                var u = new UsuarioView
                {
                    Id = (string)e.Attribute("Id"),
                    Nombre = (string)e.Element("Nombre"),
                    PalabraPaso = (string)e.Element("PalabraPaso"),
                    Categoria = (string)e.Element("Categoria"),
                    EsValido = ((string)e.Element("EsValido") ?? "false")
                                      .Trim().ToLower() == "true"
                };
                if (!string.IsNullOrEmpty(u.Id))
                    _usuarios[u.Id] = u;
            }
        }

        public CodigoAutentificacion EsUsuarioAutentificado(string id, string palabraPaso)
        {
            if (!_usuarios.TryGetValue(id, out var u))
                return CodigoAutentificacion.ErrorIdUsuario;

            var r = CodigoAutentificacion.AccesoCorrecto;
            if (u.PalabraPaso != palabraPaso) r |= CodigoAutentificacion.ErrorPalabraPaso;
            if (!u.EsValido) r |= CodigoAutentificacion.AccesoInvalido;
            return r;
        }

        public IUsuarioView ObtenerUsuario(string id)
            => _usuarios.TryGetValue(id, out var u) ? u : null;

        public bool InsertarUsuario(IUsuarioView user)
        {
            if (user == null || string.IsNullOrEmpty(user.Id)) return false;
            if (_usuarios.ContainsKey(user.Id)) return false;
            _usuarios[user.Id] = Copiar(user);
            return true;
        }

        public bool ModificarUsuario(string id, IUsuarioView user)
        {
            if (user == null || !_usuarios.ContainsKey(id)) return false;
            var n = Copiar(user);
            n.Id = id;
            _usuarios[id] = n;
            return true;
        }

        public bool EliminarUsuario(string id) => _usuarios.Remove(id);

        public void GuardarDatos()
        {
            try
            {
                var doc = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Usuarios",
                        _usuarios.Values.Select(u =>
                            new XElement("Usuario",
                                new XAttribute("Id", u.Id ?? ""),
                                new XElement("Nombre", u.Nombre ?? ""),
                                new XElement("PalabraPaso", u.PalabraPaso ?? ""),
                                new XElement("Categoria", u.Categoria ?? ""),
                                new XElement("EsValido", u.EsValido ? "true" : "false")))));
                doc.Save(_xmlFilename);
                _xmlDocument = doc;
            }
            catch (Exception ex)
            {
                throw new AutentificacionExcepcion(
                    "Error al guardar el archivo Xml: " + ex.Message,
                    CodigoAutentificacion.ErrorDatos);
            }
        }

        private static UsuarioView Copiar(IUsuarioView u) => new UsuarioView
        {
            Id = u.Id,
            Nombre = u.Nombre,
            PalabraPaso = u.PalabraPaso,
            Categoria = u.Categoria,
            EsValido = u.EsValido
        };
    }
}
