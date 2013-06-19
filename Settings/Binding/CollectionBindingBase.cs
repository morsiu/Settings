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
    public abstract class CollectionBindingBase : Disposable, ISettingBinding
    {
        private readonly ICollectionAdapter _targetAdapter;
        private readonly IValueAdapter _sourceAdapter;

        protected CollectionBindingBase(
            ICollectionAdapter targetAdapter,
            IValueAdapter sourceAdapter)
        {
            if (targetAdapter == null) throw new ArgumentNullException("targetAdapter");
            if (sourceAdapter == null) throw new ArgumentNullException("sourceAdapter");
            _targetAdapter = targetAdapter;
            _sourceAdapter = sourceAdapter;
            _sourceAdapter.ValueChangedCallback = OnSourceValueChangedCallback;
            _targetAdapter.CollectionChangedCallback = OnTargetCollectionChangedCallback;
        }

        public void UpdateSource()
        {
            FailIfDisposed();
            var targetItemsSet = GetTargetItemsAsCollection();
            _sourceAdapter.SetValue(targetItemsSet);
        }

        public void UpdateTarget()
        {
            FailIfDisposed();
            var sourceItemsSet = GetSourceItemsAsCollection();
            if (sourceItemsSet != null)
            {
                _targetAdapter.SetItems(sourceItemsSet);
            }
        }

        protected abstract ICollection<object> CreateCollection(IEnumerable items);

        protected abstract bool IsCollectionCompatible(object collectionObject);

        private ICollection<object> GetSourceItemsAsCollection()
        {
            var sourceItemsObject = _sourceAdapter.GetValue();
            var sourceItemsSet = ConvertToCollection(sourceItemsObject);
            return sourceItemsSet;
        }

        private ICollection<object> GetTargetItemsAsCollection()
        {
            var targetItems = _targetAdapter.GetItems();
            var targetItemsSet = CreateCollection(targetItems);
            return targetItemsSet;
        }

        private ICollection<object> ConvertToCollection(object itemsObject)
        {
            if (IsCollectionCompatible(itemsObject))
            {
                return (ICollection<object>)itemsObject;
            }

            var items = itemsObject as IEnumerable ?? Enumerable.Empty<object>();
            return CreateCollection(items);
        }

        private void OnSourceValueChangedCallback(object newSourceItemsObject)
        {
            var newSourceItemsSet = ConvertToCollection(newSourceItemsObject);
            _targetAdapter.SetItems(newSourceItemsSet);
        }

        private void OnTargetCollectionChangedCallback(
            IEnumerable added,
            IEnumerable removed)
        {
            var sourceItemsSet = GetSourceItemsAsCollection();
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

        protected override void DisposeManaged()
        {
            Dispose(_targetAdapter);
            Dispose(_sourceAdapter);
        }
    }
}