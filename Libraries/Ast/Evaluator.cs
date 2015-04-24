using System;
using System.Collections.Generic;

namespace Ast
{
    public class Evaluator
    {
        public Scope scope;

        public Evaluator ()
        {
            scope = new Scope();
            scope.SetVar("deg", new Boolean(true));
        }

        public EvalData Evaluation(string inputString)
        {
            var res = Parser.Parse(inputString, scope);

            var exp = res.Evaluate();

            if (exp is Assign)
            {
                if ((exp as Assign).Left is UsrFunc)
                {
                    return AssignFunction((exp as Assign).Left as UsrFunc, (exp as Assign).Right);
                }
                if ((exp as Assign).Left is Symbol)
                {
                    return AssignSymbol((exp as Assign).Left as Symbol, (exp as Assign).Right);
                }
                else
                {
                    return new MsgData(MsgType.Error, "Evaluator> Left expression is not a symbol or function");
                }
            }

            if (exp is Plot)
            {
                if ((exp as Plot).isArgsValid())
                    return new PlotData((Plot) exp);

                exp = new ArgError((Plot)exp);
            }
            else if (exp is Simplify || exp is Expand)
            {
                exp = exp.Evaluate();
            }
            else
            {
                exp = exp.Simplify().Evaluate();
            }

            if (exp is Error)
            {
                return new MsgData(MsgType.Error, exp.ToString());
            }

            if (exp is Info)
            {
                return new MsgData(MsgType.Info, exp.ToString());
            }

            return new MsgData(MsgType.Print, exp.ToString());
        }

        public EvalData AssignFunction(UsrFunc func, Expression expr)
        {
            if (expr.ContainsVariable(func))
            {
                return new MsgData(MsgType.Error, "Evaluator> Can't define function as it self");
            }

            func.expr = expr;
            func.scope = new Scope(scope);
            scope.SetVar(func.identifier, func);

            foreach (var exp in func.args)
            {
                if (exp is Symbol)
                {
                    var sym = (Symbol)exp;
                    sym.scope = func.scope;
                    func.scope.SetVar(sym.identifier, sym);
                }
                else
                {
                    return new MsgData(MsgType.Error, "Evaluator> One arg in the function is not a symbol");
                }
            }

            return new MsgData(MsgType.Info, "Evaluator> Function '" + func.ToString() + "' defined");
        }

        public EvalData AssignSymbol(Symbol sym, Expression exp)
        {
            if (exp.ContainsVariable(sym))
            {
                return new MsgData(MsgType.Error, "Evaluator> Can't define symbol as it self");
            }

            scope.SetVar(sym.identifier, exp);

            return new MsgData(MsgType.Info, "Evaluator> Variable '" + sym.ToString() + "' defined");
        }
    }
}

