using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheSettingsTests.Infrastructure
{
    internal static class AssertLifetime
    {
        public static void UnreachableInstanceIsNotLeaked<T>(
            Func<T> instanceGetter,
            Func<T, object> potentiallyLeakingCode)
            where T : class
        {
            WeakReference instanceReference = null;
            object potentiallyLeakingObject = null;
            new Action(
                () =>
                {
                    var instance = instanceGetter();
                    instanceReference = new WeakReference(instance);
                    potentiallyLeakingObject = potentiallyLeakingCode(instance);
                })();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.IsNull(instanceReference.Target, "The instance was still alive after GC.");
        }
    }
}
