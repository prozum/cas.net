using System;

namespace ImportExport
{
	public class TypeManager
	{
		public Type t;
		public Object o;
		// public string s;

		public TypeManager (Object o)
		{
			this.t = o.GetType ();
			this.o = o;
		}

		public override string ToString ()
		{
			return t.ToString ();
		}

		public object GetObject ()
		{
			return this.o;
		}
	}
}

