using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.ViewModels
{
    public class StateCaptureContext : IStateCaptureContext
    {
        private readonly Dictionary<string, object> _states = new Dictionary<string, object>();

        public void SaveState(string key, object state)
        {
            States[key] = state;
        }

        public Dictionary<string, object> States
        {
            get { return _states; }
        }
    }
}
