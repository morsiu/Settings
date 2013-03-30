using System.Linq;
using System.Windows;
using TheSettings.Binding.Wpf.Infrastructure;
using TheSettings.Binding.Wpf.Markup;

namespace TheSettings.Binding.Wpf
{
    internal class SettingsNamespaceBuilder
    {
        private readonly DependencyObject _node;
        private readonly NamespaceCreationInfo _creationInfo;

        public SettingsNamespaceBuilder(DependencyObject node, NamespaceCreationInfo creationInfo)
        {
            _node = node;
            _creationInfo = creationInfo;
        }

        public void Build()
        {
            var namespaceSource = DependencyPropertyHelper.GetValueSource(_node, Settings.NamespaceProperty);
            if (namespaceSource.BaseValueSource != BaseValueSource.Local)
            {
                var parentNamespace = GetParentNamespace();

                if (_creationInfo.CreateNew)
                {
                    var newNamespace = new SettingsNamespace(parentNamespace, _creationInfo.Name);
                    Settings.SetNamespace(_node, newNamespace);
                }
                else
                {
                    Settings.SetNamespace(_node, parentNamespace);
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
                    var walker = new WpfVisualTreeWalker(_node);
                    var nodesToSearch =
                        walker.GetDepthFirstDownards(_node)
                        .Concat(new[] { _node })
                        .Concat(walker.GetDepthFirstUpwards(_node));
                    var parent = nodesToSearch.FirstOrDefault(
                        node =>
                        {
                            var element = node as FrameworkElement;
                            return element != null && element.Name == _creationInfo.ParentSourceName;
                        });
                    parentNamespace = parent == null
                                          ? SettingsNamespace.None
                                          : Settings.GetNamespace(parent);
                }
                else
                {
                    var inheritedNamespace = Settings.GetNamespace(_node);
                    parentNamespace = inheritedNamespace;
                }
            }
            else
            {
                parentNamespace = SettingsNamespace.None;
            }
            return parentNamespace;
        }
    }
}
