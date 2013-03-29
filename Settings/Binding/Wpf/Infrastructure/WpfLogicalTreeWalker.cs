using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TheSettings.Infrastructure;

namespace TheSettings.Binding.Wpf.Infrastructure
{
    public class WpfLogicalTreeWalker : TreeWalker<DependencyObject>
    {
        public WpfLogicalTreeWalker(DependencyObject initialCurrent)
            : base(initialCurrent, new WpfLogicalTree())
        {
        }
    }
}
