using System;
using System.Collections.Generic;

namespace Ast
{
    public class SolveFunc : SysFunc
    {
        Equal equal;
        Variable sym;

        public SolveFunc() : this(null) { }
        public SolveFunc(Scope scope)
            : base("solve", scope)
        {
            ValidArguments = new List<ArgumentType>()
                {
                    ArgumentType.Equation,
                    ArgumentType.Variable
                };
        }

        public override Expression Call(List args)
        {
            Equal solved;

            equal = (Equal)args[0];
            sym = (Variable)args[1];

            if (equal.Right.ContainsVariable(sym))
            {
                solved = new Equal(new Sub(equal.Left, equal.Right).Reduce().Expand(), new Integer(0));
            }
            else
            {
                solved = equal;
            }

            while (!((solved.Left is Variable) && solved.Left.CompareTo(sym)))
            {
                if (solved.Left is IInvertable)
                {
                    if (solved.Left is BinaryOperator)
                    {
                        solved = InvertOperator(solved.Left, solved.Right);

                        if (solved == null)
                            return new Error(this, " could not solve " + sym.ToString());
                    }
//                    else if (solved.Left is Func)
//                    {
//                        solved = InvertFunction(solved.Left, solved.Right);
//
//                        if (solved == null)
//                            return new Error(this, " could not solve " + sym.ToString());
//                    }
                    else
                    {
                        return new Error(this, " could not solve " + sym.ToString());
                    }
                }
                else if (solved.Left is Variable && (solved.Left as Variable).Identifier == sym.Identifier)
                {
                    var newLeft = (solved.Left as Variable).SeberateNumbers();

                    solved = new Equal(newLeft, solved.Right);
                }
                else
                {
                    return new Error(this, " could not solve " + sym.ToString());
                }
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
                var inverted = (op as IInvertable).InvertOn(right);

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
                return null;
            }
            else
            {
                return new Equal(leftSimplified, right);
            }
        }

        private Equal InvertFunction(Expression left, Expression right)
        {
//            Func func = left as Func;
//
//            if (func.ContainsVariable(sym))
//            {
//                return new Equal(func.Arguments[0], (func as IInvertable).InvertOn(right));
//            }

            return null;
        }
    }
}

