using System;
using Ast;

namespace TaskGenLib
{
    public interface ITaskItem
    {
        string TaskDescription { get; }
        string Solution { get; }
    }
}

