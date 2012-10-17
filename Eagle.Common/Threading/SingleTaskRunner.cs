using System;
using System.Threading.Tasks;

namespace Eagle.Common.Threading
{
    public class SingleTaskRunner
    {
        private readonly Action _action;
        private readonly object _sync = new object();
        private Task _runningTask;
        private bool _newExecutionTriggered;
        private bool _stopped;

        /// <summary>
        /// Initializes a new instance of the SingleTaskRunner class.
        /// </summary>
        public SingleTaskRunner(Action action)
        {
            _action = action;
        }

        public void TriggerExecution()
        {
            lock (_sync)
            {
                if (_runningTask != null)
                {
                    _newExecutionTriggered = true;
                }
                else
                {
                    _newExecutionTriggered = false;
                    _runningTask = Task.Run(new Action(RunAction));
                }
            }
        }

        public void Start()
        {
            _stopped = false;
        }

        public Task Stop()
        {
            _stopped = true;
            lock (_sync)
            {
                if (_runningTask != null)
                    return _runningTask;
            }

            return Task.FromResult<object>(null);
        }

        private void RunAction()
        {
            try
            {
                while (!_stopped)
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
                _runningTask = null;
            }
        }
    }
}
