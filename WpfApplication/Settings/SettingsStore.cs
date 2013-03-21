using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WpfApplication.Settings
{
    public class SettingsStore : ISettingsStore
    {
        public class SettingsContainer
        {
            private readonly IDictionary<string, object> _settings;
            private readonly IReadOnlyDictionary<string, object> _settingsAccessor;

            public SettingsContainer()
            {
                _settings = new Dictionary<string, object>();
                _settingsAccessor = new ReadOnlyDictionary<string, object>(_settings);
            }

            public IReadOnlyDictionary<string, object> Settings { get { return _settingsAccessor; } }

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
                return Equals(x.ToString(), y.ToString());
            }

            public int GetHashCode(SettingsNamespace ns)
            {
                return ns.ToString().GetHashCode();
            }
        }

        private readonly IDictionary<SettingsNamespace, SettingsContainer> _settingsByNamespace;
        private readonly IReadOnlyDictionary<SettingsNamespace, SettingsContainer> _settingsByNamespaceAccessor;
        private readonly IEqualityComparer<SettingsNamespace> _namespaceComparer;

        public SettingsStore()
        {
            _namespaceComparer = new NamespaceComparer();
            _settingsByNamespace = new Dictionary<SettingsNamespace, SettingsContainer>(_namespaceComparer);
            _settingsByNamespaceAccessor = new ReadOnlyDictionary<SettingsNamespace, SettingsContainer>(_settingsByNamespace);
        }

        public IReadOnlyDictionary<SettingsNamespace, SettingsContainer> SettingsByNamespace { get { return _settingsByNamespaceAccessor; } }

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
                SettingChanged(this, EventArgs.Empty);
            }
        }

        private SettingsContainer GetContainer(SettingsNamespace @namespace)
        {
            SettingsContainer container;
            if (_settingsByNamespace.TryGetValue(@namespace, out container))
            {
                return container;
            }
            container = new SettingsContainer();
            _settingsByNamespace[@namespace] = container;
            return container;
        }

        public event EventHandler SettingChanged;

        public static readonly SettingsStore Instance = new SettingsStore();
    }
}
