﻿using TheSettings;
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
    public partial class ControlNestingNestedControl : UserControl
    {
        public ControlNestingNestedControl()
        {
            InitializeComponent();
        }

        public SettingsNamespace Level1Namespace
        {
            get { return Settings.GetNamespace(Level1); }
        }

        public SettingsNamespace Level2Namespace
        {
            get { return Settings.GetNamespace(Level2); }
        }
    }
}