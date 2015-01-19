// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using TheSettings.Binding;
using TheSettings.Infrastructure;

namespace TheSettings.Wpf.Binding.Adapters
{
    public class SelectorItemsSourceViewAdapter : Disposable, IValueAdapter
    {
        private readonly IValueAdapter _sourceItemsSourceAdapter;
        private Action<object> _valueChangedCallback;

        public SelectorItemsSourceViewAdapter(Selector source)
        {
            if (source == null) throw new ArgumentNullException("source");
            _valueChangedCallback = _ => { };
            _sourceItemsSourceAdapter = ValueAdapterFactory.CreateAdapterForDependencyProperty(source, ItemsControl.ItemsSourceProperty);
            _sourceItemsSourceAdapter.ValueChangedCallback = OnSourceValueChanged;
        }

        public Action<object> ValueChangedCallback
        {
            set
            {
                FailIfDisposed();
                _valueChangedCallback = value ?? (_ => { });
            }
        }

        public object GetValue()
        {
            FailIfDisposed();
            var sourceItemsSource = _sourceItemsSourceAdapter.GetValue();
            return sourceItemsSource as ICollectionView
                ?? CollectionViewSource.GetDefaultView(sourceItemsSource);
        }

        public void SetValue(object value)
        {
            FailIfDisposed();
            _sourceItemsSourceAdapter.SetValue(value as IEnumerable);
        }

        private void OnSourceValueChanged(object chan)
        {
            if (IsDisposed)
            {
                return;
            }
            var newValue = GetValue();
            _valueChangedCallback(newValue);
        }

        protected override void DisposeManaged()
        {
            Dispose(_sourceItemsSourceAdapter);
        }
    }
}