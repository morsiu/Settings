using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WpfApplication.Settings.Binding.Wpf
{
    public class SettingBindingBuilder
    {
        private readonly DependencyObject _target;
        private readonly object _property;
        private readonly string _name;
        private readonly ISettingsStore _store;

        public SettingBindingBuilder(DependencyObject target, object property, string name)
        {
            _target = target;
            _property = property;
            _name = name;
            _store = SettingsStore.Instance;
        }

        public void Build()
        {
            var @namespace = Markup.Settings.GetNamespace(_target);
            var settings = Markup.Settings.GetSettings(_target);
            var bindingFactory = new SettingBindingFactory();
            var binding = bindingFactory.Create(_target, _property, @namespace, _name);
            settings.Add(binding);
        }
    }
}
