using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Abstract.Json
{
    public class JsonMainPage
    {
        public string characterHeading { get; set; }
        public List<JsonCharacter> characters { get; set; }

        public string soundtrackHeading { get; set; }
        public List<JsonSoundtrack> soundtracks { get; set; }
    }
}
