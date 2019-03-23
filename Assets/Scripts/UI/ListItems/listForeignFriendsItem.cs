using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Json;

namespace Assets.Scripts.UI.ListItems
{
    public class listForeignFriendsItem : listItem
    {
        public JsonForeign user { get; set; }
    }
}
