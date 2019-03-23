using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Repository;
using UnityEngine;

namespace Assets.Scripts.UI.Panels
{
    public class pnlSplash : UIPanel
    {
        public pnlSplash()
        {
            this.Name = "pnlSplash";

            this.Idle = new UIIdlePanel()
            {
                Float = Float.MIDDLE_CENTER,
                Width = Styles.Screen_Width,
                Height = Styles.Screen_Height,
                BackgroundColor = Media.colorWhite
            };

            this.States = new Dictionary<string, UIState>()
            {
                {
                    "Initializing",
                    new UIStatePanel()
                    {
                        Width = Styles.Screen_Width,
                        Height = Styles.Screen_Height,
                        BackgroundColor = Media.colorWhite
                    }
                },
                {
                    "Page_Main,Page_Inbox,Page_FriendRequests",
                    new UIStatePanel()
                    {
                        Width = Styles.Screen_Width,
                        Height = Styles.Screen_Height * 2f,
                        BackgroundColor = Media.colorTransparent
                    }
                }
            };

            this.Components = new List<UIComponent>()
            {
                new UIImage()
                {
                    Name = "imgFade",
                    Idle = new UIIdleImage()
                    {
                        Float = Float.MIDDLE_CENTER,
                        Width = Styles.Screen_Width,
                        Height = Styles.Screen_Height * 1.5f,                                            
                        BackgroundColor = Color.black,
                        Url = "Images/faded_background"                        
                    },
                    States = new Dictionary<string,UIState>()
                    {
                        {
                            "Initializing",
                            new UIStateImage()
                            {
                                Width = Styles.Screen_Width,
                                Height = Styles.Screen_Height,
                                BackgroundColor = Color.black
                            }
                        },
                        {
                            "Page_Main,Page_Inbox",
                            new UIStateImage()
                            {
                                Width = Styles.Screen_Width,
                                Height = Styles.Screen_Height * 2f,
                                BackgroundColor = Media.colorTransparent
                            }
                        }
                    }
                },
                new UIText()
                {
                    Name = "txtConsole",
                    Idle = new UIIdleText()
                    {
                        Float = Float.BOTTOM_CENTER,
                        Width = Styles.Screen_Width,
                        Bottom = Styles.Padding_For_Anything * 2,
                        Alignment = TextAnchor.MiddleCenter,
                        Font = Media.fontExoLight,
                        FontColor = Media.colorTransparent,
                        FontSize = Styles.Font_Size_Largest,
                        LineHeight = 1,
                        Text = ""
                    },
                    States = new Dictionary<string,UIState>()
                    {
                        {
                            "Initializing",
                            new UIStateText()
                            {
                                Width = Styles.Screen_Width, 
                                Bottom = Styles.Padding_For_Anything * 2,
                                FontColor = Media.colorGreyLight
                            }
                        },
                        {
                            "Page_Main,Page_Inbox",
                            new UIStateText()
                            {
                                Width = Styles.Screen_Width,
                                Bottom = Styles.Padding_For_Anything * 2,
                                FontColor = Media.colorTransparent
                            }
                        }
                    }
                },
                new UIImage()
                {
                    Name = "imgLogo",
                    Idle = new UIIdleImage()
                    {
                        Float = Float.MIDDLE_CENTER,
                        Width = Styles.Screen_Width_Half,
                        Height = Styles.Screen_Width_Half,
                        BackgroundColor = Media.colorTransparent,
                        Url = "Images/icon_mojo"                        
                    },
                    States = new Dictionary<string,UIState>()
                    {
                        {
                            "Initializing",
                            new UIStateImage()
                            {
                                Width = Styles.Screen_Width_Half,
                                Height = Styles.Screen_Width_Half,
                                BackgroundColor = Media.colorOpaque
                            }
                        },
                        {
                            "Page_Main,Page_Inbox",
                            new UIStateImage()
                            {
                                Width = Styles.Screen_Width_Half,
                                Height = Styles.Screen_Width_Half,
                                BackgroundColor = Media.colorTransparent                                
                            }
                        }                        
                    }
                }
            };
        }
    }
}
