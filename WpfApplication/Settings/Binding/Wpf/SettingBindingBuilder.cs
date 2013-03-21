using System.Windows;

namespace WpfApplication.Settings.Binding.Wpf
{
    public class SettingBindingBuilder
    {
        private readonly DependencyObject _target;
        private readonly object _property;
        private readonly string _name;

        public SettingBindingBuilder(DependencyObject target, object property, string name)
        {
            _target = target;
            _property = property;
            _name = name;
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
