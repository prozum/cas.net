using NUnit.Framework;
using Ast;
using System.Collections.Generic;

namespace Ast.Tests
{
    [TestFixture]
    public class AstTests
    {
        public Evaluator eval;
        public Parser parser;

        public AstTests()
        {
            eval = new Evaluator ();
            parser = new Parser(eval);
        }

        [Test]
        public void Parse()
        {
            Expression res;

            string[,] testStrings =
            {
                {"(x*y)*f(10-x)-20", "x*y*f(10-x)-20"},
                {"x*10-20/x", "x*10-20/x"},
                {"f(x,y,z)=x/y*z", "f(x,y,z)=x/y*z"},
                {"x==y", "x==y"},
                {"x:=y", "x:=y"},
                {"x+x+4*5+x+x+x+x+x", "x+x+4*5+x+x+x+x+x"},
                {"(x+x)*(y+y)","(x+x)*(y+y)"},
                {"sin(sqrt(90))", "sin(sqrt(90))"},
                {"x + x + y", "x+x+y"}
            };

            for (int i = 0; i < testStrings.GetLength(0); i++) 
            {
                res = parser.Parse(testStrings[i,0]);
                Assert.AreEqual (testStrings[i,1], res.ToString());
            }
        }

        [Test]
        public void Simplify()
        {
            Expression res;

            string[,] testStrings =
            {
                {"x+x+x+x", "4x"},
                {"x+y+x+y", "2x+2y"},
                {"x-x-x", "-x"},
                {"x*x*x","x^3"},
                {"x+x+x-y-y-y-y","3x+-2y"}
            };

            for (int i = 0; i < testStrings.GetLength(0); i++)
            {
                res = Evaluator.SimplifyExp(parser.Parse(testStrings[i, 0]));
                Assert.AreEqual(testStrings[i, 1], res.ToString());
            }
        }

        [Test]
        public void ParseNumber()
        {
            Expression res;

            string[] testStrings = {"10.10"};

            foreach (string testString in testStrings) 
            {
                //res = parser.ParseNumber(new System.IO.StringReader(testString));
                //Assert.AreEqual (testString, res.ToString());
            }
        }

        #region Operators
        #region Integers
        [TestCase("10", "10")] //value = value
        [TestCase("20", "10+10")] //add
        [TestCase("0", "10-10")] //sub
        [TestCase("100", "10*10")] //mul
        [TestCase("1", "10/10")] //div
        [TestCase("10000000000", "10^10")] //exp
        [TestCase("True", "10==10")] //boolequal
        [TestCase("False", "10==20")] //boolequal
        [TestCase("True", "10<20")] //boollesser
        [TestCase("False", "20<10")] //boollesser
        [TestCase("True", "10<=20")] //boollesserequal
        [TestCase("True", "10<=10")] //boollesserequal
        [TestCase("False", "20<=10")] //boollesserequal
        [TestCase("True", "20>10")] //boolgreater
        [TestCase("False", "10>20")] //boolgreater
        [TestCase("True", "20>=10")] //boolgreaterequal
        [TestCase("True", "10>=10")] //boolgreaterequal
        [TestCase("False", "10>=20")] //boolgreaterequal
        #endregion

        #region Rationals
        [TestCase("7/12", "1/3+1/4")] //add
        [TestCase("1/12", "1/3-1/4")] //sub
        [TestCase("1/2", "2/3*3/4")] //mul
        [TestCase("8/9", "(2/3)/(3/4)")] //div 
        [TestCase("1/4", "(1/2)^(2/1)")] //exp
        [TestCase("True", "1/2==1/2")] //boolequal
        [TestCase("True", "1/2==2/4")] //boolequal
        [TestCase("False", "1/2==1/4")] //boolequal
        [TestCase("True", "1/4<1/2")] //boollesser
        [TestCase("False", "1/2<1/4")] //boollesser
        [TestCase("True", "1/4<=1/2")] //boollesserequal
        [TestCase("True", "1/4<=2/8")] //boollesserequal
        [TestCase("False", "1/2<=1/4")] //boollesserequal
        [TestCase("True", "1/2>1/4")] //boolgreater
        [TestCase("False", "1/4>1/2")] //boolgreater
        [TestCase("True", "1/2>=1/4")] //boolgreaterequal
        [TestCase("True", "1/4>=2/8")] //boolgreaterequal
        [TestCase("False", "1/4>=1/2")] //boolgreaterequal
        #endregion

        #region Irrationals
        [TestCase("2.02", "1.01+1.01")] //add
        [TestCase("1.01", "1.03-0.02")] //sub
        [TestCase("1.0201", "1.01*1.01")] //mul
        [TestCase("3.6", "4.50/1.25")] //div
        [TestCase("32", "4^2.5")] //exp
        [TestCase("True", "1.01==1.01")] //boolequal
        [TestCase("False", "1.01==1.010001")] //boolequal
        [TestCase("True", "1.01<1.02")] //boollesser
        [TestCase("False", "1.02<1.01")] //boollesser
        [TestCase("True", "1.01<=1.02")] //boollesserequal
        [TestCase("True", "1.01<=1.01")] //boollesserequal
        [TestCase("False", "1.02<=1.01")] //boollesserequal
        [TestCase("True", "1.02>1.01")] //boolgreater
        [TestCase("False", "1.01>1.02")] //boolgreater
        [TestCase("True", "1.02>=1.01")] //boolgreaterequal
        [TestCase("True", "1.02>=1.02")] //boolgreaterequal
        [TestCase("False", "1.01>=1.02")] //boolgreaterequal
        #endregion
        #endregion

        #region Program Defined Functions

        #endregion

        [TestCase("202.00", "10.10*20")] //
        [TestCase("256", "2^8")]
        [TestCase("4611686018427387904", "2^62")]
        [TestCase("0.999390827019096", "cos(2)")]
        public void Evaluate(string expected, string calculation)
        {
            Assert.AreEqual(expected, eval.Evaluation(calculation).ToString());
        }
    }
}

