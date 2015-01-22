// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Windows;
using System.Windows.Data;
using TheSettings.Infrastructure;

namespace TheSettings.Binding.ValueAdapters
{
    public sealed class DependencyPropertyAdapter : Disposable, IValueAdapter
    {
        private readonly DependencyProperty _property;
        private readonly TargetAdapter _targetAdapter;
        private bool _isSettingValue;
        private Action<object> _valueChangedCallback;

        public DependencyPropertyAdapter(DependencyObject target, DependencyProperty property)
        {
            _property = property;
            if (target == null)
                throw new ArgumentNullException("target");
            if (property == null)
                throw new ArgumentNullException("property");
            _valueChangedCallback = newValue => { };
            _targetAdapter = new TargetAdapter(target, property, PropertyChangedHandler);
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
            return _targetAdapter.Value;
        }

        public void SetValue(object value)
        {
            FailIfDisposed();
            if (!_property.ReadOnly)
            {
                _isSettingValue = true;
                try
                {
                    _targetAdapter.Value = value;
                }
                finally
                {
                    _isSettingValue = false;
                }
            }
        }

        protected override void DisposeManaged()
        {
            _targetAdapter.Disable();
        }

        private void PropertyChangedHandler()
        {
            if (_isSettingValue)
            {
                return;
            }
            var value = GetValue();
            _valueChangedCallback(value);
        }

        private sealed class TargetAdapter : DependencyObject
        {
            public static readonly DependencyProperty ValueProperty
                = DependencyProperty.Register("Value", typeof(object), typeof(TargetAdapter), new PropertyMetadata(OnValueChanged));

            private readonly Action _changeCallback;
            private bool _isChangeCallbackActive;

            public TargetAdapter(DependencyObject target, DependencyProperty property, Action changeCallback)
            {
                if (target == null) throw new ArgumentNullException("target");
                if (property == null) throw new ArgumentNullException("property");
                if (changeCallback == null) throw new ArgumentNullException("changeCallback");
                Enable(target, property);
                _changeCallback = changeCallback;
                _isChangeCallbackActive = true;
            }

            public object Value
            {
                get { return GetValue(ValueProperty); }
                set { SetValue(ValueProperty, value); }
            }

            public void Disable()
            {
                _isChangeCallbackActive = false;
                BindingOperations.ClearBinding(this, ValueProperty);
            }

            private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var self = (TargetAdapter)d;
                if (self != null && self._isChangeCallbackActive && self._changeCallback != null)
                {
                    self._changeCallback();
                }
            }

            private void Enable(DependencyObject target, DependencyProperty property)
            {
                Value = target.GetValue(property);
                var bindingMode = property.ReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;
                BindingOperations.SetBinding(
                    this,
                    ValueProperty,
                    new System.Windows.Data.Binding
                    {
                        Source = target,
                        Mode = bindingMode,
                        BindsDirectlyToSource = true,
                        FallbackValue = property.DefaultMetadata.DefaultValue,
                        Path = new PropertyPath(property)
                    });
            }
        }
    }
}