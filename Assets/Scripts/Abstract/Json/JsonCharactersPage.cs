using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Abstract.Json
{
    public class JsonCharactersPage
    {
        public List<JsonCharacterGroup> characterGroups { get; set; }
    }

    public class JsonCharacterGroup
    {
        public string heading { get; set; }
        public List<JsonCharacter> characters { get; set; }
    }
}