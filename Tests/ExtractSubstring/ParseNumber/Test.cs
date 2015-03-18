using NUnit.Framework;
using System;
using Ast;

namespace ParseNumber
{
	[TestFixture ()]
	public class ParseNumberTest
	{
		[Test ()]
		public void ParseNumber ()
		{	
			var testString = "2121";
			Integer test = new Integer (2);

			var testEnum = testString.GetEnumerator ();

			testEnum.MoveNext ();

			var res = Ast.Parser.ParseNumber (testEnum);
			Assert.AreEqual (test.GetType(), res.GetType());
			Assert.AreEqual (30, (res as Integer).value);
		}
	}
}


