using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ast
{
    public class BooleanEqual : Operator
    {
        public BooleanEqual() : this(null, null) { }
        public BooleanEqual(Expression left, Expression right) : base(left, right)
        {
            symbol = "==";
            priority = 10;
        }

        public override Expression Evaluate()
        {
            return new Boolean(left.CompareTo(right));
        }
    }

    public class Lesser : Operator
    {
        public Lesser() : this(null, null) { }
        public Lesser(Expression left, Expression right) : base(left, right)
        {
            symbol = "<";
            priority = 10;
        }

        public override Expression Evaluate()
        {
            return left < right;
        }
    }

    public class LesserOrEqual : Operator
    {
        public LesserOrEqual() : this(null, null) { }
        public LesserOrEqual(Expression left, Expression right) : base(left, right)
        {
            symbol = "<=";
            priority = 10;
        }

        public override Expression Evaluate()
        {
            return left <= right;
        }
    }

    public class Greater : Operator
    {
        public Greater() : this(null, null) { }
        public Greater(Expression left, Expression right) : base(left, right)
        {
            symbol = ">";
            priority = 10;
        }

        public override Expression Evaluate()
        {
            return left > right;
        }
    }

    public class GreaterOrEqual : Operator
    {
        public GreaterOrEqual() : this(null, null) { }
        public GreaterOrEqual(Expression left, Expression right) : base(left, right)
        {
            symbol = ">=";
            priority = 10;
        }

        public override Expression Evaluate()
        {
            return left >= right;
        }
    }
}
