using System.Collections.Generic;

namespace PSS.amg538.Practica_02
{
    public interface ISecuencia<T>
    {
        void Añadir(T item);
        bool Eliminar(T item);
        bool Contiene(T item);
        void Limpiar();
        int Cuenta { get; }
        void Ordenar(IComparer<T> comparador);
        T this[int i] { get; set; }
    }
}