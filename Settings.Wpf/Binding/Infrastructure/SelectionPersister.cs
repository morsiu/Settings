// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace TheSettings.Wpf.Binding.Infrastructure
{
    /// <summary>
    /// Class that persists restored selection during initial filling of items collection.
    /// </summary>
    /// <remarks>
    /// Typically collection, for which selection should be remembered, is initially created empty
    /// and items are added to it one-by-one.
    /// Because of this items selection must be performed after each individually added item.
    /// </remarks>
    internal class SelectionPersister
    {
        private readonly Action<IEnumerable<object>> _setSelectionCallback;
        private INotifyCollectionChanged _itemsCollection;
        private ItemsCollectionState _itemsCollectionState;
        private IEnumerable<object> _targetSelectionKeys;

        public SelectionPersister(
            IEnumerable initialItemsCollection,
            Action<IEnumerable<object>> setSelectionCallback)
        {
            if (initialItemsCollection == null) throw new ArgumentNullException("initialItemsCollection");
            if (setSelectionCallback == null) throw new ArgumentNullException("setSelectionCallback");
            _setSelectionCallback = setSelectionCallback;
            ResetItemsCollection(initialItemsCollection);
        }

        private enum ItemsCollectionState
        {
            New,
            AddingItems,
            Stable
        }

        public void ResetItemsCollection(IEnumerable items)
        {
            _itemsCollectionState = ItemsCollectionState.New;
            DetachFromItemsCollection();
            _itemsCollection = items as INotifyCollectionChanged;
            AttachToItemsCollection();
            SetTargetSelection();
        }

        public void SetTargetSelection(IEnumerable<object> selectedItemsKeys)
        {
            _targetSelectionKeys = selectedItemsKeys;
        }

        private void AttachToItemsCollection()
        {
            if (_itemsCollection != null)
            {
                _itemsCollection.CollectionChanged += OnItemsCollectionChanged;
            }
        }

        private void DetachFromItemsCollection()
        {
            if (_itemsCollection != null)
            {
                _itemsCollection.CollectionChanged -= OnItemsCollectionChanged;
            }
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (_itemsCollectionState)
            {
                case ItemsCollectionState.New:
                    if (e.Action == NotifyCollectionChangedAction.Reset
                        || e.Action == NotifyCollectionChangedAction.Add)
                    {
                        _itemsCollectionState = ItemsCollectionState.AddingItems;
                    }
                    SetTargetSelection();
                    break;

                case ItemsCollectionState.AddingItems:
                    if (e.Action != NotifyCollectionChangedAction.Add)
                    {
                        _itemsCollectionState = ItemsCollectionState.Stable;
                    }
                    SetTargetSelection();
                    break;

                case ItemsCollectionState.Stable:
                    DetachFromItemsCollection();
                    _itemsCollection = null;
                    _targetSelectionKeys = null;
                    break;
            }
        }

        private void SetTargetSelection()
        {
            if (_targetSelectionKeys != null)
            {
                _setSelectionCallback(_targetSelectionKeys);
            }
        }
    }
}