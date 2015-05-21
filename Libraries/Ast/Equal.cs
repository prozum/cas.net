﻿using System;

namespace Ast
{
    public class Equal : BinaryOperator
    {
        public override string Identifier { get { return "="; } }
        public override int Priority { get{ return 0; } }

        public Equal() { }
        public Equal(Expression left, Expression right) : base(left, right) { }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new Equal(Left.Reduce(this), Right.Reduce(this));
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Equal(Left.Expand(), Right.Expand());
        }

        public override Expression Clone()
        {
            return new Equal(Left.Clone(), Right.Clone());
        }

        internal override Expression CurrectOperator()
        {
            return new Equal(Left.CurrectOperator(), Right.CurrectOperator());
        }
    }
}

