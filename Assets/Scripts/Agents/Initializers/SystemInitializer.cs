
using Assets.Scripts.Abstract.Json;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Core;
using Assets.Scripts.Repository;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.UI.ListItems;
using Newtonsoft.Json;

namespace Assets.Scripts.Agents.Initializers
{
    public class SystemInitializer : CoreAgent<CoreMaterial>
    {
        public void Initialize()
        {
            Perform();
        }

        protected override void Job()
        {            
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;

            Input.compass.enabled = true;
            Input.gyro.enabled = true;

            Objects.Canvas = GameObject.Find("Canvas");
            Objects.Mojo_Camera = GameObject.Find("MojoCamera");
            Objects.ClickSource = GameObject.Find("ClickSource").GetComponent<AudioSource>();
            Objects.AudioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();

            Directives.App_First = false;
            Directives.App_Opened = false;
            Directives.App_Notification = false;

            Directives.Events_Initialized = false;            
            
            Variables.UI = new UIDictionary<string, UIComponent>();

            Variables.Current_Page = "pnlSplash";
            Variables.Current_Mojo = new JsonMojo();

            Variables.Keyboard_Text = "";

            Cache.Data = new Dictionary<string, string>();

            Cache.Bundle_Cache = new List<JsonBundle>();
            Cache.Audio_Cache = new List<JsonAudioClip>();

            Cache.Image_Cache = new List<JsonImage>();

            Cache.Skybox_Cache = new List<JsonCache>();
            Cache.Avatar_Cache = new List<JsonCache>();
            Cache.Music_Cache = new List<JsonCache>();
            Cache.Animator_Cache = new List<JsonCache>();
            Cache.Environment_Cache = new List<JsonCache>();

            Media_Fonts();
            Media_Colors();

            Styles_Main();
            Styles_Bars();
            Styles_Page();
            Styles_Inbox();
            Styles_About();
            Styles_Frame();

            Main_Conversion();
            Inbox_Conversion();
            Audios_Conversion();
            Avatars_Conversion();
            Friend_Picker_Conversion();
            Friend_Requests_Conversion();
            Friends_Conversion();
        }                                                       

        private void Media_Fonts()
        {
            Media.fontExoRegular = Resources.Load<Font>("Fonts/Exo 2.0/Exo2.0-Regular");
            Media.fontExoThin = Resources.Load<Font>("Fonts/Exo 2.0/Exo2.0-Thin");
            Media.fontExoLight = Resources.Load<Font>("Fonts/Exo 2.0/Exo2.0-Light");
            Media.fontExoExtraLight = Resources.Load<Font>("Fonts/Exo 2.0/Exo2.0-ExtraLight");
        }

        private void Media_Colors()
        {
            Media.colorOneThirdsTransparent = new Color32(255, 255, 255, 84);
            Media.colorSemiTransparent = new Color32(255, 255, 255, 128);
            Media.colorTwoThirdsTransparent = new Color32(255, 255, 255, 168);
            Media.colorTransparent = new Color32(255, 255, 255, 0);
            Media.colorOpaque = new Color32(255, 255, 255, 255);

            Media.colorBlackOneTenthTransparent = new Color32(0, 0, 0, 25);
            Media.colorBlackOneSixthTransparent = new Color32(0, 0, 0, 42);
            Media.colorBlackOneFifthTransparent = new Color32(0, 0, 0, 51);
            Media.colorBlackOneThirdsTransparent = new Color32(0, 0, 0, 84);
            Media.colorBlackSemiTransparent = new Color32(0, 0, 0, 128);
            Media.colorBlackTwoThirdsTransparent = new Color32(0, 0, 0, 168);
            Media.colorBlack = new Color32(0, 0, 0, 255);

            Media.colorGreyDark = new Color32(105, 105, 105, 255);
            Media.colorGrey = new Color32(156, 156, 156, 255);
            Media.colorGreyLight = new Color32(178, 178, 178, 255);
            Media.colorGreyExtraLight = new Color32(225, 225, 225, 255);
            Media.colorWhite = new Color32(241, 242, 242, 255);
        }

        private void Styles_Main()
        {
            Styles.Screen_Width = float.Parse(Screen.width.ToString());
            Styles.Screen_Height = float.Parse(Screen.height.ToString());
            Styles.Screen_Ratio = Styles.Screen_Width / Styles.Screen_Height;

            Styles.Screen_Width_One_Thirds = Styles.Screen_Width * 0.33f;
            Styles.Screen_Width_Quarter = Styles.Screen_Width * 0.25f;
            Styles.Screen_Width_Half = Styles.Screen_Width * 0.5f;
            Styles.Screen_Width_Two_Thirds = Styles.Screen_Width * 0.66f;
            Styles.Screen_Width_Three_Quarters = Styles.Screen_Width * 0.75f;
            Styles.Screen_Width_Four_Fifth = Styles.Screen_Width * 0.8f;
            Styles.Screen_Width_Five_Sixth = Styles.Screen_Width * (5f / 6f);

            Styles.Font_Size_Huge = 0.06f;
            Styles.Font_Size_Largest = 0.045f;
            Styles.Font_Size_Larger = 0.04f;
            Styles.Font_Size_Large = 0.0375f;
            Styles.Font_Size_Medium = 0.035f;
            Styles.Font_Size_Small = 0.0325f;
            Styles.Font_Size_Smaller = 0.03f;
            Styles.Font_Size_Smallest = 0.0285f;

            Styles.Default_Velocity = 0.1f;
        }

        private void Styles_Bars()
        {
            Styles.Width_Bar_Single = Styles.Screen_Width * 0.25f;
            Styles.Width_Bar_Wide = Styles.Screen_Width * 0.75f;

            Styles.Width_Indicator = Styles.Screen_Width * 0.33f;

            Styles.Height_Bar_Tall = Styles.Screen_Width * 0.15f;
            Styles.Height_Bar_Medium = Styles.Screen_Width * 0.125f;
            Styles.Height_Bar_Short = Styles.Screen_Width * 0.1f;

            Styles.Side_Tall_Bar_Icon = Styles.Height_Bar_Tall * 0.6f;
            Styles.Side_Medium_Bar_Icon = Styles.Height_Bar_Medium * 0.45f;
            Styles.Right_Medium_Bar_Icon = Styles.Screen_Width * 0.05f;

            Styles.Padding_For_Anything = Styles.Screen_Width * 0.025f;

            Styles.Width_List_Item = Styles.Screen_Width * 0.94f;
            Styles.Width_List_Item_Action_Narrow = Styles.Screen_Width * 0.15f;
            Styles.Width_List_Item_Action = Styles.Screen_Width * 0.2f;
            Styles.Width_List_Item_Action_Wide = Styles.Screen_Width * 0.25f;

            Styles.Height_List_Item = Styles.Screen_Width * 0.2f;
            Styles.Height_List_Item_Short = Styles.Screen_Width * 0.15f;
            Styles.Height_List_Item_Shorter = Styles.Screen_Width * 0.075f;

            Styles.Top_List_Item = Styles.Screen_Width * 0.01f;
        }

        private void Styles_Page()
        {
            Styles.Width_Page = Styles.Screen_Width * 0.94f;
            Styles.Width_Page_Wide = Styles.Screen_Width * 0.96f;
            Styles.Width_Page_Full = Styles.Screen_Width;

            Styles.Height_Page = Screen.height - Styles.Height_Bar_Medium - Styles.Height_Bar_Medium;            
            Styles.Height_Page_With_Nav_Bar = Styles.Screen_Height - Styles.Height_Bar_Medium;            

            Styles.Top_Page = Styles.Height_Bar_Medium;
        }

        private void Styles_Inbox()
        {    
            Styles.Side_Inbox_Mojo_Item_Profile = Screen.width * 0.125f;
            Styles.Left_Inbox_Mojo_Item_Profile = Screen.width * 0.035f;
            Styles.Width_Inbox_Mojo_Item_Username = Screen.width * 0.5f;
            Styles.Width_Inbox_Mojo_Item_Message = Screen.width * 0.5f;
            Styles.Top_Inbox_Mojo_Item_Message = Screen.width * 0.025f;
            Styles.Side_Inbox_Mojo_Item_Target = Screen.width * 0.2f;
        }

        private void Styles_About()
        {
            Styles.Side_About = Screen.width * 0.5f;
            Styles.Top_About = Screen.width * 0.25f;
        }

        private void Styles_Frame()
        {
            Styles.Side_Frame_Profile = Screen.width * 0.1f;
            Styles.Top_Frame_Profile = Screen.width * 0.03f;
            Styles.Left_Frame_Profile = Screen.width * 0.05f;
            Styles.Width_Frame_Message = Screen.width * 0.7f;
            Styles.Top_Frame_Message = Screen.width * 0.035f;
            Styles.Left_Frame_Message = Screen.width * 0.2f;            
        }

        private void Main_Conversion()
        {
            Conversions.Main = (json) =>
            {
                JsonMainPage page = JsonConvert.DeserializeObject<JsonMainPage>(json);
                List<listMainItem> items = new List<listMainItem>();

                items.Add(new listMainItem() { type = "heading", text = page.characterHeading });
                for (int i = 0;i < page.characters.Count;i += 2)
                {
                    items.Add
                    (
                        new listMainItem()
                        {
                            type = "character",
                            character = page.characters[i],
                            character2 = (i + 1 != page.characters.Count) ? page.characters[i + 1] : page.characters[i]
                        }
                    );
                }

                items.Add(new listMainItem() { type = "heading", text = page.soundtrackHeading });
                foreach (JsonSoundtrack audio in page.soundtracks)
                {
                    items.Add
                    (
                        new listMainItem()
                        {
                            type = "soundtrack",
                            soundtrack = audio
                        }
                    );
                }

                return items;
            };
        }

        private void Inbox_Conversion()
        {
            Conversions.Inbox = (json) =>
            {
                List<JsonMojo> listMojos = JsonConvert.DeserializeObject<List<JsonMojo>>(json);
                List<listInboxItem> items = new List<listInboxItem>();

                foreach (JsonMojo mojo in listMojos)
                {
                    items.Add(new listInboxItem() { mojo = mojo });
                }

                return items;
            };
        }

        private void Audios_Conversion()
        {
            Conversions.Soundtracks = (json) =>
            {
                JsonSoundtracksPage page = JsonConvert.DeserializeObject<JsonSoundtracksPage>(json);
                List<listSoundtracksItem> items = new List<listSoundtracksItem>();

                foreach (JsonSoundtrackGroup soundtrackGroup in page.soundtrackGroups)
                {
                    items.Add(new listSoundtracksItem() { type = "heading", text = soundtrackGroup.heading });

                    foreach (JsonSoundtrack jsonSoundtrack in soundtrackGroup.soundtracks)
                    {
                        items.Add
                        (
                            new listSoundtracksItem()
                            {
                                type = "soundtrack",
                                soundtrack = jsonSoundtrack
                            }
                        );
                    }
                }

                return items;
            };
        }

        private void Avatars_Conversion()
        {
            Conversions.Characters = (json) =>
            {
                JsonCharactersPage page = JsonConvert.DeserializeObject<JsonCharactersPage>(json);
                List<listCharactersItem> items = new List<listCharactersItem>();

                foreach (JsonCharacterGroup avatarGroup in page.characterGroups)
                {
                    items.Add(new listCharactersItem() { type = "heading", text = avatarGroup.heading });

                    for (int i = 0;i < avatarGroup.characters.Count;i += 2)
                    {
                        items.Add
                        (
                            new listCharactersItem()
                            {
                                type = "character",
                                character = avatarGroup.characters[i],
                                character2 = (i + 1 != avatarGroup.characters.Count) ? avatarGroup.characters[i + 1] : null
                            }
                        );
                    }
                }

                return items;
            };
        }

        private void Friend_Picker_Conversion()
        {
            Conversions.Friend_Picker = (json) =>
            {
                List<JsonUser> listUsers = JsonConvert.DeserializeObject<List<JsonUser>>(json);
                List<listFriendsItem> items = new List<listFriendsItem>();

                foreach (JsonUser user in listUsers)
                {
                    items.Add(new listFriendsItem() { user = user });
                }                

                return items;
            };
        }

        private void Friend_Requests_Conversion()
        {
            Conversions.Friend_Requests = (json) =>
            {
                List<JsonFriendRequest> listRequests = JsonConvert.DeserializeObject<List<JsonFriendRequest>>(json);

                List<listFriendRequestsItem> items = new List<listFriendRequestsItem>();
                foreach (JsonFriendRequest request in listRequests)
                {
                    items.Add(new listFriendRequestsItem() { request = request });
                }

                Variables.Friend_Request_Count = items.Count;

                return items;
            };
        }

        private void Friends_Conversion()
        {
            Conversions.Friends = (json) =>
            {
                List<JsonUser> listFriends = JsonConvert.DeserializeObject<List<JsonUser>>(json);

                List<listFriendsItem> items = new List<listFriendsItem>();
                foreach (JsonUser friend in listFriends)
                {
                    items.Add(new listFriendsItem() { user = friend });
                }                

                return items;
            };
        }
    }
}
