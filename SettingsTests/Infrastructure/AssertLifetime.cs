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
            WeakReference instanceReferece = null;
            object potentiallyLeakingObject = null;
            new Action(
                () =>
                {
                    var instance = instanceGetter();
                    instanceReferece = new WeakReference(instance);
                    potentiallyLeakingObject = potentiallyLeakingCode(instance);
                })();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.IsNull(instanceReferece.Target, "The instance was still alive after GC.");
        }
    }
}
