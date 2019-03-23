using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Repository
{
    public static class MonoBridge
    {
        public static Action<IEnumerator> StartCoroutine;
        public static Func<string, GameObject, GameObject> Instantiate;
        public static Action<GameObject> Destroy;
        public static Action<Component> DestroyComponent;
    }
}
