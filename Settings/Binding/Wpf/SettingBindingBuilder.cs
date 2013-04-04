using System.Windows;
using TheSettings.Binding.Wpf.Markup;

namespace TheSettings.Binding.Wpf
{
    internal class SettingBindingBuilder
    {
        private readonly DependencyObject _target;
        private readonly object _property;
        private readonly string _name;
        private readonly object _storeKey;

        public SettingBindingBuilder(DependencyObject target, object property, object storeKey, string name)
        {
            _target = target;
            _property = property;
            _name = name;
            _storeKey = storeKey;
        }

        public void Build()
        {
            var @namespace = Settings.GetNamespace(_target);
            var settings = Settings.GetSettings(_target);
            var bindingFactory = new SettingBindingFactory();
            var binding = bindingFactory.Create(_target, _property, _storeKey, @namespace, _name);
            settings.AddBinding(binding);
        }
    }
}
