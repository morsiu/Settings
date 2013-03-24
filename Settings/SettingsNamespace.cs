using System;

namespace TheSettings
{
    public class SettingsNamespace
    {
        public static readonly SettingsNamespace None = new SettingsNamespaceNullObject();

        public SettingsNamespace(SettingsNamespace parent, string name)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (name == null) throw new ArgumentNullException("name");
            Parent = parent;
            Name = name;
        }

        public SettingsNamespace(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            Parent = None;
            Name = name;
        }

        private SettingsNamespace()
        {
            Parent = None;
            Name = string.Empty;
        }

        public string Name { get; private set; }

        public SettingsNamespace Parent { get; private set; }

        public virtual string Path
        {
            get
            {
                return Parent + "\\" + Name;
            }
        }

        public override string ToString()
        {
            return Path;
        }

        private class SettingsNamespaceNullObject : SettingsNamespace
        {
            public override string Path
            {
                get
                {
                    return string.Empty;
                }
            }
        }
    }
}
