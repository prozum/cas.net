using System;

namespace Ast
{
    public class Dot : BinaryOperator
    {
        public override string Identifier { get { return "."; } }
        public override int Priority { get{ return 100; } }

        public Dot() { }
        public Dot(Expression left, Expression right) : base(left, right) { }

        public override Expression Value
        {
            get
            {
                Variable @var;
                Scope scope;

                if (Left is Scope)
                {
                    scope = (Scope)Left;
                    scope.Evaluate();
                }
                else
                {
                    var left = Left.Value;

                    if (left is Error)
                        return left;

                    if (left is Scope)
                        scope = (Scope)left;
                    else
                        return new Error(this, "left operator must be a Scope");
                }

                if (Right is Variable)
                    @var = (Variable)Right;
                else
                    return new Error(this, "right operator must be a Variable");

                @var.CurScope = scope;

                return @var.Value;
            }
        }

        public Scope VariabelScope
        {
            get
            {
                var res = Left.Value;

                if (res is Scope)
                    return res as Scope;
                else
                    return null;
            }
        }

        public override Expression Evaluate()
        {
            return Value.Evaluate();
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Dot(left, right);
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new Dot(left, right);
        }
    }
}

