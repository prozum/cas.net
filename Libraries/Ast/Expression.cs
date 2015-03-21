using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ast
{
	public abstract class Expression 
	{
		public Operator parent;
		//public abstract Expression Evaluate (Expression a, Expression b);
		//public abstract string ToString ();
		//public abstract bool Contains (Expression a);
	}
}