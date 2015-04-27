using System;
using System.Collections.Generic;

namespace Ast
{
    public class Solve : SysFunc
    {
        Equal equal;
        Symbol sym;

        public Solve(List<Expression> args, Scope scope)
            : base("solve", args, scope)
        {
            validArgs = new List<ArgKind>()
                {
                    ArgKind.Equation,
                    ArgKind.Symbol
                };
        }

        protected override Expression Evaluate(Expression caller)
        {
            Equal solved;

            if (!isArgsValid())
                return new ArgError(this);

            equal = (Equal)args[0];
            sym = (Symbol)args[1];

            if (equal.Right.ContainsVariable(sym))
            {
                solved = new Equal(new Sub(equal.Left, equal.Right).Simplify().Expand(), new Integer(0));
            }
            else
            {
                solved = equal;
            }

            System.Diagnostics.Debug.WriteLine(equal.ToString());
            System.Diagnostics.Debug.WriteLine(solved);

            while (!((solved.Left is Symbol) && solved.Left.CompareTo(sym)))
            {
                if (solved.Left is IInvertable)
                {
                    if (solved.Left is Operator)
                    {
                        solved = InvertOperator(solved.Left, solved.Right);

                        if (solved == null)
                            return new Error(this, " could not solve " + sym.ToString() + ": " + solved.ToString());
                    }
                    else if (solved.Left is Func)
                    {
                        solved = InvertFunction(solved.Left, solved.Right);

                        if (solved == null)
                            return new Error(this, " could not solve " + sym.ToString() + ": " + solved.ToString());
                    }
                    else
                    {
                        return new Error(this, " could not solve " + sym.ToString() + ": " + solved.ToString());
                    }
                }
                else if (solved.Left is Symbol)
                {
                    var newLeft = (solved.Left as Symbol).SeberateNumbers();

                    solved = new Equal(newLeft, solved.Right);
                }
                else
                {
                    return new Error(this, " could not solve " + sym.ToString() + ": " + solved.ToString());
                }

                System.Diagnostics.Debug.WriteLine(solved);
            }

            return solved.Simplify();
        }

        private Equal InvertOperator(Expression left, Expression right)
        {
            Operator op = left as Operator;

            if (op.Right.ContainsVariable(sym) && op.Left.ContainsVariable(sym))
            {
                return BothSideSymbolSolver(left, right);
            }
            else if (op.Left.ContainsVariable(sym))
            {
                return new Equal(op.Left, (op as IInvertable).Inverted(right));
            }
            else if (op.Right.ContainsVariable(sym))
            {
                if (op is ISwappable)
                {
                    return new Equal((op as ISwappable).Swap(), right);
                }
                else if (op is Div)
                {
                    if (!right.CompareTo(Constant.Zero))
                    {
                        return new Equal(op.Right, new Div(op.Left, right));
                    }
                }
            }

            return null;
        }

        private Equal BothSideSymbolSolver(Expression left, Expression right)
        {
            var leftSimplified = left.Simplify();

            if (leftSimplified is Operator && ((leftSimplified as Operator).Left.ContainsVariable(sym) && (leftSimplified as Operator).Right.ContainsVariable(sym)))
            {
                throw new NotImplementedException();
                return null;
            }
            else
            {
                return new Equal(leftSimplified, right);
            }
        }

        private Equal InvertFunction(Expression left, Expression right)
        {
            Func func = left as Func;

            if (func.ContainsVariable(sym))
            {
                return new Equal(func.args[0], (func as IInvertable).Inverted(right));
            }

            return null;
        }
    }
}

