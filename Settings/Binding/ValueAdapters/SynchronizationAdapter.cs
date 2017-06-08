// Copyright (C) 2015 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using TheSettings.Infrastructure;

namespace TheSettings.Binding.ValueAdapters
{
    public sealed class SynchronizationAdapter : Disposable, IValueAdapter
    {
        private readonly SynchronizationGroup _synchronizationGroup;
        private readonly IValueAdapter _valueAdapter;
        private Action<object> _valueChangedAdapter;

        public SynchronizationAdapter(IValueAdapter valueAdapter, SynchronizationGroup synchronizationGroup)
        {
            _valueAdapter = valueAdapter;
            _valueAdapter.ValueChangedCallback = OnValueAdapterValueChanged;
            _synchronizationGroup = synchronizationGroup;
        }

        public Action<object> ValueChangedCallback
        {
            set { _valueChangedAdapter = value ?? (_ => { }); }
        }

        public object GetValue()
        {
            return _valueAdapter.GetValue();
        }

        public void SetValue(object value)
        {
            using (_synchronizationGroup.DisableValueChangedCallbacks())
            {
                _valueAdapter.SetValue(value);
            }
        }

        protected override void DisposeManaged()
        {
            Dispose(_valueAdapter);
        }

        private void OnValueAdapterValueChanged(object newValue)
        {
            if (_synchronizationGroup.AreValueChangeCallbacksDisabled)
            {
                return;
            }
            _valueChangedAdapter(newValue);
        }
    }
}