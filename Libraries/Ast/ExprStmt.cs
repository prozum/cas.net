﻿using System;

namespace Ast
{
    public class ExprStmt : Statement
    {
        public Expression expr;

        public ExprStmt(Expression expr, Scope scope) : base(scope)
        {
            this.expr = expr;
        }

        public override EvalData Evaluate()
        {
            var res = expr.Evaluate();

            if (Scope.GetBool("debug"))
                Scope.SideEffects.Add(new DebugData("Debug: " + expr + " = " + res));

            return new ExprData(res);
        }

        public override string ToString()
        {
            return expr.ToString();
        }
    }
}
