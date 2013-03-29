using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TheSettings.Infrastructure;

namespace TheSettings.Binding.Wpf.Infrastructure
{
    public class WpfLogicalTree : ITree<DependencyObject>
    {
        public DependencyObject GetParent(DependencyObject element)
        {
            return LogicalTreeHelper.GetParent(element);
        }

        public DependencyObject GetChild(DependencyObject element, int index)
        {
            return LogicalTreeHelper.GetChildren(element).OfType<DependencyObject>().ElementAt(index);
        }

        public int GetChildrenCount(DependencyObject element)
        {
            return LogicalTreeHelper.GetChildren(element).OfType<DependencyObject>().Count();
        }

        public IEnumerable<DependencyObject> GetChildren(DependencyObject element)
        {
            return LogicalTreeHelper.GetChildren(element).OfType<DependencyObject>();
        }
    }
}
