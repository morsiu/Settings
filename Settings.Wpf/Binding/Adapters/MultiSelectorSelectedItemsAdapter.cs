// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TheSettings.Binding;
using TheSettings.Wpf.Binding.Infrastructure;

namespace TheSettings.Wpf.Binding.Adapters
{
    public class MultiSelectorSelectedItemsAdapter : ICollectionAdapter
    {
        private readonly MultiSelector _control;
        private readonly Func<object, object> _itemKeySelector;
        private readonly SelectionPersister _selectionPersister;
        private bool _isControlSelectionChangedCallbackDisabled;

        public MultiSelectorSelectedItemsAdapter(
            MultiSelector control,
            Func<object, object> itemKeySelector)
        {
            _control = control;
            _control.SelectionChanged += OnControlSelectionChanged;
            DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ItemsControl))
                .AddValueChanged(_control, OnControlItemsSourceChanged);
            _itemKeySelector = itemKeySelector;
            _selectionPersister = new SelectionPersister(GetControlItems(), SetItems);
        }

        public CollectionChangedCallbackHandler CollectionChangedCallback { private get; set; }

        public static ICollectionAdapter Create(DependencyObject target, Func<object, object> keySelector)
        {
            var multiSelector = target as MultiSelector;
            return multiSelector != null
                ? new MultiSelectorSelectedItemsAdapter(multiSelector, keySelector)
                : null;
        }

        public IEnumerable<object> GetItems()
        {
            return _control.SelectedItems.OfType<object>().Select(_itemKeySelector);
        }

        public void SetItems(IEnumerable<object> keysOfItemsToSelect)
        {
            _isControlSelectionChangedCallbackDisabled = true;
            try
            {
                var keyList = keysOfItemsToSelect.ToList();
                _selectionPersister.SetTargetSelection(keyList);
                SelectControlItems(keyList);
                DeselectControlItems(keyList);
            }
            finally
            {
                _isControlSelectionChangedCallbackDisabled = false;
            }
        }

        private void DeselectControlItems(IEnumerable<object> keysOfSelected)
        {
            var itemsToDeselect = _control.SelectedItems.OfType<object>()
                .Where(item => !keysOfSelected.Contains(_itemKeySelector(item)))
                .ToList();
            foreach (var item in itemsToDeselect)
            {
                _control.SelectedItems.Remove(item);
            }
        }

        private IEnumerable GetControlItems()
        {
            return _control.ItemsSource ?? _control.Items;
        }

        private IEnumerable<object> GetKeys(IEnumerable source)
        {
            return source.OfType<object>().Select(item => _itemKeySelector(item));
        }

        private void OnControlItemsSourceChanged(object sender, EventArgs e)
        {
            _selectionPersister.ResetItemsCollection(_control.ItemsSource);
        }

        private void OnControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isControlSelectionChangedCallbackDisabled)
            {
                return;
            }
            var callback = CollectionChangedCallback;
            if (callback == null)
            {
                return;
            }
            var keysOfAdded = GetKeys(e.AddedItems);
            var keysOfRemoved = GetKeys(e.RemovedItems);
            callback(keysOfAdded, keysOfRemoved);
        }

        private void SelectControlItems(IEnumerable<object> keysOfSelected)
        {
            var itemsToSelect = GetControlItems().OfType<object>()
                .Where(item => keysOfSelected.Contains(_itemKeySelector(item)))
                .ToList();
            foreach (var item in itemsToSelect)
            {
                _control.SelectedItems.Add(item);
            }
        }
    }
}