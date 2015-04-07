using System;

namespace ImEx
{
    public class MetaType
    {
        public Type type;
        public string serializedString;

        public MetaType()
        {
        }

        public MetaType(object o)
        {
            this.type = o.GetType();
            this.serializedString = Export.Serialize(o);
        }
    }
}

