// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using TheSettings.Binding.Wpf;

namespace TheSettings.Binding.ValueAdapters
{
    public class SettingAdapter : IValueAdapter
    {
        private readonly ISettingsStoreAccessor _storeAccessor;
        private readonly SettingsNamespace _settingNamespace;
        private readonly string _settingName;
        private readonly object _storeKey;

        public SettingAdapter(
            ISettingsStoreAccessor storeAccessor,
            object storeKey,
            SettingsNamespace @namespace,
            string name)
        {
            if (storeAccessor == null) throw new ArgumentNullException("storeAccessor");
            if (@namespace == null) throw new ArgumentNullException("namespace");
            if (name == null) throw new ArgumentNullException("name");
            _storeAccessor = storeAccessor;
            _settingNamespace = @namespace;
            _settingName = name;
            _storeKey = storeKey;
        }

        public Action<object> ValueChangedCallback
        {
            set { }
        }

        public object GetValue()
        {
            return _storeAccessor.GetSetting(_storeKey, _settingNamespace, _settingName);
        }

        public void SetValue(object value)
        {
            _storeAccessor.SetSetting(_storeKey, _settingNamespace, _settingName, value);
        }
    }
}
