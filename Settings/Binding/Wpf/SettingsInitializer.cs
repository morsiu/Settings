// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Windows;
using TheSettings.Binding.Wpf.Markup;

namespace TheSettings.Binding.Wpf
{
    internal class SettingsInitializer
    {
        private readonly HashSet<SettingBindingCollection> _bindingsCollectionsToInitialize = new HashSet<SettingBindingCollection>();
        private readonly List<SettingsNamespaceBuilder> _namespaceBuilders = new List<SettingsNamespaceBuilder>();
        private readonly List<SettingBindingBuilder> _settingBindingsBuilders = new List<SettingBindingBuilder>();
        private readonly FrameworkElement _root;

        public SettingsInitializer(FrameworkElement root)
        {
            _root = root;
        }

        public FrameworkElement Root { get { return _root; } }

        public static SettingsInitializer GetInstance(FrameworkElement root)
        {
            var existingInitializer = Settings.GetInitializer(root);
            if (existingInitializer != null)
            {
                return existingInitializer;
            }

            // This is called by MarkupExtensions during XAML loading,
            // so Loaded event should not have fired yet.
            if (root.IsLoaded)
            {
                throw new InvalidOperationException("Cannot return initializer instance as the root is already initialized.");
            }
            var initializer = new SettingsInitializer(root);
            Settings.SetInitializer(root, initializer);
            root.Loaded += initializer.RootLoadedCallback;
            return initializer;
        }

        public void QueueNamespaceBuild(SettingsNamespaceBuilder builder)
        {
            _namespaceBuilders.Add(builder);
        }

        public void QueueSettingBindingBuild(SettingBindingBuilder builder)
        {
            _settingBindingsBuilders.Add(builder);
        }

        public void QueueSettingsCollectionUpdate(SettingBindingCollection settingsCollection)
        {
            _bindingsCollectionsToInitialize.Add(settingsCollection);
        }

        private void RootLoadedCallback(object sender, EventArgs e)
        {
            SetupNamespaces();
            SetupBindings();
            InitializeBindingCollections();
            Cleanup();
        }

        private void SetupBindings()
        {
            foreach (var builder in _settingBindingsBuilders)
            {
                builder.Build();
            }
        }

        private void InitializeBindingCollections()
        {
            foreach (var collection in _bindingsCollectionsToInitialize)
            {
                collection.Initialize();
            }
        }

        private void Cleanup()
        {
            _root.Loaded -= RootLoadedCallback;
            _namespaceBuilders.Clear();
            _settingBindingsBuilders.Clear();
            _bindingsCollectionsToInitialize.Clear();
            _root.ClearValue(Settings.InitializerProperty);
        }

        private void SetupNamespaces()
        {
            // Nodes in initialize list were added in top-down fashion,
            // so when building Namespace for one, successors will have that Namespace inherited,
            // as each is a child of one of predecessors.
            _namespaceBuilders.ForEach(builder => builder.Build());
        }
    }
}