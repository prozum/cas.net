﻿using System;
using System.Collections.Generic;

namespace Ast
{
    public class SolveFunc : SysFunc
    {
        Equal equal;
        Symbol sym;

        public SolveFunc(List<Expression> args, Scope scope)
            : base("solve", args, scope)
        {
            ValidArguments = new List<ArgKind>()
                {
                    ArgKind.Equation,
                    ArgKind.Symbol
                };
        }

        protected override Expression Evaluate(Expression caller)
        {
            Equal solved;

            if (!isArgsValid())
                return new ArgumentError(this);

            equal = (Equal)args[0];
            sym = (Symbol)args[1];

            if (equal.Right.ContainsVariable(sym))
            {
                solved = new Equal(new Sub(equal.Left, equal.Right).Reduce().Expand(), new Integer(0));
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
                    if (solved.Left is BinaryOperator)
                    {
                        solved = InvertOperator(solved.Left, solved.Right);

                        if (solved == null)
                            return new ErrorExpr(this, " could not solve " + sym.ToString() + ": " + solved.ToString());
                    }
                    else if (solved.Left is Func)
                    {
                        solved = InvertFunction(solved.Left, solved.Right);

                        if (solved == null)
                            return new ErrorExpr(this, " could not solve " + sym.ToString() + ": " + solved.ToString());
                    }
                    else
                    {
                        return new ErrorExpr(this, " could not solve " + sym.ToString() + ": " + solved.ToString());
                    }
                }
                else if (solved.Left is Symbol)
                {
                    var newLeft = (solved.Left as Symbol).SeberateNumbers();

                    solved = new Equal(newLeft, solved.Right);
                }
                else
                {
                    return new ErrorExpr(this, " could not solve " + sym.ToString() + ": " + solved.ToString());
                }

                System.Diagnostics.Debug.WriteLine(solved);
            }

            return solved.Reduce();
        }

        private Equal InvertOperator(Expression left, Expression right)
        {
            BinaryOperator op = left as BinaryOperator;

            if (op.Right.ContainsVariable(sym) && op.Left.ContainsVariable(sym))
            {
                return BothSideSymbolSolver(left, right);
            }
            else if (op.Left.ContainsVariable(sym))
            {
                var inverted = (op as IInvertable).Inverted(right);

                if (inverted == null)
                    return null;

                return new Equal(op.Left, inverted);
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
            var leftSimplified = left.Reduce();

            if (leftSimplified is BinaryOperator && ((leftSimplified as BinaryOperator).Left.ContainsVariable(sym) && (leftSimplified as BinaryOperator).Right.ContainsVariable(sym)))
            {
                if (true)
                {
                    
                }


                throw new NotImplementedException();
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

