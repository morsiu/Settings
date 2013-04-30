// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheSettings.Binding;
using TheSettingsTests.Mocks;

namespace TheSettingsTests
{
    [TestClass]
    public class SetBindingTests
    {
        private ValueAdapter _valueAdapter;
        private CollectionAdapter _collectionAdapter;
        private IEqualityComparer<object> _comparer;

        [TestInitialize]
        public void Initialize()
        {
            _collectionAdapter = new CollectionAdapter();
            _valueAdapter = new ValueAdapter();
            _comparer = EqualityComparer<object>.Default;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldNotConstructWithoutCollectionAdapter()
        {
            new SetBinding(null, _valueAdapter, _comparer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldNotConstructWithoutSettingAdapter()
        {
            new SetBinding(_collectionAdapter, null, _comparer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldNotConstructWithoutComparer()
        {
            new SetBinding(_collectionAdapter, _valueAdapter, null);
        }

        [TestMethod]
        public void ShouldPassToSourceUnchangedSetWhenNoChangesWereMade()
        {
            var expectedItems = new List<object> { 1, 2 };
            _valueAdapter.Value = expectedItems;
            var binding = new SetBinding(_collectionAdapter, _valueAdapter, _comparer);

            _collectionAdapter.CollectionChangedCallback(Enumerable.Empty<object>(), Enumerable.Empty<object>());
            CollectionAssert.AreEqual(expectedItems, _valueAdapter.ValueAsCollection);
        }

        [TestMethod]
        public void ShouldPassToSourceSetWithAddedItems()
        {
            var expectedItems = new List<object> { 1, 2 };
            _valueAdapter.Value = new List<object>();
            var binding = new SetBinding(_collectionAdapter, _valueAdapter, _comparer);

            _collectionAdapter.CollectionChangedCallback(new List<object> { 1, 2 }, Enumerable.Empty<object>());
            CollectionAssert.AreEqual(expectedItems, _valueAdapter.ValueAsCollection);
        }

        [TestMethod]
        public void ShouldPassToSourceSetWithoutRemovedItems()
        {
            var expectedItems = new List<object>();
            _valueAdapter.Value = new List<object> { 1, 2 };
            var binding = new SetBinding(_collectionAdapter, _valueAdapter, _comparer);

            _collectionAdapter.CollectionChangedCallback(Enumerable.Empty<object>(), new List<object> { 1, 2 });
            CollectionAssert.AreEqual(expectedItems, _valueAdapter.ValueAsCollection);
        }

        [TestMethod]
        public void ShouldPassToTargetNewItems()
        {
            var expectedItems = new List<object> { 1, 2 };
            var binding = new SetBinding(_collectionAdapter, _valueAdapter, _comparer);

            _valueAdapter.ValueChangedCallback(new List<object> { 1, 2 });
            CollectionAssert.AreEqual(expectedItems, _collectionAdapter.Items);
        }

        [TestMethod]
        public void UpdateTargetShouldPassItemsToTarget()
        {
            var expectedItems = new List<object> { 1, 2 };
            _valueAdapter.Value = new List<object> { 1, 2 };
            var binding = new SetBinding(_collectionAdapter, _valueAdapter, _comparer);

            binding.UpdateTarget();
            CollectionAssert.AreEqual(expectedItems, _collectionAdapter.Items);
        }

        [TestMethod]
        public void UpdateSourceShouldPassItemsToSource()
        {
            var expectedItems = new List<object> { 1, 2 };
            _collectionAdapter.Items = new List<object> { 1, 2 };
            var binding = new SetBinding(_collectionAdapter, _valueAdapter, _comparer);

            binding.UpdateSource();
            CollectionAssert.AreEqual(expectedItems, _valueAdapter.ValueAsCollection);
        }

        private class CollectionAdapter : ICollectionAdapter
        {
            public CollectionAdapter()
            {
                Items = new List<object>();
            }

            public List<object> Items { get; set; }

            public CollectionChangedCallbackHandler CollectionChangedCallback { set; get; }

            public IEnumerable GetItems()
            {
                return Items;
            }

            public void SetItems(IEnumerable items)
            {
                Items.Clear();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
        }
    }
}