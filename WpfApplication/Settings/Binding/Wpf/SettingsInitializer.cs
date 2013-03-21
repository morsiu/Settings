﻿using System;
using System.Collections.Generic;
using System.Windows;
using WpfApplication.Settings.Binding.Wpf.Markup;

namespace WpfApplication.Settings.Binding.Wpf
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

        public static SettingsInitializer GetInstance(FrameworkElement root)
        {
            var existingInitializer = Markup.Settings.GetInitializer(root);
            if (existingInitializer != null)
            {
                return existingInitializer;
            }

            // This is called by MarkupExtensions during XAML loading,
            // so Loaded event should not have fired yet.
            if (root.IsLoaded)
            {
                throw new InvalidOperationException("Cannot return instance as the root is already initialized.");
            }
            var initializer = new SettingsInitializer(root);
            Markup.Settings.SetInitializer(root, initializer);
            root.Loaded += initializer.RootLoadedCallback;
            return initializer;
        }

        public FrameworkElement Root { get { return _root; } }

        private void RootLoadedCallback(object sender, RoutedEventArgs e)
        {
            SetupNamespaces();
            SetupBindings();
            UpdateBindings();
            Cleanup();
        }

        private void SetupBindings()
        {
            foreach (var builder in _settingBindingsBuilders)
            {
                builder.Build();
            }
        }

        private void UpdateBindings()
        {
            foreach (var collection in _bindingsCollectionsToInitialize)
            {
                collection.UpdateBindings();
            }
        }

        private void Cleanup()
        {
            _root.Loaded -= RootLoadedCallback;
            _namespaceBuilders.Clear();
            _settingBindingsBuilders.Clear();
            _bindingsCollectionsToInitialize.Clear();
            _root.ClearValue(Markup.Settings.InitializerProperty);
        }

        private void SetupNamespaces()
        {
            // Nodes in initialize list were added in top-down fashion,
            // so when building Namespace for one, successors will have that Namespace inherited,
            // as each is a child of one of predecessors.
            _namespaceBuilders.ForEach(builder => builder.Build());
        }

        public void QueueNamespaceBuild(SettingsNamespaceBuilder builder)
        {
            _namespaceBuilders.Add(builder);
        }

        public void QueueSettingBindingsUpdate(SettingBindingCollection settingsCollection)
        {
            _bindingsCollectionsToInitialize.Add(settingsCollection);
        }

        public void QueueSettingBindingBuild(SettingBindingBuilder builder)
        {
            _settingBindingsBuilders.Add(builder);
        }
    }
}