// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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

        public override bool Equals(object obj)
        {
            var other = obj as SettingsNamespace;
            if (obj == null) return false;
            return Equals(other);
        }

        public bool Equals(SettingsNamespace other)
        {
            if (other == null) return false;
            return Path.Equals(other.Path);
        }

        public override int GetHashCode()
        {
            return Path.GetHashCode();
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
