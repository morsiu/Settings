// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using TheSettings.Binding;
using TheSettings.Binding.ValueAdapters;
using TheSettings.Infrastructure;

namespace TheSettings.Wpf.Binding.Adapters
{
    public class DataGridSelectedItemsAdapter : Disposable, ICollectionAdapter
    {
        private readonly DataGrid _control;
        private readonly ICollectionAdapter _selectorAdapter;
        private readonly ICollectionAdapter _multiSelectorAdapter;
        private CollectionChangedCallbackHandler _collectionChangedCallback;

        public DataGridSelectedItemsAdapter(DataGrid control, Func<object, object> itemKeySelector)
        {
            if (control == null) throw new ArgumentNullException("control");
            if (itemKeySelector == null) throw new ArgumentNullException("itemKeySelector");
            _collectionChangedCallback = instructions => { };
            _control = control;
            _selectorAdapter = new ValueToCollectionAdapter(new SelectorSelectedItemsAdapter(control, itemKeySelector));
            _multiSelectorAdapter = new MultiSelectorSelectedItemsAdapter(control, itemKeySelector);
            _selectorAdapter.CollectionChangedCallback = OnSelectorCollectionChanged;
            _multiSelectorAdapter.CollectionChangedCallback = OnMultiSelectorCollectionChanged;
        }

        public CollectionChangedCallbackHandler CollectionChangedCallback 
        {
            set
            {
                FailIfDisposed();
                _collectionChangedCallback = value ?? (instructions => { });
            }
        }

        public static ICollectionAdapter Create(DependencyObject control, Func<object, object> itemKeySelector)
        {
            if (itemKeySelector == null) throw new ArgumentNullException("itemKeySelector");
            var dataGridControl = control as DataGrid;
            return dataGridControl != null
                ? new DataGridSelectedItemsAdapter(dataGridControl, itemKeySelector)
                : null;
        }

        public IEnumerable GetItems()
        {
            FailIfDisposed();
            var activeAdapter = ActiveAdapter;
            var items = activeAdapter.GetItems();
            return items;
        }

        public void SetItems(IEnumerable items)
        {
            FailIfDisposed();
            var activeAdapter = ActiveAdapter;
            activeAdapter.SetItems(items);
        }

        private bool IsSelectorAdapterActive
        {
            get { return _control.SelectionMode == DataGridSelectionMode.Single; }
        }

        private bool IsMultiSelectorAdapterActive
        {
            get { return _control.SelectionMode == DataGridSelectionMode.Extended; }
        }

        private ICollectionAdapter ActiveAdapter
        {
            get { return IsSelectorAdapterActive ? _selectorAdapter : _multiSelectorAdapter; }
        }

        private void OnMultiSelectorCollectionChanged(NotifyCollectionChangedEventArgs instructions)
        {
            if (IsDisposed || IsSelectorAdapterActive)
            {
                return;
            }
            var callback = _collectionChangedCallback;
            callback(instructions);
        }

        private void OnSelectorCollectionChanged(NotifyCollectionChangedEventArgs instructions)
        {
            if (IsDisposed || IsMultiSelectorAdapterActive)
            {
                return;
            }
            var callback = _collectionChangedCallback;
            callback(instructions);
        }

        protected override void DisposeManaged()
        {
            Dispose(_multiSelectorAdapter);
            Dispose(_selectorAdapter);
        }
    }
}
