using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo.Library.Internal
{
    // https://github.com/NEventStore/NEventStore.Persistence.RavenDB/blob/master/src/NEventStore.Persistence.RavenDB/ResetableEnumerable.cs
    public class ResetableEnumerable<T> : IEnumerable<T>
    {
        private readonly Func<IEnumerable<T>> _source;

        public ResetableEnumerable(Func<IEnumerable<T>> source)
        {
            _source = source;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ResetableEnumerator<T>(_source);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private sealed class ResetableEnumerator<TItem> : IEnumerator<TItem>
        {
            private readonly Func<IEnumerable<TItem>> _source;
            private IEnumerator<TItem> _enumerator;

            public ResetableEnumerator(Func<IEnumerable<TItem>> source)
            {
                _source = source;
            }

            private IEnumerator<TItem> Enumerator
            {
                get { return _enumerator ?? (_enumerator = _source().GetEnumerator()); }
                set { _enumerator = value; }
            }

            public TItem Current => Enumerator.Current;

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                return Enumerator.MoveNext();
            }

            public void Reset()
            {
                Enumerator.Dispose();
                Enumerator = null;
            }

            public void Dispose()
            {
                Enumerator.Dispose();
            }
        }
    }
}
