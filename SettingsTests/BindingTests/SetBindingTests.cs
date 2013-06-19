// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheSettings.Binding;
using TheSettingsTests.Mocks;

namespace TheSettingsTests.BindingTests
{
    [TestClass]
    public class SetBindingTests
    {
        private ValueAdapter _sourceAdapter;
        private CollectionAdapter _targetAdapter;
        private IEqualityComparer<object> _comparer;

        [TestInitialize]
        public void Initialize()
        {
            _targetAdapter = new CollectionAdapter();
            _sourceAdapter = new ValueAdapter();
            _comparer = EqualityComparer<object>.Default;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldNotConstructWithoutCollectionAdapter()
        {
            new SetBinding(null, _sourceAdapter, _comparer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldNotConstructWithoutSettingAdapter()
        {
            new SetBinding(_targetAdapter, null, _comparer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldNotConstructWithoutComparer()
        {
            new SetBinding(_targetAdapter, _sourceAdapter, null);
        }

        [TestMethod]
        public void WhenTargetCollectionChangedAndItemsWhereAddedThenTheyShouldBePassedToSource()
        {
            var expectedItems = new List<object> { 1, 2 };
            _sourceAdapter.Value = new List<object>();
            var binding = new SetBinding(_targetAdapter, _sourceAdapter, _comparer);

            _targetAdapter.CollectionChangedCallback(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<object> { 1, 2 }));

            CollectionAssert.AreEquivalent(expectedItems, _sourceAdapter.ValueAsCollection);
        }

        [TestMethod]
        public void WhenTargetCollectionChangedAndItemsWereRemovedThenTheyShouldNotBePassedToSource()
        {
            var expectedItems = new List<object>();
            _sourceAdapter.Value = new List<object> { 1, 2 };
            var binding = new SetBinding(_targetAdapter, _sourceAdapter, _comparer);

            _targetAdapter.CollectionChangedCallback(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<object> { 1, 2 }));

            CollectionAssert.AreEquivalent(expectedItems, _sourceAdapter.ValueAsCollection);
        }

        [TestMethod]
        public void WhenSourceValueChangedAndItIsAnEnumerableItShouldBePassedToTarget()
        {
            var expectedItems = new List<object> { 1, 2 };
            var binding = new SetBinding(_targetAdapter, _sourceAdapter, _comparer);

            _sourceAdapter.ValueChangedCallback(new List<object> { 1, 1, 2, 2 });

            CollectionAssert.AreEquivalent(expectedItems, _targetAdapter.Items);
        }

        [TestMethod]
        public void WhenSourceValueChangedAndItIsNotAnEnumerableThenEmptySetShouldBePassedToTarget()
        {
            var expectedItems = new List<object>();
            var binding = new SetBinding(_targetAdapter, _sourceAdapter, _comparer);

            _sourceAdapter.ValueChangedCallback(new object());

            CollectionAssert.AreEquivalent(expectedItems, _targetAdapter.Items);
        }

        [TestMethod]
        public void WhenSourceItemsIsAnEnumerableThenUpdateTargetShouldPassItAsSetToTarget()
        {
            var expectedItems = new List<object> { 1, 2 };
            _sourceAdapter.Value = new List<object> { 1, 1, 2, 2 };
            var binding = new SetBinding(_targetAdapter, _sourceAdapter, _comparer);

            binding.UpdateTarget();

            CollectionAssert.AreEqual(expectedItems, _targetAdapter.Items);
        }

        [TestMethod]
        public void WhenSourceItemsIsNotAnEnumerableThenUpdateTargetShouldPassEmptySetToTarget()
        {
            var expectedItems = new List<object>();
            _sourceAdapter.Value = new object();
            var binding = new SetBinding(_targetAdapter, _sourceAdapter, _comparer);

            binding.UpdateTarget();

            CollectionAssert.AreEqual(expectedItems, _targetAdapter.Items);
        }

        [TestMethod]
        public void UpdateSourceShouldPassTargetItemsAsSetToSource()
        {
            var expectedItems = new List<object> { 1, 2 };
            _targetAdapter.Items = new List<object> { 1, 1, 2, 2 };
            var binding = new SetBinding(_targetAdapter, _sourceAdapter, _comparer);

            binding.UpdateSource();
            CollectionAssert.AreEqual(expectedItems, _sourceAdapter.ValueAsCollection);
        }
    }
}