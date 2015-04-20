using System;

namespace TaskGen
{
    public interface ITaskItem
    {
        string TaskDescription { get; }
        string Solution { get; }
    }
}

