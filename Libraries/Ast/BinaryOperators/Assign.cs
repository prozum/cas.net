using System;

namespace Ast
{
    public class Assign : BinaryOperator
    {
        public override string Identifier { get { return ":="; } }
        public override int Priority { get{ return 0; } }

        public Assign() { }
        public Assign(Expression left, Expression right, Scope scope) : base(left, right, scope) 
        {
            CurScope = scope;
        }

        public override Expression Evaluate()
        {
            string identifier;
            Scope scope;
            Expression expr;

            Expression res;

            // Find Variable & Scope
            if (Left is Dot)
            {
                scope = (Left as Dot).VariabelScope;

                if (scope == null)
                {
                    CurScope.Errors.Add(new ErrorData(Left, " is not a valid scope"));
                    return Constant.Null;
                }

                res = (Left as Dot).Right;
            }
            else
            {
                res = Left;
                scope = CurScope;
            }

            if (CurScope.Error)
                return Constant.Null;
             

            // Find Identifier & Expression
            if (res is Variable)
            {
                identifier = (res as Variable).Identifier;
                expr = Right.Evaluate();
            }
            else if (res is Call)
            {
                var call = (Call)res;

                if (call.Child is Variable)
                {
                    identifier = (call.Child as Variable).Identifier;
                    expr = new VarFunc(identifier, Right, call.Arguments, scope);
                   
                }
                else
                {
                    CurScope.Errors.Add(new ErrorData(call.Child, "is not a variable"));
                    return Constant.Null;
                }
            }
            else
            {
                CurScope.Errors.Add(new ErrorData(res, " is not a variable"));
                return Constant.Null;
            }

            if (CurScope.Error)
                return Constant.Null;

            scope.SetVar(identifier, expr);

            return expr;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Assign(left, right, CurScope);
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new Assign(left, right, CurScope);
        }

        public override Expression Clone()
        {
            return new Assign(Left.Clone(), Right.Clone(), CurScope);
        }

        internal override Expression CurrectOperator()
        {
            return new Assign(Left.CurrectOperator(), Right.CurrectOperator(), CurScope);
        }
    }
}

