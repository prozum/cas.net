using System;
using Ast;

namespace TaskGenLib
{
    public class Task : ITaskItem
    {
        #region ITaskItem implementation

        public string TaskDescription {
            get;
            private set;
        }

        public Expression Solution {
            get;
            private set;
        }

        #endregion

        public Task (string taskDescription, Expression solution)
        {
            TaskDescription = taskDescription;
            Solution = solution;
        }
    }
}

