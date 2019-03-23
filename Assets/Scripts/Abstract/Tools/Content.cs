using System;
using Assets.Scripts.Repository;
using UnityEngine;

namespace Assets.Scripts.Abstract.Tools
{
    public class Content
    {
        public int PK { get; set; }

        public AssetBundle Bundle { get; set; }
        public GameObject Model { get; set; }

        public bool Downloaded { get; set; }
        public bool Retrieved { get; set; }

        public DateTime LastUpdate { get; set; }                         
    }
}
