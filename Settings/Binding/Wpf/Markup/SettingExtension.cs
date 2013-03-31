using System;
using System.Windows;
using System.Windows.Markup;
using System.Xaml;

namespace TheSettings.Binding.Wpf.Markup
{
    public class SettingExtension : MarkupExtension
    {
        private static readonly object UnsetValue = new object();

        public SettingExtension(string name)
        {
            Name = name;
            DefaultValue = UnsetValue;
        }

        public string Name { get; set; }

        public object DefaultValue { get; set; }

        public object Store { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetProvider = serviceProvider.RequireService<IProvideValueTarget>();
            if (targetProvider.IsInsideTemplate()
                || targetProvider.IsInsideStyleSetter())
            {
                return this;
            }
            var target = targetProvider.TargetObject as DependencyObject;
            if (target == null)
            {
                throw new InvalidOperationException("Target object is not a DependencyObject.");
            }
            var initializer = serviceProvider.RequireInitializer();
            var builder = new SettingBindingBuilder(target, targetProvider.TargetProperty, Store, Name);
            initializer.QueueSettingBindingBuild(builder);

            if (DefaultValue == UnsetValue)
            {
                var unsetValue = SettingsMarkupExtensions.GetUnsetValue(target, targetProvider.TargetProperty);
                return unsetValue;
            }
            var type = targetProvider.GetTargetPropertyValueType();
            return Convert.ChangeType(DefaultValue, type);
        }
    }
}
