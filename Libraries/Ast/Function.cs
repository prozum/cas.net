using System;
using System.Collections.Generic;

namespace Ast
{
	public class Function  : Expression
	{
		public string identifier;
        public List<Expression> args = new List<Expression>();

		public Function(string identifier, List<Expression> args)
		{
			this.identifier = identifier;
			this.args = args;
		}

		public override string ToString ()
		{
			string str = identifier + '(';

			for (int i = 0; i < args.Count; i++) {

				str += args[i].ToString ();

				if (i < args.Count - 1) {

					str += ',';
				}
			}

			return str + ')';
		}

		public override Expression Evaluate()
        {
			return this;
		}
	}
}

