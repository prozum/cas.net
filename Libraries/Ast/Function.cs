using System;
using System.Collections.Generic;

namespace Ast
{
	public class Function  : Expression
	{
		public string identifier;
		public List<Expression> args = new List<Expression> ();

		public Function(string identifier, List<Expression> args)
		{
			this.identifier = identifier;
			this.args = args;
		}
	}
}

