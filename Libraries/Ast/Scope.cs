using System;
using System.Collections.Generic;

namespace Ast
{
    public class Scope : Expression
    {
        public Dictionary<string,Expression> Locals = new Dictionary<string,Expression>();
        public List<Statement> Statements = new List<Statement>();
        public List<EvalData> SideEffects = new List<EvalData>();

        public List<Error> Errors;

        const int MaxStatementPrint = 5;

        public Scope()
        {
            SideEffects = new List<EvalData>();
            Errors = new List<Error>();
        }

        public Scope(Scope Scope)
        {
            this.Scope = Scope;
            SideEffects = Scope.SideEffects;
            Errors = Scope.Errors;
        }

        protected override Expression Evaluate(Expression caller)
        {
            return Evaluate();
        }

        public override Expression Evaluate()
        {
            var list = new List();

            if (Errors.Count > 0)
            {
                foreach (var error in Errors)
                {
                    SideEffects.Add(new ErrorData(error));
                }
                return new Null();
            }

            foreach (var stmt in Statements)
            {
                var data = stmt.Evaluate();

                if (data is ExprData)
                {
                    list.items.Add((data as ExprData).expr);
                    continue;
                }

                if (data is ReturnData)
                    return (data as ReturnData).expr;

                SideEffects.Add(data);

                if (data is ErrorData)
                    return new Null();
            }

            switch (list.items.Count)
            {
                case 0:
                    return new Null();
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

        public void SetVar(string @var, Expression expr)
        {
            if (Locals.ContainsKey(@var))
                Locals.Remove(@var);

            Locals.Add(@var, expr);
        }

        public Expression GetVar(Variable @var) { return GetVar(@var.Identifier); }
        public Expression GetVar(string @var)
        {
            Expression expr;

            if (Locals.TryGetValue(@var, out expr))
                return expr;

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

