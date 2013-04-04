using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TheSettings;
using TheSettings.Binding.Wpf;
using TheSettings.Binding.Wpf.Markup;

namespace Sample
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Settings.CurrentStoreAccessor = new SingleSettingsStoreAccessor(new SettingsStore());
        }
    }
}
