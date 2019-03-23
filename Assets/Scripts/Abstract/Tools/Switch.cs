using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Abstract.Tools
{
    public class Switch
    {
        public int ID { get; set; }
        public bool Enabled { get; set; }

        public int Cycle { get; set; }
        public int Limit { get; set; }

        public Func<bool> Condition { get; set; }

        public Action<Switch> OnAnimate { get; set; }
        public Action OnFinish { get; set; }
    }
}
