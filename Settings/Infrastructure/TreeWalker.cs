﻿// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;

namespace TheSettings.Infrastructure
{
    public class TreeWalker<TElement> : ITreeWalker<TElement>
        where TElement : class
    {
        private readonly ITree<TElement> _tree;
        private TElement _current;

        public TreeWalker(TElement initialCurrent, ITree<TElement> tree)
        {
            if (initialCurrent == null) throw new ArgumentNullException("initialCurrent");
            if (tree == null) throw new ArgumentNullException("tree");
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

        public IEnumerable<TElement> GetDescendantsDepthFirst(TElement element)
        {
            foreach (var child in _tree.GetChildren(element))
            {
                yield return child;
                foreach (var childOfChild in GetDescendantsDepthFirst(child))
                {
                    yield return childOfChild;
                }
            }
        }

        public IEnumerable<TElement> GetAncestorsDepthFirst(TElement element)
        {
            var parent = _tree.GetParent(element);
            var previousParent = element;
            while (parent != null)
            {
                foreach (var children in _tree.GetChildren(parent))
                {
                    if (Equals(children, previousParent)) continue;
                    yield return children;
                    foreach (var childOfChild in GetDescendantsDepthFirst(children))
                    {
                        yield return childOfChild;
                    }
                }
                yield return parent;
                previousParent = parent;
                parent = _tree.GetParent(parent);
            }
        }

        public ITreeWalker<TElement> ClimbUp(int levelCount)
        {
            while (levelCount > 0)
            {
                _current = _tree.GetParent(_current);
                --levelCount;
            }
            return this;
        }

        public ITreeWalker<TElement> ClimbDown(params int[] childrenIndexes)
        {
            foreach (var index in childrenIndexes)
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