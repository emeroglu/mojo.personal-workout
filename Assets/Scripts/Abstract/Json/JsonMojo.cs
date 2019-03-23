using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Abstract.Json
{
    public class JsonMojo : JsonAny
    {        
        public JsonUser user { get; set; }        
        public JsonCharacter character { get; set; }        
        public JsonSoundtrack soundtrack { get; set; }
        public string message { get; set; }
        public string date { get; set; }
        public bool viewed { get; set; }
    }
}
