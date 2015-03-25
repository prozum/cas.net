using NUnit.Framework;
using Ast;

namespace Ast.Tests
{
	[TestFixture]
	public class AstTests
	{
		[Test]
		public void Parse()
		{
			Expression res;

			string[] testStrings = {"x*y*f(10-x)-20","x*10-20/x","f(x,y,z)=x/y*z"};

			foreach (string testString in testStrings) {

				res = Ast.Parser.Parse(testString);
				Assert.AreEqual (testString, res.ToString());
			
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


	}
}

