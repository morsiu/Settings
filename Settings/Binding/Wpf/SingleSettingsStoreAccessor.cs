using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSettings.Binding.Wpf
{
    public class SingleSettingsStoreAccessor : ISettingsStoreAccessor
    {
        private readonly ISettingsStore _store;

        public SingleSettingsStoreAccessor(ISettingsStore store)
        {
            _store = store;
        }

        public ISettingsStore Store { get { return _store; } }

        public object GetSetting(object storeKey, SettingsNamespace @namespace, string setting)
        {
            return _store.GetSetting(@namespace, setting);
        }

        public void SetSetting(object storeKey, SettingsNamespace @namespace, string setting, object value)
        {
            _store.SetSetting(@namespace, setting, value);
        }
    }
}
