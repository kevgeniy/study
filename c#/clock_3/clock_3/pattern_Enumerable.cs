using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clock_3
{
    partial class Program
    {
        interface pattern_IEnumerator_logic<T> : IDisposable
        {
            bool MoveNext(out T next);
        }

        class pattern_IEnumerable<T> : IEnumerable<T>
        {
            private IEnumerator<T> _enumerator;

            public pattern_IEnumerable(IEnumerator<T> enumerator)
            {
                _enumerator = enumerator;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _enumerator;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            public void Dispose()
            {
                _enumerator.Dispose();
            }
        }
        class pattern_IEnumerator<T> : IEnumerator<T>
        {
            private T _current;
            pattern_IEnumerator_logic<T> _logic;
            public pattern_IEnumerator(pattern_IEnumerator_logic<T> logic)
            {
                _logic = logic;
            }
            public bool MoveNext()
            {
                T next;
                bool fl = _logic.MoveNext(out next);
                if (fl)
                {
                    _current = next;
                    return true;
                }
                return false;
            }
            public void Reset()
            {

            }
            public T Current { get { return _current; } }
            object IEnumerator.Current { get { return Current; } }

            public void Dispose()
            {
                _logic.Dispose();
            }
        }
        class IntegersDI : pattern_IEnumerator_logic<int>
        {
            private int _n;
            public IntegersDI(int begin)
            {
                _n = begin;
            }
            public bool MoveNext(out int next)
            {
                next = ++_n;
                return true;
            }
            public void Dispose()
            {

            }
        }
    }
}
