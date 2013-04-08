﻿// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Windows;

namespace TheSettings.Binding.Wpf
{
    public class NamespaceCreationInfo
    {
        public NamespaceCreationInfo(string name, bool useParent, string parentSourceName, DependencyObject root)
        {
            Name = name;
            UseParent = useParent;
            ParentSourceName = parentSourceName;
            Root = root;
        }

        public string Name { get; private set; }

        public string ParentSourceName { get; private set; }

        public bool UseParent { get; private set; }

        public DependencyObject Root { get; private set; }

        public bool UseParentSource
        {
            get { return ParentSourceName != null; }
        }

        public bool CreateNew
        {
            get { return Name != null; }
        }
    }
}