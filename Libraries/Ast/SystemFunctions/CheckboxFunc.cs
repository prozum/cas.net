using System;
using System.Collections.Generic;

namespace Ast
{
    public class CheckboxFunc : SysFunc
    {
        public CheckboxFunc() : this(null) { }
        public CheckboxFunc(Scope scope) : base("checkbox", scope)
        {
            ValidArguments = new List<ArgumentType>()
                {
                    ArgumentType.Text,
                    ArgumentType.Variable
                };
        }

        public override Expression Call(List args)
        {
            Text text = (Text)args[0];
            Variable @var = (Variable)args[1];

            @var.CurScope = CurScope;

            CurScope.SideEffects.Add(new CheckboxData(text, @var));
            return Constant.Null;
        }
    }
}

