using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Json;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Agents.Renderers;
using Assets.Scripts.Agents.Tools;
using Assets.Scripts.Repository;
using Assets.Scripts.UI.ListItems;
using Assets.Scripts.UI.Lists;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.UI.Pages
{
    public class pageFriends : UIPage
    {        
        public pageFriends(UIDirection direction, UIPageHeight height) : base(direction, height) { }

        protected override string GiveName()
        {
            return "pageFriends";
        }

        protected override string GiveTitle()
        {
            return "Friends";
        }

        protected override Dictionary<string, UIState> GenerateStates()
        {
            return new Dictionary<string, UIState>()
            {
                {
                    "Page_Profile,Page_Main",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page_Full,
                        Height = Styles.Height_Page_With_Nav_Bar,
                        Top = Styles.Top_Page, 
                        Left = Styles.Screen_Width,
                        BackgroundColor = Media.colorTransparent                        
                    }
                },
                {
                    "Page_AddFriend,Page_Error",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page_Full,
                        Height = Styles.Height_Page_With_Nav_Bar,
                        Top = Styles.Top_Page, 
                        Right = Styles.Screen_Width,
                        BackgroundColor = Media.colorTransparent                        
                    }
                },
                {
                    "Page_Friends",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page_Full,
                        Height = Styles.Height_Page_With_Nav_Bar,
                        Top = Styles.Top_Page,                         
                        BackgroundColor = Media.colorTransparent                        
                    }
                }
            };
        }

        protected override List<UIComponent> GenerateComponents()
        {
            return new List<UIComponent>()
            {
                new UIPanel()
                {
                    Name = "pnlFriendsSearch",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.TOP_CENTER,
                        Width = Styles.Screen_Width,
                        Height = Styles.Height_Bar_Medium,
                        BackgroundColor = Media.colorWhite
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIText()
                        {
                            Name = "txtFriendsSearch",
                            Idle = new UIIdleText()
                            {
                                Float = Float.MIDDLE_CENTER,
                                Width = Styles.Width_Page,
                                Alignment = TextAnchor.MiddleLeft,
                                Font = Media.fontExoLight,
                                FontColor = Media.colorGrey,
                                FontSize = Styles.Font_Size_Medium,
                                LineHeight = 1,
                                Text = "Find a friend by Name...",
                                FurtherAccess = true
                            }
                        }
                    },
                    OnTouchInitialization = (component) =>
                    {
                        return new List<OnTouchListener>()
                        {
                            new OnTouchListener()
                            {
                                Owner = component.Name,
                                Target = component.Name,
                                Enabled = true,
                                Released = true,
                                OnRelease = (go) =>
                                {
                                    Directives.Sense_Touch = false;

                                    new KeyboardOpener()
                                    {
                                        Material = new KeyboardOpenerMaterial()
                                        {
                                            Owner = this.Name,
                                            Listeners = new List<string>() { "clckFriendsBack","%clckRemove_"},
                                            Initial_Text = "",
                                            Model = Variables.UI["txtFriendsSearch"] as UIText,
                                            OnMask = (text) =>
                                            {
                                                string masked = "";

                                                if (!string.IsNullOrEmpty(text))
                                                {
                                                    if (text.Contains(' ') && text.Last() != ' ')
                                                    {
                                                        string reformed = "";
                                                        string[] parts = text.Split(' ');
                                                        foreach (string part in parts)
                                                        {
                                                            reformed += part[0].ToString().ToUpper() + part.Substring(1) + " ";
                                                        }

                                                        reformed = reformed.Substring(0, reformed.Length - 1);

                                                        masked = reformed;
                                                    }
                                                    else
                                                    {
                                                        masked = text[0].ToString().ToUpper() + text.Substring(1);
                                                    }
                                                }

                                                return masked;
                                            },
                                            OnClose = () =>
                                            {
                                                if (Variables.Keyboard_Text == "")
                                                {
                                                    UIText txtSearch = Variables.UI["txtFriendsSearch"] as UIText;
                                                    txtSearch.Element.text = "Find a friend by Name...";

                                                    return;
                                                }

                                                (Variables.UI["txtTopRight"] as UIText).Element.text = "Searching...";

                                                new UIListRenderer<listFriendsItem>()
                                                {
                                                    Material = new UIListRendererMaterial<listFriendsItem>()
                                                    {
                                                        List = "listFriends",
                                                        ListUrl = Config.URLs.Friend_Search,
                                                        ListFields = new Dictionary<string, string>()
                                                        {                                                                                                  
                                                            { "text", Variables.Keyboard_Text }                                                    
                                                        },
                                                        OnConversion = (json) =>
                                                        {
                                                            UIText txtTopRight = Variables.UI["txtTopRight"] as UIText;

                                                            List<JsonUser> listUsers = JsonConvert.DeserializeObject<List<JsonUser>>(json);
                                                            List<listFriendsItem> items = new List<listFriendsItem>();

                                                            if (listUsers.Count == 0)
                                                            {
                                                                (Variables.UI["txtTopRight"] as UIText).Element.text = "No users found";

                                                                new Suspender()
                                                                {
                                                                    Suspension = 3,
                                                                    OnFinish = () =>
                                                                    {
                                                                        txtTopRight.Element.text = "Find your Friends";

                                                                        UIText txtSearch = Variables.UI["txtFriendsSearch"] as UIText;
                                                                        txtSearch.Element.text = "Find a friend by Name...";
                                                                    }
                                                                }
                                                                .Suspend();
                                                            }
                                                            else
                                                            {
                                                                if (listUsers.Count == 1)
                                                                    txtTopRight.Element.text = listUsers.Count + " user found";
                                                                else
                                                                    txtTopRight.Element.text = listUsers.Count + " users found";
                                                    
                                                                foreach (JsonUser user in listUsers)
                                                                {
                                                                    items.Add(new listFriendsItem() { user = user });
                                                                }
                                                            }

                                                            return items;
                                                        }
                                                    },
                                                    OnFinish = () =>
                                                    {
                                                        new Suspender()
                                                        {
                                                            Suspension = 3,
                                                            OnFinish = () =>
                                                            {
                                                                UIText txtSearch = Variables.UI["txtFriendsSearch"] as UIText;
                                                                txtSearch.Element.text = "Find a friend by Name...";
                                                            }
                                                        }
                                                        .Suspend();
                                                    }                                                    
                                                }
                                                .Render();
                                            }
                                        }                                        
                                    }
                                    .Open();
                                }
                            }
                        };                        
                    }
                },
                new listFriends(),
                new UIPanel()
                {
                    Name = "clckNoFriends",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.TOP_CENTER,
                        Width = Styles.Width_Bar_Wide,
                        Height = Styles.Height_Bar_Medium,
                        Top = Styles.Height_Bar_Medium * 3f,
                        BackgroundColor = Media.colorWhite,
                        FurtherAccess = true
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIText()
                        {
                            Name = "txtNewHere",
                            Idle = new UIIdleText()
                            {
                                Float = Float.MIDDLE_CENTER,
                                Width = Styles.Width_Bar_Wide,
                                Bottom = Styles.Height_Bar_Medium * 1.25f,
                                Alignment = TextAnchor.MiddleCenter,
                                Font = Media.fontExoLight,
                                FontColor =  Media.colorGrey,
                                FontSize = Styles.Font_Size_Large,
                                LineHeight = 1,
                                Text = "Seems like you're new here :/"
                            }
                        },
                        new UIText()
                        {
                            Name = "txtNoFriends",
                            Idle = new UIIdleText()
                            {
                                Float = Float.MIDDLE_CENTER,
                                Width = Styles.Width_Bar_Wide,                                
                                Alignment = TextAnchor.MiddleCenter,
                                Font = Media.fontExoLight,
                                FontColor =  Media.colorGrey,
                                FontSize = Styles.Font_Size_Large,
                                LineHeight = 1,
                                Text = "Let's make some friends"
                            }
                        },
                        new UIImage()
                        {
                            Name = "imgArrow",
                            Idle = new UIIdleImage()
                            {
                                Float = Float.MIDDLE_RIGHT,
                                Width = Styles.Height_Bar_Medium * 0.3f,
                                Height = Styles.Height_Bar_Medium * 0.3f,
                                Right = Styles.Height_Bar_Medium * 0.15f,
                                BackgroundColor = Media.colorGreyDark,
                                Url = "Images/icon_arrow"
                            }
                        }
                    },
                    OnTouchInitialization = (component) =>
                    {
                        return new List<OnTouchListener>()
                        {
                            new OnTouchListener()
                            {
                                Owner = component.Name,
                                Target = component.Name,
                                Enabled = true,
                                Released = true,
                                OnRelease = (go) =>
                                {
                                    Directives.Sense_Touch = false;

                                    new UIPageRenderer<listForeignFriendsItem>()
                                    {
                                        Material = new UIPageRendererMaterial<listForeignFriendsItem>()
                                        {
                                            Page = new pageAddFriend(UIDirection.FROM_RIGHT, UIPageHeight.TALL)                                                                
                                        }                                        
                                    }
                                    .Render();
                                }
                            }
                        };                        
                    }
                },
                new UIPanel()
                {
                    Name = "clckFriendsBack",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.BOTTOM_CENTER,
                        Width = Styles.Screen_Width,
                        Height = Styles.Height_Bar_Medium,
                        BackgroundColor = Media.colorGrey
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIText()
                        {
                            Name = "txtBack",
                            Idle = new UIIdleText()
                            {
                                Float = Float.MIDDLE_CENTER,
                                Width = Styles.Screen_Width,
                                Height = Styles.Height_Bar_Medium,
                                Alignment = TextAnchor.MiddleCenter,
                                Font = Media.fontExoLight,
                                FontColor = Media.colorWhite,
                                FontSize =Styles.Font_Size_Larger,
                                LineHeight = 1,
                                Text = "Back"
                            }
                        }
                    },
                    OnTouchInitialization = (component) =>
                    {
                        return new List<OnTouchListener>()
                        {
                            new OnTouchListener()
                            {
                                Owner = component.Name,
                                Target = component.Name,
                                Enabled = true,
                                Released = true,
                                OnRelease = (go) =>
                                {
                                    Directives.Sense_Touch = false;

                                    new UIPageRenderer<listItem>()
                                    {
                                        Material = new UIPageRendererMaterial<listItem>()
                                        {
                                            Page = new pageProfile(UIDirection.FROM_LEFT, UIPageHeight.NORMAL),
                                            Url = Config.URLs.Profile_Page,
                                            OnPageLoad = (text) => { Variables.Friend_Request_Count = int.Parse(text); },
                                            OnPageRegeneration = () => { return new pageProfile(UIDirection.FROM_LEFT, UIPageHeight.NORMAL); }
                                        }                                        
                                    }
                                    .Render();
                                }
                            }
                        };                        
                    }
                }
            };
        }
    }
}
