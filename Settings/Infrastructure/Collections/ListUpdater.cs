// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;

namespace TheSettings.Infrastructure.Collections
{
    public class ListUpdater : ICollectionUpdater
    {
        private readonly IList _targetList;

        public ListUpdater(IList targetList)
        {
            if (targetList == null) throw new ArgumentNullException("targetList");
            _targetList = targetList;
        }

        public void Update(NotifyCollectionChangedEventArgs instructions)
        {
            if (instructions == null) throw new ArgumentNullException("instructions");
            switch (instructions.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (instructions.NewStartingIndex == -1)
                    {
                        AddItems(instructions.NewItems);
                    }
                    else
                    {
                        InsertItems(instructions.NewItems, instructions.NewStartingIndex);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    RemoveItems(instructions.OldItems);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    ReplaceItems(instructions.OldItems, instructions.NewItems);
                    break;

                case NotifyCollectionChangedAction.Move:
                    RemoveItems(instructions.OldItems);
                    InsertItems(instructions.NewItems, instructions.NewStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    _targetList.Clear();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddItems(IEnumerable newItems)
        {
            foreach (var item in newItems)
            {
                _targetList.Add(item);
            }
        }

        private void ReplaceItems(IList oldItems, IList newItems)
        {
            foreach (var itemIndex in Enumerable.Range(0, oldItems.Count))
            {
                var targetIndex = _targetList.IndexOf(oldItems[itemIndex]);
                _targetList[targetIndex] = newItems[itemIndex];
            }
        }

        private void InsertItems(IEnumerable newItems, int startingIndex)
        {
            startingIndex = Math.Min(startingIndex, _targetList.Count);
            foreach (var item in newItems.OfType<object>().Reverse())
            {
                _targetList.Insert(startingIndex, item);
            }
        }

        private void RemoveItems(IEnumerable oldItems)
        {
            foreach (var item in oldItems)
            {
                _targetList.Remove(item);
            }
        }
    }
}