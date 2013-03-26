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
using TheSettings.Binding.Wpf.Markup;

namespace TheSettingsTests.MarkupTests.NamespaceExtensionTests.Windows
{
    public partial class DataTemplating : Window
    {
        public DataTemplating()
        {
            InitializeComponent();
        }

        public SettingsNamespace TemplateNamespace
        {
            get
            {
                return Settings.GetNamespace(Content);
            }
        }

        public object TemplateDataContext
        {
            get
            {
                var child = VisualTreeHelper.GetChild(Content, 0);
                return ((FrameworkElement)child).DataContext;
            }
        }

    }
}
