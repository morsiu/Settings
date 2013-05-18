using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSettings.Binding;
using TheSettings.Binding.ValueAdapters;
using TheSettingsTests.Mocks;

namespace TheSettingsTests.ValueAdapterTests.ExceptionHandlingAdapterTests
{
    [TestClass]
    public class ExceptionHandlingAdapterTests
    {
        private static readonly ExceptionHandlingAdapter.ExceptionHandler IgnoreException = (action, exception) => true;

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenConstructedWithNullSourceAdapter()
        {
            new ExceptionHandlingAdapter(null, IgnoreException);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenConstructedWithNullExceptionHandler()
        {
            new ExceptionHandlingAdapter(new Mock<IValueAdapter>().Object, null);
        }

        [TestMethod]
        public void ShouldGetValueFromSourceAdapterWhenGettingValue()
        {
            var sourceAdapter = new ValueAdapter();
            sourceAdapter.Value = 5;
            var adapter = new ExceptionHandlingAdapter(sourceAdapter, IgnoreException);

            var value = adapter.GetValue();

            Assert.AreEqual(5, value);
        }

        [TestMethod]
        public void ShouldSetValueToSourceAdapterWhenSettingValue()
        {
            var sourceAdapter = new ValueAdapter();
            var adapter = new ExceptionHandlingAdapter(sourceAdapter, IgnoreException);

            adapter.SetValue(5);

            Assert.AreEqual(5, sourceAdapter.Value);
        }

        [TestMethod]
        public void ShouldForwardSourceValueChangedCallbackCalls()
        {
            var sourceAdapter = new ValueAdapter();
            var adapter = new ExceptionHandlingAdapter(sourceAdapter, IgnoreException);
            var callbackCalled = false;
            adapter.ValueChangedCallback = newValue => callbackCalled = true;

            sourceAdapter.ValueChangedCallback(5);

            Assert.IsTrue(callbackCalled);
        }

        [TestMethod]
        public void ShouldForwardConvertedValueFromSourceValueChangedCallback()
        {
            var sourceAdapter = new ValueAdapter();
            var adapter = new ExceptionHandlingAdapter(sourceAdapter, IgnoreException);
            object actualValue = null;
            adapter.ValueChangedCallback = newValue => actualValue = newValue;

            sourceAdapter.ValueChangedCallback(5);

            Assert.AreEqual(5, actualValue);
        }

        [TestMethod]
        public void ShouldCallExceptionHandlerOnExceptionWhileGettingValue()
        {
            var sourceAdapter = GetThrowingValueAdapter(new Exception());
            var wasCalled = false;
            var adapter = new ExceptionHandlingAdapter(sourceAdapter, (action, exception) => { wasCalled = true; return true; });

            try
            {
                adapter.GetValue();
            }
            catch
            {
            };

            Assert.IsTrue(wasCalled);
        }

        [TestMethod]
        public void ShouldPassExceptionToExceptionHandlerOnExceptionWhileGettingValue()
        {
            var expectedException = new Exception();
            var sourceAdapter = GetThrowingValueAdapter(expectedException);
            Exception actualException = null;
            var adapter = new ExceptionHandlingAdapter(sourceAdapter, (action, exception) => { actualException = exception; return true; });

            try
            {
                adapter.GetValue();
            }
            catch
            {
            };

            Assert.AreSame(expectedException, actualException);
        }

        [TestMethod]
        public void ShouldPassGetValueActionToExceptionHandlerOnExceptionWhileGettingValue()
        {
            var sourceAdapter = GetThrowingValueAdapter(new Exception());
            ExceptionHandlingAdapter.Action? actualAction = null;
            var adapter = new ExceptionHandlingAdapter(sourceAdapter, (action, exception) => { actualAction = action; return true; });

            try
            {
                adapter.GetValue();
            }
            catch
            {
            };

            Assert.AreEqual(ExceptionHandlingAdapter.Action.GetValue, actualAction);
        }

        [TestMethod]
        public void ShouldNotRethrowExceptionIfHandlerReturnsTrueOnExceptionWhileGettingValue()
        {
            var sourceAdapter = GetThrowingValueAdapter(new Exception());
            var adapter = new ExceptionHandlingAdapter(sourceAdapter, (action, exception) => true);

            try
            {
                adapter.GetValue();
            }
            catch
            {
                Assert.Fail("Should have not thrown the exception.");
            };
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ShouldRethrowExceptionIfHandlerReturnsFalseOnExceptionWhileGettingValue()
        {
            var sourceAdapter = GetThrowingValueAdapter(new Exception());
            var adapter = new ExceptionHandlingAdapter(sourceAdapter, (action, exception) => false);

            adapter.GetValue();
        }

        [TestMethod]
        public void ShouldCallExceptionHandlerOnExceptionWhileSettingValue()
        {
            var sourceAdapter = GetThrowingValueAdapter(new Exception());
            var wasCalled = false;
            var adapter = new ExceptionHandlingAdapter(sourceAdapter, (action, exception) => { wasCalled = true; return true; });

            try
            {
                adapter.SetValue(null);
            }
            catch
            {
            };

            Assert.IsTrue(wasCalled);
        }

        [TestMethod]
        public void ShouldPassExceptionToExceptionHandlerOnExceptionWhileSettingValue()
        {
            var expectedException = new Exception();
            var sourceAdapter = GetThrowingValueAdapter(expectedException);
            Exception actualException = null;
            var adapter = new ExceptionHandlingAdapter(sourceAdapter, (action, exception) => { actualException = exception; return true; });

            try
            {
                adapter.SetValue(null);
            }
            catch
            {
            };

            Assert.AreSame(expectedException, actualException);
        }

        [TestMethod]
        public void ShouldPassSetValueActionToExceptionHandlerOnExceptionWhileSettingValue()
        {
            var sourceAdapter = GetThrowingValueAdapter(new Exception());
            ExceptionHandlingAdapter.Action? actualAction = null;
            var adapter = new ExceptionHandlingAdapter(sourceAdapter, (action, exception) => { actualAction = action; return true; });

            try
            {
                adapter.SetValue(null);
            }
            catch
            {
            };

            Assert.AreEqual(ExceptionHandlingAdapter.Action.SetValue, actualAction);
        }

        [TestMethod]
        public void ShouldNotRethrowExceptionIfHandlerReturnsTrueOnExceptionWhileSettingValue()
        {
            var sourceAdapter = GetThrowingValueAdapter(new Exception());
            var adapter = new ExceptionHandlingAdapter(sourceAdapter, (action, exception) => true);

            try
            {
                adapter.SetValue(null);
            }
            catch
            {
                Assert.Fail("Should have not thrown the exception.");
            };
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ShouldRethrowExceptionIfHandlerReturnsFalseOnExceptionWhileSettingValue()
        {
            var sourceAdapter = GetThrowingValueAdapter(new Exception());
            var adapter = new ExceptionHandlingAdapter(sourceAdapter, (action, exception) => false);

            adapter.SetValue(null);
        }

        private IValueAdapter GetThrowingValueAdapter(Exception exception)
        {
            var adapter = new Mock<IValueAdapter>();
            adapter.Setup(a => a.GetValue()).Throws(exception);
            adapter.Setup(a => a.SetValue(It.IsAny<object>())).Throws(exception);
            return adapter.Object;
        }
    }
}
