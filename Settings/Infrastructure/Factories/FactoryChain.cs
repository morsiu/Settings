﻿// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;

namespace TheSettings.Infrastructure.Factories
{
    public class FactoryChain<TFactory>
    {
        private readonly ICollection<TFactory> _factories = new List<TFactory>();

        /// <summary>
        /// Adds specified factory to the end of factory chain.
        /// </summary>
        /// <param name="factory"></param>
        public void RegisterFactory(TFactory factory)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            _factories.Add(factory);
        }

        /// <summary>
        /// Returns first non-null value obtained from specified creator
        /// called for each factory in their registration order.
        /// </summary>
        /// <typeparam name="TValue">Type of created value.</typeparam>
        /// <param name="creator">Function that given a factory creates a value by using it.</param>
        public TValue CreateValue<TValue>(Func<TFactory, TValue> creator)
        {
            if (creator == null) throw new ArgumentNullException("creator");
            return _factories.Select(creator).FirstOrDefault(value => value != null);
        }
    }
}
