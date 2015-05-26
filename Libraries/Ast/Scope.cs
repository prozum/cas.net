using System;
using System.Collections.Generic;

namespace Ast
{
    public class Scope : Expression
    {
        public List<Expression> Expressions = new List<Expression>();

        public Dictionary<string,Expression> Locals;
        public List<EvalData> SideEffects;

        public List<Expression> Returns;
        public Boolean Return;

        public Error Error;

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
            Error = null;

            Locals =  new Dictionary<string,Expression>();
            Returns = new List<Expression>();
            Return = new Boolean(false);
        }

        public Scope(Scope scope, bool share = false)
        {
            CurScope = scope;
            SideEffects = scope.SideEffects;
            Error = scope.Error;

            if (share)
            {
                Locals = scope.Locals;
                Returns = scope.Returns;
                Return = scope.Return;
            }
            else
            {
                Locals =  new Dictionary<string,Expression>();
                Returns = new List<Expression>();
                Return = new Boolean(false);
            }
        }

        public override Expression Evaluate()
        {
            Returns.Clear();
            Return.@bool = false;

            foreach (var expr in Expressions)
            {
                var res = expr.ReduceEvaluate();

                if (GetBool("debug"))
                    SideEffects.Add(new DebugData("Debug: " + expr + " = " + res));

                if (res is Error)
                    return res;

                if (!(res is Null))
                    Returns.Add(res);

                if (Return)
                    break;
            }

            switch (Returns.Count)
            {
                case 0:
                    return Constant.Null;
                case 1:
                    return Returns[0];
                default:
                    return new List(Returns);
            }
        }

        public override bool ContainsVariable(Variable other)
        {
            return false;
        }

        public void SetVar(string identifier, Expression expr)
        {
            if (Locals.ContainsKey(identifier))
                Locals.Remove(identifier);

            Locals.Add(identifier, expr);
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

