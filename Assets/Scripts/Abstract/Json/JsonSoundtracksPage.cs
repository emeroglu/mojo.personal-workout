using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Abstract.Json
{
    public class JsonSoundtracksPage
    {
        public List<JsonSoundtrackGroup> soundtrackGroups { get; set; }
    }

    public class JsonSoundtrackGroup
    {
        public string heading { get; set; }
        public List<JsonSoundtrack> soundtracks { get; set; }
    }
}