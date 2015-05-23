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

            Expression res = Left;

//            if (res is Dot)
//            {
//                res = new Variable( (res as Dot).Right.Value);
//            }

            if (res is Error)
                return res;

            if (res is Variable)
            {
                @var = (Variable)res;
                scope = @var.CurScope;
            }
            else
                return new Error(res, " is not a variable");


            if (@var is SysFunc)
            {
                return new Error(this, "Cannot override system function");
            }

            if (@var is VariableFunc)
            {
                var customFunc = (VariableFunc)@var;

                foreach (var arg in customFunc.Arguments)
                {
                    if (!(arg is Variable))
                        return new Error(this, "All arguments must be symbols");
                }

                Right.CurScope = customFunc;
                customFunc.Value = Right;
               
                scope.SetVar(customFunc);

                return @var;
            }

            // Variable
            @var.Value = Right.Evaluate();

            if (@var.Value is Error)
                return @var.Value;

            scope.SetVar(@var.Identifier, @var);

            return @var.Value;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Assign(left, right);
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new Assign(left, right);
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

