using System;
using System.Linq;
using System.Collections.Generic;

namespace Ast
{
    public enum ArgKind
    {
        Expression,
        Number,
        Symbol,
        Function,
        Equation
    }

    public abstract class Function : Variable
    {
        public List<Expression> args;
        public List<ArgKind> validArgs;

        public Function(string identifier, List<Expression> args, Evaluator evaluator)
            : base(identifier, evaluator) 
        {
            this.args = args;
        }

        public bool isArgsValid()
        {
            if (args.Count != validArgs.Count)
                return false;

            for (int i = 0; i < args.Count; i++)
            {
                switch (validArgs[i])
                {
                    case ArgKind.Expression:
                        if (!(args[i] is Expression))
                            return false;
                        break;
                    case ArgKind.Number:
                        if (!(args[i] is Number))
                            return false;
                        break;
                    case ArgKind.Symbol:
                        if (!(args[i] is Symbol))
                            return false;
                        break;
                    case ArgKind.Function:
                        if (!(args[i] is Function))
                            return false;
                        break;
                    case ArgKind.Equation:
                        if (!(args[i] is Equal))
                            return false;
                        break;
                }
            }

            return true;
        }

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

        public override void SetFunctionCall(UserDefinedFunction functionCall)
        {
            foreach (var item in args)
            {
                item.SetFunctionCall(functionCall);
            }

            base.SetFunctionCall(functionCall);
        }
    }

    public class UserDefinedFunction : Function
    {
        public Dictionary<string, Expression> tempDefinitions;

        public UserDefinedFunction() : this(null, null, null) { }
        public UserDefinedFunction(string identifier, List<Expression> args, Evaluator evaluator) : base(identifier, args, evaluator) { }

        public override Expression Evaluate()
        {
            return Evaluator.SimplifyExp(GetValue()).Evaluate();
        }

        public Expression GetValue() { return GetValue(this); }
        public Expression GetValue(Variable other)
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

                    if (res.ContainsVariable(other))
                    {
                        return new Error(this, "Could not get value of: " + other.identifier);
                    }

                    return ReturnValue(res.Clone());
                }
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

        public override Expression Clone()
        {
            return MakeClone<UserDefinedFunction>();
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

            return GetValue();
        }
    }

    public class Sin : Function, IInvertable
    {
        public Sin() : this(null) { }
        public Sin(List<Expression> args)
            : base("sin", args, null)
        {
            validArgs = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)Math.Sin((res as Integer).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }
            
            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)Math.Sin((double)(res as Rational).value.value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }
            
            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)Math.Sin((double)(res as Irrational).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }

            return new Error(this, "Could not take Sin of: " + args[0]);
        }

        public override Expression Clone()
        {
            return MakeClone<Sin>();
        }

        public Expression Inverted(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new ASin(newArgs);
        }
    }

    public class ASin : Function, IInvertable
    {
        public ASin() : this(null) { }
        public ASin(List<Expression> args)
            : base("asin", args, null)
        {
            validArgs = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)(Math.Asin((res as Integer).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)(Math.Asin((double)(res as Rational).value.value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)(Math.Asin((double)(res as Irrational).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }

            return new Error(this, "Could not take ASin of: " + args[0]);
        }

        public override Expression Clone()
        {
            return MakeClone<ASin>();
        }

        public Expression Inverted(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new Sin(newArgs);
        }
    }

    public class Cos : Function, IInvertable
    {
        public Cos() : this(null) { }
        public Cos(List<Expression> args)
            : base("cos", args, null)
        {
            validArgs = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)Math.Cos((res as Integer).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)Math.Cos((double)(res as Rational).value.value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)Math.Cos((double)(res as Irrational).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }

            return new Error(this, "Could not take Cos of: " + args[0]);
        }

        public override Expression Clone()
        {
            return MakeClone<Cos>();
    }

        public Expression Inverted(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new ACos(newArgs);
        }
    }

    public class ACos : Function, IInvertable
    {
        public ACos() : this(null) { }
        public ACos(List<Expression> args)
            : base("acos", args, null)
        {
            validArgs = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)(Math.Acos((res as Integer).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)(Math.Acos((double)(res as Rational).value.value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)(Math.Acos((double)(res as Irrational).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }

            return new Error(this, "Could not take ACos of: " + args[0]);
        }

        public override Expression Clone()
        {
            return MakeClone<ACos>();
        }

        public Expression Inverted(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new Cos(newArgs);
        }
    }

    public class Tan : Function, IInvertable
    {
        public Tan() : this(null) { }
        public Tan(List<Expression> args)
            : base("tan", args, null)
        {
            validArgs = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)Math.Tan((res as Integer).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)Math.Tan((double)(res as Rational).value.value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)Math.Tan((double)(res as Irrational).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }

            return new Error(this, "Could not take Tan of: " + args[0]);
        }

        public override Expression Clone()
        {
            return MakeClone<Tan>();
        }

        public Expression Inverted(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new ATan(newArgs);
        }
    }

    public class ATan : Function, IInvertable
    {
        public ATan() : this(null) { }
        public ATan(List<Expression> args)
            : base("atan", args, null)
        {
            validArgs = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)(Math.Atan((res as Integer).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)(Math.Atan((double)(res as Rational).value.value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)(Math.Atan((double)(res as Irrational).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)))).Evaluate();
            }

            return new Error(this, "Could not take ATan of: " + args[0]);
        }

        public override Expression Clone()
        {
            return MakeClone<ATan>();
        }

        public Expression Inverted(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new Tan(newArgs);
        }
    }

    public class Sqrt : Function, IInvertable
    {
        public Sqrt() : this(null) { }
        public Sqrt(List<Expression> args)
            : base("sqrt", args, null)
        {
            validArgs = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)Math.Sqrt((res as Integer).value))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)Math.Sqrt((double)(res as Rational).value.value))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)Math.Sqrt((double)(res as Irrational).value))).Evaluate();
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

        public override Expression Clone()
        {
            return MakeClone<Sqrt>();
        }

        public Expression Inverted(Expression other)
        {
            return new Exp(other, new Integer(2));
        }
    }

    public class Negation : Function
    {
        public Negation() : base(null, null, null) { }
        public Negation(List<Expression> args)
            : base("negation", args, null)
        {
            validArgs = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            throw new NotImplementedException();
        }

        public override Expression Clone()
        {
            return MakeClone<Negation>();
        }
    }

    public class Simplify : Function
    {
        public Simplify(List<Expression> args)
            : base("simplify", args, null)
        {
            validArgs = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            return Evaluator.SimplifyExp(args[0]);
        }

        public override Expression Clone()
        {
            throw new NotImplementedException();
        }
    }

    public class Expand : Function
    {
        public Expand(List<Expression> args)
            : base("expand", args, null) 
        {
            validArgs = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            return Evaluator.ExpandExp(args[0]);
        }

        public override Expression Clone()
        {
            throw new NotImplementedException();
        }
    }

    public class Range : Function
    {
        public Range(List<Expression> args)
            : base("range", args, null) 
        {
            validArgs = new List<ArgKind>()
            {
                ArgKind.Number,
                ArgKind.Number,
                ArgKind.Number
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            Decimal start;
            Decimal end;
            Decimal step;

            if (args[0] is Integer)
                start = (args[0] as Integer).value;
            else if (args[0] is Irrational)
                start = (args[0] as Irrational).value;
            else
                return new Error(this, "argument 1 cannot be: " + args[0].GetType().Name);

            if (args[1] is Integer)
                end = (args[1] as Integer).value;
            else if (args[1] is Irrational)
                end = (args[1] as Irrational).value;
            else
                return new Error(this, "argument 2 cannot be: " + args[1].GetType().Name);

            if (args[2] is Integer)
                step = (args[2] as Integer).value;
            else if (args[2] is Irrational)
                step = (args[2] as Irrational).value;
            else
                return new Error(this, "argument 3 cannot be: " + args[2].GetType().Name);

            var list = new Ast.List ();
            for (Decimal i = start; i < end; i += step)
            {
                list.elements.Add(new Irrational(i));
            }

            return list;
        }

        public override Expression Clone()
        {
            throw new NotImplementedException();
        }
    }

    public class Plot : Function
    {
        public Expression exp;
        public Symbol sym;

        public Plot() : this(null) { }
        public Plot(List<Expression> args)
            : base("plot", args, null)
        {
            validArgs = new List<ArgKind>()
            {
                ArgKind.Expression,
                ArgKind.Symbol
            };
        }

        public override Expression Evaluate()
        {
            return new Error(this, "Cannot evaluate plot");
        }

        public override Expression Clone()
        {
            return MakeClone<Plot>();
        }
    }

    public class Solve : Function
    {
        Equal equal;
        Symbol sym;

        public Solve() : this(null) { }
        public Solve(List<Expression> args)
            : base("solve", args, null)
        {
            validArgs = new List<ArgKind>()
            {
                ArgKind.Equation,
                ArgKind.Symbol
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            equal = (Equal)args[0];
            sym = (Symbol)args[1];

            Expression resLeft = new Sub(equal.left, equal.right).Expand();
            Expression resRight = new Integer(0);

            Console.WriteLine(equal.ToString());
            Console.WriteLine(resLeft.ToString() + "=" + resRight.ToString());

            while (!((resLeft is Symbol) && resLeft.CompareTo(sym)))
            {
                if (resLeft is IInvertable)
                {
                    if (resLeft is Operator)
                    {
                        if (InvertOperator(ref resLeft, ref resRight))
                        {
                            return new Error(this, " could not solve " + sym.ToString());
                        }
                    }
                    else if (resLeft is Function)
                    {
                        if (InvertFunction(ref resLeft, ref resRight))
                        {
                            return new Error(this, " could not solve " + sym.ToString());
                        }
                    }
                    else
                    {
                        return new Error(this, " could not solve " + sym.ToString());
                    }
                }
                else
                {
                    return new Error(this, " could not solve " + sym.ToString());
                }

                Console.WriteLine(resLeft.ToString() + "=" + resRight.ToString());
            }

            return new Equal(resLeft, Evaluator.SimplifyExp(resRight));
        }

        private bool InvertOperator(ref Expression resLeft, ref Expression resRight)
        {
            Operator op = resLeft as Operator;

            if (op.right.ContainsVariable(sym) && op.left.ContainsVariable(sym))
            {
                throw new NotImplementedException();
            }
            else if (op.left.ContainsVariable(sym))
            {
                resRight = (op as IInvertable).Inverted(resRight);
                resLeft = op.left;
                return false;
            }
            else if (op.right.ContainsVariable(sym))
            {
                if (op is ISwappable)
                {
                    resLeft = (op as ISwappable).Swap();
                    return false;
                }
                else if (op is Div)
                {
                    if (!resRight.CompareTo(new Integer(0)))
                    {
                        resRight = new Div(op.left, resRight);
                        resLeft = op.right;
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (op is Exp)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        private bool InvertFunction(ref Expression resLeft, ref Expression resRight)
        {
            Function func = resLeft as Function;

            if (func.ContainsVariable(sym))
            {
                resRight = (func as IInvertable).Inverted(resRight);
                resLeft = func.args[0];
                return false;
            }
            else
            {
                return true;
            }
        }

        public override Expression Clone()
        {
            return MakeClone<Solve>();
        }
    }
}

