// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using TheSettings.Binding;
using TheSettings.Infrastructure;

namespace TheSettings.Wpf.Binding.Adapters
{
    public class SelectorItemsSourceViewAdapter : Disposable, IValueAdapter
    {
        private readonly Selector _source;
        private readonly DependencyPropertyDescriptor _descriptor;
        private Action<object> _valueChangedCallback;

        public SelectorItemsSourceViewAdapter(Selector source)
        {
            if (source == null) throw new ArgumentNullException("source");
            _source = source;
            _valueChangedCallback = _ => { };
            _descriptor = DependencyPropertyDescriptor.FromProperty(Selector.ItemsSourceProperty, typeof(Selector));
            _descriptor.AddValueChanged(source, OnSourceValueChanged);
        }

        private void OnSourceValueChanged(object sender, EventArgs e)
        {
            var newValue = GetValue();
            _valueChangedCallback(newValue);
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
            return _source.ItemsSource as ICollectionView
                ?? CollectionViewSource.GetDefaultView(_source.ItemsSource);
        }

        public void SetValue(object value)
        {
            FailIfDisposed();
            _source.ItemsSource = value as IEnumerable;
        }

        protected override void DisposeManaged()
        {
            _descriptor.RemoveValueChanged(_source, OnSourceValueChanged);
            Dispose(_descriptor);
        }
    }
}