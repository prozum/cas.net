using NUnit.Framework;
using System;
using Parser;

namespace Parser.Tests
{
	[TestFixture ()]
	public class Test
	{
		[Test ()]
		public void TestCase ()
		{
			var scan = new Scanner ();

			var l = scan.Tokenize("x455 * x * x");
			var s = l.ToString ();
			//res = Evaluator.SimplifyExp(parser.Parse(testStrings[i, 0]));
			//Assert.AreEqual(testStrings[i, 1], res.ToString());

		}
	}
}

