﻿// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Windows;
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
