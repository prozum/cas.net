using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SymbolicMath
{


	abstract public class Element : List<Element>
	{
		public enum Operators {Add, Subtract, Multiply, Divide, OpNum};
		public string[] OperatorStrings = {"+", "-", "*", "/"};
	
		abstract public string Repr {
			get;
		}

		public static Expression operator +(Element op1, Element op2)
		{
			Expression newExpression = new Expression ();

			newExpression.elements.Add(op1);

			newExpression.elements.Add (new Symbol("+"));

			newExpression.elements.Add(op2);

			return newExpression;
		}

		public static Expression operator -(Element op1, Element op2)
		{
			Expression newExpression = new Expression ();

			if (op1 is Expression) {
				newExpression.elements.Add(op1);
			} else {
				newExpression.elements.AddRange(op1);
			}

			newExpression.elements.Add (new Symbol("-"));

			if (op2 is Expression) {
				newExpression.elements.Add(op2);
			} else {
				newExpression.elements.AddRange(op2);
			}

			return newExpression;
		}

		public static Expression operator *(Element op1, Element op2)
		{
			Expression newExpression = new Expression ();
			if (op1 is Expression) {
				newExpression.elements.Add(op1);
			} else {
				newExpression.elements.AddRange(op1);
			}

			newExpression.elements.Add (new Symbol("*"));

			if (op2 is Expression) {
				newExpression.elements.Add(op2);
			} else {
				newExpression.elements.AddRange(op2);
			}

			return newExpression;
		}

		public static Expression operator /(Element op1, Element op2)
		{
			Expression newExpression = new Expression ();

			newExpression.elements.Add(op1);

			newExpression.elements.Add (new Symbol("/"));

			newExpression.elements.Add(op2);

			return newExpression;
		}


		public static Expression operator +(Element op1, decimal op2)
		{
			Expression newExpression = new Expression ();

			newExpression.elements.Add(op1);

			newExpression.elements.Add (new Symbol("+"));

			newExpression.elements.Add(new Literal(op2));

			return newExpression;
		}

		public static Expression operator -(Element op1, decimal op2)
		{
			Expression newExpression = new Expression ();

			newExpression.elements.Add(op1);

			newExpression.elements.Add (new Symbol("-"));

			newExpression.elements.Add(new Literal(op2));

			return newExpression;
		}

		public static Expression operator *(Element op1, decimal op2)
		{
			Expression newExpression = new Expression ();

			newExpression.elements.Add(op1);

			newExpression.elements.Add (new Symbol("*"));

			newExpression.elements.Add(new Literal(op2));

			return newExpression;
		}

		public static Expression operator /(Element op1, decimal op2)
		{
			Expression newExpression = new Expression ();

			newExpression.elements.Add(op1);

			newExpression.elements.Add (new Symbol("/"));

			newExpression.elements.Add(new Literal(op2));

			return newExpression;
		}
	}



	public class Symbol: Element
	{
		public string symbol;

		public override string Repr {
			get {
				return symbol;
			}
		}

		public Symbol(string str)
		{
			symbol = str;
		}

	}

	public class Literal: Element
	{
		public decimal real;
		public decimal imag;

		public override string Repr {
			get {
				if (imag != 0) {
					var sb = new System.Text.StringBuilder();

					sb.Append("(");
					sb.Append(real.ToString());
					sb.Append(" ");
					sb.Append(imag.ToString());
					sb.Append("I)");

					return sb.ToString();
				}
			    
				return real.ToString();
			}
		}

		public Literal(decimal r, decimal i)
		{
			real = r;
			imag = i;
		}

		public Literal(decimal r) : this(r, 0)
		{
		}
	}

	public class Operator: Element
	{

		public Operators type;

		public override string Repr {
			get {
				return OperatorStrings[(int)type];
			}
		}

		public Operator(Operators i)
		{
			if (i < Operators.OpNum) {
				type = i;
			}
		}

		public Operator(string str)
		{

			switch (str)
			{
				case "+":
					type = Operators.Add;
					break;
				case "-":
					type = Operators.Subtract;
					break;
				case "*":
					type = Operators.Multiply;
					break;
				case "/":
					type = Operators.Divide;
					break;
			}

		}
	}

	public class Expression: Element
	{
		public List<Element> elements;

		public Expression()
		{
			elements = new List<Element>();
		}

		public void Parse(string str)
		{
			var newExpression = new Expression();

			var array = str.ToCharArray();


			char c;

			//for (var i; i < array.Length; i++) {
			//	c = array[i];
			    
			//	if (Regex.IsMatch(c.ToString(), "0-9|."))
					;
			//	Operator.OperatorStings


			//	if IsLetter(c);
			//}

			//return ...
			//return newExpression;
		}

		public void Simplify()
		{
		}

		public static Expression operator +(Expression e1, Expression e2)
		{
			var newExpression = new Expression ();

			newExpression.elements.AddRange (e1.elements);
			newExpression.elements.Add (new Operator ("+"));
			newExpression.elements.AddRange (e2.elements);

			return newExpression;
		}

		public static Expression operator -(Expression e1, Expression e2)
		{
			var newExpression = new Expression ();

			newExpression.elements.Add (e1);
			newExpression.elements.Add (new Operator ("-"));
			newExpression.elements.Add (e2);

			return newExpression;
		}

		public static Expression operator *(Expression e1, Expression e2)
		{
			var newExpression = new Expression ();

			newExpression.elements.Add (e1);
			newExpression.elements.Add (new Operator ("*"));
			newExpression.elements.Add (e2);

			return newExpression;
		}

		public static Expression operator /(Expression e1, Expression e2)
		{
			var newExpression = new Expression ();

			newExpression.elements.Add (e1);
			newExpression.elements.Add (new Operator ("/"));
			newExpression.elements.Add (e2);

			return newExpression;
		}

		public static Expression operator +(Expression op1, decimal op2)
		{
			var newExpression = new Expression();

			newExpression.elements.AddRange (op1.elements);
			newExpression.elements.Add (new Symbol("+"));
			newExpression.elements.Add (new Literal (op2));

			return newExpression;
		}

		public static Expression operator -(Expression op1, decimal op2)
		{
			var newExpression = new Expression();

			newExpression.elements.AddRange (op1.elements);
			newExpression.elements.Add (new Symbol("-"));
			newExpression.elements.Add (new Literal (op2));

			return newExpression;
		}

		public static Expression operator *(Expression op1, decimal op2)
		{
			var newExpression = new Expression();

			newExpression.elements.AddRange (op1.elements);
			newExpression.elements.Add (new Symbol("*"));
			newExpression.elements.Add (new Literal (op2));

			return newExpression;
		}

		public static Expression operator /(Expression op1, decimal op2)
		{
			var newExpression = new Expression();

			newExpression.elements.AddRange (op1.elements);
			newExpression.elements.Add (new Symbol("/"));
			newExpression.elements.Add (new Literal (op2));

			return newExpression;
		}

		public override string Repr
		{
			get
			{
				var sb = new System.Text.StringBuilder ();

				sb.Append ("(");

				foreach (Element e in elements)
				{
					sb.Append(e.Repr);
				}

				sb.Append (")");
       			
				return sb.ToString();
			}

		}
	}
}