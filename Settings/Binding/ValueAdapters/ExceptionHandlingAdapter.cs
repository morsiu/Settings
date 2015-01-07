﻿// // Copyright (C) 2013 Łukasz Mrozek
// // Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// // The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;

namespace TheSettings.Binding.ValueAdapters
{
    public class ExceptionHandlingAdapter : IValueAdapter
    {
        private readonly IValueAdapter _sourceAdapter;
        private readonly ExceptionHandler _exceptionHandler;

        public ExceptionHandlingAdapter(IValueAdapter sourceAdapter, ExceptionHandler exceptionHandler)
        {
            if (sourceAdapter == null) throw new ArgumentNullException("sourceAdapter");
            if (exceptionHandler == null) throw new ArgumentNullException("exceptionHandler");
            _sourceAdapter = sourceAdapter;
            _exceptionHandler = exceptionHandler;
        }

        public enum Action { GetValue, SetValue }

        public delegate bool ExceptionHandler(Action action, Exception exception);

        public Action<object> ValueChangedCallback
        {
            set { _sourceAdapter.ValueChangedCallback = value; }
        }

        public object GetValue()
        {
            try
            {
                return _sourceAdapter.GetValue();
            }
            catch (Exception e)
            {
                if (!_exceptionHandler(Action.GetValue, e))
                {
                    throw;
                }
            }
            return SettingsConstants.NoValue;
        }

        public void SetValue(object value)
        {
            try
            {
                _sourceAdapter.SetValue(value);
            }
            catch (Exception e)
            {
                if (!_exceptionHandler(Action.SetValue, e))
                {
                    throw;
                }
            }
        }
    }
}