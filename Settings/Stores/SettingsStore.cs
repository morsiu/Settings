// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TheSettings.Stores
{
    public sealed class SettingsStore : ISettingsStore
    {
        private readonly IDictionary<SettingsNamespace, SettingsContainer> _namespaceContainers;
        private readonly IReadOnlyDictionary<SettingsNamespace, SettingsContainer> _namespaceContainersAccessor;
        private readonly IEqualityComparer<SettingsNamespace> _namespaceComparer;

        public SettingsStore()
        {
            _namespaceComparer = new NamespaceComparer();
            _namespaceContainers = new Dictionary<SettingsNamespace, SettingsContainer>(_namespaceComparer);
            _namespaceContainersAccessor = new ReadOnlyDictionary<SettingsNamespace, SettingsContainer>(_namespaceContainers);
        }

        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        public IReadOnlyDictionary<SettingsNamespace, SettingsContainer> SettingsByNamespace
        {
            get { return _namespaceContainersAccessor; }
        }

        public object GetSetting(SettingsNamespace @namespace, string name)
        {
            var container = GetContainer(@namespace);
            return container.GetSetting(name);
        }

        public void SetSetting(SettingsNamespace @namespace, string name, object value)
        {
            var container = GetContainer(@namespace);
            container.SetSetting(name, value);
            if (SettingChanged != null)
            {
                SettingChanged(this, new SettingChangedEventArgs(@namespace, name, value));
            }
        }

        private SettingsContainer GetContainer(SettingsNamespace @namespace)
        {
            SettingsContainer container;
            if (_namespaceContainers.TryGetValue(@namespace, out container))
            {
                return container;
            }
            container = new SettingsContainer();
            _namespaceContainers[@namespace] = container;
            return container;
        }

        public class SettingsContainer
        {
            private readonly IDictionary<string, object> _settings;
            private readonly IReadOnlyDictionary<string, object> _settingsAccessor;

            public SettingsContainer()
            {
                _settings = new Dictionary<string, object>();
                _settingsAccessor = new ReadOnlyDictionary<string, object>(_settings);
            }

            public IReadOnlyDictionary<string, object> Settings
            {
                get { return _settingsAccessor; }
            }

            public void SetSetting(string name, object value)
            {
                _settings[name] = value;
            }

            public object GetSetting(string name)
            {
                object value;
                if (_settings.TryGetValue(name, out value))
                {
                    return value;
                }
                return SettingsConstants.NoValue;
            }
        }

        private class NamespaceComparer : IEqualityComparer<SettingsNamespace>
        {
            public bool Equals(SettingsNamespace x, SettingsNamespace y)
            {
                return x.Path.Equals(y.Path, StringComparison.Ordinal);
            }

            public int GetHashCode(SettingsNamespace ns)
            {
                return ns.Path.GetHashCode();
            }
        }
    }
}