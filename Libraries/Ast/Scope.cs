using System;
using System.Collections.Generic;

namespace Ast
{
    public class Scope : Expression
    {
        public Dictionary<string,Expression> Locals = new Dictionary<string,Expression>();
        public List<Expression> Statements = new List<Expression>();

        int curStep = -1;
        public Error ScopeError;
        public List ReturnExpr = new List();

        const int MaxStatementPrint = 5;

        public Scope() : this(null) { }
        public Scope(Scope parent)
        {
            this.parent = parent;
        }

        public override Expression Evaluate()
        {
            return this;
        }

        public override EvalData Step()
        {
            EvalData res;

            if (curStep == -1)
            {
                curStep = 0;
                ReturnExpr.items.Clear();
            }

            do
            {
                res = Statements[curStep].Step();

                if (res is ReturnData)
                {
                    Reset();
                    return new ReturnData((res as ReturnData).expr);
                }

                if (res is ExprData)
                {
                    ReturnExpr.items.Add((res as ExprData).expr);
                    return new DebugData("Evaluate: ", (res as ExprData).expr);
                }

                if (res is ErrorData)
                {
                    Reset();
                    return res;
                }

                if (res is DoneData)
                {
                    curStep++;
                }
                else
                {
                    return res;
                }
            }
            while (curStep < Statements.Count);
                
            Reset();
            switch (ReturnExpr.items.Count)
            {
                case 0:
                    return new DoneData();
                case 1:
                    return new ReturnData(ReturnExpr.items[0]);
                default:
                    return new ReturnData(ReturnExpr);
            }
        }

        private void Reset()
        {
            curStep = -1;
        }

        public override bool ContainsVariable(Variable other)
        {
            // TODO
            return false;
        }

        public void SetVar(string @var, Expression exp)
        {
            if (Locals.ContainsKey(@var))
            {
                Locals.Remove(@var);
            }

            Locals.Add(@var, exp);
        }

        public Expression GetVar(string @var)
        {
            Expression exp;

            if (Locals.TryGetValue(@var, out exp))
            {
                return exp;
            }

            if (parent != null)
            {
                return Scope.GetVar(@var);
            }

            return null;
        }

        public decimal GetReal(string @var)
        {
            Expression expr;

            if (Locals.TryGetValue(@var, out expr))
            {
                if (expr is Real)
                    return (expr as Real).Value;
            }

            if (parent != null)
            {
                return Scope.GetReal(@var);
            }

            return 0;
        }

        public Int64 GetInt(string @var)
        {
            Expression expr;

            if (Locals.TryGetValue(@var, out expr))
            {
                if (expr is Integer)
                    return (expr as Integer).@int;
            }

            if (parent != null)
            {
                return Scope.GetInt(@var);
            }

            return 0;
        }

        public bool GetBool(string @var)
        {
            Expression expr;

            if (Locals.TryGetValue(@var, out expr))
            {
                if (expr is Boolean)
                    return (expr as Boolean).@bool;
            }

            if (parent != null)
            {
                return Scope.GetBool(@var);
            }

            return false;
        }

        public string GetText(string @var)
        {
            Expression expr;

            if (Locals.TryGetValue(@var, out expr))
            {
                if (expr is Text)
                    return (expr as Text).Value;
            }

            if (parent != null)
            {
                return Scope.GetText(@var);
            }

            return "";
        }
            
        public override string ToString()
        {
            string str = "{";

            for (int i = 0; i < Statements.Count; i++) 
            {
                if (i >= MaxStatementPrint)
                {
                    str += "...";
                    break;
                }
                else
                {
                    str += Statements[i].ToString ();

                    if (i < Statements.Count - 1) 
                    {
                        str += ';';
                    }
                }
            }

            str += "}";

            return str;
        }
    }
}

