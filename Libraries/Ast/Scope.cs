using System;
using System.Collections.Generic;

namespace Ast
{
    public class Scope : Expression
    {
        public Dictionary<string,Variable> Locals = new Dictionary<string,Variable>();
        public List<Statement> Statements = new List<Statement>();
        public List<EvalData> SideEffects = new List<EvalData>();

        public List<Error> Errors;

        readonly int MaxStatementPrint = 5;

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

        internal override Expression Evaluate(Expression caller)
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

        public Variable SetVar(string identifier, Expression expr)
        {
            var @var = new Variable(identifier, this);
            @var.Value = expr;

            return SetVar(@var);
        }

        public Variable SetVar(Variable @var)
        {
            if (Locals.ContainsKey(@var.Identifier))
                Locals.Remove(@var.Identifier);

            Locals.Add(@var.Identifier, @var);

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

