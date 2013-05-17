// // Copyright (C) 2013 Łukasz Mrozek
// // Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// // The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Diagnostics;

namespace TheSettings.Binding.ValueAdapters
{
    public class IgnoreExceptionsAdapter : IValueAdapter
    {
        private readonly IValueAdapter _sourceAdapter;

        public IgnoreExceptionsAdapter(IValueAdapter sourceAdapter)
        {
            if (sourceAdapter == null) throw new ArgumentNullException("sourceAdapter");
            _sourceAdapter = sourceAdapter;
        }

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
                HandleException(e, true);
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
                HandleException(e, false);
            }
        }

        [Conditional("DEBUG")]
        private void HandleException(Exception exception, bool isReading)
        {
            Debug.Write(string.Format("Exception occured while {0} value from/to settings value adapter.", isReading ? "reading" : "writing"), "Settings Bindings");
        }
    }
}