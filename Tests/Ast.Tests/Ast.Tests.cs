using NUnit.Framework;
using Ast;
using System.Collections.Generic;

namespace Ast.Tests
{
    [TestFixture]
    public class AstTests
    {
        [Test]
        public void Parse()
        {
            Expression res;

            string[,] testStrings = {
                {"(x*y)*f(10-x)-20", "x*y*f(10-x)-20"},
                {"x*10-20/x", "x*10-20/x"},
                {"f(x,y,z)=x/y*z", "f(x,y,z)=x/y*z"},
                {"x==y", "x==y"},
                {"x:=y", "x:=y"},
                {"x+x+4*5+x+x+x+x+x", "x+x+4*5+x+x+x+x+x"}
            };

            for (int i = 0; i < testStrings.GetLength(0); i++) {

                res = Ast.Parser.Parse(testStrings[i,0]);
                Assert.AreEqual (testStrings[i,1], res.ToString());
            
            }
        }

        [Test]
        public void ParseNumber()
        {
            Expression res;

            string[] testStrings = {"10.10"};

            foreach (string testString in testStrings) {

                res = Ast.Parser.ParseNumber(new System.IO.StringReader(testString));
                Assert.AreEqual (testString, res.ToString());

            }
        }

        [Test]
        public void Evaluate()
        {
            Expression res;

            string[,] testStrings = {
                {"10.10*20", "202.00"},
                {"10", "10"},
                {"2^8", "256"},
                {"cos(2)", "0.999390827019096"},
                {"4^40000/0", "fug"},
                {"sqrt(2)*sqrt(2)", "2"},
                {"(1/9)*9", "9"}

            };

            for (int i = 0; i < testStrings.GetLength(0); i++) {

                res = Ast.Parser.Parse(testStrings[i,0]);
                Assert.AreEqual (testStrings[i,1], res.Evaluate().ToString());

            }
        }

    }
}

