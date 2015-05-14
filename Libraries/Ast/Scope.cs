using System;
using System.Collections.Generic;

namespace Ast
{
    public class Scope : Expression
    {
        public Dictionary<string,Expression> Locals = new Dictionary<string,Expression>();
        public List<Statement> Statements = new List<Statement>();
        public List<EvalData> SideEffects = new List<EvalData>();

        public Error Error;
        public List ReturnExpr = new List();

        const int MaxStatementPrint = 5;

        public Scope()
        {
            SideEffects = new List<EvalData>();
        }

        public Scope(Scope Scope)
        {
            this.Scope = Scope;
            SideEffects = Scope.SideEffects;
        }

        public override Expression Evaluate()
        {
            var list = new List();

            if (Error != null)
            {
                SideEffects.Add(new ErrorData(Error));
                return Error;
            }

            foreach (var stmt in Statements)
            {
                var data = stmt.Evaluate();

                if (data is ExprData)
                {
                    list.items.Add((data as ExprData).expr);
                    if (GetBool("debug"))
                        SideEffects.Add(stmt.GetDebugData());
                    continue;
                }

                if (data is ReturnData)
                    return (data as ReturnData).expr;

                if (data is ErrorData)
                {
                    SideEffects.Add(data);
                    break;
                }  

                SideEffects.Add(data);
            }

            switch (list.items.Count)
            {
                case 0:
                    return this;
                case 1:
                    return list.items[0];
                default:
                    return list;
            }
        }

        public override bool ContainsVariable(Variable other)
        {
            // TODO
            return false;
        }

        public void SetVar(string @var, Expression exp)
        {
            if (Locals.ContainsKey(@var))
                Locals.Remove(@var);

            Locals.Add(@var, exp);
        }

        public Expression GetVar(string @var)
        {
            Expression exp;

            if (Locals.TryGetValue(@var, out exp))
                return exp;

            if (Scope != null)
                return Scope.GetVar(@var);

            return new Error(this, @var + " has no definition");
        }

        public decimal GetReal(string @var)
        {
            Expression expr;

            if (Locals.TryGetValue(@var, out expr))
            {
                if (expr is Real)
                    return (expr as Real).Value;
            }

            if (Scope != null)
                return Scope.GetReal(@var);

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

            if (Scope != null)
                return Scope.GetInt(@var);

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

            if (Scope != null)
                return Scope.GetBool(@var);

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

            if (Scope != null)
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

