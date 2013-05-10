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
    public class KeyedFactoryContainerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowWhenCreatingFromFactoryThatIsNotRegistered()
        {
            var container = new KeyedFactoryContainer<int, int>();

            container.CreateValue(5, factory => factory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenCreatingValueWithNullKey()
        {
            var container = new KeyedFactoryContainer<object, int>();

            container.CreateValue(null, factory => factory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenCreatingValueWithNullCreator()
        {
            var container = new KeyedFactoryContainer<object, int>();

            container.CreateValue<int>(new object(), null);
        }

        [TestMethod]
        public void ShouldReturnValueFromCreatorWhenCreatingOne()
        {
            var container = new KeyedFactoryContainer<int, int>();
            container.RegisterFactory(5, 5);

            var value = container.CreateValue(5, factory => 3);
            Assert.AreEqual(3, value);
        }

        [TestMethod]
        public void ShouldPassToCreatorFactoryWithMatchingKeyWhenCreatingValue()
        {
            var container = new KeyedFactoryContainer<int, int>();
            container.RegisterFactory(5, 10);
            var someFactory = 0;

            container.CreateValue<object>(5, factory => { someFactory = factory; return null; });

            Assert.AreEqual(10, someFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenRegisteringNullFactory()
        {
            var container = new KeyedFactoryContainer<int, object>();
            container.RegisterFactory(5, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenRegisteringNullKey()
        {
            var container = new KeyedFactoryContainer<object, object>();
            container.RegisterFactory(null, new object());
        }
    }
}
