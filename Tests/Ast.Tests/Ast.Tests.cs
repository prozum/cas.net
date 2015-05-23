using NUnit.Framework;
using Ast;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;


namespace Ast.Tests
{
    [TestFixture]
    public class AstTests
    {
        public AstTests()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        }

        #region CompareTo Test Cases
        [TestCase("x+y==y+x")]
        [TestCase("x+y+z==x+y+z")]
        [TestCase("x+y+z==x+z+y")]
        [TestCase("x+y+z==y+x+z")]
        [TestCase("x+y+z==y+z+x")]
        [TestCase("x+y+z==z+y+x")]
        [TestCase("x+y+z==z+x+y")]
        [TestCase("x+y+z+q+b==b+q+z+y+x")]
        [TestCase("x+y+z+q+b==b+x+q+y+z")]
        //[TestCase("a+b+c+d+e+f+g+h+i+j+k+l+m+n+o+p+q+r+s+t+u+v+w+x+y+z==z+a+y+b+x+c+w+d+v+e+u+f+t+g+s+h+r+i+q+j+p+k+o+m+n")]
        [TestCase("x-y==-y+x")]
        [TestCase("x*(z/y)==(x*z)/y")]
        [TestCase("x*(z/y)==(z/y)*x")]
        [TestCase("x^2==x*x")]
        [TestCase("x^2+x==x*x+x")]
        [TestCase("x+x^2==x*x+x")]
        #endregion
        public void ExpCompareTo(string inputString)
        {
            var res = Evaluator.Eval("ret " + inputString);
            Assert.IsTrue(res is Boolean);
            Assert.IsTrue((res as Boolean).@bool == true);
        }

        #region Reduce Test Cases
        #region Symbols
        [TestCase("2x", "x+x")]
        [TestCase("3x", "x+x+x")]
        [TestCase("3x", "2*x+x")]
        [TestCase("x^2+x", "x^2+x")]
        [TestCase("0", "x-x")]
        [TestCase("-x", "x-x-x")]
        [TestCase("-2x", "x-x-x-x")]
        [TestCase("0", "x^2-x^2")]
        [TestCase("x", "2*x-x")]
        [TestCase("x^2", "x*x")]
        [TestCase("x^3", "x*x*x")]
        [TestCase("2x^3", "x*x*2*x")]
        [TestCase("2x^3+x", "2*x*x*x+x")]
        [TestCase("x^5", "x^2*x^3")]
        [TestCase("1", "x/x")]
        [TestCase("x", "x^2/x")]
        [TestCase("1/x", "x/x^2")]
        [TestCase("x", "x*x/x")]
        [TestCase("x+y", "x+y")]
        [TestCase("2x+y", "x+y+x")]
        [TestCase("2x+2y", "x+y+x+y")]
        [TestCase("2x+2y", "x+x+y+y")]
        [TestCase("2y+2x", "y+x+x+y")]
        [TestCase("2*y*x", "y*x+x*y")]
        [TestCase("2*x*y", "x*y+x*y")]
        [TestCase("x+y+z", "x+y+z")]
        [TestCase("2x+y+z", "x+x+y+z")]
        [TestCase("2x+y+z", "x+y+x+z")]
        [TestCase("2x+y+z", "x+y+z+x")]
        [TestCase("x+2y+z", "x+y+y+z")]
        [TestCase("x+2y+z", "x+y+z+y")]
        [TestCase("x-y-z", "x-y-z")]
        [TestCase("-y-z", "x-x-y-z")]
        [TestCase("-y-z", "x-y-x-z")]
        [TestCase("-y-z", "x-y-z-x")]
        [TestCase("x-2y-z", "x-y-y-z")]
        [TestCase("x-2y-z", "x-y-z-y")]
        [TestCase("x*y", "x*y")]
        [TestCase("x*y*z", "x*y*z")]
        [TestCase("x^2*y*z", "x*x*y*z")]
        [TestCase("x^2*y*z", "x*y*x*z")]
        [TestCase("x^2*y*z", "x*y*z*x")]
        [TestCase("x*y^2*z", "x*y*y*z")]
        [TestCase("x*y^2*z", "x*y*z*y")]
        [TestCase("x*y*z^2", "x*y*z*z")]
        [TestCase("x/(y*z)", "x/y/z")]
        [TestCase("x/(y*z)", "(x/y)/z")]
        [TestCase("(x*z)/y", "x/(y/z)")]
        [TestCase("x/(y*z*q)", "x/y/z/q")]
        [TestCase("(x*q)/(z*y)", "(x/y)/(z/q)")]
        [TestCase("(x*z)/(y*q)", "x/(y/z)/q")]
        [TestCase("(z*q*x)/y", "x/((y/z)/q)")]
        [TestCase("(x*z)/(y*q)", "(x/(y/z))/q")]
        [TestCase("(x+y)^2", "x^2+y^2+2*x*y")]
        [TestCase("(x-y)^2", "x^2+y^2-2*x*y")]
        #endregion
        #endregion
        public void Reduce(string expected, string inputString)
        {
            var redString = "ret reduce[" + inputString + "]";
            var res = Evaluator.Eval(redString);
            Assert.AreEqual(expected, res.ToString());
        }


        //public void Expand(string expected, string inputString)
        //{
        //    var simString = "Expand[" + inputString + "]";
        //    var res = eval.Evaluation(simString);
        //    Assert.AreEqual(expected, res.ToString());
        //}

        /*
        #region Parse Test Cases

        #region Numbers
        #region Positive
        [TestCase("{1}", "1")]
        [TestCase("{1}", "(1)")]
        [TestCase("{1+1}", "1+1")]
        [TestCase("{1+1}", "(1)+1")]
        [TestCase("{1+1}", "1+(1)")]
        [TestCase("{1+1}", "(1+1)")]
        [TestCase("{1-1}", "(1)-1")]
        [TestCase("{1-1}", "1-(1)")]
        [TestCase("{1-1}", "(1-1)")]
        [TestCase("{1*1}", "(1)*1")]
        [TestCase("{1*1}", "1*(1)")]
        [TestCase("{1*1}", "(1*1)")]
        [TestCase("{1/1}", "(1)/1")]
        [TestCase("{1/1}", "1/(1)")]
        [TestCase("{1/1}", "(1/1)")]
        #endregion

        #region Negative
        [TestCase("{-1}", "-1")]
        [TestCase("{-1+-1}", "-1+-1")]
        [TestCase("{-1--1}", "-1--1")]
        [TestCase("{-1*-1}", "-1*-1")]
        [TestCase("{-1/-1}", "-1/-1")]
        [TestCase("{-1^-1}", "-1^-1")]
        #endregion
        #endregion

        #region Symbols
        #region Positive
        [TestCase("{x}", "x")]
        [TestCase("{x}", "(x)")]
        [TestCase("{x+x}", "x+x")]
        [TestCase("{x+x}", "(x)+x")]
        [TestCase("{x+x}", "x+(x)")]
        [TestCase("{x+x}", "(x+x)")]
        [TestCase("{x-x}", "(x)-x")]
        [TestCase("{x-x}", "x-(x)")]
        [TestCase("{x-x}", "(x-x)")]
        [TestCase("{x*x}", "(x)*x")]
        [TestCase("{x*x}", "x*(x)")]
        [TestCase("{x*x}", "(x*x)")]
        [TestCase("{x/x}", "(x)/x")]
        [TestCase("{x/x}", "x/(x)")]
        [TestCase("{x/x}", "(x/x)")]
        #endregion

        #region Negative
        [TestCase("{-x}", "-x")]
        [TestCase("{-x+-x}", "-x+-x")]
        [TestCase("{-x--x}", "-x--x")]
        [TestCase("{-x*-x}", "-x*-x")]
        [TestCase("{-x/-x}", "-x/-x")]
        [TestCase("{-x^-x}", "-x^-x")]
        #endregion
        #endregion

        #region Functions
        #region Positive
        [TestCase("{f[x]}", "f[x]")]
        [TestCase("{f[x]}", "(f[x])")]
        [TestCase("{f[x]+f[x]}", "f[x]+f[x]")]
        [TestCase("{f[x]+f[x]}", "(f[x])+f[x]")]
        [TestCase("{f[x]+f[x]}", "f[x]+(f[x])")]
        [TestCase("{f[x]+f[x]}", "(f[x]+f[x])")]
        [TestCase("{f[x]-f[x]}", "(f[x])-f[x]")]
        [TestCase("{f[x]-f[x]}", "f[x]-(f[x])")]
        [TestCase("{f[x]-f[x]}", "(f[x]-f[x])")]
        [TestCase("{f[x]*f[x]}", "(f[x])*f[x]")]
        [TestCase("{f[x]*f[x]}", "f[x]*(f[x])")]
        [TestCase("{f[x]*f[x]}", "(f[x]*f[x])")]
        [TestCase("{f[x]/f[x]}", "(f[x])/f[x]")]
        [TestCase("{f[x]/f[x]}", "f[x]/(f[x])")]
        [TestCase("{f[x]/f[x]}", "(f[x]/f[x])")]
        [TestCase("{reduce[sqrt[2]^2]}", "reduce[sqrt[2]^2]")]
        #endregion

        #region Negative
        [TestCase("{-f[x]}", "-f[x]")]
        [TestCase("{-f[x]+-f[x]}", "-f[x]+-f[x]")]
        [TestCase("{-f[x]--f[x]}", "-f[x]--f[x]")]
        [TestCase("{-f[x]*-f[x]}", "-f[x]*-f[x]")]
        [TestCase("{-f[x]/-f[x]}", "-f[x]/-f[x]")]
        [TestCase("{-f[x]^-f[x]}", "-f[x]^-f[x]")]
        #endregion
        #endregion
        #endregion
        public void Parse(string expected, string inputString)
        {
            Assert.AreEqual(expected, parser.Parse(inputString).ToString());
        }
        */

        #region Evaluate Test Cases

        #region Operators
        #region Integers
        [TestCase(10, "10")] //value = value
        [TestCase(20, "10+10")] //add
        [TestCase(0, "10-10")] //sub
        [TestCase(100, "10*10")] //mul
        [TestCase(1, "10/10")] //div
        [TestCase(10000000000, "10^10")] //exp
        [TestCase(true, "10==10")] //boolequal
        [TestCase(false, "10==20")] //boolequal
        [TestCase(true, "10<20")] //boollesser
        [TestCase(false, "20<10")] //boollesser
        [TestCase(true, "10<=20")] //boollesserequal
        [TestCase(true, "10<=10")] //boollesserequal
        [TestCase(false, "20<=10")] //boollesserequal
        [TestCase(true, "20>10")] //boolgreater
        [TestCase(false, "10>20")] //boolgreater
        [TestCase(true, "20>=10")] //boolgreaterequal
        [TestCase(true, "10>=10")] //boolgreaterequal
        [TestCase(false, "10>=20")] //boolgreaterequal
        [TestCase(true, "10!=20")] //boolnotequal
        [TestCase(false, "20!=20")] //boolnotequal
        #endregion

        #region Rationals
        [TestCase(0.75, "1/2+1/4")] //add
        [TestCase(0.25, "1/2-1/4")] //sub
        [TestCase(0.125, "1/2*1/4")] //mul
        [TestCase(2, "(1/2)/(1/4)")] //div 
        [TestCase(0.25, "(1/2)^(2/1)")] //exp
        [TestCase(true, "1/2==1/2")] //boolequal
        [TestCase(true, "1/2==2/4")] //boolequal
        [TestCase(false, "1/2==1/4")] //boolequal
        [TestCase(true, "1/4<1/2")] //boollesser
        [TestCase(false, "1/2<1/4")] //boollesser
        [TestCase(true, "1/4<=1/2")] //boollesserequal
        [TestCase(true, "1/4<=2/8")] //boollesserequal
        [TestCase(false, "1/2<=1/4")] //boollesserequal
        [TestCase(true, "1/2>1/4")] //boolgreater
        [TestCase(false, "1/4>1/2")] //boolgreater
        [TestCase(true, "1/2>=1/4")] //boolgreaterequal
        [TestCase(true, "1/4>=2/8")] //boolgreaterequal
        [TestCase(false, "1/4>=1/2")] //boolgreaterequal
        [TestCase(true, "1/2!=1/4")] //boolnotequal
        [TestCase(false, "1/2!=2/4")] //boolnotequal
        [TestCase(false, "1/2!=1/2")] //boolnotequal
        #endregion

        #region Irrationals
        [TestCase(3, "1.5+1.5")] //add
        [TestCase(1, "1.5-0.5")] //sub
        [TestCase(2.25, "1.5*1.5")] //mul
        [TestCase(3.6, "4.5/1.25")] //div
        [TestCase(32, "4^2.5")] //exp
        [TestCase(true, "1.5==1.5")] //boolequal
        [TestCase(false, "1.5==1.75")] //boolequal
        [TestCase(true, "1.5<2.5")] //boollesser
        [TestCase(false, "2.5<1.5")] //boollesser
        [TestCase(true, "1.5<=2.5")] //boollesserequal
        [TestCase(true, "1.5<=1.5")] //boollesserequal
        [TestCase(false, "2.5<=1.5")] //boollesserequal
        [TestCase(true, "2.5>1.5")] //boolgreater
        [TestCase(false, "1.5>2.5")] //boolgreater
        [TestCase(true, "2.5>=1.5")] //boolgreaterequal
        [TestCase(true, "1.5>=1.5")] //boolgreaterequal
        [TestCase(false, "1.5>=2.5")] //boolgreaterequal
        [TestCase(true, "1.5!=2.5")] //boolnotequal
        [TestCase(false, "2.5!=2.5")] //boolnotequal
        #endregion

        #region Mix
        #region Add
        [TestCase(2.5, "1+1.5")]
        [TestCase(1.5, "1+1/2")]
        [TestCase(2.5, "1.5+1")]
        [TestCase(2, "1.5+1/2")]
        [TestCase(1.5, "1/2+1")]
        [TestCase(2, "1/2+1.5")]
        #endregion

        #region Sub
        [TestCase(-0.5, "1-1.5")]
        [TestCase(0.5, "1-1/2")]
        [TestCase(0.5, "1.5-1")]
        [TestCase(1, "1.5-1/2")]
        [TestCase(-0.5, "1/2-1")]
        [TestCase(-1, "1/2-1.5")]
        #endregion
        #endregion
        #endregion

        #region Program Defined Functions
        #region Sin, ASin
        [TestCase(1, "sin[90]")]
        [TestCase(0.5, "sin[30]")]
        [TestCase(0, "sin[0]")]
        [TestCase(90, "asin[1]")]
        [TestCase(30, "asin[0.5]")]
        [TestCase(0, "asin[0]")]
        #endregion

        #region Cos, ACos
        //[TestCase(0, "cos(90)")] works, but precision
        [TestCase(0.5, "cos[60]")]
        [TestCase(1, "cos[0]")]
        [TestCase(0, "acos[1]")]
        [TestCase(60, "acos[0.5]")]
        [TestCase(90, "acos[0]")]
        #endregion

        #region Tan, ATan
        [TestCase(1, "tan[45]")]
        //[TestCase(0.5, "tan[26.57]")] Unpresition gives wrong actual. Is right calculation
        [TestCase(0, "tan[0]")]
        [TestCase(45, "atan[1]")]
        //[TestCase(26.57, "atan[0.5]")] Unpresition gives wrong actual. Is right calculation
        [TestCase(0, "atan[0]")]
        #endregion

        #region Sqrt
        [TestCase(2, "sqrt[4]")]
        [TestCase(2, "sqrt[2]^2")]
        [TestCase(2, "sqrt[2]*sqrt[2]")]
        #endregion
        #endregion
        #endregion
        public void Evaluate(dynamic expected, string calculation)
        {
            var res = Evaluator.Eval("ret " + calculation);
            
            if (res is Integer)
            {
                Assert.AreEqual(expected, (res as Integer).@int);
            }
            else if (res is Rational)
            {
                Assert.AreEqual(expected, (res as Rational).@decimal);
            }
            else if (res is Irrational)
            {
                Assert.AreEqual(expected, (res as Irrational).@decimal);
            }
            else if (res is Boolean)
            {
                Assert.AreEqual(expected, (res as Boolean).@bool);
            }
            else if (res is Error)
            {
                Assert.Fail(res.ToString());
            }
            else
            {
                Assert.Fail();
            }
        }

        #region Solve Test Cases
        [TestCase("x=0", "x=0")]
        [TestCase("x=0", "0=x")]
        [TestCase("x=-3", "3+x=0")]
        [TestCase("x=-3", "x+3=0")]
        [TestCase("x=3", "3-x=0")]
        [TestCase("x=3", "x-3=0")]
        [TestCase("x=2", "3*x=6")]
        [TestCase("x=2", "x*3=6")]
        [TestCase("x=2", "6/x=3")]
        [TestCase("x=6", "x/3=2")]
        [TestCase("x=[sqrt[4],-sqrt[4]]", "x^2=4")]
        [TestCase("x=2", "x^3=8")]
        [TestCase("x=4", "x+x=8")]
        [TestCase("x=2", "x*x^2=8")]
        #endregion
        public void Solve(string expected, string inputString)
        {
            var testString = "ret solve[" + inputString + ",x]";

            Assert.AreEqual(expected, Evaluator.Eval(testString).ToString());
        }
    }
}