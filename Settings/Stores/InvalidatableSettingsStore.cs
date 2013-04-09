// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;

namespace TheSettings.Stores
{
    /// <summary>
    /// A store that allows its contents to be invalidated upon request.
    /// </summary>
    public class InvalidatableSettingsStore : ISettingsStore
    {
        private readonly Func<ISettingsStore, ISettingsStore> _invalidator;
        private ISettingsStore _store;

        public InvalidatableSettingsStore(
            ISettingsStore store,
            Func<ISettingsStore, ISettingsStore> invalidator)
        {
            if (invalidator == null) throw new ArgumentNullException("invalidator");
            _store = store;
            _invalidator = invalidator;
        }

        public void Invalidate()
        {
            _store = _invalidator(_store);
        }

        public object GetSetting(SettingsNamespace @namespace, string name)
        {
            return _store.GetSetting(@namespace, name);
        }

        public void SetSetting(SettingsNamespace @namespace, string name, object value)
        {
            _store.SetSetting(@namespace, name, value);
        }
    }
}
