using System;

namespace Ast
{
    public class Assign : BinaryOperator
    {
        public override string Identifier { get { return ":="; } }
        public override int Priority { get{ return 0; } }

        public Assign() { }
        public Assign(Expression left, Expression right) : base(left, right) { }

        public override Expression Evaluate()
        {
            Variable @var;
            Scope scope;

            Expression res;

            if (Left is Error)
                return Left;
            
            if (Left is Dot)
            {
                res = Left.Value;

                if (res is Error)
                    return res;

                if (res is Variable)
                    @var = res as Variable;
                else
                    return new Error(res, " is not a variable");
            }
            else if (Left is Variable)
                @var = (Variable)Left;
            else
                return new Error(Left, " is not a variable");

            scope = @var.Scope;

            if (@var is CustomFunc)
            {
                var customFunc = (CustomFunc)Left;

                foreach (var arg in customFunc.Arguments)
                {
                    if (!(arg is Variable))
                        return new Error(this, "All arguments must be symbols");
                }

                Right.Scope = customFunc;
                customFunc.Value = Right;
               
                scope.SetVar(customFunc);

                return @var;
            }


            if (Left is SysFunc)
            {
                return new Error(this, "Cannot override system function");
            }

            if (@var is Variable)
            {
                @var.Value = Right.Evaluate();

                if (@var.Value is Error)
                    return @var.Value;

                scope.SetVar(@var.Identifier, @var.Value);

                return @var.Value;
            }

            return new Error(this, "Left operand must be Symbol or Function");
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
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

        internal override Expression CurrectOperator()
        {
            return new Assign(Left.CurrectOperator(), Right.CurrectOperator());
        }
    }
}

