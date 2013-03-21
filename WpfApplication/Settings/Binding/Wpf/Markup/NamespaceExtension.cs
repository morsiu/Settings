using System;
using System.Windows;
using System.Windows.Markup;
using System.Xaml;

namespace WpfApplication.Settings.Binding.Wpf.Markup
{
    public class NamespaceExtension : MarkupExtension
    {
        public NamespaceExtension()
        {
            UseParent = true;
        }

        public NamespaceExtension(string name)
            : this()
        {
            Name = name;
        }

        public string Name { get; set; }

        public bool UseParent { get; set; }

        public string ParentSourceName { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetProvider = serviceProvider.RequireService<IProvideValueTarget>();
            var rootProvider = serviceProvider.RequireService<IRootObjectProvider>();
            var initializer = rootProvider.RequireInitializer();
            var root = rootProvider.RootObject as DependencyObject;
            if (root == null)
            {
                throw new InvalidOperationException("Root object is required that is a DependencyObject.");
            }
            var target = targetProvider.TargetObject as DependencyObject;
            if (target == null)
            {
                throw new InvalidOperationException("Target is not a DependencyObject.");
            }
            var creationInfo = new NamespaceCreationInfo(Name, UseParent, ParentSourceName, root);
            var builder = new SettingsNamespaceBuilder(target, creationInfo);
            initializer.QueueNamespaceBuild(builder);

            return DependencyProperty.UnsetValue;
        }
    }
}
