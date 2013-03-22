namespace Settings
{
    public class SettingsNamespace
    {
        public SettingsNamespace(SettingsNamespace parent, string name)
        {
            Parent = parent;
            Name = name;
        }

        public static readonly SettingsNamespace None = new SettingsNamespace(null, null);

        public string Name { get; private set; }

        public SettingsNamespace Parent { get; private set; }

        public override string ToString()
        {
            return Parent == null
                ? Name ?? "{{{None}}}"
                : Parent + "\\" + Name;
        }
    }
}
