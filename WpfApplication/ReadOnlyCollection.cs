using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication
{
    public class ReadOnlyCollection<T> : IReadOnlyCollection<T>
    {
        private readonly ICollection<T> _collection;

        public ReadOnlyCollection(ICollection<T> collection)
        {
            _collection = collection;
        }

        public int Count
        {
            get { return _collection.Count; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
    }
}
