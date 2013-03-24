using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TheSettings;
using TheSettings.Binding.Wpf;
using TheSettings.Binding.Wpf.Markup;

namespace WpfApplication.Sample
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        private SettingsStore _store;

        public UserControl1()
        {
            InitializeComponent();
            var accessor = (SingleSettingsStoreAccessor)Settings.CurrentStoreAccessor;
            _store = (SettingsStore)accessor.Store;
            _store.SettingChanged += (o, e) => { RefreshSettings(); };
        }

        private void RefreshSettingsSource(object sender, RoutedEventArgs e)
        {
            RefreshSettings();
        }

        private void RefreshSettings()
        {
            var allSettings = _store.SettingsByNamespace
                .SelectMany(
                    n => n.Value.Settings,
                    (n, c) => new { Namespace = n.Key, Key = c.Key, Value = c.Value })
                .ToList();
            SettingsDataGrid.ItemsSource = allSettings;
        }
    }
}
