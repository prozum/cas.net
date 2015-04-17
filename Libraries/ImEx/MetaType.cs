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
        // IMPORTANT:
        // type and serialized string must be public for deserializing

        public Type type;

        public string metastring1, metastring2, metastring3,
            metastring4, metastring5, metastring6, metastring7,
            metastring8, metastring9, metastring0;

        public int metaint1, metaint2, metaint3, metaint4,
            metaint5, metaint6, metaint7, metaint8, metaint9, metaint0;


        // If more than one string is, or if another metavar is needed, it is
        // recommended to create an empty metatype, and then add content manually.
        public MetaType()
        {
        }

        public MetaType(object o)
        {
            this.type = o.GetType();
            this.metastring0 = Export.Serialize(o);
        }

        public MetaType(Type t, string s)
        {
            this.type = t;
            this.metastring0 = s;
        }
    }
}

