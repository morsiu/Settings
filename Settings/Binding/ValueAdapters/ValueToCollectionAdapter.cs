// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSettings.Infrastructure;

namespace TheSettings.Binding.ValueAdapters
{
    public class ValueToCollectionAdapter : Disposable, ICollectionAdapter
    {
        private readonly IValueAdapter _valueAdapter;

        public ValueToCollectionAdapter(IValueAdapter valueAdapter)
        {
            if (valueAdapter == null) throw new ArgumentNullException("valueAdapter");
            _valueAdapter = valueAdapter;
            _valueAdapter.ValueChangedCallback = OnValueChanged;
        }

        private void OnValueChanged(object newValue)
        {
            if (IsDisposed)
            {
                return;
            }
            var callback = CollectionChangedCallback;
            if (callback == null)
            {
                return;
            }
            callback(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            callback(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { newValue }));
        }

        public CollectionChangedCallbackHandler CollectionChangedCallback { private get; set; }

        public IEnumerable GetItems()
        {
            FailIfDisposed();
            return new[] { _valueAdapter.GetValue() };
        }

        public void SetItems(IEnumerable items)
        {
            FailIfDisposed();
            _valueAdapter.SetValue(items.OfType<object>().FirstOrDefault());
        }
     
        protected override void DisposeManaged()
        {
            Dispose(_valueAdapter);
        }
    }
}
