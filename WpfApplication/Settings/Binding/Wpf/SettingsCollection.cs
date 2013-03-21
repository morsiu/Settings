using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WpfApplication.Settings.Binding.Wpf.Markup;

namespace WpfApplication.Settings.Binding.Wpf
{
    public class SettingBindingCollection : IList
    {
        private DependencyObject _owner;
        private readonly List<ISettingBindingsProvider> _providersToInitialize = new List<ISettingBindingsProvider>();

        public SettingBindingCollection(DependencyObject owner)
        {
            _owner = owner;
        }

        private readonly List<ISettingBinding> _bindings = new List<ISettingBinding>();

        public static readonly SettingBindingCollection Empty = new SettingBindingCollection(null);

        public void Add(ISettingBinding binding)
        {
            OnBindingAdded(binding);
        }

        public void Add(ISettingBindingsProvider bindingProvider)
        {
            OnBindingsProviderAdded(bindingProvider);
        }

        public void UpdateBindings()
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

        private void OnBindingsProviderAdded(ISettingBindingsProvider bindingsProvider)
        {
            var initializer = Markup.Settings.GetInitializer(_owner);
            if (initializer != null)
            {
                _providersToInitialize.Add(bindingsProvider);
                initializer.QueueSettingBindingsUpdate(this);
            }
            else
            {
                var newBindings = bindingsProvider.ProvideBindings(_owner);
                UpdateBindings(newBindings);
            }
        }

        private void OnBindingAdded(ISettingBinding bindingOrProvider)
        {
            _bindings.Add(bindingOrProvider);

            var initializer = Markup.Settings.GetInitializer(_owner);
            if (initializer != null)
            {
                initializer.QueueSettingBindingsUpdate(this);
            }
            else
            {
                bindingOrProvider.UpdateTarget();
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