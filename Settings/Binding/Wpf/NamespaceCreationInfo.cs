using System.Windows;

namespace Settings.Binding.Wpf
{
    public class NamespaceCreationInfo
    {
        public NamespaceCreationInfo(string name, bool useParent, string parentSourceName, DependencyObject root)
        {
            Name = name;
            UseParent = useParent;
            ParentSourceName = parentSourceName;
            Root = root;
        }

        public string Name { get; private set; }

        public string ParentSourceName { get; private set; }

        public bool UseParent { get; private set; }

        public DependencyObject Root { get; private set; }

        public bool UseParentSource
        {
            get { return ParentSourceName != null; }
        }

        public bool CreateNew
        {
            get { return Name != null; }
        }
    }
}