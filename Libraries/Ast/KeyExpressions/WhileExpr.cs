namespace Ast
{
    public class WhileExpr : Expression
    {
        public Expression Condition;
        public Scope WhileScope;

        readonly int MaxIterations = 100000;

        public WhileExpr(Scope scope)
        {
            CurScope = scope;
        }

        public override Expression Evaluate()
        {
            int i = 0;
            var resList = new List();

            while (i++ < MaxIterations)
            {
                var res = Condition.ReduceEvaluate();

                if (res is Error)
                    return res;

                if (res is Boolean)
                {
                    if (!(res as Boolean).@bool)
                        break;
                }

                res = WhileScope.Evaluate();

                if (res is Error)
                    return res;

                resList.Items.Add(res);
            }

            if (i > MaxIterations)
                return new Error(this, "Overflow");

            return resList;

        }

        public override Expression Clone(Scope scope)
        {
            var whileExpr = new WhileExpr(scope);

            whileExpr.Condition = Condition.Clone(scope);
            whileExpr.WhileScope = WhileScope.Clone(scope) as Scope;

            return whileExpr;
        }

        public override string ToString()
        {
            return WhileScope.ToString();
        }

    }
}

