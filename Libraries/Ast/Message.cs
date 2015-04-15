using System;

namespace Ast
{
    public abstract class Message : Expression
    {
        public string message;

        public Message (string message)
        {
            this.message = message;
        }

        public override string ToString()
        {
            return message;
        }

        public override Expression Evaluate()
        {
            return this;
        }

        public override bool CompareTo(Expression other)
        {
            return false;
        }

        #region Add Overload
        public static Message operator +(Message left, Expression right)
        {
            return left;
        }

        public static Message operator +(Expression left, Message right)
        {
            return right;
        }

        public static Message operator +(Message left, Message right)
        {
            return left;
        }
        #endregion

        #region Sub Overload
        public static Message operator -(Message left, Expression right)
        {
            return left;
        }

        public static Message operator -(Expression left, Message right)
        {
            return right;
        }

        public static Message operator -(Message left, Message right)
        {
            return left;
        }
        #endregion

        #region Div Overload
        public static Message operator /(Message left, Expression right)
        {
            return left;
        }

        public static Message operator /(Expression left, Message right)
        {
            return right;
        }

        public static Message operator /(Message left, Message right)
        {
            return left;
        }
        #endregion

        #region Exp Overload
        public static Message operator ^(Message left, Expression right)
        {
            return left;
        }

        public static Message operator ^(Expression left, Message right)
        {
            return right;
        }

        public static Message operator ^(Message left, Message right)
        {
            return left;
        }
        #endregion

        #region LessThan Overload

        #endregion

        #region LessThanOrEqual Overload

        #endregion

        #region GreaterThan Overload

        #endregion

        #region GreaterThanOrEqual Overload

        #endregion
    }

    public class Info: Message
    {
        public Info(string message) : base(message)
        {
        }
    }

    public class Error: Message
    {
        public Error(string message) : base(message)
        {
        }
    }
}

