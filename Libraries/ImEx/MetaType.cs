using System;

/* Usage:
 * The MetaType is used to store any type in an easily identified type.
 * It is meant for packing any objects into one type of obejct, that 
 * can be serialized before being sent to the server.
 * And by using the MetaType type, it is possible to deserialize it
 * before checking it's type, allowing it to return its content to an object.
 */

namespace ImEx
{
    public class MetaType
    {
        public Type type;

        public string metastring;

        public MetaType()
        {
        }

        public MetaType(object o)
        {
            this.type = o.GetType();
            this.metastring = Export.Serialize(o);
        }

        public MetaType(Type t, string s)
        {
            this.type = t;
            this.metastring = s;
        }
    }
}

