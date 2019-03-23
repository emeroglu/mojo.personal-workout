using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Abstract.Json
{
    public class JsonFriendRequest : JsonAny
    {
        public string name { get; set; }
        public string picture { get; set; }
    }
}
