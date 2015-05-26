using System.Collections.Generic;

namespace Ast
{
    public class SolveFunc : SysFunc
    {
        Equal equal;
        Variable @var;

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
            @var = (Variable)args[1];

            if (equal.Right.ContainsVariable(@var))
            {
                solved = new Equal(new Sub(equal.Left, equal.Right).Reduce().Expand(), new Integer(0));
            }
            else
            {
                solved = equal;
            }

            while (!((solved.Left is Variable) && solved.Left.CompareTo(@var)))
            {
                if (solved.Left is BinaryOperator && solved.Left is IInvertable)
                {
                    solved = InvertOperator(solved.Left, solved.Right);

                    if (solved == null)
                        return new Error(this, " could not solve " + @var.ToString());
                }
                else if (solved.Left is Call && (solved.Left as Call).Child.Value is IInvertable)
                {
                    solved = InvertFunction((solved.Left as Call), solved.Right);

                    if (solved == null)
                        return new Error(this, " could not solve " + @var.ToString());
                }
                else if (solved.Left is Variable && (solved.Left as Variable).Identifier == @var.Identifier)
                {
                    var newLeft = (solved.Left as Variable).SeberateNumbers();

                    solved = new Equal(newLeft, solved.Right);
                }
                else
                {
                    return new Error(this, " could not solve " + @var.ToString());
                }
            }

            return solved.Reduce();
        }

        private Equal InvertOperator(Expression left, Expression right)
        {
            BinaryOperator op = left as BinaryOperator;

            if (op.Right.ContainsVariable(@var) && op.Left.ContainsVariable(@var))
            {
                return BothSideSymbolSolver(left, right);
            }
            else if (op.Left.ContainsVariable(@var))
            {
                var inverted = (op as IInvertable).InvertOn(right);

                if (inverted == null)
                    return null;

                return new Equal(op.Left, inverted);
            }
            else if (op.Right.ContainsVariable(@var))
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

            if (leftSimplified is BinaryOperator && ((leftSimplified as BinaryOperator).Left.ContainsVariable(@var) && (leftSimplified as BinaryOperator).Right.ContainsVariable(@var)))
            {
                return null;
            }
            else
            {
                return new Equal(leftSimplified, right);
            }
        }

        private Equal InvertFunction(Call call, Expression right)
        {
            SysFunc func = call.Child.Value as SysFunc;

            if (call.ContainsVariable(@var))
            {
                return new Equal(call.Arguments[0], (func as IInvertable).InvertOn(right));
            }

            return null;
        }
    }
}

