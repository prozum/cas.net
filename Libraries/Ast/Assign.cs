using System;

namespace Ast
{
    public class Assign : BinaryOperator
    {
        public Assign() : base(":=", 0) { }
        public Assign(Expression left, Expression right) : base(left, right, ":=", 0) { }

        protected override Expression Evaluate(Expression caller)
        {
            if (Left is Symbol)
            {
                var sym = (Symbol)Left;
                sym.scope.SetVar(sym.identifier, Right);
                return new Info(sym.identifier + ":=" + Right.ToString());
            }

            if (Left is UsrFunc)
            {
                var usrFunc = (UsrFunc)Left;

                foreach (var arg in usrFunc.args)
                {
                    if (!(arg is Symbol))
                        return new Error(this, "All arguments must be symbols");
                }

                var defFunc = new UsrFunc(usrFunc.identifier, usrFunc.args, usrFunc.scope);

                defFunc.expr = Right;

                usrFunc.scope.SetVar(usrFunc.identifier, defFunc);

                return new Info(usrFunc.ToString() + ":=" + Right.ToString());
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

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new Assign(Left.Reduce(this), Right.Reduce(this));
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

