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
using Assets.Scripts.UI.Panels;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.UI.Pages
{
    public class pageFriendPicker : UIPage
    {        
        public pageFriendPicker(UIDirection direction, UIPageHeight height) : base(direction, height) { }

        protected override string GiveName()
        {
            return "pageFriendPicker";
        }

        protected override string GiveTitle()
        {
            return "Find a Friend";
        }

        protected override Dictionary<string, UIState> GenerateStates()
        {
            return new Dictionary<string, UIState>()
            {
                {
                    "Page_Inbox,Page_Main,Page_Message",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page,
                        Height = Styles.Height_Page_With_Nav_Bar,
                        Top = Styles.Top_Page,
                        Left = Styles.Screen_Width,
                        BackgroundColor = Media.colorTransparent                        
                    }
                },
                {
                    "Page_Profile,Page_AddFriend,Page_Error",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page,
                        Height = Styles.Height_Page_With_Nav_Bar,
                        Top = Styles.Top_Page, 
                        Right = Styles.Screen_Width,
                        BackgroundColor = Media.colorTransparent                        
                    }
                },
                {
                    "Page_FriendPicker",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page,
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
                    Name = "pnlFriendPickerSearch",
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
                            Name = "txtFriendPickerSearch",
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

                                    UIText txtFriendsNext = Variables.UI["txtFriendsNext"] as UIText;
                                    txtFriendsNext.Element.text = "Waiting for selection...";

                                    new KeyboardOpener()
                                    {
                                        Material = new KeyboardOpenerMaterial()
                                        {
                                            Owner = this.Name,
                                            Listeners = new List<string>() { "clckFriendsNext","clckFriendsBack"},
                                            Initial_Text = "",
                                            Model = Variables.UI["txtFriendPickerSearch"] as UIText,
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
                                                    UIText txtSearch = Variables.UI["txtFriendPickerSearch"] as UIText;
                                                    txtSearch.Element.text = "Find a friend by Name...";

                                                    return;
                                                }

                                                (Variables.UI["txtTopRight"] as UIText).Element.text = "Searching...";

                                                new UIListRenderer<listFriendsItem>()
                                                {
                                                    Material = new UIListRendererMaterial<listFriendsItem>()
                                                    {
                                                        List = "listFriendPicker",
                                                        ListUrl = Config.URLs.Friend_Search,
                                                        ListFields = new Dictionary<string, string>()
                                                        {                                                                                                 
                                                            { "text", Variables.Keyboard_Text }                                                    
                                                        },
                                                        OnConversion = Conversions.Friend_Picker                                            
                                                    },
                                                    OnFinish = () =>
                                                    {
                                                        (Variables.UI["txtTopRight"] as UIText).Element.text = "Find a Friend";
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
                new listFriendPicker(),
                new UIPanel()
                {
                    Name = "clckFriendPickerNoFriends",
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
                            Name = "txtFriendPickerNewHere",
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
                            Name = "txtFriendPickerNoFriends",
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
                            Name = "imgFriendPickerArrow",
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
                    Name = "pnlFriendsPageBottomBar",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.BOTTOM_CENTER,
                        Width = Styles.Screen_Width,
                        Height = Styles.Height_Bar_Medium,
                        BackgroundColor = Media.colorTransparent
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIPanel()
                        {
                            Name = "clckFriendsBack",
                            Idle = new UIIdlePanel()
                            {
                                Float = Float.MIDDLE_LEFT,
                                Width = Styles.Width_Bar_Single,
                                Height = Styles.Height_Bar_Medium,
                                BackgroundColor = Media.colorBlack
                            },
                            Components = new List<UIComponent>()
                            {
                                new UIText()
                                {
                                    Name = "txtFriendsBack",
                                    Idle = new UIIdleText()
                                    {
                                        Float = Float.MIDDLE_CENTER,
                                        Width = Styles.Width_Bar_Single,
                                        Alignment = TextAnchor.MiddleCenter,
                                        Font = Media.fontExoLight,
                                        FontColor = Media.colorWhite,
                                        FontSize = Styles.Font_Size_Large,
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
                                                    Page = new pageMessage(UIDirection.FROM_LEFT, UIPageHeight.TALL),                                                                                
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
                            Name = "clckFriendsNext",
                            Idle = new UIIdlePanel()
                            {
                                Float = Float.MIDDLE_RIGHT,
                                Width = Styles.Width_Bar_Wide,
                                Height = Styles.Height_Bar_Medium,
                                BackgroundColor = Media.colorGrey
                            },
                            Components = new List<UIComponent>()
                            {
                                new UIText()
                                {
                                    Name = "txtFriendsNext",
                                    Idle = new UIIdleText()
                                    {
                                        Float = Float.MIDDLE_CENTER,
                                        Width = Styles.Width_Bar_Wide,
                                        Alignment = TextAnchor.MiddleCenter,
                                        Font = Media.fontExoLight,
                                        FontColor = Media.colorWhite,
                                        FontSize = Styles.Font_Size_Large,
                                        LineHeight = 1,
                                        Text = "Waiting for selection...",
                                        FurtherAccess = true
                                    }
                                },
                                new UIImage()
                                {
                                    Name = "imgFriendsNextArrow",
                                    Idle = new UIIdleImage()
                                    {
                                        Float = Float.MIDDLE_RIGHT,
                                        Width = Styles.Height_Bar_Tall * 0.3f,
                                        Height = Styles.Height_Bar_Tall * 0.3f,
                                        Right = Styles.Height_Bar_Tall * 0.15f,
                                        BackgroundColor = Media.colorWhite,
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

                                            UIText txtTopRight = Variables.UI["txtTopRight"] as UIText;
                                            txtTopRight.Element.text = "Sending...";

                                            new HttpPostRequestSender()
                                            {
                                                Material = new HttpPostRequestSenderMaterial()
                                                {
                                                    Url = Config.URLs.Send,
                                                    Fields = new Dictionary<string,string>()
                                                    {
                                                        { "appInstanceKey", Variables.App_Instance_Key },
                                                        { "friendFk", Variables.Current_Mojo.user.pk.ToString() },
                                                        { "characterFk", Variables.Current_Mojo.character.pk.ToString() },
                                                        { "soundtrackFk", Variables.Current_Mojo.soundtrack.pk.ToString() },
                                                        { "message", Variables.Current_Mojo.message }
                                                    }                                        
                                                },
                                                OnSuccess = (www) =>
                                                {
                                                    txtTopRight.Element.text = "Mojo sent...";

                                                    new Suspender()
                                                    {
                                                        Suspension = 1,
                                                        OnFinish = () =>
                                                        {
                                                            new UIPageRenderer<listMainItem>()
                                                            {
                                                                Material = new UIPageRendererMaterial<listMainItem>()
                                                                {
                                                                    Page = new pageMain(UIDirection.FROM_LEFT, UIPageHeight.NORMAL),
                                                                    List = "listMain",
                                                                    ListUrl = Config.URLs.Main_Page,
                                                                    OnConversion = Conversions.Main,
                                                                    FromCache = true
                                                                }                                                                
                                                            }
                                                            .Render();
                                                        }
                                                    }
                                                    .Suspend();                                        
                                                }                                                
                                            }
                                            .Send();
                                        }
                                    }
                                };
                            }
                        }
                    }
                }                
            };
        }
    }
}
