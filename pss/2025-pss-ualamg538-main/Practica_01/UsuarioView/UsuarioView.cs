using System;
using System.Collections.Generic;
using System.Text;

namespace PSS.amg538.Practica_02
{
    public class UsuarioView : IUsuarioView, IEquatable<UsuarioView>, IEqualityComparer<UsuarioView>, IComparable<UsuarioView>
    {
        private string _id;
        private string _nombre;
        private string _palabraPaso;
        private string _categoria;
        private bool _esValido;

        public UsuarioView()
        {

        }
        public UsuarioView(int id, string nombre, string palabraPaso, string categoria, bool esValido)
        {
            _id = id.ToString();
            _nombre = nombre;
            _palabraPaso = palabraPaso;
            _categoria = categoria;
            _esValido = esValido;
        }
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
        public string PalabraPaso
        {
            get { return _palabraPaso; }
            set { _palabraPaso = value; }
        }
        public string Categoria
        {
            get { return _categoria; }
            set { _categoria = value; }
        }
        public bool EsValido
        {
            get { return _esValido; }
            set { _esValido = value; }
        }

        public bool Equals(UsuarioView other)
        {
            try
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Id, other.Id, StringComparison.Ordinal);
            }
            catch
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            try
            {
                return Equals(obj as UsuarioView);
            }
            catch
            {
                return false;
            }
        }

        public bool Equals(UsuarioView x, UsuarioView y)
        {
            try
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(null, x) || ReferenceEquals(null, y)) return false;
                return string.Equals(x.Id, y.Id, StringComparison.Ordinal);
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            try
            {
                return StringComparer.Ordinal.GetHashCode(Id ?? string.Empty);
            }
            catch
            {
                return 0;
            }
        }

        public int GetHashCode(UsuarioView obj)
        {
            try
            {
                if (ReferenceEquals(null, obj)) return 0;
                return StringComparer.Ordinal.GetHashCode(obj.Id ?? string.Empty);
            }
            catch
            {
                return 0;
            }
        }

        public static bool operator ==(UsuarioView left, UsuarioView right)
        {
            try
            {
                if (ReferenceEquals(left, right)) return true;
                if (ReferenceEquals(null, left) || ReferenceEquals(null, right)) return false;
                return left.Equals(right);
            }
            catch
            {
                return false;
            }
        }

        public static bool operator !=(UsuarioView left, UsuarioView right)
        {
            try
            {
                return !(left == right);
            }
            catch
            {
                return false;
            }
        }

        public static bool operator <(UsuarioView left, UsuarioView right)
        {
            try
            {
                if (ReferenceEquals(left, right)) return false;
                if (ReferenceEquals(null, left)) return !ReferenceEquals(null, right);
                if (ReferenceEquals(null, right)) return false;
                return left.CompareTo(right) < 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool operator >(UsuarioView left, UsuarioView right)
        {
            try
            {
                if (ReferenceEquals(left, right)) return false;
                if (ReferenceEquals(null, left)) return false;
                if (ReferenceEquals(null, right)) return true;
                return left.CompareTo(right) > 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool operator <=(UsuarioView left, UsuarioView right)
        {
            try
            {
                if (ReferenceEquals(left, right)) return true;
                if (ReferenceEquals(null, left)) return true;
                if (ReferenceEquals(null, right)) return false;
                return left.CompareTo(right) <= 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool operator >=(UsuarioView left, UsuarioView right)
        {
            try
            {
                if (ReferenceEquals(left, right)) return true;
                if (ReferenceEquals(null, left)) return false;
                if (ReferenceEquals(null, right)) return true;
                return left.CompareTo(right) >= 0;
            }
            catch
            {
                return false;
            }
        }

        public int CompareTo(UsuarioView? other)
        {
            if (other == null)
                return 1;
            if (ReferenceEquals(this, other))
                return 0;
            return string.Compare(this.Id, other.Id, StringComparison.Ordinal);
        }

    }
}
       
    


