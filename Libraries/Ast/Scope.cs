using System;
using System.Collections.Generic;

namespace Ast
{
    public class Scope : Expression
    {
        public List<Expression> Expressions = new List<Expression>();

        public Dictionary<string,Variable> Locals;
        public List<EvalData> SideEffects;

        public List<Expression> Returns;
        public Boolean Return;

        public List<ErrorData> Errors;
        public bool Error 
        { 
            get 
            {
                if (Errors.Count > 0)
                {
                    foreach (var error in Errors)
                    {
                        SideEffects.Add(error);
                    }
                    return true;
                }
                else
                    return false;
            }
        }

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
            Errors = new List<ErrorData>();

            Locals =  new Dictionary<string,Variable>();
            Returns = new List<Expression>();
            Return = new Boolean(false);
        }

        public Scope(Scope scope, bool share = false)
        {
            CurScope = scope;
            SideEffects = scope.SideEffects;
            Errors = scope.Errors;

            if (share)
            {
                Locals = scope.Locals;
                Returns = scope.Returns;
                Return = scope.Return;
            }
            else
            {
                Locals =  new Dictionary<string,Variable>();
                Returns = new List<Expression>();
                Return = new Boolean(false);
            }
        }

        public override Expression Evaluate()
        {
            if (Error)
                return new Null();

            Returns.Clear();
            Return.@bool = false;

            foreach (var expr in Expressions)
            {
                var res = expr.Evaluate();

                if (GetBool("debug"))
                    SideEffects.Add(new DebugData("Debug: " + expr + " = " + res));

                if (res is Error)
                    Errors.Add(new ErrorData(res as Error));

                if (Error)
                    return new Null();

                if (!(res is Null))
                    Returns.Add(res);

                if (Return)
                    break;
            }

            switch (Returns.Count)
            {
                case 0:
                    return new Null();
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

        public Variable SetVar(Variable @var)
        {
            return SetVar(@var.Identifier, @var);
        }

        public Variable SetVar(string identifier, Expression expr)
        {
            Variable @var;

            if (expr is Variable)
            {
                @var = (Variable)expr;
                @var.CurScope = this;
            }
            else
            {
                @var = new Variable(identifier, this);
                @var.Value = expr;
            }

            if (Locals.ContainsKey(identifier))
                Locals.Remove(identifier);

            Locals.Add(identifier, @var);

            return @var;
        }

        // TODO Fix position
        public Expression GetVar(Variable @var) { return GetVar(@var.Identifier); }
        public Expression GetVar(string identifier)
        {
            Variable @var;

            if (Locals.TryGetValue(identifier, out @var))
                return @var;

            if (CurScope != null)
                return CurScope.GetVar(identifier);

            return new Error(identifier + " is not defined");
        }

        public decimal GetReal(string identifier)
        {
            Variable @var;

            if (Locals.TryGetValue(identifier, out @var))
            {
                if (@var.Value is Real)
                    return @var.Value as Real;
            }

            if (CurScope != null)
                return CurScope.GetReal(identifier);

            return 0;
        }

        public Int64 GetInt(string identifier)
        {
            Variable @var;

            if (Locals.TryGetValue(identifier, out @var))
            {
                if (@var.Value is Integer)
                    return (@var.Value as Integer).@int;
            }

            if (CurScope != null)
                return CurScope.GetInt(identifier);

            return 0;
        }

        public bool GetBool(string identifier)
        {
            Variable @var;

            if (Locals.TryGetValue(identifier, out @var))
            {
                if (@var.Value is Boolean)
                    return (@var.Value as Boolean).@bool;
            }

            if (CurScope != null)
                return CurScope.GetBool(identifier);

            return false;
        }

        public string GetText(string identifier)
        {
            Variable @var;

            if (Locals.TryGetValue(identifier, out @var))
            {
                if (@var.Value is Text)
                    return (@var.Value as Text).@string;
            }

            if (CurScope != null)
                return CurScope.GetText(identifier);

            return "";
        }
            
        public override string ToString()
        {
            string str;
            if (CurScope == null)
                str = "Global Scope: {";
            else
                str = "{";

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
    }
}

