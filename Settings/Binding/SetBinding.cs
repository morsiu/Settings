// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TheSettings.Binding
{
    public class SetBinding : ISettingBinding
    {
        private readonly ICollectionAdapter _collectionAdapter;
        private readonly IValueAdapter _settingAdapter;
        private readonly IEqualityComparer<object> _comparer;

        public SetBinding(
            ICollectionAdapter collectionAdapter,
            IValueAdapter settingAdapter,
            IEqualityComparer<object> comparer)
        {
            if (collectionAdapter == null) throw new ArgumentNullException("collectionAdapter");
            if (settingAdapter == null) throw new ArgumentNullException("settingAdapter");
            if (comparer == null) throw new ArgumentNullException("comparer");
            _collectionAdapter = collectionAdapter;
            _settingAdapter = settingAdapter;
            _comparer = comparer;
            _settingAdapter.ValueChangedCallback = OnStoredItemsChangedCallback;
            _collectionAdapter.CollectionChangedCallback = OnCollectionChangedCallback;
        }

        public void UpdateSource()
        {
            var items = _collectionAdapter.GetItems();
            var itemsToStore = new HashSet<object>(items.OfType<object>(), _comparer);
            _settingAdapter.SetValue(itemsToStore);
        }

        public void UpdateTarget()
        {
            var storedItems = GetSourceItems();
            if (storedItems != null)
            {
                _collectionAdapter.SetItems(storedItems);
            }
        }

        private void OnCollectionChangedCallback(
            IEnumerable added,
            IEnumerable removed)
        {
            var storedItems = GetSourceItemsAsSet();
            foreach (var item in added)
            {
                storedItems.Add(item);
            }
            foreach (var item in removed)
            {
                storedItems.Remove(item);
            }
            _settingAdapter.SetValue(storedItems);
        }

        private void OnStoredItemsChangedCallback(object value)
        {
            var items = value as IEnumerable;
            if (items != null)
            {
                _collectionAdapter.SetItems(items);
            }
        }

        private IEnumerable GetSourceItems()
        {
            var value = _settingAdapter.GetValue();
            return value as IEnumerable;
        }

        private ISet<object> GetSourceItemsAsSet()
        {
            var value = _settingAdapter.GetValue();
            var storedItems = value as HashSet<object>;
            if ((value != null && value == storedItems) ||
                (storedItems != null && Equals(storedItems.Comparer, _comparer)))
            {
                return storedItems;
            }

            var sourceItems = value as IEnumerable ?? Enumerable.Empty<object>();
            return new HashSet<object>(sourceItems.OfType<object>(), _comparer);
        }
    }
}