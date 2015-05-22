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

                var res = Left;

                if (res is Variable)
                    res = Scope.GetVar((res as Variable).Identifier);
                else if (res is Scope)
                    res.Evaluate();
                else if (res is Dot)
                    res = res.Evaluate();

                if (res is Error)
                    return res;

                if (!(res is Scope))
                    return new Error(this, "left operator must be a Scope");
                scope = (Scope)res;
 
                if (Right is Variable)
                    @var = Right as Variable;
                else if (Right is Self)
                    return scope;
                else
                    return new Error(this, "right operator must be a Variable");

                return scope.GetVar(@var).Value;
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

