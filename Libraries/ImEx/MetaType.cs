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
        // All are public for JSON to properly access them
        public Type type;
        public string metastring;
        public bool locked;

        // Empty constructor, for when setting the type, string and lock seperately.
        public MetaType()
        {
        }

        // Constructor used for serializing an object when all important elements are serializable.
        public MetaType(object o)
        {
            this.type = o.GetType();
            this.metastring = Export.Serialize(o);
        }

        // Constructor used when creating metatype where elements are known beforehand
        public MetaType(Type t, string s, bool locked)
        {
            this.type = t;
            this.metastring = s;
            this.locked = locked;
        }
    }
}

