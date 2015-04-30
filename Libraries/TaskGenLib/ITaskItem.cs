using System;
using Ast;

namespace TaskGenLib
{
    public interface ITaskItem
    {
        string TaskDescription { get; }
        Expression Solution { get; }
    }
}

