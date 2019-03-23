using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Json;

namespace Assets.Scripts.UI.ListItems
{
    public class listSoundtracksItem : listItem
    {
        public string type { get; set; }
        public JsonSoundtrack soundtrack { get; set; }
        public string text { get; set; }
    }
}
