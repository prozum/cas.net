using System;

namespace Ast
{
    /// <summary>
    /// 
    /// </summary>
    public class Dot : BinaryOperator
    {
        public override string Identifier { get { return "."; } }
        public override int Priority { get{ return 100; } }

        public Dot() { }
        public Dot(Expression left, Expression right) : base(left, right) { }

        public override Expression Evaluate()
        {
            return GetValue();
        }

        public override Expression GetValue()
        {
            Variable sym;
            Scope scope;

            if (Right is Symbol)
                sym = Right as Variable;
            else
                return new Error(this, "right operator must be Symbol/SymbolFunc");

            if (Left.GetValue() is Scope)
                scope = (Scope)Left.GetValue();
            else if (Left.GetValue() is Error)
                return Left.GetValue();
            else
                return new Error(this, "left operator must be a Scope");

            return scope.GetVar(sym);
        }
    }
}

