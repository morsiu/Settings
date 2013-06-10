// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheSettings.Infrastructure;

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
    public class SetBinding : Disposable, ISettingBinding
    {
        private readonly ICollectionAdapter _targetAdapter;
        private readonly IValueAdapter _sourceAdapter;
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
        {
            if (targetAdapter == null) throw new ArgumentNullException("targetAdapter");
            if (sourceAdapter == null) throw new ArgumentNullException("sourceAdapter");
            if (itemComparer == null) throw new ArgumentNullException("itemComparer");
            _targetAdapter = targetAdapter;
            _sourceAdapter = sourceAdapter;
            _itemComparer = itemComparer;
            _sourceAdapter.ValueChangedCallback = OnSourceValueChangedCallback;
            _targetAdapter.CollectionChangedCallback = OnTargetCollectionChangedCallback;
        }

        public void UpdateSource()
        {
            FailIfDisposed();
            var targetItemsSet = GetTargetItemsAsSet();
            _sourceAdapter.SetValue(targetItemsSet);
        }

        public void UpdateTarget()
        {
            FailIfDisposed();
            var sourceItemsSet = GetSourceItemsAsSet();
            if (sourceItemsSet != null)
            {
                _targetAdapter.SetItems(sourceItemsSet);
            }
        }

        private void OnSourceValueChangedCallback(object newSourceItemsObject)
        {
            var newSourceItemsSet = ConvertToSet(newSourceItemsObject);
            _targetAdapter.SetItems(newSourceItemsSet);
        }

        private void OnTargetCollectionChangedCallback(
            IEnumerable added,
            IEnumerable removed)
        {
            var sourceItemsSet = GetSourceItemsAsSet();
            foreach (var item in added)
            {
                sourceItemsSet.Add(item);
            }
            foreach (var item in removed)
            {
                sourceItemsSet.Remove(item);
            }
            _sourceAdapter.SetValue(sourceItemsSet);
        }

        private ISet<object> GetSourceItemsAsSet()
        {
            var sourceItemsObject = _sourceAdapter.GetValue();
            var sourceItemsSet = ConvertToSet(sourceItemsObject);
            return sourceItemsSet;
        }

        private ISet<object> GetTargetItemsAsSet()
        {
            var targetItems = _targetAdapter.GetItems();
            var targetItemsSet = CreateSet(targetItems);
            return targetItemsSet;
        }

        private ISet<object> ConvertToSet(object itemsObject)
        {
            if (IsSetCompatible(itemsObject))
            {
                return (ISet<object>)itemsObject;
            }

            var items = itemsObject as IEnumerable ?? Enumerable.Empty<object>();
            return CreateSet(items);
        }

        private bool IsSetCompatible(object setObject)
        {
            var set = setObject as HashSet<object>;
            return set != null && Equals(set.Comparer, _itemComparer);
        }

        private ISet<object> CreateSet(IEnumerable items)
        {
            return new HashSet<object>(items.OfType<object>(), _itemComparer);
        }

        protected override void DisposeManaged()
        {
            Dispose(_targetAdapter);
            Dispose(_sourceAdapter);
        }
    }
}