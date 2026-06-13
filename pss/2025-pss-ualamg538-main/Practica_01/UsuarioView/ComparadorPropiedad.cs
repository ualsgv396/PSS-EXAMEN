using System.ComponentModel;

namespace PSS.amg538.Practica_02
{
    public class ComparadorPropiedad<T> : IComparer<T>
    {
        private readonly string _nombrePropiedad;

        public ComparadorPropiedad(string nombrePropiedad)
        {
            _nombrePropiedad = nombrePropiedad;
        }

        private PropertyDescriptor GetProperty(string name)
        {
            T item = (T)Activator.CreateInstance(typeof(T));
            PropertyDescriptor propName = null;
            foreach (PropertyDescriptor propDesc in TypeDescriptor.GetProperties(item))
            {
                if (propDesc.Name.Contains(name))
                    propName = propDesc;
            }
            return propName;
        }

        public int Compare(T a, T b)
        {
            PropertyDescriptor prop = GetProperty(_nombrePropiedad);

            if (prop == null)
                throw new ArgumentException($"No se encontró la propiedad '{_nombrePropiedad}' en {typeof(T).Name}");

            IComparable valorA = prop.GetValue(a) as IComparable;
            IComparable valorB = prop.GetValue(b) as IComparable;

            if (valorA == null && valorB == null) return 0;
            if (valorA == null) return -1;
            if (valorB == null) return 1;

            return valorA.CompareTo(valorB);
        }
    }
}