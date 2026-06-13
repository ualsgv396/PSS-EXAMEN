using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_04
{
    public class AutentificacionTextFile : IAutentificacion
    {
        private readonly string _textFile;
        private readonly FormatoRegistro _formato;
        private readonly string _finCampo;
        private readonly Dictionary<string, UsuarioView> _usuarios = new();

        public AutentificacionTextFile(string textFile, FormatoRegistro formatoRegistro, string finCampo)
        {
            _textFile = textFile;
            _formato = formatoRegistro;
            _finCampo = finCampo;

            if (string.IsNullOrEmpty(_textFile) || !File.Exists(_textFile))
                throw new AutentificacionExcepcion(
                    "No se encuentra el fichero de texto: " + textFile,
                    CodigoAutentificacion.ErrorDatos);

            try
            {
                CargarUsuarios();
            }
            catch (AutentificacionExcepcion) { throw; }
            catch (Exception ex)
            {
                throw new AutentificacionExcepcion(
                    "Error al leer el fichero de texto: " + ex.Message,
                    CodigoAutentificacion.ErrorDatos);
            }
        }

        private void CargarUsuarios()
        {
            _usuarios.Clear();
            // ReadAllLines sin encoding explícito detecta el BOM (UTF-8/UTF-16).
            foreach (var linea in File.ReadAllLines(_textFile))
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;
                var u = ParsearLinea(linea);
                if (!string.IsNullOrEmpty(u.Id))
                    _usuarios[u.Id] = u;
            }
        }

        private UsuarioView ParsearLinea(string linea)
        {
            string[] partes = linea.Split(new[] { _finCampo }, StringSplitOptions.None);
            var u = new UsuarioView();
            var campos = _formato.CamposRegistro;
            for (int i = 0; i < campos.Length && i < partes.Length; i++)
            {
                string valor = partes[i];
                switch (campos[i])
                {
                    case CamposRegistro.Id: u.Id = valor; break;
                    case CamposRegistro.Nombre: u.Nombre = valor; break;
                    case CamposRegistro.PalabraPaso: u.PalabraPaso = valor; break;
                    case CamposRegistro.Categoria: u.Categoria = valor; break;
                    case CamposRegistro.EsValido: u.EsValido = valor.Trim().ToUpper() == "TRUE"; break;
                }
            }
            return u;
        }

        public CodigoAutentificacion EsUsuarioAutentificado(string id, string palabraPaso)
        {
            if (!_usuarios.TryGetValue(id, out var u))
                return CodigoAutentificacion.ErrorIdUsuario;

            var resultado = CodigoAutentificacion.AccesoCorrecto;
            if (u.PalabraPaso != palabraPaso) resultado |= CodigoAutentificacion.ErrorPalabraPaso;
            if (!u.EsValido) resultado |= CodigoAutentificacion.AccesoInvalido;
            return resultado;
        }

        public IUsuarioView ObtenerUsuario(string id)
            => _usuarios.TryGetValue(id, out var u) ? u : null;

        public bool InsertarUsuario(IUsuarioView user)
        {
            if (user == null || string.IsNullOrEmpty(user.Id)) return false;
            if (_usuarios.ContainsKey(user.Id)) return false;     // ya existe
            _usuarios[user.Id] = Copiar(user);
            return true;
        }

        public bool ModificarUsuario(string id, IUsuarioView user)
        {
            if (user == null || !_usuarios.ContainsKey(id)) return false;
            var nuevo = Copiar(user);
            nuevo.Id = id;                                        // la clave no cambia
            _usuarios[id] = nuevo;
            return true;
        }

        public bool EliminarUsuario(string id) => _usuarios.Remove(id);

        public void GuardarDatos()
        {
            try
            {
                var lineas = _usuarios.Values.Select(FormatearLinea).ToList();
                File.WriteAllLines(_textFile, lineas, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new AutentificacionExcepcion(
                    "Error al guardar el fichero de texto: " + ex.Message,
                    CodigoAutentificacion.ErrorDatos);
            }
        }

        private string FormatearLinea(UsuarioView u)
        {
            var campos = _formato.CamposRegistro;
            var valores = new string[campos.Length];
            for (int i = 0; i < campos.Length; i++)
            {
                valores[i] = campos[i] switch
                {
                    CamposRegistro.Id => u.Id,
                    CamposRegistro.Nombre => u.Nombre,
                    CamposRegistro.PalabraPaso => u.PalabraPaso,
                    CamposRegistro.Categoria => u.Categoria,
                    CamposRegistro.EsValido => u.EsValido ? "true" : "false",
                    _ => ""
                };
            }
            return string.Join(_finCampo, valores);
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
