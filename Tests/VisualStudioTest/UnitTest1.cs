﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ast;

namespace VisualStudioTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var calculate = new Add(new Mul(new Integer(5), new Integer(5)), new Integer(5));

            Assert.AreEqual(new Integer(30).value, (calculate.Evaluate() as Integer).value);
        }

        [TestMethod]
        public void TestEvaluation()
        {
            var evaluator1 = new Evaluator();
            string[,] tests = {
                                  {"sqrt(2)*sqrt(2)","2"}
                              };


                              
            for (int i = 0; i < tests.GetLength(0); i++) 
            {
                Assert.AreEqual(tests[i, 1], evaluator1.Evaluation(tests[i, 0]).ToString());
            }
        }

        [TestMethod]
        public void FunctionParsing()
        {
            var evaluator1 = new Evaluator();

            Assert.AreEqual("sin(30)", Parser.Parse("sin(30)").ToString());
        }

        [TestMethod]
        public void Simplify()
        {
            var evaluator1 = new Evaluator();
            var testkvat = new Add(new Add(new Exp(new Symbol(evaluator1, "z"), new Integer(2)), new Exp(new Symbol(evaluator1, "y"), new Integer(2))), new Mul(new Integer(2), new Mul(new Symbol(evaluator1, "x"), new Symbol(evaluator1, "y"))));
            var teststring = "x*x";

            string prev = "";
            Expression test = Parser.Parse(teststring);
            Expression current = test;

            do
	        {
	            prev = current.ToString();
                current = current.Simplify();
	        } while (prev != current.ToString());

            Assert.AreEqual(test.ToString(), test.Simplify().ToString());
        }
    }
}
