using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Json;
using UnityEngine;

namespace Assets.Scripts.Repository
{
    public static class Cache
    {
        public static Dictionary<string, string> Data;

        public static List<JsonBundle> Bundle_Cache;
        public static List<JsonAudioClip> Audio_Cache;

        public static List<JsonImage> Image_Cache;
        
        public static List<JsonCache> Avatar_Cache;
        public static List<JsonCache> Music_Cache;
        public static List<JsonCache> Animator_Cache;
        public static List<JsonCache> Skybox_Cache;
        public static List<JsonCache> Environment_Cache;
    }
}
