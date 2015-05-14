﻿using System;

namespace Ast
{
    public class Assign : BinaryOperator
    {
        public Assign() : base(":=", 0) { }
        public Assign(Expression left, Expression right) : base(left, right, ":=", 0) { }

        protected override Expression Evaluate(Expression caller)
        {
            Variable sym;
            Scope scope;

            Expression res;

            if (Left is ErrorExpr)
                return Left;

            if (Left is Dot)
            {
                var dot = Left as Dot;

                if (dot.Right is Variable)
                    sym = dot.Right as Variable;
                else
                    return new ErrorExpr(dot.Right, " is not a symbol");

                res = dot.Left.Evaluate();

                if (res is ErrorExpr)
                    return res;

                if (res is Scope)
                    scope = res as Scope;
                else
                    return new ErrorExpr(res, " is not a scope");
            }
            else if (Left is Variable)
            {
                sym = (Variable)Left;
                scope = Left.Scope;
            }
            else
                return new ErrorExpr(Left, " is not a symbol");


            if (sym is Symbol)
            {
                res = Right.Evaluate();

                if (res is ErrorExpr)
                    return res;

                scope.SetVar(sym.identifier, res);

                return res;
            }

            if (sym is UsrFunc)
            {
                var usrFunc = (UsrFunc)Left;

                foreach (var arg in usrFunc.args)
                {
                    if (!(arg is Symbol))
                        return new ErrorExpr(this, "All arguments must be symbols");
                }

                var defFunc = new UsrFunc(usrFunc.identifier, usrFunc.args, usrFunc.Scope);

                defFunc.expr = Right;

                usrFunc.Scope.SetVar(usrFunc.identifier, defFunc);

                return this;
            }

            if (Left is SysFunc)
            {
                return new ErrorExpr(this, "Cannot override system function");
            }

            return new ErrorExpr(this, "Left operand must be Symbol or Function");
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

