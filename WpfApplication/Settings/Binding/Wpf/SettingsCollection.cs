using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfApplication.Settings.Binding.Wpf
{
    /// <summary>
    /// Collection that stores setting bindings for object that onws it.
    /// Also supports setting binding providers.
    /// </summary>
    public class SettingBindingCollection : IList
    {
        private readonly DependencyObject _owner;
        private readonly List<ISettingBindingsProvider> _providersToInitialize = new List<ISettingBindingsProvider>();
        private readonly List<ISettingBinding> _bindings = new List<ISettingBinding>();
        public static readonly SettingBindingCollection UnsetValue = new SettingBindingCollection(null);

        public SettingBindingCollection(DependencyObject owner)
        {
            _owner = owner;
        }

        public void Add(ISettingBinding binding)
        {
            _bindings.Add(binding);

            var initializer = Markup.Settings.GetInitializer(_owner);
            if (initializer != null)
            {
                initializer.QueueSettingsCollectionUpdate(this);
            }
            else
            {
                binding.UpdateTarget();
            }
        }

        public void Add(ISettingBindingsProvider bindingsProvider)
        {
            var initializer = Markup.Settings.GetInitializer(_owner);
            if (initializer != null)
            {
                _providersToInitialize.Add(bindingsProvider);
                initializer.QueueSettingsCollectionUpdate(this);
            }
            else
            {
                var newBindings = bindingsProvider.ProvideBindings(_owner);
                UpdateBindings(newBindings);
            }
        }

        /// <summary>
        /// Used by SettingsInitializer
        /// to update bindings inside collection
        /// and run bindings providers added to it.
        /// </summary>
        internal void Initialize()
        {
            RunProviders();
            UpdateBindings(_bindings);
        }

        private void UpdateBindings(IEnumerable<ISettingBinding> bindings)
        {
            foreach(var binding in bindings)
            {
                binding.UpdateTarget();
            }
        }

        private void RunProviders()
        {
            if (_providersToInitialize.Any())
            {
                var newBindings = _providersToInitialize.SelectMany(provider => provider.ProvideBindings(_owner));
                _bindings.AddRange(newBindings);
                _providersToInitialize.Clear();
            }
        }

        // This class implements IList just so XAML reader recognizes it's a collection.
        // Furthermore, it only needs a working Add implementation, so the rest is left out.

        #region Implementation of IList

        int IList.Add(object value)
        {
            var bindingProvider = value as ISettingBindingsProvider;
            if (bindingProvider != null)
            {
                Add(bindingProvider);
                return -1;
            }
            else
            {
                var binding = (ISettingBinding)value;
                Add(binding);
                return _bindings.IndexOf(binding);
            }
        }

        void IList.Clear()
        {
            throw new System.NotImplementedException();
        }

        bool IList.Contains(object value)
        {
            throw new System.NotImplementedException();
        }

        int IList.IndexOf(object value)
        {
            throw new System.NotImplementedException();
        }

        void IList.Insert(int index, object value)
        {
            throw new System.NotImplementedException();
        }

        bool IList.IsFixedSize
        {
            get { throw new System.NotImplementedException(); }
        }

        bool IList.IsReadOnly
        {
            get { throw new System.NotImplementedException(); }
        }

        void IList.Remove(object value)
        {
            throw new System.NotImplementedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        object IList.this[int index]
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        void ICollection.CopyTo(System.Array array, int index)
        {
            throw new System.NotImplementedException();
        }

        int ICollection.Count
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ICollection.IsSynchronized
        {
            get { throw new System.NotImplementedException(); }
        }

        object ICollection.SyncRoot
        {
            get { throw new System.NotImplementedException(); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}