// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TheSettings.Binding;

namespace TheSettings.Wpf.Binding.Adapters
{
    public class MultiSelectorSelectedItemsAdapter : ICollectionAdapter
    {
        private readonly MultiSelector _control;
        private readonly Func<object, object> _itemKeySelector;
        private bool _isControlSelectionChangedCallbackDisabled;

        public MultiSelectorSelectedItemsAdapter(
            MultiSelector control,
            Func<object, object> itemKeySelector)
        {
            if (control == null) throw new ArgumentNullException("control");
            if (itemKeySelector == null) throw new ArgumentNullException("itemKeySelector");
            _control = control;
            _control.SelectionChanged += OnControlSelectionChanged;
            _itemKeySelector = itemKeySelector;
        }

        public CollectionChangedCallbackHandler CollectionChangedCallback { private get; set; }

        public static ICollectionAdapter Create(DependencyObject control, Func<object, object> itemKeySelector)
        {
            if (itemKeySelector == null) throw new ArgumentNullException("itemKeySelector");
            var multiSelectorControl = control as MultiSelector;
            return multiSelectorControl != null
                ? new MultiSelectorSelectedItemsAdapter(multiSelectorControl, itemKeySelector)
                : null;
        }

        public IEnumerable GetItems()
        {
            var selectedItems = GetControlSelectedItems();
            var keysOfSelectedItems = ConvertToKeys(selectedItems);
            return keysOfSelectedItems;
        }

        public void SetItems(IEnumerable newItems)
        {
            if (newItems == null) throw new ArgumentNullException("newItems");
            var keysOfSelectedItems = newItems;
            _isControlSelectionChangedCallbackDisabled = true;
            try
            {
                SynchronizeControlSelectedItems(keysOfSelectedItems);
            }
            finally
            {
                _isControlSelectionChangedCallbackDisabled = false;
            }
        }

        private void SynchronizeControlSelectedItems(IEnumerable keysOfSelectedItems)
        {
            var controlItems = GetControlItems();
            var keysOfSelectedItemsSet = new HashSet<object>(keysOfSelectedItems.OfType<object>());
            foreach (var item in controlItems)
            {
                if (ShouldBeSelected(item, keysOfSelectedItemsSet))
                {
                    SelectItem(item);
                }
                else if (ShouldBeDeselected(item, keysOfSelectedItemsSet))
                {
                    DeselectItem(item);
                }
            }
        }

        private void SelectItem(object item)
        {
            _control.SelectedItems.Add(item);
        }

        private void DeselectItem(object item)
        {
            _control.SelectedItems.Remove(item);
        }

        private bool ShouldBeSelected(object item, ISet<object> keysOfItemsToSelect)
        {
            var itemKey = ConvertToKey(item);
            return keysOfItemsToSelect.Contains(itemKey);
        }

        private bool ShouldBeDeselected(object item, ISet<object> keysOfItemsToSelect)
        {
            var itemKey = ConvertToKey(item);
            return GetControlSelectedItems().Contains(item) &&
                !keysOfItemsToSelect.Contains(itemKey);
        }

        private IEnumerable<object> GetControlItems()
        {
            var items = _control.ItemsSource ?? _control.Items;
            return items.OfType<object>();
        }

        private IEnumerable<object> GetControlSelectedItems()
        {
            var selectedItems = _control.SelectedItems;
            return selectedItems.OfType<object>();
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
            var keysOfSelectedItems = ConvertToKeys(e.AddedItems);
            var keysOfDeselectedItems = ConvertToKeys(e.RemovedItems);
            callback(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, keysOfSelectedItems));
            callback(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, keysOfDeselectedItems));
        }

        private object[] ConvertToKeys(IEnumerable source)
        {
            return source.OfType<object>().Select(ConvertToKey).ToArray();
        }

        private object ConvertToKey(object item)
        {
            return _itemKeySelector(item);
        }
    }
}