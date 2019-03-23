using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.UI.ListItems;

namespace Assets.Scripts.Repository
{
    public static class Conversions
    {
        public static Func<string, List<listMainItem>> Main;
        public static Func<string, List<listInboxItem>> Inbox;
        public static Func<string, List<listSoundtracksItem>> Soundtracks;
        public static Func<string, List<listCharactersItem>> Characters;
        public static Func<string, List<listFriendsItem>> Friend_Picker;
        public static Func<string, List<listFriendRequestsItem>> Friend_Requests;
        public static Func<string, List<listFriendsItem>> Friends;
    }
}
