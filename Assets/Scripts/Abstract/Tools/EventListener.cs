using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Abstract.Tools
{
    public class EventListener
    {
        public string Owner { get; set; }
        public string Name { get; set; }

        public Func<bool> Event { get; set; }

        public Action<EventListener> OnFired { get; set; }
        public Action<EventListener> OnMiss { get; set; }
        public Action<EventListener> OnEither { get; set; }        
        public Action OnExit { get; set; }

        public bool Enabled { get; set; }
        public bool Remove { get; set; }
    }
}
