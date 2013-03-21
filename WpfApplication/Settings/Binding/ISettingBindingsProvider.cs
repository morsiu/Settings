using System.Collections.Generic;
using System.Windows;

namespace WpfApplication.Settings.Binding
{
    public interface ISettingBindingsProvider
    {
        IEnumerable<ISettingBinding> ProvideBindings(DependencyObject target);
    }
}
