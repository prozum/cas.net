using System;
using System.Collections.Generic;

namespace Ast
{
    public class Scope : Expression
    {
        public Scope Global;
        public List<Expression> Expressions = new List<Expression>();

        public HashSet<string> Globals;
        public Dictionary<string,Expression> Locals;
        public List<EvalData> SideEffects;

        public List Returns = new List();
        public Boolean Return;

        public Error Error;
        public bool Shared;

        public override Scope CurScope
        {
            get
            {
                return base.CurScope;
            }
            set
            {
                base.CurScope = value;

//                foreach (var stmt in Expressions)
//                {
//                    stmt.CurScope = value;
//                }
            }
        }

        readonly int MaxStatementPrint = 5;

        public Scope()
        {
            SideEffects = new List<EvalData>();
            Locals =  new Dictionary<string,Expression>();
            Globals = new HashSet<string>();
            Return = new Boolean(false);
        }

        public Scope(Scope scope, bool shared = false)
        {
            CurScope = scope;
            Global = scope.Global;
            SideEffects = scope.SideEffects;
            Error = scope.Error;
            Shared = shared;

            if (Shared)
            {
                Locals = scope.Locals;
                Globals = scope.Globals;
                Return = scope.Return;
            }
            else
            {
                Locals =  new Dictionary<string,Expression>();
                Globals = new HashSet<string>();
                Return = new Boolean(false);
            }
        }

        public override Expression Evaluate()
        {
            Expression res;

            if (Error != null)
                return Error;

            Returns.Items.Clear();
            Return.@bool = false;

            foreach (var expr in Expressions)
            {
                if (GetBool("reduc"))
                    res = expr.ReduceEvaluate();
                else
                    res = expr.Evaluate();

                if (GetBool("debug"))
                    SideEffects.Add(new DebugData("Debug: " + expr + " = " + res));

                if (res is Error)
                    return res;

                if (Return)
                {
                    if (Shared)
                        CurScope.Returns.Items.Add(Returns[0]);

                    break;
                }

                if (!(res is Null))
                    Returns.Items.Add(res);
            }

            switch (Returns.Count)
            {
                case 0:
                    return Constant.Null;
                case 1:
                    return Returns[0];
                default:
                    return Returns;
            }
        }

        public override bool ContainsVariable(Variable other)
        {
            return false;
        }

        public void SetVar(string identifier, Expression expr)
        {
            if (Globals.Contains(identifier))
            {
                if (Global.Locals.ContainsKey(identifier))
                    Global.Locals.Remove(identifier);
                
                Global.Locals.Add(identifier, expr);
            }
            else
            {
                if (Locals.ContainsKey(identifier))
                    Locals.Remove(identifier);

                Locals.Add(identifier, expr);
            }
        }

        // TODO Fix position
        public Expression GetVar(Variable @var) { return GetVar(@var.Identifier); }
        public Expression GetVar(string identifier)
        {
            Expression expr;

            if (Locals.TryGetValue(identifier, out expr))
                return expr;

            if (CurScope != null)
                return CurScope.GetVar(identifier);

            return new Error(identifier + " is not defined");
        }

        public decimal GetReal(string identifier)
        {
            Expression expr;

            if (Locals.TryGetValue(identifier, out expr))
            {
                if (expr is Real)
                    return expr as Real;
            }

            if (CurScope != null)
                return CurScope.GetReal(identifier);

            return 0;
        }

        public Int64 GetInt(string identifier)
        {
            Expression expr;

            if (Locals.TryGetValue(identifier, out expr))
            {
                if (expr is Integer)
                    return Value as Integer;
            }

            if (CurScope != null)
                return CurScope.GetInt(identifier);

            return 0;
        }

        public bool GetBool(string identifier)
        {
            Expression expr;

            if (Locals.TryGetValue(identifier, out expr))
            {
                if (expr is Boolean)
                    return (expr as Boolean).@bool;
            }

            if (CurScope != null)
                return CurScope.GetBool(identifier);

            return false;
        }

        public string GetText(string identifier)
        {
            Expression expr;

            if (Locals.TryGetValue(identifier, out expr))
            {
                if (expr is Text)
                    return expr as Text;
            }

            if (CurScope != null)
                return CurScope.GetText(identifier);

            return "";
        }
            
        public override string ToString()
        {
            if (Expressions.Count == 0)
                return "";

            string str = "{";
                
            for (int i = 0; i < Expressions.Count; i++) 
            {
                if (i >= MaxStatementPrint)
                {
                    str += "...";
                    break;
                }
                else
                {
                    str += Expressions[i].ToString ();

                    if (i < Expressions.Count - 1) 
                    {
                        str += ';';
                    }
                }
            }

            str += "}";

            return str;
        }

        public bool IsDefined(string identifier)
        {
            if (Locals.ContainsKey(identifier))
                return true;

            if (CurScope != null)
                return CurScope.IsDefined(identifier);

            return false;
        }
    }
}

