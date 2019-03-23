using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Abstract.Tools
{
    public class OnTouchListener
    {
        public string Owner { get; set; }

        public string Target { get; set; }

        public Action<GameObject> OnTouch { get; set; }
        public Action<GameObject> OnTouching { get; set; }
        public Action<GameObject> OnRelease { get; set; }

        public bool Enabled { get; set; }
        public bool Released { get; set; }

        public bool Remove { get; set; } 
    }
}
