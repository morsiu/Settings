// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Windows;
using TheSettings.Wpf.Infrastructure;

namespace TheSettings.Wpf.Initialization
{
    internal class SettingsNamespaceBuilder
    {
        private readonly DependencyObject _node;
        private readonly NamespaceCreationInfo _creationInfo;

        public SettingsNamespaceBuilder(DependencyObject node, NamespaceCreationInfo creationInfo)
        {
            _node = node;
            _creationInfo = creationInfo;
        }

        public void Build()
        {
            var namespaceSource = DependencyPropertyHelper.GetValueSource(_node, Settings.NamespaceProperty);
            if (namespaceSource.BaseValueSource != BaseValueSource.Local)
            {
                var parentNamespace = GetParentNamespace();

                if (_creationInfo.CreateNew)
                {
                    var newNamespace = new SettingsNamespace(parentNamespace, _creationInfo.Name);
                    Settings.SetNamespace(_node, newNamespace);
                }
                else
                {
                    Settings.SetNamespace(_node, parentNamespace);
                }
            }
        }

        private SettingsNamespace GetParentNamespace()
        {
            SettingsNamespace parentNamespace;
            if (_creationInfo.UseParent)
            {
                if (_creationInfo.UseParentSource)
                {
                    var nameLokup = new NameLookup(_node);
                    var parent = nameLokup.Find(_creationInfo.ParentSourceName);
                    parentNamespace = parent == null
                                          ? SettingsNamespace.None
                                          : Settings.GetNamespace(parent);
                }
                else
                {
                    var inheritedNamespace = Settings.GetNamespace(_node);
                    parentNamespace = inheritedNamespace;
                }
            }
            else
            {
                parentNamespace = SettingsNamespace.None;
            }
            return parentNamespace;
        }
    }
}
