using System;

namespace Ast
{
    public class Dot : BinaryOperator
    {
        public override string Identifier { get { return "."; } }
        public override int Priority { get{ return 100; } }

        public Dot() { }
        public Dot(Expression left, Expression right, Scope scope) : base(left, right, scope) { }

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

                    if (CurScope.Error)
                        return Constant.Null;

                    if (left is Scope)
                        scope = (Scope)left;
                    else
                    {
                        CurScope.Errors.Add(new ErrorData(this, "left operator must be a Scope"));
                        return Constant.Null;
                    }
                }

                if (Right is Variable)
                {
                    @var = (Variable)Right;

                    @var.CurScope = scope;

                    return @var.Value;
                }
                else if (Right is Call)
                {
                    var call = (Call)Right;

                    if (call.Child is Variable)
                    {
                        @var = (Variable)call.Child;

                        @var.CurScope = scope;

                        return call;
                    }
                }

                CurScope.Errors.Add(new ErrorData(this, "right operator must be a Variable/Function"));
                return Constant.Null;
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

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return Value;
        }
    }
}

