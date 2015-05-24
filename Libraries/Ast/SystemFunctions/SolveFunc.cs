using System;
using System.Collections.Generic;

namespace Ast
{
    public class SolveFunc : SysFunc
    {
        Equal equal;
        Variable sym;

        public SolveFunc() : this(null, null) { }
        public SolveFunc(List<Expression> args, Scope scope)
            : base("solve", args, scope)
        {
            ValidArguments = new List<ArgKind>()
                {
                    ArgKind.Equation,
                    ArgKind.Variable
                };
        }

        public override Expression Evaluate()
        {
            Equal solved;

            if (!IsArgumentsValid())
                return new ArgumentError(this);

            equal = (Equal)Arguments[0];
            sym = (Variable)Arguments[1];

            // When right side has the symbol, move it to the left side of the equation
            if (equal.Right.ContainsVariable(sym))
            {
                solved = new Equal(new Sub(equal.Left, equal.Right).Reduce().Expand(), new Integer(0));
            }
            else
            {
                solved = equal;
            }

            // Running as long as the left side is not the wanted symbol
            while (!((solved.Left is Variable) && solved.Left.CompareTo(sym)))
            {
                // When left is invertable, use InvertOn.
                if (solved.Left is IInvertable)
                {
                    if (solved.Left is BinaryOperator)
                    {
                        solved = InvertOperator(solved.Left, solved.Right);

                        if (solved == null)
                            return new Error(this, " could not solve " + sym.ToString());
                    }
                    else if (solved.Left is Func)
                    {
                        solved = InvertFunction(solved.Left, solved.Right);

                        if (solved == null)
                            return new Error(this, " could not solve " + sym.ToString());
                    }

                     return new Error(this, " could not solve " + sym.ToString());
                }
                // This only happens, when left is the same Variable as sym, but they have different exponent and prefix.
                else if (solved.Left is Variable && (solved.Left as Variable).Identifier == sym.Identifier)
                {
                    var newLeft = (solved.Left as Variable).SeberateNumbers();

                    solved = new Equal(newLeft, solved.Right);
                }

                return new Error(this, " could not solve " + sym.ToString());
            }

            return solved.Reduce();
        }

        private Equal InvertOperator(Expression left, Expression right)
        {
            BinaryOperator op = left as BinaryOperator;

            // When both sides of the operator have the symbol:
            // 1. Try to make it so the symbol is on only one of the sides.
            // 2. Use a mathematical rule for this case.
            // 3. Returns null, if none of the above worked.
            if (op.Right.ContainsVariable(sym) && op.Left.ContainsVariable(sym))
            {
                return BothSideSymbolSolver(left, right);
            }
            // When left side of operator is contains symbol, use InvertOn
            else if (op.Left.ContainsVariable(sym))
            {
                var inverted = (op as IInvertable).InvertOn(right);

                if (inverted == null)
                    return null;

                return new Equal(op.Left, inverted);
            }
            else if (op.Right.ContainsVariable(sym))
            {
                // When right side of operator is contains symbol, and the op is swappable. Swap the op.
                if (op is ISwappable)
                {
                    return new Equal((op as ISwappable).Swap(), right);
                }
                // Special solving rule for Div, when right side contains sym. solve[y/x=z, x] -> x = y/z
                else if (op is Div)
                {
                    //right side of equation must not be zero.
                    if (!right.CompareTo(Constant.Zero))
                    {
                        return new Equal(op.Right, new Div(op.Left, right));
                    }
                }
            }

            return null;
        }

        // Is called when left is op, and both of it's sides contains sym.
        private Equal BothSideSymbolSolver(Expression left, Expression right)
        {
            //Try to reduce left, so it no longer has the sym on both sides.
            var leftReduced = left.Reduce();

            //If left is still a op, and has the sym on both sides.
            if (leftReduced is BinaryOperator && ((leftReduced as BinaryOperator).Left.ContainsVariable(sym) && (leftReduced as BinaryOperator).Right.ContainsVariable(sym)))
            {
                return null;
            }

            //Manged to reduce sym away from both sides of the op.
            return new Equal(leftReduced, right);
        }

        private Equal InvertFunction(Expression left, Expression right)
        {
            Func func = left as Func;

            if (func.ContainsVariable(sym))
            {
                //Invert function to the other side.
                return new Equal(func.Arguments[0], (func as IInvertable).InvertOn(right));
            }

            //If the functions paramenter doesn't contain sym, a error occurred.
            return null;
        }

        public override Expression Clone()
        {
            return MakeClone<SolveFunc>();
        }
    }
}

