// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Windows;
using System.Windows.Controls;
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
