using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Abstract.Json
{
    public class JsonSoundtrack : JsonAny
    {
        public string type { get; set; }
        public string name { get; set; }
        public string belongsTo { get; set; }

        public JsonAnimator animator { get; set; }
        public JsonSkybox skybox { get; set; }
        public JsonEnvironment environment { get; set; }

        public JsonVector position { get; set; }
        public JsonVector rotation { get; set; }
        public JsonVector scale { get; set; }

        public int length { get; set; }
        public bool loop { get; set; }
        public bool touch { get; set; }

        public string lastUpdate { get; set; }
    }

    public class JsonVector
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
    }
}
