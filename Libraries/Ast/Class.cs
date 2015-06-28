using System;

namespace Ast
{
    public class Class : Scope
    {
        public Class(Scope scope) : base(scope)
        {
        }

        public override Expression Evaluate()
        {
            return CreateInstance();
        }

        public Expression CreateInstance()
        {
            var instance = new Scope(CurScope);

            foreach (var @var in Locals)
            {
                instance.SetVar(@var.Key, @var.Value.Clone(instance));
            }

            foreach (var expr in Expressions)
            {
                var cloneExpr = expr.Clone(instance);
                var res = cloneExpr.Evaluate();

                if (res is Error)
                    return res;

                //instance.Expressions.Add(expr.Clone());
            }

            //instance.Evaluate();
            return instance;
        }

        public override string ToString()
        {
            return "class" + base.ToString();
        }
    }
}

