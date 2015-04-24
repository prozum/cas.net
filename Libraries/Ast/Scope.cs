using System;
using System.Collections.Generic;

namespace Ast
{
    public class Scope : Expression
    {
        public new Scope parent;
        public Dictionary<string,Expression> locals = new Dictionary<string,Expression>();
        public Queue<Expression> statements = new Queue<Expression>();

        public override Expression Evaluate()
        {
            List res = new List();

            while(statements.Count > 0)
            {
                var exp = statements.Dequeue().Evaluate();

                if (exp is Error)
                {
                    statements.Clear();
                    return exp;
                }

                res.elements.Add(exp);
            }

            return res;
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
    }
}

