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
            if (!isArgsValid())
                return new ArgError(this);

            equal = (Equal)args[0];
            sym = (Symbol)args[1];

            Expression resLeft = new Sub(equal.Left, equal.Right).Simplify().Expand();
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
                        return new Error(this, " could not solve " + sym.ToString() + ". Left was not a valid type");
                    }
                }
                else
                {
                    return new Error(this, " could not solve " + sym.ToString() + ". Left was not invertable");
                }

                System.Diagnostics.Debug.WriteLine(resLeft.ToString() + "=" + resRight.ToString());
            }

            return new Equal(resLeft, resRight.Simplify());
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

