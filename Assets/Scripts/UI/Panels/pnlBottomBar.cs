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
using Assets.Scripts.UI.Pages;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.UI.Panels
{
    public class pnlBottomBar : UIPanel
    {
        public pnlBottomBar()
        {
            this.Name = "pnlBottomBar";

            this.Idle = new UIIdlePanel()
            {
                Float = Float.BOTTOM_CENTER,
                Width = Styles.Screen_Width,
                Height = Styles.Height_Bar_Medium,
                BackgroundColor = Media.colorWhite
            };

            this.States = new Dictionary<string, UIState>()
            {
                {
                    "Page_Target,Page_Characters,Page_Soundtracks,Page_FriendPicker,Page_Friends,Page_Logout,Page_AddFriend,Page_FriendRequests,Preview_Back,Page_About_1",
                    new UIStatePanel()
                    {
                        Width = Styles.Screen_Width,
                        Height = Styles.Height_Bar_Medium,
                        Right = Styles.Screen_Width,
                        BackgroundColor = Media.colorWhite                        
                    }
                },
                {
                    "Page_Inbox,Page_Main,Page_Profile,View,Previewing,Back,Page_Error",
                    new UIStatePanel()
                    {
                        Width = Styles.Screen_Width,
                        Height = Styles.Height_Bar_Medium,                        
                        BackgroundColor = Media.colorWhite                        
                    }
                }
            };

            this.Components = new List<UIComponent>()
            {
                new UIPanel()
                {
                    Name = "pnlIndicator",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.TOP_CENTER,
                        Width = Styles.Width_Indicator,
                        Height = 2,                        
                        BackgroundColor = Media.colorBlack
                    },
                    States = new Dictionary<string,UIState>()
                    {
                        {
                            "Page_Inbox",
                            new UIStatePanel()
                            {
                                Width = Styles.Height_Bar_Medium,                                
                                Height = 2,
                                Right = (Styles.Screen_Width - Styles.Height_Bar_Medium) * 0.5f,
                                BackgroundColor = Media.colorBlack                                
                            }
                        },
                        {
                            "Page_Main",
                            new UIStatePanel()
                            {
                                Width = Styles.Width_Indicator,
                                Height = 2,                                
                                BackgroundColor = Media.colorBlack                                
                            }
                        },
                        {
                            "Page_Profile",
                            new UIStatePanel()
                            {
                                Width = Styles.Height_Bar_Medium,
                                Height = 2,
                                Left = (Styles.Screen_Width - Styles.Height_Bar_Medium) * 0.5f,
                                BackgroundColor = Media.colorBlack                                
                            }
                        }
                    }
                },
                new UIPanel()
                {
                    Name = "clckBottom_Inbox",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.MIDDLE_LEFT,
                        Width = Styles.Height_Bar_Medium,
                        Height = Styles.Height_Bar_Medium,
                        BackgroundColor = Media.colorTransparent
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIImage()
                        {
                            Name = "imgInbox",
                            Idle = new UIIdleImage()
                            {
                                Float = Float.MIDDLE_CENTER,
                                Width = Styles.Side_Medium_Bar_Icon,
                                Height = Styles.Side_Medium_Bar_Icon,
                                BackgroundColor = Media.colorGreyDark,
                                Url = "Images/icon_notifications"
                            }
                        }
                    }
                },
                new UIPanel()
                {
                    Name = "clckBottom_Main",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.MIDDLE_LEFT,
                        Width = Styles.Width_Indicator,
                        Left =  Styles.Width_Indicator,
                        Height = Styles.Height_Bar_Medium,
                        BackgroundColor = Media.colorTransparent
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIImage()
                        {
                            Name = "imgBottomIcon",
                            Idle = new UIIdleImage()
                            {
                                Float = Float.BOTTOM_CENTER,
                                Width = Styles.Side_Tall_Bar_Icon * 2,
                                Height = Styles.Side_Tall_Bar_Icon * 2,
                                Top = Styles.Side_Tall_Bar_Icon * 0.75f,
                                BackgroundColor = Media.colorBlackOneFifthTransparent,
                                Url = "Images/icon_mj"
                            }
                        },
                        new UIText()
                        {
                            Name = "txtMojo",
                            Idle = new UIIdleText()
                            {
                                Float = Float.MIDDLE_CENTER,
                                Width = Styles.Width_Indicator,
                                Alignment = TextAnchor.MiddleCenter,
                                Font = Media.fontExoLight,
                                FontColor = Media.colorGreyDark,
                                FontSize = Styles.Font_Size_Largest,
                                LineHeight = 1,
                                Text = "Let's Mojo"
                            }
                        }
                    }
                },
                new UIPanel()
                {
                    Name = "clckBottom_Profile",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.MIDDLE_RIGHT,
                        Width = Styles.Height_Bar_Medium,
                        Height = Styles.Height_Bar_Medium,
                        BackgroundColor = Media.colorTransparent
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIImage()
                        {
                            Name = "imgProfile",
                            Idle = new UIIdleImage()
                            {
                                Float = Float.MIDDLE_CENTER,
                                Width = Styles.Side_Medium_Bar_Icon,
                                Height = Styles.Side_Medium_Bar_Icon,
                                BackgroundColor = Media.colorGreyDark,
                                Url = "Images/icon_profile"
                            }
                        }
                    }
                }                             
            };

            new EventRegistrar()
            {
                Material = new EventRegistrarMaterial()
                {
                    OnTouchListener = new OnTouchListener()
                    {
                        Target = "%clckBottom_",
                        Enabled = true,
                        Released = true,
                        OnTouch = (go) =>
                        {                            
                            string pageToOpen = "page" + go.name.Split('_')[1];

                            if (pageToOpen == Variables.Current_Page)
                                return;

                            Directives.Sense_Touch = false;

                            if (pageToOpen == "pageInbox")
                            {
                                new UIPageRenderer<listInboxItem>()
                                {
                                    Material = new UIPageRendererMaterial<listInboxItem>()
                                    {
                                        Page = new pageInbox(UIDirection.FROM_LEFT, UIPageHeight.NORMAL),
                                        List = "listInbox",
                                        ListUrl = Config.URLs.Inbox_Page,
                                        OnConversion = Conversions.Inbox,
                                        FromCache = true
                                    }                                    
                                }
                                .Render();
                            }
                            else if (pageToOpen == "pageMain")
                            {
                                UIDirection d;

                                if (Variables.Current_Page == "pageInbox")
                                    d = UIDirection.FROM_RIGHT;
                                else if (Variables.Current_Page == "pageProfile")
                                    d = UIDirection.FROM_LEFT;
                                else
                                    d = UIDirection.FROM_LEFT;

                                new UIPageRenderer<listMainItem>()
                                {
                                    Material = new UIPageRendererMaterial<listMainItem>()
                                    {
                                        Page = new pageMain(d, UIPageHeight.NORMAL),
                                        List = "listMain",
                                        ListUrl = Config.URLs.Main_Page,
                                        OnConversion = Conversions.Main,
                                        FromCache = true
                                    }                                    
                                }
                                .Render();
                            }
                            else if (pageToOpen == "pageProfile")
                            {
                                new UIPageRenderer<listItem>()
                                {
                                    Material = new UIPageRendererMaterial<listItem>()
                                    {
                                        Page = new pageProfile(UIDirection.FROM_RIGHT, UIPageHeight.NORMAL),
                                        Url = Config.URLs.Profile_Page,
                                        OnPageLoad = (text) => { Variables.Friend_Request_Count = int.Parse(text); },
                                        OnPageRegeneration = () => { return new pageProfile(UIDirection.FROM_RIGHT, UIPageHeight.NORMAL); }
                                    }
                                }
                                .Render();
                            }
                        }
                    }
                }                
            }
            .Register();
        }
    }
}
