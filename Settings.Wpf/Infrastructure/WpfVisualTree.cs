// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using TheSettings.Infrastructure;

namespace TheSettings.Wpf.Infrastructure
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
