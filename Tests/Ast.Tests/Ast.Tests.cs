using NUnit.Framework;
using System;
using Ast;

namespace ExtractSubstring
{
	[TestFixture]
	public class ExtractSubstringTest
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
	}
}

