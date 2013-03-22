using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace WpfApplication.Functional
{
    [TestClass]
    public class MatchTests
    {
        [TestMethod]
        public void ShouldMatchProperPattern()
        {
            int value = new Match<int>('a', 'b')
                            {
                                { new [] { 'a', 'c' }, () => 1 },
                                { new [] { 'a', Any.Value }, () => 3 },
                                { new [] { Any.Value, Any.Value }, () => 2 }
                            };
            Assert.AreEqual(3, value);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldReportInexhaustiveMatch()
        {
            int value = new Match<int>('c', 'd')
                            {
                                { new [] {'a', 'c'}, () => 1 }
                            };
        }
    }
}
