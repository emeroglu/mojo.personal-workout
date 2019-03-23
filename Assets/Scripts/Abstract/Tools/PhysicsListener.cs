using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Abstract.Tools
{
    public class PhysicsListener
    {
        public string Name { get; set; }

        public List<string> Objects { get; set; }

        public Action<PhysicsListener,string> OnFired { get; set; }

        public bool Enabled { get; set; }
    }
}
