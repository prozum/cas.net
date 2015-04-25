using System;

namespace Ast
{
    public class Assign : Operator
    {
        public Assign() : base(":=", 0) { }
        public Assign(Expression left, Expression right) : base(left, right, ":=", 0) { }

        public override Expression Evaluate()
        {
            if (Left is Symbol)
            {
                var sym = (Symbol)Left;
                sym.scope.SetVar(sym.identifier, Right);
                return new Info(sym.identifier + ":=" + Right.ToString());
            }

            if (Left is InstanceFunc)
            {
                var insFunc = (InstanceFunc)Left;

                foreach (var arg in insFunc.args)
                {
                    if (!(arg is Symbol))
                        return new Error(this, "All arguments must be symbols");
                }

                var defFunc = new UsrFunc(insFunc.identifier, insFunc.args, insFunc.scope);

                defFunc.expr = Right;

                insFunc.scope.SetVar(insFunc.identifier, defFunc);

                return new Info(insFunc.ToString() + ":=" + Right.ToString());
            }

            if (Left is SysFunc)
            {
                return new Error(this, "Cannot override system function");
            }

            return new Error(this, "Left operand must be Symbol or Function");
        }

        public override Expression Expand()
        {
            return new Assign(Left.Expand(), Right.Expand());
        }

        protected override Expression SimplifyHelper(Expression left, Expression right)
        {
            return new Assign(Left.Simplify(), Right.Simplify());
        }

        public override Expression Clone()
        {
            return new Assign(Left.Clone(), Right.Clone());
        }

        public override Expression CurrectOperator()
        {
            return new Assign(Left.CurrectOperator(), Right.CurrectOperator());
        }
    }
}

