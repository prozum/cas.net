using System;
using System.Linq;
using System.Collections.Generic;

namespace Ast
{
    public abstract class Function : NotNumber
    {
        public List<Expression> args;

        public Function(string identifier, Number prefix, Number exponent) : base(identifier, prefix, exponent) { }

        public override string ToString ()
        {
            string str = "";

            if (((prefix is Integer) && (prefix as Integer).value != 1) || ((prefix is Rational) && (prefix as Rational).value.value != 1) || ((prefix is Irrational) && (prefix as Irrational).value != 1))
            {
                str += prefix.ToString() + identifier + '(';
            }
            else
            {
                str += identifier + '(';
            }

            for (int i = 0; i < args.Count; i++) 
            {
                str += args[i].ToString ();

                if (i < args.Count - 1) 
                {
                    str += ',';
                }
            }

            str += ')';

            if (((exponent is Integer) && (exponent as Integer).value != 1) || ((exponent is Rational) && (exponent as Rational).value.value != 1) || ((exponent is Irrational) && (exponent as Irrational).value != 1))
            {
                str += '^' + exponent.ToString();
            }

            return str;
        }

        public override bool CompareTo(Expression other)
        {
            if (other is Function)
            {
                return identifier == (other as Function).identifier && prefix.CompareTo((other as Function).prefix) && exponent.CompareTo((other as Function).exponent) && CompareArgsTo(other as Function);
            }

            if (this is UserDefinedFunction)
            {
                return (this as UserDefinedFunction).GetValue().CompareTo(other);
            }

            return false;
        }

        public bool CompareArgsTo(Function other)
        {
            bool res = true;

            if (args.Count == (other as Function).args.Count)
            {
                for (int i = 0; i < args.Count; i++)
                {
                    if (!args[i].CompareTo((other as Function).args[i]))
                    {
                        res = false;
                        break;
                    }
                }
            }
            else
            {
                res = false;
            }

            return res;
        }

        public override Expression Simplify()
        {
            if (prefix.CompareTo(new Integer(0)))
            {
                return new Integer(0);
            }
            if (exponent.CompareTo(new Integer(0)))
            {
                return new Integer(1);
            }

            return base.Simplify();
        }

        protected override T MakeClone<T>()
        {
            T res = base.MakeClone<T>();
            (res as Function).args = new List<Expression>(args);

            return res;
    }
    }

    public class UserDefinedFunction : Function
    {
        public Dictionary<string, Expression> tempDefinitions;

        public UserDefinedFunction() : this(null, null, new Integer(1), new Integer(1)) { }
        public UserDefinedFunction(string identifier, List<Expression> args) : this(identifier, args, new Integer(1), new Integer(1)) { }
        public UserDefinedFunction(string identifier, List<Expression> args, Number prefix, Number exponent) : base(identifier, prefix, exponent)
        {
            this.args = args;
        }

        public override Expression Evaluate()
        {
            
            return Evaluator.SimplifyExp(GetValue()).Evaluate();
        }

        public Expression GetValue() { return GetValue(this); }
        public Expression GetValue(NotNumber other)
        {
            List<string> functionParemNames;
            Expression res;

            tempDefinitions = new Dictionary<string, Expression>(evaluator.variableDefinitions);

            if (evaluator.functionParams.TryGetValue(identifier, out functionParemNames))
            {
                if (functionParemNames.Count == args.Count)
                {
                    for (int i = 0; i < functionParemNames.Count; i++)
                    {
                        if (tempDefinitions.ContainsKey(functionParemNames[i]))
                        {
                            tempDefinitions.Remove(functionParemNames[i]);
                        }

                        tempDefinitions.Add(functionParemNames[i], args[i]);
                    }

                    evaluator.functionDefinitions.TryGetValue(identifier, out res);

                    res.SetFunctionCall(this);

                    if (res.ContainsNotNumber(other))
                    {
                            return new Error(this, "Could not get value of: " + callerIdentifier);
                    }

                    return ReturnValue(res);
                else if (functionParemNames.Count == 0)
                {
                    return new Error(this, "Can't call function with 0 parameters");
                }
                else
                {
                    return new Error(this, "Function has the wrong number for parameters");
                }
            }
            else
            {
                return new Error(this, "Function has no definition");
            }
        }

        private Expression ReturnValue(Expression definition)
        {
            Expression res = null;

            if (prefix.CompareTo(new Integer(0)))
            {
                res = new Integer(0);
            }
            else
            {
                if (exponent.CompareTo(new Integer(0)))
                {
                    res = prefix.Clone();
                }
                else
                {
                    if (!exponent.CompareTo(new Integer(1)))
                    {
                        res = new Exp(definition, exponent);
                    }
                    else
                    {
                        res = definition;
                    }

                    if (!prefix.CompareTo(new Integer(1)))
                    {
                        return new Mul(prefix, res);
                    }
	            }
            }

            return res;
        }

        public override NotNumber Clone()
        {
            return MakeClone<UserDefinedFunction>();
        }
    }

    public abstract class UnaryOperation : Function
    {
        public UnaryOperation() : this(null, null, new Integer(1), new Integer(1)) { }
        public UnaryOperation(string identifier, Expression arg) : this(identifier, arg, new Integer(1), new Integer(1)) { }
        public UnaryOperation(string identifier, Expression arg, Number prefix, Number exponent) : base(identifier, prefix, exponent)
        {
            this.args = new List<Expression>();
            args.Add(arg);
        }
    }

    public class Sin : UnaryOperation
    {
        public Sin() : this(null, null, new Integer(1), new Integer(1)) { }
        public Sin(string identifier, Expression arg) : base(identifier, arg) { }
        public Sin(string identifier, Expression arg, Number prefix, Number exponent) : base(identifier, arg, prefix, exponent) { }

        public override Expression Evaluate()
        {
            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)Math.Sin((res as Integer).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }
            
            if (res is Rational)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)Math.Sin((double)(res as Rational).value.value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }
            
            if (res is Irrational)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)Math.Sin((double)(res as Irrational).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }

            return new Error(this, "Could not take Sin of: " + args[0]);
        }

        public override NotNumber Clone()
        {
            return MakeClone<Sin>();
    }
    }

    public class ASin : UnaryOperation
    {
        public ASin() : this(null, null, new Integer(1), new Integer(1)) { }
        public ASin(string identifier, Expression arg) : base(identifier, arg) { }
        public ASin(string identifier, Expression arg, Number prefix, Number exponent) : base(identifier, arg, prefix, exponent) { }

        public override Expression Evaluate()
        {
            var res = args[0].Evaluate();
            if (res is Integer)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)(Math.Asin((res as Integer).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }

            if (res is Rational)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)(Math.Asin((double)(res as Rational).value.value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }

            if (res is Irrational)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)(Math.Asin((double)(res as Irrational).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }

            return new Error(this, "Could not take ASin of: " + args[0]);
        }

        public override NotNumber Clone()
        {
            return MakeClone<ASin>();
        }
    }

    public class Cos : UnaryOperation
    {
        public Cos() : this(null, null, new Integer(1), new Integer(1)) { }
        public Cos(string identifier, Expression arg) : base(identifier, arg) { }
        public Cos(string identifier, Expression arg, Number prefix, Number exponent) : base(identifier, arg, prefix, exponent) { }

        public override Expression Evaluate()
        {
            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)Math.Cos((res as Integer).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }

            if (res is Rational)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)Math.Cos((double)(res as Rational).value.value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }

            if (res is Irrational)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)Math.Cos((double)(res as Irrational).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }

            return new Error(this, "Could not take Cos of: " + args[0]);
        }

        public override NotNumber Clone()
        {
            return MakeClone<Cos>();
    }
    }

    public class ACos : UnaryOperation
    {
        public ACos() : this(null, null, new Integer(1), new Integer(1)) { }
        public ACos(string identifier, Expression arg) : base(identifier, arg) { }
        public ACos(string identifier, Expression arg, Number prefix, Number exponent) : base(identifier, arg, prefix, exponent) { }

        public override Expression Evaluate()
        {
            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)(Math.Acos((res as Integer).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }

            if (res is Rational)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)(Math.Acos((double)(res as Rational).value.value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }

            if (res is Irrational)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)(Math.Acos((double)(res as Irrational).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }

            return new Error(this, "Could not take ACos of: " + args[0]);
        }

        public override NotNumber Clone()
        {
            return MakeClone<ACos>();
    }
    }

    public class Tan : UnaryOperation
    {
        public Tan() : this(null, null, new Integer(1), new Integer(1)) { }
        public Tan(string identifier, Expression arg) : base(identifier, arg) { }
        public Tan(string identifier, Expression arg, Number prefix, Number exponent) : base(identifier, arg, prefix, exponent) { }

        public override Expression Evaluate()
        {
            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)Math.Tan((res as Integer).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }

            if (res is Rational)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)Math.Tan((double)(res as Rational).value.value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }

            if (res is Irrational)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)Math.Tan((double)(res as Irrational).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }

            return new Error(this, "Could not take Tan of: " + args[0]);
        }

        public override NotNumber Clone()
        {
            return MakeClone<Tan>();
        }
    }

    public class ATan : UnaryOperation
    {
        public ATan() : this(null, null, new Integer(1), new Integer(1)) { }
        public ATan(string identifier, Expression arg) : base(identifier, arg) { }
        public ATan(string identifier, Expression arg, Number prefix, Number exponent) : base(identifier, arg, prefix, exponent) { }

        public override Expression Evaluate()
        {
            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)(Math.Atan((res as Integer).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }

            if (res is Rational)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)(Math.Atan((double)(res as Rational).value.value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }

            if (res is Irrational)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)(Math.Atan((double)(res as Irrational).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0))), exponent)).Evaluate();
            }

            return new Error(this, "Could not take ATan of: " + args[0]);
        }

        public override NotNumber Clone()
        {
            return MakeClone<ATan>();
    }
    }

    public class Sqrt : UnaryOperation
    {
        public Sqrt() : this(null, null, new Integer(1), new Integer(1)) { }
        public Sqrt(string identifier, Expression arg) : base(identifier, arg) { }
        public Sqrt(string identifier, Expression arg, Number prefix, Number exponent) : base(identifier, arg, prefix, exponent) { }

        public override Expression Evaluate()
        {
            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)Math.Sqrt((res as Integer).value)), exponent)).Evaluate();
            }

            if (res is Rational)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)Math.Sqrt((double)(res as Rational).value.value)), exponent)).Evaluate();
            }

            if (res is Irrational)
            {
                return new Mul(prefix, new Exp(new Irrational((decimal)Math.Sqrt((double)(res as Irrational).value)), exponent)).Evaluate();
            }

            return new Error(this, "Could not take Sqrt of: " + args[0]);
        }

        public override Expression Simplify()
        {
            if (exponent.CompareTo(new Integer(2)))
            {
                return args[0];
            }

            return base.Simplify();
        }

        public override NotNumber Clone()
        {
            return MakeClone<Sqrt>();
    }
    }

    public class Negation : UnaryOperation
    {
        public Negation(string identifier, Expression arg) : base(identifier, arg) { }

        public override Expression Evaluate()
        {
            throw new NotImplementedException();
        }

        public override NotNumber Clone()
        {
            throw new NotImplementedException();
        }
    }

    public class Simplify : UnaryOperation
    {
        public Simplify(string identifier, Expression arg) : base(identifier, arg) { }

        public override Expression Evaluate()
        {
            return Evaluator.SimplifyExp(args[0]);
        }

        public override NotNumber Clone()
        {
            throw new NotImplementedException();
    }
    }

    public class Expand : UnaryOperation
    {
        public Expand(string identifier, Expression arg) : base(identifier, arg) { }

        public override Expression Evaluate()
        {
            return Evaluator.ExpandExp(args[0]);
        }

        public override NotNumber Clone()
        {
            throw new NotImplementedException();
    }
    }

    public class Range : UnaryOperation
    {
        public Range(string identifier, Expression arg) : base(identifier, arg) { }

        public override Expression Evaluate()
        {
            if (args[0] is Integer)
            {
                var list = new Ast.List ();

                Int64 max = ((Integer)args[0]).value;

                for (Int64 i = 0; i < max; i++)
                {
                    list.elements.Add(new Integer(i));
                }

                return list;
            }

            return new Error(this, "Range only supports integers");
        }

        public override NotNumber Clone()
        {
            throw new NotImplementedException();
        }
    }
}

