using System;
using System.Collections.Generic;

namespace Ast
{
    public class Scope : Expression
    {
        public Dictionary<string,Variable> Locals = new Dictionary<string,Variable>();
        public List<Statement> Statements = new List<Statement>();
        public List<Expression> Returns = new List<Expression>();
        public List<EvalData> SideEffects;
        public List<ErrorData> Errors;
        public bool Return = false;

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

        readonly int MaxStatementPrint = 5;

        public Scope()
        {
            SideEffects = new List<EvalData>();
            Errors = new List<ErrorData>();
        }

        public Scope(Scope scope)
        {
            Scope = scope;
            SideEffects = scope.SideEffects;
            Errors = scope.Errors;
        }

        protected virtual T MakeClone<T>() where T : Scope, new()
        {
            T res = new T();
            res.Scope = Scope;
            res.Position = Position;
            res.Statements = new List<Statement>(Statements);

            return res;
        }

        public override Expression Clone()
        {
            return MakeClone<Scope>();
        }

        internal override Expression Evaluate(Expression caller)
        {
            return Evaluate();
        }

        public override Expression Evaluate()
        {
            if (Error)
                return new Null();

            Returns.Clear();
            Return = false;

            foreach (var stmt in Statements)
            {
                stmt.Evaluate();

                if (Error)
                    return new Null();

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
                @var.Scope = this;
            }
            else
            {
                @var = new Variable(identifier, this);
                @var.Value = expr;
            }

            //return SetVar(@var);

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

            if (Scope != null)
                return Scope.GetVar(identifier);

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

            if (Scope != null)
                return Scope.GetReal(identifier);

            return 0;
        }

//        public Int64 GetInt(string @var)
//        {
//            Expression expr;
//
//            if (Locals.TryGetValue(@var, out expr))
//            {
//                if (expr is Integer)
//                    return (expr as Integer).@int;
//            }
//
//            if (Scope != null)
//                return Scope.GetInt(@var);
//
//            return 0;
//        }
//
        public bool GetBool(string identifier)
        {
            Variable @var;

            if (Locals.TryGetValue(identifier, out @var))
            {
                if (@var.Value is Boolean)
                    return (@var.Value as Boolean).@bool;
            }

            if (Scope != null)
                return Scope.GetBool(identifier);

            return false;
        }
//
//        public string GetText(string @var)
//        {
//            Expression expr;
//
//            if (Locals.TryGetValue(@var, out expr))
//            {
//                if (expr is Text)
//                    return (expr as Text).Value;
//            }
//
//            if (Scope != null)
//            {
//                return Scope.GetText(@var);
//            }
//
//            return "";
//        }
            
        public override string ToString()
        {
            string str;
            if (Scope == null)
                str = "Global Scope: {";
            else
                str = "{";

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

