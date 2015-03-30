using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ast
{
	public abstract class Expression 
	{
        public Evaluator evaluator;
        public Operator parent;
        public Function functionCall;
        public abstract Expression Evaluate();

        public virtual void SetFunctionCall(Function functionCall)
        {
            this.functionCall = functionCall;
        }
        

		//public abstract string ToString ();
		//public abstract bool Contains (Expression a);

        #region Add
        public static Expression operator +(Expression a, Expression b)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Sub
        public static Expression operator -(Expression a, Expression b)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Mul
        public static Expression operator *(Expression a, Expression b)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Div
        public static Expression operator /(Expression a, Expression b)
        {
            throw new NotImplementedException();
        }
        #endregion
	}
}