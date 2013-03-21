using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication.Settings.Binding.Wpf.Markup
{
    public class DataGridColumnsBinding : ISettingBindingsProvider
    {
        private const string DefaultColumnSettingFormat = "Column{0}{1}";
        private static readonly IEnumerable<DependencyProperty> StoredProperties =
            new[]
            {
                DataGridColumn.DisplayIndexProperty,
                DataGridColumn.SortDirectionProperty,
                DataGridColumn.WidthProperty,
                DataGridColumn.VisibilityProperty
            };

        public DataGridColumnsBinding()
        {
            ColumnSettingFormat = DefaultColumnSettingFormat;
        }

        public string ColumnSettingFormat { get; set; }

        public IEnumerable<ISettingBinding> ProvideBindings(DependencyObject target)
        {
            var factory = new SettingBindingFactory();
            var dataGrid = (DataGrid)target;
            var @namespace = Settings.GetNamespace(target);
            var bindings = dataGrid.Columns.SelectMany(
                (column, index) => BindColumn(column, index, @namespace, factory));
            return bindings;
        }

        private IEnumerable<ISettingBinding> BindColumn(
            DataGridColumn column, 
            int index, 
            SettingsNamespace @namespace, 
            SettingBindingFactory factory)
        {
            return
                from storedProperty in StoredProperties
                let name = string.Format(ColumnSettingFormat, index, storedProperty.Name)
                let binding = factory.Create(column, storedProperty, @namespace, name)
                select binding;
        }
    }
}
