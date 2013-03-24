using System.Collections.Generic;
using System.Windows;

namespace TheSettings.Binding
{
    public interface ISettingBindingsProvider
    {
        IEnumerable<ISettingBinding> ProvideBindings(DependencyObject target);
    }
}
