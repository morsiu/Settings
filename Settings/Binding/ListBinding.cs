﻿// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheSettings.Binding.Infrastructure;
using TheSettings.Infrastructure.Collections;

namespace TheSettings.Binding
{
    public class ListBinding : CollectionBindingBase
    {
        public ListBinding(
            ICollectionAdapter targetAdapter,
            IValueAdapter sourceAdapter)
            : base(targetAdapter, sourceAdapter)
        {
        }

        protected override bool IsCollectionCompatible(object collectionObject)
        {
            return collectionObject as List<object> != null;
        }

        protected override ICollectionUpdater GetUpdaterFor(ICollection<object> collection)
        {
            return new ListUpdater((IList)collection);
        }

        protected override ICollection<object> CreateCollection(IEnumerable items)
        {
            return new List<object>(items.OfType<object>());
        }
    }
}