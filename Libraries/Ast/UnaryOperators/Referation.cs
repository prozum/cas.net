﻿namespace Ast
{
    public class Referation : PrefixOperator
    {
        public Referation() : base("~") { }

        public override Expression Value
        {
            get
            {
                return Child.Value;
            }
            set
            {
                Child.Value = value;
            }
        }

        public override Expression Evaluate()
        {
            return Child.Value;
        }
    }
}

