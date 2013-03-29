using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TheSettings.Infrastructure
{
    public interface ITree<TElement>
    {
        TElement GetParent(TElement element);

        TElement GetChild(TElement element, int index);
    }
}
