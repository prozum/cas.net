using System;

namespace Ast
{
	public abstract class Operator
	{
		public abstract Expression Evaluate(Expression a, Expression b);
	}

	public class Equals : Operator
	{
		public Equals()
		{

		}

		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}
	}

	public class Add : Operator
	{
		public Add()
		{

		}

		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}
	}

	public class Sub : Operator
	{
		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}
	}

	public class Mul : Operator
	{
		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}
	}

	public class Div : Operator
	{
		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}
	}

	public class Exp : Operator
	{
		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}
	}

	public class LesserThan : Operator
	{
		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}
	}

	public class GreaterThan : Operator
	{
		public override Expression Evaluate (Expression a, Expression b)
		{
			throw new NotImplementedException ();
		}
	}
}

