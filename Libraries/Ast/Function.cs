using System;
using System.Collections.Generic;

namespace Ast
{
    public abstract class Function  : Expression
    {
        public string identifier;
        public List<Expression> args;

        public Function(string identifier)
        {
            this.identifier = identifier;
        }

        public override string ToString ()
        {
            string str = identifier + '(';

            for (int i = 0; i < args.Count; i++) 
            {
                str += args[i].ToString ();

                if (i < args.Count - 1) 
                {
                    str += ',';
                }
            }

            return str + ')';
        }

        public override bool CompareTo(Expression other)
        {
            var res = base.CompareTo(other);

            if (res)
            {
                if (identifier == (other as Function).identifier)
                {
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
                }
                else
                {
                    res = false;
                }
            }

            return res;
        }
    }

    public class UserDefinedFunction : Function
    {
        public Dictionary<string, Expression> tempDefinitions;

        public UserDefinedFunction(string identifier, List<Expression> args) : base(identifier)
        {
            this.args = args;
        }

        public override Expression Evaluate()
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

                    return res.Evaluate();
                }
                else if (functionParemNames.Count == 0)
                {
                    return new Error("Can't call function with 0 parameters");
                }
                else
                {
                    return new Error("Function has the wrong number for parameters");
                }
            }
            else
            {
                return new Error("Function has no definition");
            }
        }
    }

    public abstract class UnaryOperation : Function
    {
        public UnaryOperation(string identifier, Expression arg) : base(identifier)
        {
            this.args = new List<Expression>();
            args.Add(arg);
        }
    }

    public class Sin : UnaryOperation
    {
        public Sin(string identifier, Expression arg) : base(identifier, arg) { }

        public override Expression Evaluate()
        {
            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return new Irrational((decimal)Math.Sin((res as Integer).value * Math.Pow((Math.PI / 180), (evaluator.degrees)? 1 : 0)));
            }

            if (res is Rational)
            {
                return new Irrational((decimal)Math.Sin((double)(res as Rational).value.value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)));
            }

            if (res is Irrational)
            {
                return new Irrational((decimal)Math.Sin((double)(res as Irrational).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)));
            }

            return new Error("Could not take Sin of: " + args[0]);
        }
    }

    public class ASin : UnaryOperation
    {
        public ASin(string identifier, Expression arg) : base(identifier, arg) { }

        public override Expression Evaluate()
        {
            var res = args[0].Evaluate();
            if (res is Integer)
            {
                return new Irrational((decimal)(Math.Asin((res as Integer).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)));
            }

            if (res is Rational)
            {
                return new Irrational((decimal)(Math.Asin((double)(res as Rational).value.value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)));
            }

            if (res is Irrational)
            {
                return new Irrational((decimal)(Math.Asin((double)(res as Irrational).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)));
            }

            return new Error("Could not take ASin of: " + args[0]);
        }
    }

    public class Cos : UnaryOperation
    {
        public Cos(string identifier, Expression arg) : base(identifier, arg) { }

        public override Expression Evaluate()
        {
            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return new Irrational((decimal)Math.Cos((res as Integer).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)));
            }

            if (res is Rational)
            {
                return new Irrational((decimal)Math.Cos((double)(res as Rational).value.value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)));
            }

            if (res is Irrational)
            {
                return new Irrational((decimal)Math.Cos((double)(res as Irrational).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)));
            }

            return new Error("Could not take Cos of: " + args[0]);
        }
    }

    public class ACos : UnaryOperation
    {
        public ACos(string identifier, Expression arg) : base(identifier, arg) { }

        public override Expression Evaluate()
        {
            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return new Irrational((decimal)(Math.Acos((res as Integer).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)));
            }

            if (res is Rational)
            {
                return new Irrational((decimal)(Math.Acos((double)(res as Rational).value.value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)));
            }

            if (res is Irrational)
            {
                return new Irrational((decimal)(Math.Acos((double)(res as Irrational).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)));
            }

            return new Error("Could not take ACos of: " + args[0]);
        }
    }

    public class Tan : UnaryOperation
    {
        public Tan(string identifier, Expression arg) : base(identifier, arg) { }

        public override Expression Evaluate()
        {
            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return new Irrational((decimal)Math.Tan((res as Integer).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)));
            }

            if (res is Rational)
            {
                return new Irrational((decimal)Math.Tan((double)(res as Rational).value.value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)));
            }

            if (res is Irrational)
            {
                return new Irrational((decimal)Math.Tan((double)(res as Irrational).value * Math.Pow((Math.PI / 180), (evaluator.degrees) ? 1 : 0)));
            }

            return new Error("Could not take Tan of: " + args[0]);
        }
    }

    public class ATan : UnaryOperation
    {
        public ATan(string identifier, Expression arg) : base(identifier, arg) { }

        public override Expression Evaluate()
        {
            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return new Irrational((decimal)(Math.Atan((res as Integer).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)));
            }

            if (res is Rational)
            {
                return new Irrational((decimal)(Math.Atan((double)(res as Rational).value.value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)));
            }

            if (res is Irrational)
            {
                return new Irrational((decimal)(Math.Atan((double)(res as Irrational).value) * Math.Pow((180 / Math.PI), (evaluator.degrees) ? 1 : 0)));
            }

            return new Error("Could not take ATan of: " + args[0]);
        }
    }

    public class Sqrt : UnaryOperation
    {
        public Sqrt(string identifier, Expression arg) : base(identifier, arg) { }

        public override Expression Evaluate()
        {
            var res = args[0].Evaluate();

            if (res is Integer)
            {
                return new Irrational((decimal)Math.Sqrt((res as Integer).value));
            }

            if (res is Rational)
            {
                return new Irrational((decimal)Math.Sqrt((double)(res as Rational).value.value));
            }

            if (res is Irrational)
            {
                return new Irrational((decimal)Math.Sqrt((double)(res as Irrational).value));
            }

            return new Error("Could not take Sqrt of: " + args[0]);
        }
    }

    public class Negation : UnaryOperation
    {
        public Negation(string identifier, Expression arg) : base(identifier, arg) { }

        public override Expression Evaluate()
        {
            throw new NotImplementedException();
        }
    }
}

