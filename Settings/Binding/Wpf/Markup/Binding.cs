﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace TheSettings.Binding.Wpf.Markup
{
    public class Binding : ISettingBindingsProvider
    {
        public string Property { get; set; }

        public string Setting { get; set; }

        public object Store { get; set; }

        public IEnumerable<ISettingBinding> ProvideBindings(DependencyObject target)
        {
            var factory = new SettingBindingFactory();
            var @namespace = Settings.GetNamespace(target);
            var descriptor = TypeDescriptor.GetProperties(target)[Property];
            var binding = factory.Create(target, descriptor, Store, @namespace, Setting);
            return new[] { binding };
        }
    }
}
