using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TheSettings.Infrastructure
{
    public class TreeWalker<TElement> : ITreeWalker<TElement>
        where TElement : class
    {
        private TElement _current;
        private ITree<TElement> _tree;

        public TreeWalker(TElement initialCurrent, ITree<TElement> tree)
        {
            if (initialCurrent == null) throw new ArgumentNullException();
            _current = initialCurrent;
            _tree = tree;
        }

        public ITreeWalker<TElement> ClimbToRoot()
        {
            var parent = _tree.GetParent(_current);
            while (parent != null)
            {
                _current = parent;
                parent = _tree.GetParent(parent);
            }
            return this;
        }

        public ITreeWalker<TElement> ClimbUp(int ladderCount)
        {
            while (ladderCount > 0)
            {
                _current = _tree.GetParent(_current);
                --ladderCount;
            }
            return this;
        }

        public ITreeWalker<TElement> ClimbDown(params int[] ladderIndexes)
        {
            foreach (var index in ladderIndexes)
            {
                _current = _tree.GetChild(_current, index);
            }
            return this;
        }

        public TElement Current { get { return _current; } }

        public T CurrentAs<T>()
            where T : class, TElement
        {
            return _current as T;
        }
    }
}
