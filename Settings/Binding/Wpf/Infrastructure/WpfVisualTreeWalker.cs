using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TheSettings.Infrastructure;

namespace TheSettings.Binding.Wpf.Infrastructure
{
    public class WpfVisualTreeWalker : TreeWalker<DependencyObject>
    {
        public WpfVisualTreeWalker(DependencyObject initialCurrent)
            : base(initialCurrent, new WpfLogicalTree())
        {
        }
    }
}
