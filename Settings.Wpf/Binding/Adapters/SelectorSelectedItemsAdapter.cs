// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TheSettings.Binding;
using TheSettings.Infrastructure;

namespace TheSettings.Wpf.Binding.Adapters
{
    public class SelectorSelectedItemsAdapter : Disposable, IValueAdapter
    {
        private readonly Selector _control;
        private readonly Func<object, object> _itemKeySelector;
        private bool _isControlSelectionChangedCallbackDisabled;
        private Action<object> _valueChangedCallback;

        public SelectorSelectedItemsAdapter(
            Selector control,
            Func<object, object> itemKeySelector)
        {
            if (control == null) throw new ArgumentNullException("control");
            if (itemKeySelector == null) throw new ArgumentNullException("itemKeySelector");
            _control = control;
            _control.SelectionChanged += OnControlSelectionChanged;
            _itemKeySelector = itemKeySelector;
        }

        public static IValueAdapter Create(DependencyObject control, Func<object, object> itemKeySelector)
        {
            if (itemKeySelector == null) throw new ArgumentNullException("itemKeySelector");
            var selectorControl = control as Selector;
            return selectorControl != null
                ? new SelectorSelectedItemsAdapter(selectorControl, itemKeySelector)
                : null;
        }

        public Action<object> ValueChangedCallback
        {
            set
            {
                FailIfDisposed();
                _valueChangedCallback = value;
            }
        }

        public object GetValue()
        {
            FailIfDisposed();
            var selectedItem = GetControlSelectedItem();
            var keyOfSelectedItem = ConvertToKey(selectedItem);
            return keyOfSelectedItem;
        }

        public void SetValue(object newValue)
        {
            FailIfDisposed();
            var keyOfSelectedItem = newValue;
            _isControlSelectionChangedCallbackDisabled = true;
            try
            {
                SynchronizeControlSelectedItem(keyOfSelectedItem);
            }
            finally
            {
                _isControlSelectionChangedCallbackDisabled = false;
            }
        }

        private void SynchronizeControlSelectedItem(object keyOfSelectedItem)
        {
            var controlItems = GetControlItems();
            foreach (var item in controlItems)
            {
                if (ShouldBeSelected(item, keyOfSelectedItem))
                {
                    SelectItem(item);
                }
            }
        }

        private void SelectItem(object item)
        {
            _control.SelectedItem = item;
        }

        private bool ShouldBeSelected(object item, object keyOfItemsToSelect)
        {
            var itemKey = ConvertToKey(item);
            return Equals(keyOfItemsToSelect, itemKey);
        }

        private IEnumerable<object> GetControlItems()
        {
            var items = _control.ItemsSource ?? _control.Items;
            return items.OfType<object>();
        }

        private object GetControlSelectedItem()
        {
            return _control.SelectedItem;
        }

        private void OnControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsDisposed || _isControlSelectionChangedCallbackDisabled)
            {
                return;
            }
            var callback = _valueChangedCallback;
            if (callback == null)
            {
                return;
            }
            var selectedItem = GetControlSelectedItem();
            var keyOfSelectedItem = ConvertToKey(selectedItem);
            callback(keyOfSelectedItem);
        }

        private object ConvertToKey(object item)
        {
            return _itemKeySelector(item);
        }

        protected override void DisposeManaged()
        {
            _control.SelectionChanged -= OnControlSelectionChanged;
        }
    }
}
