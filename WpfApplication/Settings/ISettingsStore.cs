using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication.Settings
{
    public interface ISettingsStore
    {
        object GetSetting(SettingsNamespace @namespace, string name);

        void SetSetting(SettingsNamespace @namespace, string name, object value);
    }
}
