using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSettings.Binding.Wpf
{
    public interface ISettingsStoreAccessor
    {
        object GetSetting(object storeKey, SettingsNamespace @namespace, string setting);

        void SetSetting(object storeKey, SettingsNamespace @namespace, string setting, object value);
    }
}
