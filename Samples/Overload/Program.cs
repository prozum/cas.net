using System;

namespace Overload
{
	abstract class Expression
	{
		public virtual Expression Add(Integer e)
		{
			return new Error (this, "Don't support adding " + e.GetType().Name);
		}

		public virtual Expression Add(Irrational e)
		{
			return new Error (this, "Don't support adding " + e.GetType().Name);
		}

		public static Expression operator +(Expression r, dynamic l)
		{
			return r.Add(l);
		}
	}

	abstract class Number : Expression
	{
	}

	class Integer : Number
	{
		public Int64 value;

		public Integer(Int64 i)
		{
			value = i;
		}

		public override Expression Add(Integer i)
		{
			return new Integer(value + i.value);
		}

		public override Expression Add(Irrational f)
		{
			return new Irrational(value + f.value);
		}

		public override string ToString ()
		{
			return value.ToString ();
		}
	}

	class Irrational : Number
	{
		public decimal value;

		public Irrational(decimal ir)
		{
			value = ir;
		}

		public override string ToString ()
		{
			return value.ToString ();
		}
	}

	class Error : Expression
	{
		public string  msg;

		public Error (object obj, string msg)
		{
			this.msg = obj.GetType().Name + "> " + msg;
		}

		public override string ToString ()
		{
			return msg;
		}
	}



	class MainClass
	{
		public static void Main (string[] args)
		{
			var i = (Expression)new Integer (10);
			var ir = (Expression) new Irrational (32.2M);

			var n = i + i + ir;
			Console.Write (n.ToString());
		}
	}
}
