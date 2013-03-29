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

        private IEnumerable<TElement> GetDescendants(TElement element)
        {
            yield return element;
            foreach (var child in _tree.GetChildren(element))
            {
                yield return child;
                foreach (var childOfChild in GetDescendants(child))
                {
                    yield return childOfChild;
                }
            }
        }

        private IEnumerable<TElement> GetAllUpwards(TElement element)
        {
            var parent = _tree.GetParent(element);
            var previousParent = element;
            yield return element;
            while (parent != null)
            {
                foreach (var children in _tree.GetChildren(parent))
                {
                    if (children == previousParent) continue;
                    foreach (var childOfChild in GetAllUpwards(children))
                    {
                        yield return children;
                    }

                }
                previousParent = parent;
                parent = _tree.GetParent(element);
            }
        }

        public TElement SearchAll(Func<TElement, bool> predicate)
        {
            foreach (var descendant in GetDescendants(_current))
            {
                if (predicate(descendant)) return descendant;
            }
            foreach (var element in GetAllUpwards(_current))
            {
                if (predicate(element)) return element;
            }
            return null;
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
