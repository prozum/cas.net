using System;
using System.Collections.Generic;

namespace Ast
{
    enum ConditionResult
    {
        True,
        False,
        Failed,
        NotEvaluated
    }

    public class IfState : State
    {
        public List<Expression> conditions = new List<Expression>();
        public List<Expression> expressions = new List<Expression>();

        int curCond = 0;
        ConditionResult resCond = ConditionResult.NotEvaluated;

        public IfState()
        {
        }

        public override Expression Evaluate()
        {
            for(int i = 0; i < conditions.Count; i++)
            {
                var res = conditions[i].Evaluate();

                if (res is Boolean)
                {
                    if ((res as Boolean).value)
                        return expressions[i].Evaluate();

                    continue;
                }
                else
                    return res;
            }

            if (conditions.Count + 1 == expressions.Count)
                return expressions[expressions.Count - 1].Evaluate();

            throw new Exception("Something went wrong in the parser");
        }

        public override EvalData Step()
        {

//            switch (resCond)
//            {
//                case ConditionResult.NotEvaluated:
//                    var res = conditions[curCond].Evaluate();
//                    if (res is Boolean)
//                    {
//                        if ((res as Boolean).value)
//                            resCond = ConditionResult.True;
//                        else
//                            resCond = ConditionResult.False;
//                    }
//                    else
//                    {
//                        resCond = ConditionResult.Failed;
//                        return new MsgData(MsgType.Error, res.ToString());
//                    }
//                    break;
//                case ConditionResult.True:
//                    return new MsgData(MsgType.Print, expressions[curCond].Evaluate().ToString());
//                case ConditionResult.False:
//                    if (curCond < conditions.Count)
//                        resCond = ConditionResult.NotEvaluated;
//
//            }
//
//            if (resCond == ConditionResult.True)
//            {
//                return new MsgData(MsgType.Print, expressions[curCond].Evaluate().ToString());
//            }
//
//            if (resCond == ConditionResult.False)
//            {
//
//            }
            throw new NotImplementedException();
        }
    }
}

