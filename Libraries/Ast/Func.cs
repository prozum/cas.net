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
        Equation,
        List
    }

    public abstract class Func : Variable
    {
        public List<Expression> args;

        public Func(string identifier, List<Expression> args, Scope scope)
            : base(identifier, scope) 
        {
            this.args = args;
        }
            

        public override bool CompareTo(Expression other)
        {
            if (other is SysFunc)
            {
                return identifier == (other as Func).identifier && prefix.CompareTo((other as Func).prefix) && exponent.CompareTo((other as Func).exponent) && CompareArgsTo(other as Func);
            }

            if (this is UsrFunc)
            {
                return (this as UsrFunc).GetValue().CompareTo(other);
            }

            return false;
        }

        public bool CompareArgsTo(Func other)
        {
            bool res = true;

            if (args.Count == (other as Func).args.Count)
            {
                for (int i = 0; i < args.Count; i++)
                {
                    if (!args[i].CompareTo((other as Func).args[i]))
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
            if (prefix.CompareTo(Constant.Zero))
            {
                return new Integer(0);
            }
            if (exponent.CompareTo(Constant.Zero))
            {
                return new Integer(1);
            }

            return base.Simplify();
        }

        protected override T MakeClone<T>()
        {
            T res = base.MakeClone<T>();
            (res as Func).args = new List<Expression>(args);

            return res;
        }
    }
        
    public abstract class SysFunc : Func
    {
        public List<ArgKind> argKinds;

        public SysFunc(string identifier, List<Expression> args, Scope scope)
            : base(identifier, args, scope) { }

        public override string ToString ()
        {
            string str = "";

            if (((prefix is Integer) && (prefix as Integer).value != 1) || ((prefix is Rational) && (prefix as Rational).value.value != 1) || ((prefix is Irrational) && (prefix as Irrational).value != 1))
            {
                str += prefix.ToString() + identifier + '[';
            }
            else
            {
                str += identifier + '[';
            }

            for (int i = 0; i < argKinds.Count; i++) 
            {
                str += argKinds[i].ToString ();

                if (i < argKinds.Count - 1) 
                {
                    str += ',';
                }
            }

            str += ']';

            if (((exponent is Integer) && (exponent as Integer).value != 1) || ((exponent is Rational) && (exponent as Rational).value.value != 1) || ((exponent is Irrational) && (exponent as Irrational).value != 1))
            {
                str += '^' + exponent.ToString();
            }

            return str;
        }

        public bool isArgsValid()
        {
            if (args.Count != argKinds.Count)
                return false;

            for (int i = 0; i < args.Count; i++)
            {
                switch (argKinds[i])
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
                        if (!(args[i] is Func))
                            return false;
                        break;
                    case ArgKind.Equation:
                        if (!(args[i] is Equal))
                            return false;
                        break;
                    case ArgKind.List:
                        if (!(args[i] is List))
                            return false;
                        break;
                }
            }

            return true;
        }
    }

    public class InstanceFunc : Func
    {
        public Expression expr;

        public InstanceFunc() : this(null, null, null) { }
        public InstanceFunc(string identifier, List<Expression> args, Scope scope) : base(identifier, args, scope) { }

        public override Expression Evaluate()
        {
            return Evaluator.SimplifyExp(GetValue()).Evaluate();
        }

        public Expression GetValue()
        {
            Expression @var;

            @var = scope.GetVar(identifier);

            if (@var == null)
                return new Error(identifier + "> has no definition");

            if (@var is SysFunc)
            {
                var sysFuncDef = (SysFunc)@var;

                sysFuncDef.args = args;
                if (!sysFuncDef.isArgsValid())
                    return new ArgError(sysFuncDef);
                    
                return sysFuncDef.Evaluate();

            }
            else if (@var is UsrFunc)
            {
                var usrFuncDef = (UsrFunc)@var;

                if (args.Count != usrFuncDef.args.Count)
                    return new Error(identifier + " Function takes " + usrFuncDef.args.Count.ToString() + " arguments. Not " + args.Count.ToString() + ".");

                for (int i = 0; i < args.Count; i++)
                {
                    var arg = (Symbol)usrFuncDef.args[i];

                    scope.SetVar(arg.identifier, args[i]);
                }

                return expr.Evaluate();
            }
            else
            {
                return new Error(this, "Variable is not a function");
            }

            throw new Exception("This should not happen");


//            if (def.ContainsVariable(this))
//            {
//                return new Error(this, "Could not get value of: " + this.identifier);
//            }

            //return ReturnValue(res.Clone());
            //return ReturnValue(func.expr.);
        }

        public override Expression Clone()
        {
            return MakeClone<UsrFunc>();
        }

        public override Expression Simplify()
        {
            if (prefix.CompareTo(Constant.Zero))
            {
                return new Integer(0);
            }
            if (exponent.CompareTo(Constant.Zero))
            {
                return new Integer(1);
            }

            return GetValue();
        }

        public override string ToString()
        {
            string str = identifier + "[";

            for (int i = 0; i < args.Count; i++)
            {
                str += args[i];

                if (i < args.Count - 1)
                    str += ",";
            }
            str += "]";

            return str;
        }
    }

    public class UsrFunc : Func
    {
        public Expression expr;

        public UsrFunc() : this(null, null, null) { }
        public UsrFunc(string identifier, List<Expression> args, Scope scope) : base(identifier, args, scope) { }

//        public override Expression Evaluate()
//        {
//            return Evaluator.SimplifyExp(GetValue()).Evaluate();
//        }
            
        public Expression GetValue()
        {
//            Expression @var;

//            // TODO 
//            //if (scope != null)
//            @var = scope.GetVar(identifier);
//            //else
//            //    @var = evaluator.scope.GetVar(identifier);
//
//            if (@var == null)
//            {
//                return new Error(this, "Function has no definition");
//            }
//                
//            var def = (InstFunc)@var;
//
//            if (def.args.Count != args.Count)
//            {
//                return new Error(this, "Function takes " + def.args.Count.ToString() + " arguments. Not " + args.Count.ToString() + ".");
//            }
//
//            for (int i = 0; i < def.args.Count; i++)
//            {
//                var arg = (Symbol)def.args[i];
//
//                scope.SetVar(arg.identifier, args[i]);
//            }
//
//            if (def.ContainsVariable(this))
//            {
//                return new Error(this, "Could not get value of: " + this.identifier);
//            }

            //return ReturnValue(res.Clone());
//            return ReturnValue(def.expr);
            return null;
        }

        public override Expression Clone()
        {
            return MakeClone<UsrFunc>();
        }

        public override Expression Simplify()
        {
            if (prefix.CompareTo(Constant.Zero))
            {
                return new Integer(0);
            }
            if (exponent.CompareTo(Constant.Zero))
            {
                return new Integer(1);
            }

            return GetValue();
        }

        public override string ToString()
        {
            string str = identifier + "[";

            for (int i = 0; i < args.Count; i++)
            {
                str += args[i];

                if (i < args.Count - 1)
                    str += ",";
            }
            str += "]";

            return str;
        }
    }

    public class Sin : SysFunc, IInvertable
    {
        public Sin() : this(null, null) { }
        public Sin(List<Expression> args, Scope scope)
            : base("sin", args, scope)
        {
            argKinds = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            var degrees = (Boolean)scope.GetVar("deg");
            if (degrees == null)
                degrees = new Boolean(false);
                
            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)Math.Sin((res as Integer).value * Math.Pow((Math.PI / 180), (degrees.value) ? 1 : 0)))).Evaluate();
            }
            
            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)Math.Sin((double)(res as Rational).value.value * Math.Pow((Math.PI / 180), (degrees.value) ? 1 : 0)))).Evaluate();
            }
            
            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)Math.Sin((double)(res as Irrational).value * Math.Pow((Math.PI / 180), (degrees.value) ? 1 : 0)))).Evaluate();
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
            return new ASin(newArgs, scope);
        }
    }

    public class ASin : SysFunc, IInvertable
    {
        public ASin() : this(null, null) { }
        public ASin(List<Expression> args, Scope scope)
            : base("asin", args, scope)
        {
            argKinds = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            var degrees = (Boolean)scope.GetVar("deg");
            if (degrees == null)
                degrees = new Boolean(false);

            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)(Math.Asin((res as Integer).value) * Math.Pow((180 / Math.PI), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)(Math.Asin((double)(res as Rational).value.value) * Math.Pow((180 / Math.PI), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)(Math.Asin((double)(res as Irrational).value) * Math.Pow((180 / Math.PI), (degrees.value) ? 1 : 0)))).Evaluate();
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
            return new Sin(newArgs, scope);
        }
    }

    public class Cos : SysFunc, IInvertable
    {
        public Cos() : this(null, null) { }
        public Cos(List<Expression> args, Scope scope)
            : base("cos", args, scope)
        {
            argKinds = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            var degrees = (Boolean)scope.GetVar("deg");
            if (degrees == null)
                degrees = new Boolean(false);

            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)Math.Cos((res as Integer).value * Math.Pow((Math.PI / 180), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)Math.Cos((double)(res as Rational).value.value * Math.Pow((Math.PI / 180), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)Math.Cos((double)(res as Irrational).value * Math.Pow((Math.PI / 180), (degrees.value) ? 1 : 0)))).Evaluate();
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
            return new ACos(newArgs, scope);
        }
    }

    public class ACos : SysFunc, IInvertable
    {
        public ACos() : this(null, null) { }
        public ACos(List<Expression> args, Scope scope)
            : base("acos", args, scope)
        {
            argKinds = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            var degrees = (Boolean)scope.GetVar("deg");
            if (degrees == null)
                degrees = new Boolean(false);

            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)(Math.Acos((res as Integer).value) * Math.Pow((180 / Math.PI), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)(Math.Acos((double)(res as Rational).value.value) * Math.Pow((180 / Math.PI), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)(Math.Acos((double)(res as Irrational).value) * Math.Pow((180 / Math.PI), (degrees.value) ? 1 : 0)))).Evaluate();
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
            return new Cos(newArgs, scope);
        }
    }

    public class Tan : SysFunc, IInvertable
    {
        public Tan() : this(null, null) { }
        public Tan(List<Expression> args, Scope scope)
            : base("tan", args, scope)
        {
            argKinds = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            var degrees = (Boolean)scope.GetVar("deg");
            if (degrees == null)
                degrees = new Boolean(false);

            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)Math.Tan((res as Integer).value * Math.Pow((Math.PI / 180), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)Math.Tan((double)(res as Rational).value.value * Math.Pow((Math.PI / 180), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)Math.Tan((double)(res as Irrational).value * Math.Pow((Math.PI / 180), (degrees.value) ? 1 : 0)))).Evaluate();
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
            return new ATan(newArgs, scope);
        }
    }

    public class ATan : SysFunc, IInvertable
    {
        public ATan() : this(null, null) { }
        public ATan(List<Expression> args, Scope scope)
            : base("atan", args, scope)
        {
            argKinds = new List<ArgKind>()
            {
                ArgKind.Expression
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            var degrees = (Boolean)scope.GetVar("deg");
            if (degrees == null)
                degrees = new Boolean(false);

            if (res is Integer)
            {
                return ReturnValue(new Irrational((decimal)(Math.Atan((res as Integer).value) * Math.Pow((180 / Math.PI), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            if (res is Rational)
            {
                return ReturnValue(new Irrational((decimal)(Math.Atan((double)(res as Rational).value.value) * Math.Pow((180 / Math.PI), (degrees.value) ? 1 : 0)))).Evaluate();
            }

            if (res is Irrational)
            {
                return ReturnValue(new Irrational((decimal)(Math.Atan((double)(res as Irrational).value) * Math.Pow((180 / Math.PI), (degrees.value) ? 1 : 0)))).Evaluate();
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
            return new Tan(newArgs, scope);
        }
    }

    public class Sqrt : SysFunc, IInvertable
    {
        public Sqrt() : this(null, null) { }
        public Sqrt(List<Expression> args, Scope scope)
            : base("sqrt", args, scope)
        {
            argKinds = new List<ArgKind>()
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
            if (exponent.CompareTo(Constant.Two))
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

    public class Negation : SysFunc
    {
        public Negation() : this(null, null) { }
        public Negation(List<Expression> args, Scope scope)
            : base("negation", args, scope)
        {
            argKinds = new List<ArgKind>()
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

    public class Simplify : SysFunc
    {
        public Simplify(List<Expression> args, Scope scope)
            : base("simplify", args, scope)
        {
            argKinds = new List<ArgKind>()
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

    public class Expand : SysFunc
    {
        public Expand(List<Expression> args, Scope scope)
            : base("expand", args, scope)
        {
            argKinds = new List<ArgKind>()
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

    public class Range : SysFunc
    {
        public Range(List<Expression> args, Scope scope)
            : base("range", args, scope)
        {
            argKinds = new List<ArgKind>()
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

    public class Map : SysFunc
    {
        public Func func;
        public List list;

        public Map(List<Expression> args, Scope scope)
            : base("map", args, scope)
        {
            argKinds = new List<ArgKind>()
            {
                ArgKind.Symbol,
                ArgKind.List
            };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);


            var sym = (Symbol)args[0];
            func = (Func)sym.GetValue();
            list = (List)args[1];

            //Expression exp;
            //List<string> argNames;
            //evaluator.funcDefs.TryGetValue(sym.identifier, out exp);
            //evaluator.funcParams.TryGetValue(sym.identifier, out argNames);
            //parser.
            //list = (List)args[1];

            //if (func.argKinds.Count > 1)
            //    return new Error(this, "only supports unary functions");
            //string arg = argNames[0];

            //var locals = new Dictionary<string, Expression>(func.locals);

            var res = new List();
            foreach (var element in list.elements)
            {
                //locals.Remove(func.argNames[0]);
                //locals.Add(func.argNames[0], element);
                func.args[0] = element;

                res.elements.Add(func.Evaluate());
            }

            return res;
        }

        public override Expression Clone()
        {
            throw new NotImplementedException();
        }
    }

    public class Plot : SysFunc
    {
        public Expression exp;
        public Symbol sym;

        public Plot(List<Expression> args, Scope scope)
            : base("plot", args, scope)
        {
            argKinds = new List<ArgKind>()
            {
                ArgKind.Expression,
                ArgKind.Symbol
            };

            if (isArgsValid())
            {
                exp = args[0];
                sym = (Symbol)args[1];
            }
        }

        public override Expression Evaluate()
        {
            return new Error(this, "Cannot evaluate plot");
        }
    }

    public class Solve : SysFunc
    {
        Equal equal;
        Symbol sym;

        public Solve(List<Expression> args, Scope scope)
            : base("solve", args, scope)
        {
            argKinds = new List<ArgKind>()
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

            Expression resLeft = Evaluator.SimplifyExp(new Sub(equal.Left, equal.Right)).Expand();
            Expression resRight = new Integer(0);

            System.Diagnostics.Debug.WriteLine(equal.ToString());
            System.Diagnostics.Debug.WriteLine(resLeft.ToString() + "=" + resRight.ToString());

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
                    else if (resLeft is Func)
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

                System.Diagnostics.Debug.WriteLine(resLeft.ToString() + "=" + resRight.ToString());
            }

            return new Equal(resLeft, Evaluator.SimplifyExp(resRight));
        }

        private bool InvertOperator(ref Expression resLeft, ref Expression resRight)
        {
            Operator op = resLeft as Operator;

            if (op.Right.ContainsVariable(sym) && op.Left.ContainsVariable(sym))
            {
                throw new NotImplementedException();
            }
            else if (op.Left.ContainsVariable(sym))
            {
                resRight = (op as IInvertable).Inverted(resRight);
                resLeft = op.Left;
                return false;
            }
            else if (op.Right.ContainsVariable(sym))
            {
                if (op is ISwappable)
                {
                    resLeft = (op as ISwappable).Swap();
                    return false;
                }
                else if (op is Div)
                {
                    if (!resRight.CompareTo(Constant.Zero))
                    {
                        resRight = new Div(op.Left, resRight);
                        resLeft = op.Right;
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
            Func func = resLeft as Func;

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
    }
}

