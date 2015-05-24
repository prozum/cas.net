using System;

namespace Ast
{
    public static class Constant
    {
        // these are used so many times for simplifying the expressions that they are just made constants
        public static readonly Integer MinusOne = new Integer(-1);
        public static readonly Integer Zero = new Integer(0);
        public static readonly Integer One = new Integer(1);
        public static readonly Integer Two = new Integer(2);

        public static readonly Null Null = new Null();
        
        public static readonly Irrational DegToRad = new Irrational(Math.PI / 180);
        public static readonly Irrational RadToDeg = new Irrational(180 / Math.PI);
    }
}

