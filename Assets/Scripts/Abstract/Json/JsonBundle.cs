using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Repository;
using UnityEngine;

namespace Assets.Scripts.Abstract.Json
{
    public class JsonBundle
    {
        public int pk { get; set; }
        public BundleType type { get; set; }
        public AssetBundle bundle { get; set; }
    }    
}
