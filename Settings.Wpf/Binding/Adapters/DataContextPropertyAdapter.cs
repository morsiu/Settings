// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Windows;
using TheSettings.Binding;
using TheSettings.Binding.ValueAdapters;
using TheSettings.Infrastructure;

namespace TheSettings.Wpf.Binding.Adapters
{
    public class DataContextPropertyAdapter : Disposable, IValueAdapter
    {
        private readonly DependencyPropertyAdapter _dataContextAdapter;
        private readonly string _propertyInDataContext;
        private IValueAdapter _valueAdapter;
        private Action<object> _valueChangedCallback;

        public DataContextPropertyAdapter(FrameworkElement target, string propertyInDataContext)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (propertyInDataContext == null) throw new ArgumentNullException("propertyInDataContext");
            _propertyInDataContext = propertyInDataContext;
            _dataContextAdapter = new DependencyPropertyAdapter(target, FrameworkElement.DataContextProperty);
            _dataContextAdapter.ValueChangedCallback = OnDataContextChanged;
            _valueChangedCallback = newValue => { };
            _valueAdapter = CreateValueAdapter();
        }

        public Action<object> ValueChangedCallback
        {
            set
            {
                FailIfDisposed();
                _valueChangedCallback = value ?? (newValue => { });
                if (_valueAdapter != null)
                {
                    _valueAdapter.ValueChangedCallback = _valueChangedCallback;
                }
            }
        }

        public object GetValue()
        {
            FailIfDisposed();
            if (_valueAdapter == null)
            {
                return SettingsConstants.NoValue;
            }
            return _valueAdapter.GetValue();
        }

        public void SetValue(object value)
        {
            FailIfDisposed();
            if (_valueAdapter == null)
            {
                return;
            }
            _valueAdapter.SetValue(value);
        }

        private void ClearValueAdapter()
        {
            Dispose(_valueAdapter);
            _valueAdapter = null;
        }

        private IValueAdapter CreateValueAdapter()
        {
            var dataContext = _dataContextAdapter.GetValue();
            if (dataContext == null)
                return null;
            var propertyInfo = dataContext.GetType().GetProperty(_propertyInDataContext);
            if (propertyInfo == null)
                return null;
            var valueAdapter = new ClrPropertyAdapter(dataContext, propertyInfo);
            valueAdapter.ValueChangedCallback = _valueChangedCallback;
            return valueAdapter;
        }

        private void OnDataContextChanged(object newDataContext)
        {
            ClearValueAdapter();
            _valueAdapter = CreateValueAdapter();
            var value = GetValue();
            _valueChangedCallback(value);
        }

        protected override void DisposeManaged()
        {
            Dispose(_valueAdapter);
            _dataContextAdapter.Dispose();
            _valueChangedCallback = null;
        }
    }
}