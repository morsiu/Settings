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
using TheSettings.Binding.Wpf.Infrastructure;
using TheSettings.Binding.Wpf.Markup;
using TheSettings.Infrastructure;

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
                var grid = GetTemplateRoot().CurrentAs<Grid>();
                return Settings.GetNamespace(grid);
            }
        }

        public SettingsNamespace TemplateInTemplateNamespace
        {
            get
            {
                // -> ContentPresenter -> Grid
                var grid = GetTemplateRoot().ClimbDown(0, 0).CurrentAs<Grid>();
                return Settings.GetNamespace(grid);
            }
        }

        public SettingsNamespace TemplateChild2Namespace
        {
            get
            {
                // -> 2nd Grid
                var grid = GetTemplateRoot().ClimbDown(2).CurrentAs<Grid>();
                return Settings.GetNamespace(grid);
            }
        }

        public SettingsNamespace TemplateChild3Namespace
        {
            get
            {
                // -> 3rd grid
                var grid = GetTemplateRoot().ClimbDown(3).CurrentAs<Grid>();
                return Settings.GetNamespace(grid);
            }
        }

        public SettingsNamespace TemplateChild4Namespace
        {
            get
            {
                // -> 4th grid
                var grid = GetTemplateRoot().ClimbDown(4).CurrentAs<Grid>();
                return Settings.GetNamespace(grid);
            }
        }

        public SettingsNamespace TemplateChild5Namespace
        {
            get
            {
                // -> 5th grid
                var grid = GetTemplateRoot().ClimbDown(5).CurrentAs<Grid>();
                return Settings.GetNamespace(grid);
            }
        }

        private ITreeWalker<DependencyObject> GetTemplateRoot()
        {
            var walker = new WpfVisualTreeWalker(ContentPresenter).ClimbDown(0);
            return walker;
        }
    }
}
