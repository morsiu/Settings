﻿// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;

namespace TheSettings.Binding.ValueAdapters
{
    public class ConvertingAdapter : IValueAdapter
    {
        private readonly IValueAdapter _sourceAdapter;
        private readonly IValueConverter _converter;
        private Action<object> _valueChangedCallback = value => { };

        public ConvertingAdapter(IValueAdapter sourceAdapter, IValueConverter converter)
        {
            if (converter == null) throw new ArgumentNullException("converter");
            if (sourceAdapter == null) throw new ArgumentNullException("sourceAdapter");
            _converter = converter;
            _sourceAdapter = sourceAdapter;
            _sourceAdapter.ValueChangedCallback = OnSourceAdapterValueChanged;
        }

        public Action<object> ValueChangedCallback
        {
            set { _valueChangedCallback = value ?? (_ => { }); }
        }

        public object GetValue()
        {
            var value = _sourceAdapter.GetValue();
            var convertedValue = _converter.ConvertToTarget(value);
            return convertedValue;
        }

        public void SetValue(object value)
        {
            var convertedValue = _converter.ConvertToSource(value);
            _sourceAdapter.SetValue(convertedValue);
        }

        private void OnSourceAdapterValueChanged(object value)
        {
            var convertedValue = _converter.ConvertToTarget(value);
            _valueChangedCallback(convertedValue);
        }
    }
}