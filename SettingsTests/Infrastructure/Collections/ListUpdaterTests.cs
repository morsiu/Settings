// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSettings.Infrastructure.Collections;

namespace TheSettingsTests.Infrastructure.Collections
{
    [TestClass]
    public class ListUpdaterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenConstructedWithNullTargetList()
        {
            new ListUpdater(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenUpdatingWithNullInstructions()
        {
            var updater = new ListUpdater(new List<int>());

            updater.Update(null);
        }

        [TestMethod]
        public void ShouldHandleResetByClearingTargetList()
        {
            var test = new TestCase(1, 2, 3);
            var instructions = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

            test.Update(instructions);

            test.AssertEmpty();
        }

        [TestMethod]
        public void ShouldHandleAddWithItemsByAddingThemToTargetList()
        {
            var test = new TestCase(1);
            var instructions = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { 2, 3 });

            test.Update(instructions);

            test.AssertEqual(1, 2, 3);
        }

        [TestMethod]
        public void ShouldHandleRemoveWithItemsByRemovingThemFromTargetList()
        {
            var test = new TestCase(1, 2, 3, 4, 5);
            var instructions = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new[] { 2, 4 });

            test.Update(instructions);

            test.AssertEqual(1, 3, 5);
        }

        [TestMethod]
        public void ShouldHandleResetWithNoItemsByClearingTargetItems()
        {
            var test = new TestCase(1, 2, 3);
            var instructions = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, (IList)null);

            test.Update(instructions);

            test.AssertEmpty();
        }

        [TestMethod]
        public void ShouldHandleAddWithItemByAddingItToTargetList()
        {
            var test = new TestCase(1, 2);
            var instructions = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, 3);

            test.Update(instructions);

            test.AssertEqual(1, 2, 3);
        }

        [TestMethod]
        public void ShouldHandleRemoveWithItemByRemovingItFromTargetList()
        {
            var test = new TestCase(1, 2, 3);
            var instructions = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, 2);

            test.Update(instructions);

            test.AssertEqual(1, 3);
        }

        [TestMethod]
        public void ShouldHandleResetWithoutItemByClearingTargetList()
        {
            var test = new TestCase(1, 2, 3);
            var instructions = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, null);

            test.Update(instructions);

            test.AssertEmpty();
        }

        [TestMethod]
        public void ShouldHandleReplaceOfItemsByReplacingOldOnesWithNewOnesInTargetList()
        {
            var test = new TestCase(1, 2, 3, 4);
            var instructions = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Replace,
                new[] { 5, 6, 7 },
                new[] { 1, 3, 4 });

            test.Update(instructions);

            test.AssertEqual(5, 2, 6, 7);
        }

        [TestMethod]
        public void ShouldHandleAddOfItemsWithIndexByInsertingThemAtThatIndexInTargetList()
        {
            var test = new TestCase(1, 2, 3);
            var instructions = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Add, 
                new[] { 4, 5 },
                1);

            test.Update(instructions);

            test.AssertEqual(1, 4, 5, 2, 3);
        }

        [TestMethod]
        public void ShouldHandleRemoveOfItemsWithIndexByRemovingThemFromTargetList()
        {
            var test = new TestCase(1, 2, 3, 4);
            var instructions = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Remove,
                new[] { 2, 4 },
                1);

            test.Update(instructions);

            test.AssertEqual(1, 3);
        }

        [TestMethod]
        public void ShouldHandleResetWithNoItemsAndInvalidIndexByClearingTargetList()
        {
            var test = new TestCase(1, 2, 3);
            var instructions = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Reset,
                null,
                -1);

            test.Update(instructions);

            test.AssertEmpty();
        }

        [TestMethod]
        public void ShouldHandleAddWithItemAndIndexByInsertingItAtThatIndexInTargetList()
        {
            var test = new TestCase(1, 2, 3);
            var instructions = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Add, 4, 1);

            test.Update(instructions);

            test.AssertEqual(1, 4, 2, 3);
        }

        [TestMethod]
        public void ShouldHandleRemoveWithItemAndIndexByRemovingItFromTargetList()
        {
            var test = new TestCase(1, 2, 3, 4);
            var instructions = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Remove, 3, 2);

            test.Update(instructions);

            test.AssertEqual(1, 2, 4);
        }

        [TestMethod]
        public void ShouldHandleResetWithItemAndInvalidIndexByClearingTargetList()
        {
            var test = new TestCase(1, 2, 3);
            var instructions = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Reset, null, -1);

            test.Update(instructions);

            test.AssertEmpty();
        }

        [TestMethod]
        public void ShouldHandleReplaceOfItemWithNewOneByReplacingItInTargetList()
        {
            var test = new TestCase(1, 2, 3, 4);
            var instructions = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Replace, 5, (object)3);

            test.Update(instructions);

            test.AssertEqual(1, 2, 5, 4);
        }

        [TestMethod]
        public void ShouldHandleReplaceOfItemsWithNewItemsByReplacingThemInTargetList()
        {
            var test = new TestCase(1, 2, 3, 4, 5);
            var instructions = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Replace,
                new[] { 6, 7 },
                new[] { 2, 4 },
                1);

            test.Update(instructions);

            test.AssertEqual(1, 6, 3, 7, 5);
        }

        [TestMethod]
        public void ShouldHandleMoveOfItemsBetweenIndexesByMovingThemInTargetList()
        {
            var test = new TestCase(1, 2, 3, 4, 5, 6, 7);
            var instructions = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Move,
                new[] { 3, 4 }, 4, 2);

            test.Update(instructions);

            test.AssertEqual(1, 2, 5, 6, 3, 4, 7);
        }

        [TestMethod]
        public void ShouldHandleMoveOfItemBetweenIndexesByMovingItInTargetList()
        {
            var test = new TestCase(1, 2, 3, 4, 5);
            var instructions = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Move, 4, 0, 3);

            test.Update(instructions);

            test.AssertEqual(4, 1, 2, 3, 5);
        }

        [TestMethod]
        public void ShouldHandleReplaceOfItemWithNewOneAtIndexByReplacingItInTargetList()
        {
            var test = new TestCase(1, 2, 3, 4, 5);
            var instructions = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Replace, 6, (object)3, 2);

            test.Update(instructions);

            test.AssertEqual(1, 2, 6, 4, 5);
        }

        private class TestCase
        {
            private readonly List<int> _items;
            private readonly ListUpdater _updater;

            public TestCase(params int[] items)
            {
                _items = new List<int>(items);
                _updater = new ListUpdater(_items);
            }

            public void Update(NotifyCollectionChangedEventArgs instructions)
            {
                _updater.Update(instructions);
            }

            public void AssertEmpty()
            {
                CollectionAssert.AreEqual(new int[0], _items);
            }

            public void AssertEqual(params int[] expected)
            {
                CollectionAssert.AreEqual(expected, _items);
            }
        }
    }
}
