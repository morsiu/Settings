using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSettings
{
    public class NullSettingsStore : ISettingsStore
    {
        public object GetSetting(SettingsNamespace @namespace, string name)
        {
            return SettingsConstants.NoValue;
        }

        public void SetSetting(SettingsNamespace @namespace, string name, object value)
        {
        }
    }
}
