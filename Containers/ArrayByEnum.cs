using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAC2.Containers
{
    /// <summary>
    /// Array of T index by enum E. Query by enum or equivalent int.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="E"></typeparam>
    public class ArrayByEnum<T,E> : IEnumerable where E : Enum
    {
        public readonly T[] Array;
        private readonly int _first;

        public readonly int Length;

        public ArrayByEnum()
        {
            if (Enum.GetValues(typeof(E)).Length < 1)
            {
                Array = new T[0];
                Length = 0;
            }
            else
            {
                _first = Convert.ToInt32(Enum.GetValues(typeof(E)).Cast<E>().Min());
                int last = Convert.ToInt32(Enum.GetValues(typeof(E)).Cast<E>().Max());
                Array = new T[1 + last - _first];
                Length = Array.Length;
            }
        }

        public T this[E key]
        {
            get { return Array[Convert.ToInt32(key) - _first]; }
            set { Array[Convert.ToInt32(key) - _first] = value; }
        }

        public T this[int key]
        {
            get { return Array[key - _first]; }
            set { Array[key - _first] = value; }
        }

        public IEnumerator GetEnumerator()
        {
            return Enum.GetValues(typeof(E)).Cast<E>().Select(i => this[i]).GetEnumerator();
        }
    }
}
