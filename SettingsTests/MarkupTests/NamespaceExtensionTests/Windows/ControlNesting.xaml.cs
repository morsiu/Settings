using TheSettings;
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
using TheSettings.Binding.Wpf.Markup;

namespace TheSettingsTests.MarkupTests.NamespaceExtensionTests.Windows
{
    public partial class ControlNesting : Window
    {
        public ControlNesting()
        {
            InitializeComponent();
        }

        public SettingsNamespace NestedLevel1Namespace
        {
            get { return Nested.Level1Namespace; }
        }

        public SettingsNamespace NestedLevel2Namespace
        {
            get { return Nested.Level2Namespace; }
        }

        public SettingsNamespace RootNamespace
        {
            get { return Settings.GetNamespace(this); }
        }
    }
}
