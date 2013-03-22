using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication.Settings.Binding.Wpf
{
    /// <summary>
    /// IMarkupSettingBinding which has Initialize(DependencyObject owner)
    /// - can be used when adding the binding during xaml construction
    /// and initializtion must be defered.
    /// </summary>
    public interface ISettingBindingsProvider
    {
        IEnumerable<ISettingBinding> ProvideBindings(DependencyObject target);
    }
}
