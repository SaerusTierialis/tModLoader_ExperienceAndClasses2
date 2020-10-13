using System;
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
    public class ArrayByEnum<T,E> where E : Enum
    {
        private readonly T[] _data;
        private readonly int _first;

        public readonly int Length;

        public ArrayByEnum()
        {
            _first = Convert.ToInt32(Enum.GetValues(typeof(E)).Cast<E>().Min());
            int last = Convert.ToInt32(Enum.GetValues(typeof(E)).Cast<E>().Max());
            _data = new T[1 + last - _first];
            Length = _data.Length;
        }

        public T this[E key]
        {
            get { return _data[Convert.ToInt32(key) - _first]; }
            set { _data[Convert.ToInt32(key) - _first] = value; }
        }

        public T this[int key]
        {
            get { return _data[key - _first]; }
            set { _data[key - _first] = value; }
        }
    }
}
