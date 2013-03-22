using System.Collections.Generic;
using System.Windows;

namespace Settings.Binding
{
    public interface ISettingBindingsProvider
    {
        IEnumerable<ISettingBinding> ProvideBindings(DependencyObject target);
    }
}
