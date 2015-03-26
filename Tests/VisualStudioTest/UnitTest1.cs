using System;
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

            evaluator1.definitions.Add("x", new Integer(5));

            Assert.AreEqual("10", (evaluator1.Evaluation("x+5") as Integer).ToString());
        }
    }
}
