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

namespace WpfApplication.Sample
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            SettingsStore.Instance.SettingChanged += (o, e) => { RefreshSettings(); };
        }

        private void RefreshSettingsSource(object sender, RoutedEventArgs e)
        {
            RefreshSettings();
        }

        private void RefreshSettings()
        {
            var allSettings = SettingsStore.Instance.SettingsByNamespace
                .SelectMany(
                    n => n.Value.Settings,
                    (n, c) => new { Namespace = n.Key, Key = c.Key, Value = c.Value })
                .ToList();
            SettingsDataGrid.ItemsSource = allSettings;
        }
    }
}
