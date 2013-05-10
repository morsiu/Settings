// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TheSettings.Infrastructure.Factories;

namespace TheSettingsTests.Infrastructure.Factories
{
    [TestClass]
    public class FactoryChainTests
    {
        [TestMethod]
        public void ShouldUseRegisteredFactoryWhenCreatingValue()
        {
            var chain = new FactoryChain<Func<int>>();
            chain.RegisterFactory(() => 5);

            var value = chain.CreateValue<int>(factory => factory());

            Assert.AreEqual(5, value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldNotRegisterNullFactory()
        {
            var chain = new FactoryChain<object>();
            chain.RegisterFactory(null);
        }

        [TestMethod]
        public void ShouldCreateNullValueWhenNoFactoriesAreRegistered()
        {
            var chain = new FactoryChain<Func<object>>();

            var value = chain.CreateValue<object>(factory => factory());

            Assert.IsNull(value);
        }

        [TestMethod]
        public void ShouldUseFirstNonNullValueReturnedFromCreator()
        {
            var chain = new FactoryChain<Func<int?>>();
            chain.RegisterFactory(() => null);
            chain.RegisterFactory(() => 2);
            chain.RegisterFactory(() => 3);

            var value = chain.CreateValue<int?>(factory => factory());

            Assert.AreEqual(value, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowIfCreatorIsNullWhenCreatingValue()
        {
            var chain = new FactoryChain<object>();
            chain.CreateValue<object>(null);
        }
    }
}
