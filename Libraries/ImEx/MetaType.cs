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
        public string serializedString;

        public MetaType ()
        {
        }

        public MetaType (object o)
        {
            this.type = o.GetType ();
            this.serializedString = Export.Serialize (o);
        }
    }
}

