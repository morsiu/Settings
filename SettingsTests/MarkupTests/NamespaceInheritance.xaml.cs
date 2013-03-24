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

namespace TheSettingsTests.MarkupTests
{
    public partial class NamespaceInheritance : UserControl
    {
        public NamespaceInheritance()
        {
            InitializeComponent();
        }

        public SettingsNamespace Level1Namespace
        {
            get { return Settings.GetNamespace(Level1); }
        }

        public SettingsNamespace NestedLevel1Namespace
        {
            get { return Level2.Level1Namespace; }
        }

        public SettingsNamespace RootNamespace
        {
            get { return Settings.GetNamespace(this); }
        }
    }
}
