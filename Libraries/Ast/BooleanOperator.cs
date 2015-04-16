using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ast
{
    public class BooleanEqual : Operator
    {
        public BooleanEqual() : base("==", 10) { }
        public BooleanEqual(Expression left, Expression right) : base(left, right, "==", 10) { }

        public override Expression Evaluate()
        {
            return new Boolean(left.CompareTo(right));
        }
    }

    public class Lesser : Operator
    {
        public Lesser() : base("<", 10) { }
        public Lesser(Expression left, Expression right) : base(left, right, "<", 10) { }

        public override Expression Evaluate()
        {
            return left < right;
        }
    }

    public class LesserOrEqual : Operator
    {
        public LesserOrEqual() : base("<=", 10) { }
        public LesserOrEqual(Expression left, Expression right) : base(left, right, "<=", 10) { }

        public override Expression Evaluate()
        {
            return left <= right;
        }
    }

    public class Greater : Operator
    {
        public Greater() : base(">", 10) { }
        public Greater(Expression left, Expression right) : base(left, right, ">", 10) { }

        public override Expression Evaluate()
        {
            return left > right;
        }
    }

    public class GreaterOrEqual : Operator
    {
        public GreaterOrEqual() : base(">=", 10) { }
        public GreaterOrEqual(Expression left, Expression right) : base(left, right, ">=", 10) { }

        public override Expression Evaluate()
        {
            return left >= right;
        }
    }
}
