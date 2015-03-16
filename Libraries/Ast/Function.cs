using System;
using System.Collections.Generic;

namespace Ast
{
	public abstract class Function  : Expression
	{
		public List<Expression> arglist = new List<Expression> ();

		protected Function(params Expression[] args)
		{
			foreach (var item in args) {
				arglist.Add (item);
			}
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

