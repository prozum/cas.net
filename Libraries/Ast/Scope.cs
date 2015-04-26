using System;
using System.Collections.Generic;

namespace Ast
{
    public class Scope : Expression
    {
        public new Scope parent;
        public Dictionary<string,Expression> locals = new Dictionary<string,Expression>();
        public List<State> statements = new List<State>();

        public int curStep = 0;

        const int MaxStatementPrint = 5;

        public override Expression Evaluate()
        {
            if (statements.Count == 1)
                return statements[0].Evaluate();

            List res = new List();

            foreach (var statement in statements)
            {
                var exp = statement.Evaluate();

                if (exp is Error)
                {
                    statements.Clear();
                    return exp;
                }

                res.items.Add(exp);
            }

            return res;
        }

        public EvalData Step()
        {
            if (curStep < statements.Count)
                return statements[curStep++].Step();

            curStep = 0;
            return new DoneData();
        }

        public override bool ContainsVariable(Variable other)
        {
            // TODO
            return false;
        }

        public Scope() : this(null) { }
        public Scope(Scope parent)
        {
            this.parent = parent;
        }

        public void SetVar(string @var, Expression exp)
        {
            if (locals.ContainsKey(@var))
            {
                locals.Remove(@var);
            }

            locals.Add(@var, exp);
        }

        public Expression GetVar(string @var)
        {
            Expression exp;

            if (locals.TryGetValue(@var, out exp))
            {
                return exp;
            }

            if (parent != null)
            {
                return parent.GetVar(@var);
            }

            return null;
        }
            
        public override string ToString()
        {
            string str = "{";

            for (int i = 0; i < statements.Count; i++) 
            {
                if (i >= MaxStatementPrint)
                {
                    str += "...";
                    break;
                }
                else
                {
                    str += statements[i].ToString ();

                    if (i < statements.Count - 1) 
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

