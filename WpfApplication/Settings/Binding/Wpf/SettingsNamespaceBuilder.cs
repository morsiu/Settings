﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApplication.Settings.Binding.Wpf.Markup;

namespace WpfApplication.Settings.Binding.Wpf
{
    internal class SettingsNamespaceBuilder
    {
        private DependencyObject _node;
        private NamespaceCreationInfo _creationInfo;

        public SettingsNamespaceBuilder(DependencyObject node, NamespaceCreationInfo creationInfo)
        {
            _node = node;
            _creationInfo = creationInfo;
        }

        public void Build()
        {
            var namespaceSource = DependencyPropertyHelper.GetValueSource(_node, Markup.Settings.NamespaceProperty);
            if (namespaceSource.BaseValueSource != BaseValueSource.Local)
            {
                var parentNamespace = GetParentNamespace();

                if (_creationInfo.CreateNew)
                {
                    var newNamespace = new SettingsNamespace(parentNamespace, _creationInfo.Name);
                    Markup.Settings.SetNamespace(_node, newNamespace);
                }
                else
                {
                    Markup.Settings.SetNamespace(_node, parentNamespace);
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
                    var parent = LogicalTreeHelper.FindLogicalNode(_creationInfo.Root, _creationInfo.ParentSourceName);
                    parentNamespace = parent == null
                                          ? null
                                          : Markup.Settings.GetNamespace(parent);
                }
                else
                {
                    var inheritedNamespace = Markup.Settings.GetNamespace(_node);
                    parentNamespace = inheritedNamespace == SettingsNamespace.None
                                          ? null
                                          : inheritedNamespace;
                }
            }
            else
            {
                parentNamespace = null;
            }
            return parentNamespace;
        }
    }
}
