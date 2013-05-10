// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;

namespace TheSettings.Infrastructure.Factories
{
    public class KeyedFactoryContainer<TKey, TFactory>
    {
        private IDictionary<TKey, TFactory> _factories = new Dictionary<TKey, TFactory>();

        public TValue CreateValue<TValue>(TKey key, Func<TFactory, TValue> creator)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (creator == null) throw new ArgumentNullException("creator");
            if (!_factories.ContainsKey(key)) throw new ArgumentException(string.Format("There is no factory registered with key {0}", key), "key");
            var factory = _factories[key];
            return creator(factory);
        }

        public void RegisterFactory(TKey key, TFactory factory)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (factory == null) throw new ArgumentNullException("factory");
            _factories.Add(key, factory);
        }
    }
}
