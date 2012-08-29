using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Common.Threading
{
    public class SingleTaskRunner
    {
        private readonly Action _action;
        private readonly object _sync = new object();
        private bool _taskRunning;
        private bool _newExecutionTriggered;

        /// <summary>
        /// Initializes a new instance of the SingleTaskRunner class.
        /// </summary>
        public SingleTaskRunner(Action action)
        {
            _action = action;
        }

        public void TriggerExecution()
        {
            bool scheduleNewTask = false;
            lock (_sync)
            {
                if (_taskRunning)
                {
                    _newExecutionTriggered = true;
                }
                else
                {
                    _newExecutionTriggered = false;
                    _taskRunning = true;
                    scheduleNewTask = true;
                }
            }

            if (scheduleNewTask)
                Task.Run(new Action(RunAction));
        }

        private void RunAction()
        {
            try
            {
                while (true)
                {
                    _action();

                    lock (_sync)
                    {
                        if (_newExecutionTriggered)
                        {
                            _newExecutionTriggered = false;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            finally
            {
                _taskRunning = false;
            }
        }
    }
}
