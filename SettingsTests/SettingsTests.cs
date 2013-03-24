using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TheSettings.Binding.Wpf.Markup;

namespace TheSettingsTests
{
    [TestClass]
    public class SettingsTests
    {
        [TestMethod]
        public void ShouldGetDistinctEmptyCollectionsAsSettingsPropertyDefaultValueForDifferentObjects()
        {
            var objectA = new DependencyObject();
            var objectB = new DependencyObject();
            var collectionA = Settings.GetSettings(objectA);
            var collectionB = Settings.GetSettings(objectB);
            Assert.AreNotSame(collectionA, collectionB);
        }
    }
}
