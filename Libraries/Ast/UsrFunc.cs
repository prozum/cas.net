using System;
using System.Collections.Generic;

namespace Ast
{
    public class UsrFunc : Func
    {
        public Expression expr;

        public UsrFunc() : this(null, null, null) { }
        public UsrFunc(string identifier, List<Expression> args, Scope scope) : base(identifier, args, scope) { }

        public override Expression Evaluate()
        {
            return GetValue().Simplify().Evaluate();
        }

        public Expression GetValue()
        {
            Expression @var;

            @var = scope.GetVar(identifier);

            if (@var == null)
                return new Error(identifier + "> has no definition");

            if (@var is SysFunc)
            {
                var sysFuncDef = (SysFunc)@var;

                sysFuncDef.args = args;
                if (!sysFuncDef.isArgsValid())
                    return new ArgError(sysFuncDef);

                return sysFuncDef.Evaluate();

            }
            else if (@var is UsrFunc)
            {
                var usrFuncDef = (UsrFunc)@var;

                if (args.Count != usrFuncDef.args.Count)
                    return new Error(identifier + " takes " + usrFuncDef.args.Count.ToString() + " arguments. Not " + args.Count.ToString() + ".");

                for (int i = 0; i < args.Count; i++)
                {
                    var arg = (Symbol)usrFuncDef.args[i];

                    scope.SetVar(arg.identifier, args[i]);
                }

                expr = usrFuncDef.expr;

                return expr.Evaluate();
            }
            else
            {
                return new Error(this, "Variable is not a function");
            }

            //            if (def.ContainsVariable(this))
            //            {
            //                return new Error(this, "Could not get value of: " + this.identifier);
            //            }
        }

        public override Expression Clone()
        {
            return MakeClone<UsrFunc>();
        }

        public override Expression Simplify()
        {
            if (prefix.CompareTo(Constant.Zero))
            {
                return new Integer(0);
            }
            if (exponent.CompareTo(Constant.Zero))
            {
                return new Integer(1);
            }

            return GetValue();
        }

        public override string ToString()
        {
            string str = identifier + "[";

            for (int i = 0; i < args.Count; i++)
            {
                str += args[i];

                if (i < args.Count - 1)
                    str += ",";
            }
            str += "]";

            return str;
        }

        public override bool ContainsVariable(Variable other)
        {
            if (base.ContainsVariable(other))
            {
                return true;
            }
            else
            {
                foreach (var item in (this as Func).args)
                {
                    if (item.ContainsVariable(other))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

