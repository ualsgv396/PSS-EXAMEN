using System.Collections;
using System.Collections.Generic;

namespace PSS.amg538.Practica_02
{
    public class Secuencia<T> : ISecuencia<T>, IEnumerable<T>
    {
        private List<T> _elementos = new List<T>();
        private IComparer<T> _comparador;

        public void Añadir(T item)
        {
            _elementos.Add(item);
        }

        public bool Eliminar(T item)
        {
            return _elementos.Remove(item);
        }

        public bool Contiene(T item)
        {
            return _elementos.Contains(item);
        }

        public void Limpiar()
        {
            _elementos.Clear();
        }

        public int Cuenta => _elementos.Count;

        public void Ordenar(IComparer<T> comparador)
        {
            _comparador = comparador;
            _elementos.Sort(comparador);
        }

        public T this[int i]
        {
            get => _elementos[i];
            set => _elementos[i] = value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _elementos.Count - 1; i >= 0; i--)
                yield return _elementos[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<T> RecorridoAdelante
        {
            get
            {
                for (int i = 0; i < _elementos.Count; i++)
                    yield return _elementos[i];
            }
        }

        public IEnumerable<T> RecorridoAtras
        {
            get
            {
                for (int i = _elementos.Count - 1; i >= 0; i--)
                    yield return _elementos[i];
            }
        }
        public IEnumerable<T> RecorridoAscendente
        {
            get
            {
                List<T> copia = new List<T>(_elementos);
                if (_comparador != null)
                    copia.Sort(_comparador);
                else
                    copia.Sort();

                foreach (T item in copia)
                    yield return item;
            }
        }
        public IEnumerable<T> RecorridoDescendente
        {
            get
            {
                List<T> copia = new List<T>(_elementos);
                if (_comparador != null)
                    copia.Sort(_comparador);
                else
                    copia.Sort();

                for (int i = copia.Count - 1; i >= 0; i--)
                    yield return copia[i];
            }
        }
    }
}