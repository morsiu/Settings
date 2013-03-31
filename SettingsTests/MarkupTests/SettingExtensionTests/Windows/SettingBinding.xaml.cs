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

namespace TheSettingsTests.MarkupTests.SettingExtensionTests.Windows
{
    public partial class SettingBinding : Window
    {
        public SettingBinding()
        {
            InitializeComponent();
        }

        public object Child1DataContext
        {
            get { return Child1.DataContext; }
            set { Child1.DataContext = value; }
        }
    }
}
