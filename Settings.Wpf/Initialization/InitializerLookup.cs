// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Windows;
using System.Windows.Markup;
using System.Xaml;
using TheSettings.Wpf.Infrastructure;
using TheSettings.Wpf.Markup;

namespace TheSettings.Wpf.Initialization
{
    internal class InitializerLookup
    {
        public SettingsInitializer RequireInitializer(IServiceProvider provider)
        {
            var rootProvider = provider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            FrameworkElement root = null;
            if (rootProvider != null)
            {
                root = rootProvider.RootObject as FrameworkElement;
            }
            var targetProvider = provider.RequireService<IProvideValueTarget>();
            var target = targetProvider.TargetObject as DependencyObject;
            var owner = FindSuitableOwner(target, root);
            if (owner == null)
            {
                throw new InvalidOperationException("Cannot determine owner for SettingsInitializer.");
            }
            if (owner.IsLoaded)
            {
                throw new InvalidOperationException("Owner is already loaded.");
            }
            return GetOrCreateInitializer(owner);
        }

        public SettingsInitializer TryGetInitializer(DependencyObject element)
        {
            var owner = FindSuitableOwner(element);
            if (owner == null || owner.IsLoaded)
            {
                return null;
            }
            return GetOrCreateInitializer(owner);
        }

        private static SettingsInitializer GetOrCreateInitializer(FrameworkElement owner)
        {
            var initializer = Settings.GetInitializer(owner);
            if (initializer == null)
            {
                initializer = new SettingsInitializer(owner);
                Settings.SetInitializer(owner, initializer);
            }
            return initializer;
        }

        private static FrameworkElement FindSuitableOwner(
            DependencyObject element,
            FrameworkElement root = null)
        {
            if (root != null)
            {
                return root;
            }
            var treeWalker = new WpfLogicalTreeWalker(element);
            var foundRoot = treeWalker.ClimbToRoot().CurrentAs<FrameworkElement>();
            if (foundRoot != null)
            {
                return foundRoot;
            }
            return element as FrameworkElement;
        }
    }
}