using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TheSettings.Infrastructure;

namespace TheSettings.Binding.Wpf.Infrastructure
{
    public class WpfVisualTree : ITree<DependencyObject>
    {
        public DependencyObject GetParent(DependencyObject element)
        {
            return VisualTreeHelper.GetParent(element);
        }

        public DependencyObject GetChild(DependencyObject element, int index)
        {
            return VisualTreeHelper.GetChild(element, index);
        }

        public int GetChildrenCount(DependencyObject element)
        {
            return VisualTreeHelper.GetChildrenCount(element);
        }

        public IEnumerable<DependencyObject> GetChildren(DependencyObject element)
        {
            for (int index = 0; index < GetChildrenCount(element); ++index)
            {
                yield return GetChild(element, index);
            }
        }
    }
}
