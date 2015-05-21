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

                var left = Left.Value;

                if (left == null &&
                    Left is Variable)
                {
                    left = Scope.SetVar((Left as Variable).Identifier, new Null());
                }

                if (left is Variable)
                    scope = (Variable)Scope.GetVar((left as Variable).Identifier);
                else if (left is Scope)
                    scope = (Scope)Left;
                else if (left is Error)
                    return left;
                else
                    return new Error(this, "left operator must be a Scope");
 
                if (Right is Variable)
                    @var = Right as Variable;
                else
                    return new Error(this, "right operator must be Symbol/SymbolFunc");

                return scope.GetVar(@var);
            }
        }

        public override Expression Evaluate()
        {
            return Value;
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

