﻿using System;
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
    public partial class Basic : Window
    {
        public Basic()
        {
            InitializeComponent();
        }

        public SettingsNamespace RootNamespace
        {
            get { return Settings.GetNamespace(this); }
        }

        public SettingsNamespace Level1Namespace
        {
            get { return Settings.GetNamespace(Level1); }
        }

        public SettingsNamespace Child1Namespace
        {
            get { return Settings.GetNamespace(Child1); }
        }

        public SettingsNamespace Child2Namespace
        {
            get { return Settings.GetNamespace(Child2); }
        }

        public SettingsNamespace Child4Namespace
        {
            get { return Settings.GetNamespace(Child4); }
        }
    }
}