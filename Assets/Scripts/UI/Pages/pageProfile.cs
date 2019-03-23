using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Json;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Agents.Renderers;
using Assets.Scripts.Agents.Tools;
using Assets.Scripts.Repository;
using Assets.Scripts.UI.ListItems;
using Facebook.Unity;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Pages
{
    public class pageProfile : UIPage
    {
        public pageProfile(UIDirection direction, UIPageHeight height) : base(direction, height) { }

        protected override string GiveName()
        {
            return "pageProfile";
        }

        protected override string GiveTitle()
        {
            return "Profile";
        }

        protected override Dictionary<string, UIState> GenerateStates()
        {
            return new Dictionary<string, UIState>()
            {
                {
                    "Page_Inbox,Page_Main",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page,
                        Height = Styles.Height_Page,
                        Top = Styles.Top_Page,
                        Left = Styles.Screen_Width,
                        BackgroundColor = Media.colorWhite                        
                    }
                },
                {
                    "Page_Friends,Page_AddFriend,Page_FriendRequests,Page_Logout,Page_About_1,Page_Error",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page,
                        Height = Styles.Height_Page,
                        Top = Styles.Top_Page,
                        Right = Styles.Screen_Width,
                        BackgroundColor = Media.colorWhite                        
                    }
                },
                {
                    "Page_Profile",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page,
                        Height = Styles.Height_Page,
                        Top = Styles.Top_Page,                         
                        BackgroundColor = Media.colorWhite                        
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
                    Name = "pnlPicture",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.TOP_CENTER,
                        Width = Styles.Screen_Width * 0.96f,
                        Height = Styles.Screen_Width_Half,
                        Top = Styles.Height_Bar_Short * 0.5f,
                        BackgroundColor = Media.colorTransparent
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIImage()
                        {
                            Name = "imgPicture",
                            Idle = new UIIdleImage()
                            {
                                Float = Float.MIDDLE_CENTER,
                                Width = Styles.Screen_Width_Quarter,
                                Height = Styles.Screen_Width_Quarter,
                                Bottom = Styles.Height_Bar_Short * 0.5f,
                                BackgroundColor = Media.colorOpaque,
                                Url = Variables.Self.picture,
                                LazyLoad = true,
                                LazyLoadSuspension = 1.5f
                            },
                            Components = new List<UIComponent>()
                            {
                                new UIImage()
                                {
                                    Name = "imgPictureMask",
                                    Idle = new UIIdleImage()
                                    {
                                        Float = Float.MIDDLE_CENTER,
                                        Width = Styles.Screen_Width_Quarter,
                                        Height = Styles.Screen_Width_Quarter,
                                        BackgroundColor = Media.colorWhite,
                                        Url = "Images/in_rounded"
                                    }
                                }
                            }
                        },
                        new UIText()
                        {
                            Name = "txtName",
                            Idle = new UIIdleText()
                            {
                                Float = Float.BOTTOM_CENTER,
                                Width = Styles.Screen_Width,
                                Bottom = Styles.Height_Bar_Short * 0.85f,
                                Alignment = TextAnchor.MiddleCenter,
                                Font = Media.fontExoRegular,
                                FontColor = Media.colorGreyDark,
                                FontSize = Styles.Font_Size_Largest,
                                LineHeight = 1,
                                Text = Variables.Self.name
                            }
                        },
                        new UIText()
                        {
                            Name = "txtEmail",
                            Idle = new UIIdleText()
                            {
                                Float = Float.BOTTOM_CENTER,
                                Width = Styles.Screen_Width,    
                                Bottom = Styles.Height_Bar_Short * 0.33f,
                                Alignment = TextAnchor.MiddleCenter,
                                Font = Media.fontExoLight,
                                FontColor = Media.colorGrey,
                                FontSize = Styles.Font_Size_Small,
                                LineHeight = 1,
                                Text = Variables.Self.email
                            }
                        }
                    }
                },                
                new UIPanel()
                {
                    Name = "pnlConsole",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.BOTTOM_CENTER,
                        Width = Styles.Screen_Width * 0.96f,
                        Height = Styles.Height_Bar_Tall * 5,
                        Bottom = Styles.Height_Bar_Short * 0.5f,
                        BackgroundColor = Media.colorTransparent
                    },
                    Components = new List<UIComponent>()
                    {                       
                        new UIPanel()
                        {
                            Name = "clckAdd",
                            Idle = new UIIdlePanel()
                            {
                                Float = Float.BOTTOM_CENTER,
                                Width = Styles.Screen_Width_Four_Fifth,
                                Height = Styles.Height_Bar_Tall,
                                Bottom = Styles.Height_Bar_Tall * 5,
                                BackgroundColor = Media.colorWhite
                            },
                            Components = new List<UIComponent>()
                            {
                                new UIPanel()
                                {
                                    Name = "pnlLine",
                                    Idle = new UIIdlePanel()
                                    {
                                        Float = Float.BOTTOM_CENTER,
                                        Width = Styles.Screen_Width_Five_Sixth,
                                        Height = 1,
                                        BackgroundColor = Media.colorGreyLight
                                    }
                                },
                                new UIText()
                                {
                                    Name = "txtAdd",
                                    Idle = new UIIdleText()
                                    {
                                        Float = Float.MIDDLE_CENTER,
                                        Width = Styles.Screen_Width_Four_Fifth,
                                        Alignment = TextAnchor.MiddleLeft,
                                        Font = Media.fontExoLight,
                                        FontColor = Media.colorGreyDark,
                                        FontSize = Styles.Font_Size_Larger,
                                        LineHeight = 1,
                                        Text = "Find Friends"
                                    }
                                },
                                new UIImage()
                                {
                                    Name = "imgAdd",
                                    Idle = new UIIdleImage()
                                    {
                                        Float = Float.MIDDLE_RIGHT,
                                        Width = Styles.Side_Medium_Bar_Icon,
                                        Height = Styles.Side_Medium_Bar_Icon,
                                        BackgroundColor = Media.colorGreyLight,
                                        Url = "Images/icon_search"
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
                                                    Page = new pageAddFriend(UIDirection.FROM_RIGHT,UIPageHeight.TALL)                                                                              
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
                            Name = "clckFriends",
                            Idle = new UIIdlePanel()
                            {
                                Float = Float.BOTTOM_CENTER,
                                Width = Styles.Screen_Width_Four_Fifth,
                                Height = Styles.Height_Bar_Tall,
                                Bottom = Styles.Height_Bar_Tall * 4,
                                BackgroundColor = Media.colorWhite
                            },
                            Components = new List<UIComponent>()
                            {
                                new UIPanel()
                                {
                                    Name = "pnlLine",
                                    Idle = new UIIdlePanel()
                                    {
                                        Float = Float.BOTTOM_CENTER,
                                        Width = Styles.Screen_Width_Five_Sixth,
                                        Height = 1,
                                        BackgroundColor = Media.colorGreyLight
                                    }
                                },
                                new UIText()
                                {
                                    Name = "txtFriends",
                                    Idle = new UIIdleText()
                                    {
                                        Float = Float.MIDDLE_CENTER,
                                        Width = Styles.Screen_Width_Four_Fifth,
                                        Alignment = TextAnchor.MiddleLeft,
                                        Font = Media.fontExoLight,
                                        FontColor = Media.colorGreyDark,
                                        FontSize = Styles.Font_Size_Larger,
                                        LineHeight = 1,
                                        Text = "My Friends"
                                    }
                                },
                                new UIImage()
                                {
                                    Name = "imgFriends",
                                    Idle = new UIIdleImage()
                                    {
                                        Float = Float.MIDDLE_RIGHT,
                                        Width = Styles.Side_Medium_Bar_Icon,
                                        Height = Styles.Side_Medium_Bar_Icon,
                                        BackgroundColor = Media.colorGreyLight,
                                        Url = "Images/icon_friends"
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
                                                    Page = new pageFriends(UIDirection.FROM_RIGHT, UIPageHeight.TALL),                                       
                                                    List = "listFriends",                                        
                                                    ListUrl = Config.URLs.Friends_Page,                                        
                                                    OnConversion = Conversions.Friends,
                                                    OnListRender = (page,list) =>
                                                    {
                                                        if (list.Data.Count != 0)
                                                        {
                                                            Variables.UI["clckNoFriends"].Destroy();                                                
                                                        }
                                                    }
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
                            Name = "clckRequests",
                            Idle = new UIIdlePanel()
                            {
                                Float = Float.BOTTOM_CENTER,
                                Width = Styles.Screen_Width_Four_Fifth,
                                Height = Styles.Height_Bar_Tall,
                                Bottom = Styles.Height_Bar_Tall * 3,
                                BackgroundColor = Media.colorWhite
                            },
                            Components = new List<UIComponent>()
                            {
                                new UIPanel()
                                {
                                    Name = "pnlLine",
                                    Idle = new UIIdlePanel()
                                    {
                                        Float = Float.BOTTOM_CENTER,
                                        Width = Styles.Screen_Width_Five_Sixth,
                                        Height = 1,
                                        BackgroundColor = Media.colorGreyLight
                                    }
                                },
                                new UIText()
                                {
                                    Name = "txtRequests",
                                    Idle = new UIIdleText()
                                    {
                                        Float = Float.MIDDLE_CENTER,
                                        Width = Styles.Screen_Width_Four_Fifth,
                                        Alignment = TextAnchor.MiddleLeft,
                                        Font = Media.fontExoLight,
                                        FontColor = Media.colorGreyDark,
                                        FontSize = Styles.Font_Size_Larger,
                                        LineHeight = 1,
                                        Text = "Friend Requests" + ((Variables.Friend_Request_Count == 0)?"":" (" + Variables.Friend_Request_Count + ")")
                                    }
                                },
                                new UIImage()
                                {
                                    Name = "imgRequests",
                                    Idle = new UIIdleImage()
                                    {
                                        Float = Float.MIDDLE_RIGHT,
                                        Width = Styles.Side_Medium_Bar_Icon,
                                        Height = Styles.Side_Medium_Bar_Icon,
                                        BackgroundColor = Media.colorGreyLight,
                                        Url = "Images/icon_add"
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

                                            new UIPageRenderer<listFriendRequestsItem>()
                                            {
                                                Material = new UIPageRendererMaterial<listFriendRequestsItem>()
                                                {
                                                    Page = new pageFriendRequests(UIDirection.FROM_RIGHT,UIPageHeight.TALL),
                                                    List = "listFriendRequests",
                                                    ListUrl = Config.URLs.Friend_Requests_Page,                                                    
                                                    OnConversion = Conversions.Friend_Requests                                        
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
                            Name = "clckAbout",
                            Idle = new UIIdlePanel()
                            {
                                Float = Float.BOTTOM_CENTER,
                                Width = Styles.Screen_Width_Four_Fifth,
                                Height = Styles.Height_Bar_Tall,
                                Bottom = Styles.Height_Bar_Tall * 2,
                                BackgroundColor = Media.colorWhite
                            },
                            Components = new List<UIComponent>()
                            {
                                new UIPanel()
                                {
                                    Name = "pnlLine",
                                    Idle = new UIIdlePanel()
                                    {
                                        Float = Float.BOTTOM_CENTER,
                                        Width = Styles.Screen_Width_Five_Sixth,
                                        Height = 1,
                                        BackgroundColor = Media.colorGreyLight
                                    }
                                },
                                new UIText()
                                {
                                    Name = "txtAbout",
                                    Idle = new UIIdleText()
                                    {
                                        Float = Float.MIDDLE_CENTER,
                                        Width = Styles.Screen_Width_Four_Fifth,
                                        Alignment = TextAnchor.MiddleLeft,
                                        Font = Media.fontExoLight,
                                        FontColor = Media.colorGreyDark,
                                        FontSize = Styles.Font_Size_Larger,
                                        LineHeight = 1,
                                        Text = "About"
                                    }
                                },
                                new UIImage()
                                {
                                    Name = "imgAbout",
                                    Idle = new UIIdleImage()
                                    {
                                        Float = Float.MIDDLE_RIGHT,
                                        Width = Styles.Side_Medium_Bar_Icon,
                                        Height = Styles.Side_Medium_Bar_Icon,
                                        BackgroundColor = Media.colorGreyLight,
                                        Url = "Images/icon_about"
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
                                                     Page = new pageAbout_1( UIDirection.FROM_RIGHT, UIPageHeight.TALL)
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
                            Name = "clckContact",
                            Idle = new UIIdlePanel()
                            {
                                Float = Float.BOTTOM_CENTER,
                                Width = Styles.Screen_Width_Four_Fifth,
                                Height = Styles.Height_Bar_Tall,
                                Bottom = Styles.Height_Bar_Tall,
                                BackgroundColor = Media.colorWhite
                            },
                            Components = new List<UIComponent>()
                            {
                                new UIPanel()
                                {
                                    Name = "pnlLine",
                                    Idle = new UIIdlePanel()
                                    {
                                        Float = Float.BOTTOM_CENTER,
                                        Width = Styles.Screen_Width_Five_Sixth,
                                        Height = 1,
                                        BackgroundColor = Media.colorGreyLight
                                    }
                                },
                                new UIText()
                                {
                                    Name = "txtContact",
                                    Idle = new UIIdleText()
                                    {
                                        Float = Float.MIDDLE_CENTER,
                                        Width = Styles.Screen_Width_Four_Fifth,
                                        Alignment = TextAnchor.MiddleLeft,
                                        Font = Media.fontExoLight,
                                        FontColor = Media.colorGreyDark,
                                        FontSize = Styles.Font_Size_Larger,
                                        LineHeight = 1,
                                        Text = "Contact"
                                    }
                                },
                                new UIImage()
                                {
                                    Name = "imgContact",
                                    Idle = new UIIdleImage()
                                    {
                                        Float = Float.MIDDLE_RIGHT,
                                        Width = Styles.Side_Medium_Bar_Icon,
                                        Height = Styles.Side_Medium_Bar_Icon,
                                        BackgroundColor = Media.colorGreyLight,
                                        Url = "Images/icon_contact"
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
                                            Application.OpenURL("mailto:info@letsmojoapp.com");
                                        }
                                    }
                                };
                            }
                        },
                        new UIPanel()
                        {
                            Name = "clckLogout",
                            Idle = new UIIdlePanel()
                            {
                                Float = Float.BOTTOM_CENTER,
                                Width = Styles.Screen_Width_Four_Fifth,
                                Height = Styles.Height_Bar_Tall,
                                BackgroundColor = Media.colorWhite
                            },
                            Components = new List<UIComponent>()
                            {
                                new UIText()
                                {
                                    Name = "txtLogout",
                                    Idle = new UIIdleText()
                                    {
                                        Float = Float.MIDDLE_CENTER,
                                        Width = Styles.Screen_Width_Four_Fifth,
                                        Alignment = TextAnchor.MiddleLeft,
                                        Font = Media.fontExoLight,
                                        FontColor = Media.colorGreyDark,
                                        FontSize = Styles.Font_Size_Larger,
                                        LineHeight = 1,
                                        Text = "Logout"
                                    }
                                },
                                new UIImage()
                                {
                                    Name = "imgLogout",
                                    Idle = new UIIdleImage()
                                    {
                                        Float = Float.MIDDLE_RIGHT,
                                        Width = Styles.Side_Medium_Bar_Icon,
                                        Height = Styles.Side_Medium_Bar_Icon,
                                        BackgroundColor = Media.colorGreyLight,
                                        Url = "Images/icon_logout"
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
                                                    Page = new pageLogout(UIDirection.FROM_RIGHT, UIPageHeight.TALL)
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
