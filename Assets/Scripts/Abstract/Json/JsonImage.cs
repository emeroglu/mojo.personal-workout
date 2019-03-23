using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Repository;
using UnityEngine;

namespace Assets.Scripts.Abstract.Json
{
    public class JsonImage
    {
        public int pk { get; set; }
        public string suffix { get; set; }
        public string url { get; set; }
        public Texture texture { get; set; }
        public bool loading { get; set; }
    }    
}
