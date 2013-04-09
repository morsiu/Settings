// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;

namespace TheSettings.Binding.Accessors
{
    public class DictionarySettingsStoreAccessor : ISettingsStoreAccessor
    {
        private static readonly object NullStoreKey = new object();
        private readonly IDictionary<object, ISettingsStore> _stores;

        public DictionarySettingsStoreAccessor()
        {
            _stores = new Dictionary<object, ISettingsStore>();
        }

        public object GetSetting(object storeKey, SettingsNamespace @namespace, string setting)
        {
            var store = GetEffectiveStore(storeKey);
            return store.GetSetting(@namespace, setting);
        }

        public void Set(object key, ISettingsStore store)
        {
            var normalizedKey = NormalizeStoreKey(key);
            _stores[normalizedKey] = store;
        }

        public void Clear(object key)
        {
            var normalizedKey = NormalizeStoreKey(key);
            _stores.Remove(normalizedKey);
        }

        public void SetSetting(object storeKey, SettingsNamespace @namespace, string setting, object value)
        {
            var store = GetEffectiveStore(storeKey);
            store.SetSetting(@namespace, setting, value);
        }

        private ISettingsStore GetEffectiveStore(object key)
        {
            ISettingsStore store;
            var normalizedKey = NormalizeStoreKey(key);
            if (_stores.TryGetValue(normalizedKey, out store))
            {
                return store;
            }
            return SettingsConstants.NullStore;
        }

        private object NormalizeStoreKey(object key)
        {
            return key ?? NullStoreKey;
        }
    }
}
