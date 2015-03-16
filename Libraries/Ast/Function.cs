using System;
using System.Collections.Generic;

namespace Ast
{
	public class Function  : Expression
	{
		public string identifier;
		public List<Expression> args = new List<Expression> ();

		protected Function(string identifier, List<Expression> args)
		{
			this.identifier = identifier;
			this.args = args;
		}
	}

	public class Cos : Function
	{
		public Cos(params Expression[] args) : base(args)
		{

		}
	}

	public class Sin : Function
	{
		public Sin(params Expression[] args) : base(args)
		{

		}
	}

	public class Tan : Function
	{
		public Tan(params Expression[] args) : base(args)
		{

		}
	}

	public class ACos : Function
	{
		public ACos(params Expression[] args) : base(args)
		{

		}
	}

	public class ASin : Function
	{
		public ASin(params Expression[] args) : base(args)
		{

		}
	}

	public class ATan: Function
	{
		public ATan(params Expression[] args) : base(args)
		{

		}
	}
}

