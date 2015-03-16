using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ast
{
	public class Expression 
	{
		public bool Contains(Expression e)
		{
			if (e is Symbol) {
				return true;
			} else {
				return false;
			}
		}
	}
}