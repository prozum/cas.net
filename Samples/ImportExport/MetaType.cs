using System;

namespace ImportExport
{
    public class MetaType
    {
        public Type type;
        public string serializedString;

        public MetaType(Object obj)
        {
            this.type = obj.GetType();
            this.serializedString = ImEx.Export.Serialize(obj);
        }

        public MetaType()
        {

        }

        public override string ToString()
        {
            return type + " " + serializedString;
        }
    }
}

