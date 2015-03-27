using System;

namespace ImportExport
{
    public class TypeManager
    {
        public Type t;
        // public Object o;
        public string s;

        public TypeManager(Object o)
        {
            this.t = o.GetType();
            // this.o = o;
            this.s = ImEx.Export.Serialize(o);
        }

        public TypeManager()
        {

        }

        public override string ToString()
        {
            return t.ToString();
        }

        public object GetObject()
        {
            return this.t + " " + this.s;
        }
    }
}

