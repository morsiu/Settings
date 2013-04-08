// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.ComponentModel;
using System.Windows;

namespace TheSettings.Binding.ValueAdapters
{
    public sealed class DependencyPropertyAdapter : IValueAdapter, IDisposable
    {
        private readonly DependencyProperty _property;
        private readonly DependencyPropertyDescriptor _descriptor;
        private readonly DependencyObject _target;
        private bool _isDisposed;
        private Action<object> _valueChangedCallback;

        public DependencyPropertyAdapter(DependencyObject target, DependencyProperty property)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (property == null) throw new ArgumentNullException("property");
            _target = target;
            _property = property;
            _descriptor = DependencyPropertyDescriptor.FromProperty(property, property.OwnerType);
            _valueChangedCallback = newValue => { };
            // TODO: Use weak event pattern here
            _descriptor.AddValueChanged(target, PropertyChangedHandler);
        }

        private void PropertyChangedHandler(object sender, EventArgs e)
        {
            var value = GetValue();
            _valueChangedCallback(value);
        }

        public Action<object> ValueChangedCallback
        {
            set 
            {
                FailIfDisposed();
                _valueChangedCallback = value ?? (newValue => { });
            }
        }

        public object GetValue()
        {
            FailIfDisposed();
            return _target.GetValue(_property);
        }

        public void SetValue(object value)
        {
            FailIfDisposed();
            if (!_property.ReadOnly)
            {
                _target.SetValue(_property, value);
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _descriptor.RemoveValueChanged(_target, PropertyChangedHandler);
            _isDisposed = true;
        }

        private void FailIfDisposed()
        {
            if (_isDisposed) throw new ObjectDisposedException("DependencyPropertyAdapter");
        }
    }
}
