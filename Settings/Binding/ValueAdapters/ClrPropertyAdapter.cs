// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.ComponentModel;
using System.Reflection;
using TheSettings.Infrastructure;

namespace TheSettings.Binding.ValueAdapters
{
    public sealed class ClrPropertyAdapter : Disposable, IValueAdapter
    {
        private readonly object _target;
        private readonly PropertyInfo _propertyInfo;
        private Action<object> _valueChangedCallback;

        public ClrPropertyAdapter(object target, PropertyInfo propertyInfo)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (propertyInfo == null) throw new ArgumentNullException("propertyInfo");
            _propertyInfo = propertyInfo;
            _target = target;
            var notifyingTarget = _target as INotifyPropertyChanged;
            if (notifyingTarget != null)
            {
                _valueChangedCallback = newValue => { };
                PropertyChangedEventManager.AddHandler(notifyingTarget, PropertyChangedHandler, _propertyInfo.Name);
            }
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
            return _propertyInfo.CanRead
                ? _propertyInfo.GetValue(_target)
                : SettingsConstants.NoValue;
        }

        public void SetValue(object value)
        {
            FailIfDisposed();
            if (_propertyInfo.CanWrite)
            {
                _propertyInfo.SetValue(_target, value);
            }
        }

        protected override void DisposeManaged()
        {
            var notifyingTarget = _target as INotifyPropertyChanged;
            if (notifyingTarget != null)
            {
                PropertyChangedEventManager.RemoveHandler(notifyingTarget, PropertyChangedHandler, _propertyInfo.Name);
            }
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            var value = GetValue();
            _valueChangedCallback(value);
        }
    }
}