using System;

namespace Ast
{
    public class Dot : BinaryOperator
    {
        public Dot() : base(".", 100) { }
        public Dot(Expression left, Expression right) : base(left, right, ".", 60) { }

        protected override Expression Evaluate(Expression caller)
        {
            Variable sym;
            Scope scope;

            if (Right is Symbol)
            {
                sym = Right as Symbol;
            }
            else if (Right is UsrFunc)
            {
                sym = Right as UsrFunc;
            }
            else
                return new Error(this, "right operator must be Symbol/UsrFunc");

            if (Left is Scope)
            {
                scope = Left as Scope;
            }
            else if (Left is Symbol)
            {
                var symVal = (Left as Symbol).GetValue();

                if (symVal is Scope)
                    scope = symVal as Scope;
                else
                    return symVal;
            }
            else if (Left is Dot)
            {
                var dotVal = Left.Evaluate();

                if (dotVal is Error)
                    return dotVal;

                if (dotVal is Scope)
                    scope = dotVal as Scope;
                else
                    return new Error(this, "left operator must be Symbol/Scope");
            }
            else
                return new Error(this, "left operator must be Symbol/Scope");

            scope.Evaluate();
            return scope.GetVar(sym.identifier);
        }
    }
}

