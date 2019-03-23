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
    public class pageMessage : UIPage
    {
        public pageMessage(UIDirection direction, UIPageHeight height) : base(direction, height) { }

        protected override string GiveName()
        {
            return "pageMessage";
        }

        protected override string GiveTitle()
        {
            return "Leave a Message";
        }

        protected override Dictionary<string, UIState> GenerateStates()
        {
            return new Dictionary<string, UIState>()
            {
                {
                    "Page_Inbox,Page_Main,Page_Characters,Page_Soundtracks",
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
                    "Page_Profile,Page_FriendPicker,Page_Error",
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
                    "Page_Message",
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
                    Name = "pnlMessage",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.TOP_CENTER,
                        Width = Styles.Width_Page,
                        Height = Styles.Height_Bar_Tall * 1.25f,
                        Top = Styles.Padding_For_Anything,
                        BackgroundColor = Media.colorWhite
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIText()
                        {
                            Name = "txtMessage",
                            Idle = new UIIdleText()
                            {
                                Float = Float.MIDDLE_CENTER,
                                Width = Styles.Width_Page * 0.9f,
                                Height = Styles.Height_Bar_Tall,
                                Alignment = TextAnchor.MiddleLeft,
                                Font = Media.fontExoLight,
                                FontColor = Media.colorGrey,
                                FontSize = Styles.Font_Size_Large,
                                LineHeight = 1,
                                Text = (Variables.Current_Mojo.message == "")?"Leave your message here...":Variables.Current_Mojo.message,
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
                                    string message = (Variables.UI["txtMessage"] as UIText).Element.text;
                                    string initialText = "";

                                    if (message == "Leave your message here...")
                                        initialText = "";
                                    else
                                        initialText = message;

                                    Directives.Sense_Touch = false;

                                    new KeyboardOpener()
                                    {
                                        Material = new KeyboardOpenerMaterial()
                                        {
                                            Owner = this.Name,
                                            Listeners = new List<string>()
                                            {
                                                "clckMessageNext",
                                                "clckMessageCancel",
                                                "clckPreviewAvatar",
                                                "clckChangeAvatar",
                                                "clckChangeAudio"
                                            },
                                            Model = Variables.UI["txtMessage"] as UIText,
                                            Initial_Text = initialText,
                                            OnMask = (text) =>
                                            {
                                                string masked = "";

                                                int length = text.Length;
                                    
                                                if (length <= 100)
                                                    masked = Variables.Keyboard_Text.Substring(0, length);
                                                else
                                                    masked = Variables.Keyboard_Text.Substring(0, 100);                                   

                                                UIText txtTopRight = Variables.UI["txtTopRight"] as UIText;                                    

                                                if (masked.Length == 0)                                    
                                                    txtTopRight.Element.text = "Leave a Message";                                                                            
                                                else                                    
                                                    txtTopRight.Element.text = masked.Length + "/" + 100;                                                                            

                                                Variables.Current_Mojo.message = masked;

                                                return masked;
                                            },
                                            OnClose = () =>
                                            {
                                                UIText txtMessage = Variables.UI["txtMessage"] as UIText;
                                                UIText txtTopRight = Variables.UI["txtTopRight"] as UIText;
                                                UIText txtMessageNext = Variables.UI["txtMessageNext"] as UIText;                                    

                                                txtTopRight.Element.text = "Leave a Message";

                                                if (Variables.Current_Mojo.message == "")
                                                {
                                                    txtMessage.Element.text = "Leave your message here...";
                                                    txtMessageNext.Element.text = "Let's send without text...";
                                                }
                                                else
                                                    txtMessageNext.Element.text = "OK, let's send this Mojo...";
                                            }
                                        }                                        
                                    }
                                    .Open();
                                }
                            }
                        };                                                                                                                         
                    }
                }, 
                new UIText()
                {
                    Name = "txtLetMeSee",
                    Idle = new UIIdleText()
                    {
                        Float = Float.TOP_CENTER,
                        Width = Styles.Width_List_Item,
                        Height = Styles.Height_Bar_Medium,
                        Top = Styles.Height_Bar_Tall * 1.75f,
                        Alignment = TextAnchor.MiddleCenter,
                        Font = Media.fontExoLight,
                        FontColor = Media.colorGrey,
                        FontSize = Styles.Font_Size_Large,
                        LineHeight = 1,
                        Text = "Let me see what I've created"
                    }
                },
                new UIPanel()
                {
                    Name = "pnlPreview",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.TOP_CENTER,
                        Width = Styles.Width_List_Item,
                        Height = Styles.Height_List_Item,
                        Top = Styles.Height_Bar_Tall * 2.725f,
                        BackgroundColor = Media.colorWhite
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIImage()
                        {
                            Name = "imgPreviewProfile",
                            Idle = new UIIdleImage()
                            {
                                Float = Float.MIDDLE_LEFT,
                                Width = Styles.Side_Inbox_Mojo_Item_Profile,
                                Height = Styles.Side_Inbox_Mojo_Item_Profile, 
                                Left = Styles.Left_Inbox_Mojo_Item_Profile,                               
                                BackgroundColor = Media.colorOpaque,
                                Url = Variables.Self.picture,
                                LazyLoad = true,
                                LazyLoadSuspension = 1.5f
                            },
                            Components = new List<UIComponent>()
                            {
                                new UIImage()
                                {
                                    Name = "imgPreviewProfileMask",
                                    Idle = new UIIdleImage()
                                    {
                                        Float = Float.TOP_LEFT,
                                        Width = Styles.Side_Inbox_Mojo_Item_Profile,
                                        Height = Styles.Side_Inbox_Mojo_Item_Profile,                                       
                                        BackgroundColor = Media.colorWhite,
                                        Url = "Images/in_rounded"
                                    }
                                }
                            }
                        },
                        new UIText()
                        {
                            Name = "txtPreviewUser",
                            Idle = new UIIdleText()
                            {
                                Float = Float.MIDDLE_LEFT,
                                Width = Styles.Width_Inbox_Mojo_Item_Username,
                                Bottom = Screen.width * 0.05f,
                                Left = Styles.Side_Inbox_Mojo_Item_Profile + Styles.Left_Inbox_Mojo_Item_Profile * 2,
                                Alignment = TextAnchor.MiddleLeft,
                                Font = Media.fontExoRegular,
                                FontColor = Media.colorGreyDark,
                                FontSize = Styles.Font_Size_Large,
                                LineHeight = 1,
                                Text = Variables.Self.name
                            }
                        },
                        new UIText()
                        {
                            Name = "txtPreviewMessage",
                            Idle = new UIIdleText()
                            {
                                Float = Float.TOP_LEFT,
                                Width = Styles.Width_Inbox_Mojo_Item_Message,
                                Top = Styles.Top_Inbox_Mojo_Item_Message * 3.25f,
                                Left = Styles.Side_Inbox_Mojo_Item_Profile + Styles.Left_Inbox_Mojo_Item_Profile * 2,
                                Alignment = TextAnchor.MiddleLeft,
                                Font = Media.fontExoLight,
                                FontColor = Media.colorGrey,
                                FontSize = Styles.Font_Size_Medium,
                                LineHeight = 1.25f,
                                Text = "sent you a mojo with " + Variables.Current_Mojo.character.name
                            }
                        },
                        new UIImage()
                        {
                            Name = "clckPreviewAvatar",
                            Idle = new UIIdleImage()
                            {
                                Float = Float.MIDDLE_RIGHT,
                                Width = Styles.Side_Inbox_Mojo_Item_Target,
                                Height = Styles.Side_Inbox_Mojo_Item_Target,                                                             
                                BackgroundColor = Media.colorOpaque,
                                Url = Variables.Current_Mojo.character.picture,
                                LazyLoad = true,
                                LazyLoadSuspension = 1.5f
                            },
                            Components = new List<UIComponent>()
                            {
                                new UIImage()
                                {
                                    Name = "imgPreviewAvatarMask",
                                    Idle = new UIIdleImage()
                                    {
                                        Float = Float.MIDDLE_RIGHT,
                                        Width = Styles.Side_Inbox_Mojo_Item_Target,
                                        Height = Styles.Side_Inbox_Mojo_Item_Target,                                       
                                        BackgroundColor = Media.colorBlackTwoThirdsTransparent,
                                        Url = "Images/solid"
                                    },
                                    States = new Dictionary<string,UIState>()
                                    {
                                        {
                                            "Preview",
                                            new UIStateImage()
                                            {
                                                Width = Styles.Width_List_Item,
                                                Height = Styles.Height_List_Item,                                       
                                                BackgroundColor = Media.colorBlack                                                
                                            }
                                        },
                                        {
                                            "Delayed_Preview",
                                            new UIStateImage()
                                            {
                                                Width = Styles.Side_Inbox_Mojo_Item_Target,
                                                Height = Styles.Side_Inbox_Mojo_Item_Target,                                       
                                                BackgroundColor = Media.colorBlackTwoThirdsTransparent,                                                
                                            }
                                        }
                                    }
                                },
                                new UIText()
                                {
                                    Name = "txtPreview",
                                    Idle = new UIIdleText()
                                    {
                                        Float = Float.MIDDLE_RIGHT,
                                        Width = Styles.Side_Inbox_Mojo_Item_Target,                                        
                                        Alignment = TextAnchor.MiddleCenter,
                                        Font = Media.fontExoLight,
                                        FontColor = Media.colorWhite,
                                        FontSize = Styles.Font_Size_Large,
                                        LineHeight = 1,
                                        Text = "Preview"                                        
                                    },
                                    States = new Dictionary<string,UIState>()
                                    {
                                        {
                                            "Preview",
                                            new UIStateText()
                                            {
                                                Width = Styles.Width_List_Item,                                                                                      
                                                FontColor = Media.colorWhite                                                
                                            }
                                        },
                                        {
                                            "Delayed_Preview",
                                            new UIStateText()
                                            {
                                                Width = Styles.Side_Inbox_Mojo_Item_Target,                                                                                      
                                                FontColor = Media.colorWhite                                                
                                            }
                                        }
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

                                            new Previewer()
                                            {
                                                OnFinish = () => { Directives.Sense_Touch = true; }                                                
                                            }
                                            .Preview();
                                        }
                                    }
                                };                                
                            }
                        }
                    }
                },
                new UIPanel()
                {
                    Name = "clckChangeAvatar",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.TOP_CENTER,
                        Width = Styles.Width_Bar_Wide,
                        Height = Styles.Height_Bar_Medium,
                        Top = Styles.Height_Bar_Tall * 4.5f,
                        BackgroundColor = Media.colorWhite
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIText()
                        {
                            Name = "txtChangeAvatar",
                            Idle = new UIIdleText()
                            {
                                Float = Float.MIDDLE_CENTER,
                                Width = Styles.Width_Bar_Wide,
                                Alignment = TextAnchor.MiddleCenter,
                                Font = Media.fontExoLight,
                                FontColor = Media.colorGrey,
                                FontSize = Styles.Font_Size_Large,
                                LineHeight = 1,
                                Text = "Change Character"
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

                                    new UIPageRenderer<listCharactersItem>()
                                    {
                                        Material = new UIPageRendererMaterial<listCharactersItem>()
                                        {
                                            Page = new pageCharacters(UIDirection.FROM_LEFT, UIPageHeight.TALL),
                                            List = "listCharacters",
                                            ListUrl = Config.URLs.Characters_Page,
                                            ListFields = new Dictionary<string, string>()
                                            {
                                                { "soundtrackFk" , Variables.Current_Mojo.soundtrack.pk.ToString() }
                                            },
                                            OnConversion = Conversions.Characters,
                                            FromCache = true
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
                    Name = "clckChangeAudio",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.TOP_CENTER,
                        Width = Styles.Width_Bar_Wide,
                        Height = Styles.Height_Bar_Medium,
                        Top = Styles.Height_Bar_Tall * 5.5f,
                        BackgroundColor = Media.colorWhite
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIText()
                        {
                            Name = "txtChangeAudio",
                            Idle = new UIIdleText()
                            {
                                Float = Float.MIDDLE_CENTER,
                                Width = Styles.Width_Bar_Wide,
                                Alignment = TextAnchor.MiddleCenter,
                                Font = Media.fontExoLight,
                                FontColor = Media.colorGrey,
                                FontSize = Styles.Font_Size_Large,
                                LineHeight = 1,
                                Text = "Change Soundtrack"
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

                                    new UIPageRenderer<listSoundtracksItem>()
                                    {
                                        Material = new UIPageRendererMaterial<listSoundtracksItem>()
                                        {
                                            Page = new pageSoundtracks(UIDirection.FROM_LEFT, UIPageHeight.TALL),
                                            List = "listSoundtracks",
                                            ListUrl = Config.URLs.Soundtracks_Page,
                                            ListFields = new Dictionary<string,string>()
                                            {
                                                { "characterFk" , Variables.Current_Mojo.character.pk.ToString() }
                                            },
                                            OnConversion = Conversions.Soundtracks,
                                            FromCache = true
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
                    Name = "pnlMessagePageBottomBar",
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
                            Name = "clckMessageCancel",
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
                                    Name = "txtMessageCancel",
                                    Idle = new UIIdleText()
                                    {
                                        Float = Float.MIDDLE_CENTER,
                                        Width = Styles.Width_Bar_Single,
                                        Alignment = TextAnchor.MiddleCenter,
                                        Font = Media.fontExoLight,
                                        FontColor = Media.colorWhite,
                                        FontSize = Styles.Font_Size_Large,
                                        LineHeight = 1,
                                        Text = "Cancel"
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
                                };                                
                            }
                        },
                        new UIPanel()
                        {
                            Name = "clckMessageNext",
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
                                    Name = "txtMessageNext",
                                    Idle = new UIIdleText()
                                    {
                                        Float = Float.MIDDLE_CENTER,
                                        Width = Styles.Width_Bar_Wide,
                                        Alignment = TextAnchor.MiddleCenter,
                                        Font = Media.fontExoLight,
                                        FontColor = Media.colorWhite,
                                        FontSize = Styles.Font_Size_Large,
                                        LineHeight = 1,
                                        Text = (Variables.Current_Mojo.message == "")?"Let's send without text...":"OK, let's send this Mojo...",
                                        FurtherAccess = true
                                    }
                                },
                                new UIImage()
                                {
                                    Name = "imgMessageNextArrow",
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

                                            new UIPageRenderer<listFriendsItem>()
                                            {
                                                Material = new UIPageRendererMaterial<listFriendsItem>()
                                                {
                                                    Page = new pageFriendPicker(UIDirection.FROM_RIGHT, UIPageHeight.TALL),
                                                    List = "listFriendPicker",
                                                    ListUrl = Config.URLs.Friends_Page,
                                                    OnConversion = Conversions.Friend_Picker,                                            
                                                    OnListRender = (page,list) =>
                                                    {
                                                        if (list.Data.Count != 0)
                                                        {
                                                            Variables.UI["clckFriendPickerNoFriends"].Destroy();                                                    
                                                        }
                                                    }                                            
                                                }                                                    
                                            }
                                            .Render();                                            
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
