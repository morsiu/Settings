// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheSettings.Binding.Infrastructure;
using TheSettings.Infrastructure.Collections;

namespace TheSettings.Binding
{
    /// <summary>
    /// Binds source set to target collection.
    /// </summary>
    /// <remarks>
    /// Supports any object as source value.
    /// Objects of type IEnumerable are converted to ISet instance that uses specified comparer.
    /// Objects of other types are converted to empty set.
    /// </remarks>
    public class SetBinding : CollectionBindingBase
    {
        private readonly IEqualityComparer<object> _itemComparer;

        /// <summary>
        /// Constructs new instance that binds source set with target collection
        /// and uses comparer to determine items equality.
        /// </summary>
        /// <param name="targetAdapter">Adapter for target collection.</param>
        /// <param name="sourceAdapter">Adapter for source set.</param>
        /// <param name="itemComparer">Comparer that is used for determing items equality.</param>
        public SetBinding(
            ICollectionAdapter targetAdapter,
            IValueAdapter sourceAdapter,
            IEqualityComparer<object> itemComparer)
            : base(targetAdapter, sourceAdapter)
        {
            if (itemComparer == null) throw new ArgumentNullException("itemComparer");
            _itemComparer = itemComparer;
        }

        protected override bool IsCollectionCompatible(object setObject)
        {
            var set = setObject as HashSet<object>;
            return set != null && Equals(set.Comparer, _itemComparer);
        }

        protected override ICollectionUpdater GetUpdaterFor(ICollection<object> collection)
        {
            return new CollectionUpdater<object>(collection);
        }

        protected override ICollection<object> CreateCollection(IEnumerable items)
        {
            return new HashSet<object>(items.OfType<object>(), _itemComparer);
        }
    }
}