using System;
using System.Collections.Generic;

namespace Ast
{
    public class Scope
    {
        public new Scope parent;
        public Dictionary<string,Expression> locals = new Dictionary<string,Expression>();
        public List<Expression> statements = new List<Expression>();

//        public override Expression Evaluate()
//        {
//            foreach (var statement in statements)
//            {
//                statement.Evaluate();
//            }
//        }

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

