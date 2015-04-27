using System;
using System.Collections.Generic;

namespace Ast
{
    public class Map : SysFunc
    {
        public Func func;
        public List list;

        public Map(List<Expression> args, Scope scope)
            : base("map", args, scope)
        {
            validArgs = new List<ArgKind>()
                {
                    ArgKind.Symbol,
                    ArgKind.List
                };
        }

        protected override Expression Evaluate(Expression caller)
        {
            if (!isArgsValid())
                return new ArgError(this);


            var sym = (Symbol)args[0];
            func = (Func)sym.GetValue();
            list = (List)args[1];

            //Expression exp;
            //List<string> argNames;
            //evaluator.funcDefs.TryGetValue(sym.identifier, out exp);
            //evaluator.funcParams.TryGetValue(sym.identifier, out argNames);
            //parser.
            //list = (List)args[1];

            //if (func.argKinds.Count > 1)
            //    return new Error(this, "only supports unary functions");
            //string arg = argNames[0];

            //var locals = new Dictionary<string, Expression>(func.locals);

            var res = new List();
            foreach (var element in list.items)
            {
                //locals.Remove(func.argNames[0]);
                //locals.Add(func.argNames[0], element);
                func.args[0] = element;

                res.items.Add(func.Evaluate());
            }

            return res;
        }

        public override Expression Clone()
        {
            throw new NotImplementedException();
        }
    }
}

