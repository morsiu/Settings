using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TheSettings.Binding.Wpf.Infrastructure
{
    public class NameLookup
    {
        DependencyObject _startingElement;

        public NameLookup(DependencyObject startingElement)
        {
            _startingElement = startingElement;
        }

        public DependencyObject Find(string name)
        {
            // This code could use INameScope etc. (see WPF XAML Namescopes help topic),
            // as now it walks the WPF logical tree manualy
            // checking all FrameworkElement nodes Name property.
            var walker = new WpfLogicalTreeWalker(_startingElement);
            var nodesToSearch =
                walker.GetDepthFirstDownards(_startingElement)
                .Concat(new[] { _startingElement })
                .Concat(walker.GetDepthFirstUpwards(_startingElement));
            var parent = nodesToSearch.FirstOrDefault(
                node =>
                {
                    var element = node as FrameworkElement;
                    return element != null && element.Name == name;
                });
            return parent;
        }
    }
}
