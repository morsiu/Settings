using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSettings.Binding.ValueAdapters;

namespace TheSettings.Wpf.Binding.Adapters
{
    internal class DebugValueAdapterExceptionHandler
    {
        private const string ContextFormat = "The target property is `{0}`, of object `{1}` ({2}). The setting is `{3}`, with namespace `{4}` and store with key `{5}`";

        private static readonly IDictionary<ExceptionHandlingAdapter.Action, string> ActionNames =
            new Dictionary<ExceptionHandlingAdapter.Action, string>
            {
                { ExceptionHandlingAdapter.Action.GetValue, "getting" },
                { ExceptionHandlingAdapter.Action.SetValue, "setting" }
            };

        private string _context;

        public DebugValueAdapterExceptionHandler(string propertyName, object target, object settingsStoreKey, string settingName, SettingsNamespace settingsNamespace)
        {
            _context = string.Format(
                ContextFormat,
                propertyName,
                target,
                target == null ? "no type" : target.GetType().FullName,
                settingName,
                settingsNamespace,
                settingsStoreKey);
        }

        public bool HandleException(ExceptionHandlingAdapter.Action action, Exception exception)
        {
            WriteExceptionToDebug(action, exception);
            return true;
        }

        private void WriteExceptionToDebug(ExceptionHandlingAdapter.Action action, Exception exception)
        {
            Debug.WriteLine(
                string.Format(
                    "Exception {0} occured while {1} value from/to setting adapter. {2}.",
                    exception.GetType().FullName,
                    ActionNames[action],
                    _context));
        }
    }
}
