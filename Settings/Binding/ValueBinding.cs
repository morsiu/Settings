// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using TheSettings.Infrastructure;

namespace TheSettings.Binding
{
    public class ValueBinding : Disposable, ISettingBinding
    {
        private readonly IValueAdapter _targetAdapter;
        private readonly IValueAdapter _sourceAdapter;

        public ValueBinding(IValueAdapter targetAdapter, IValueAdapter sourceAdapter)
        {
            if (targetAdapter == null) throw new ArgumentNullException("targetAdapter");
            if (sourceAdapter == null) throw new ArgumentNullException("sourceAdapter");
            _targetAdapter = targetAdapter;
            _sourceAdapter = sourceAdapter;
            _targetAdapter.ValueChangedCallback = _sourceAdapter.SetValue;
            _sourceAdapter.ValueChangedCallback = ForwardValueToTargetIfNotNoValue;
        }

        public void UpdateTarget()
        {
            FailIfDisposed();
            var value = _sourceAdapter.GetValue();
            ForwardValueToTargetIfNotNoValue(value);
        }

        protected override void DisposeManaged()
        {
            Dispose(_targetAdapter);
            Dispose(_sourceAdapter);
        }

        private void ForwardValueToTargetIfNotNoValue(object value)
        {
            if (value == SettingsConstants.NoValue)
            {
                return;
            }
            _targetAdapter.SetValue(value);
        }
    }
}