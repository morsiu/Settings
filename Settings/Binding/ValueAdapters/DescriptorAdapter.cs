// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.ComponentModel;
using TheSettings.Infrastructure;

namespace TheSettings.Binding.ValueAdapters
{
    public class DescriptorAdapter : Disposable, IValueAdapter
    {
        private readonly PropertyDescriptor _descriptor;
        private readonly object _target;
        private bool _isSettingValue;
        private Action<object> _valueChangedCallback;

        public DescriptorAdapter(object target, PropertyDescriptor descriptor)
        {
            _target = target;
            _descriptor = descriptor;
            _valueChangedCallback = newValue => { };
            descriptor.AddValueChanged(target, OnValueChanged);
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (_isSettingValue)
            {
                return;
            }
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
            var value = _descriptor.GetValue(_target);
            return value;
        }

        public void SetValue(object value)
        {
            FailIfDisposed();
            _isSettingValue = true;
            try
            {
                _descriptor.SetValue(_target, value);
            }
            finally
            {
                _isSettingValue = false;
            }
        }

        protected override void DisposeManaged()
        {
            _descriptor.RemoveValueChanged(_target, OnValueChanged);
        }
    }
}