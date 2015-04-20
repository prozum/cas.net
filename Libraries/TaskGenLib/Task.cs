using System;

namespace TaskGenLib
{
    public class Task : ITaskItem
    {
        #region ITaskItem implementation

        public string TaskDescription {
            get;
            private set;
        }

        public string Solution {
            get;
            private set;
        }

        #endregion

        public Task (string taskDescription, string solution)
        {
            TaskDescription = taskDescription;
            Solution = solution;
        }
    }
}

