using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Agents.Renderers;
using Assets.Scripts.Agents.Tools;
using Assets.Scripts.Repository;
using Assets.Scripts.UI.ListItems;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Pages
{
    public class pageLogout : UIPage
    {
        public pageLogout(UIDirection direction, UIPageHeight height) : base(direction, height) { }

        protected override string GiveName()
        {
            return "pageLogout";
        }

        protected override string GiveTitle()
        {
            return "Logout";
        }

        protected override Dictionary<string, UIState> GenerateStates()
        {
            return new Dictionary<string, UIState>()
            {
                {
                    "Page_Profile",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page,
                        Height = Styles.Height_Page_With_Nav_Bar,
                        Top = Styles.Top_Page,
                        Left = Styles.Screen_Width,
                        BackgroundColor = Media.colorWhite                        
                    }
                },
                {
                    "Page_Error",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page,
                        Height = Styles.Height_Page_With_Nav_Bar,
                        Top = Styles.Top_Page,
                        Right = Styles.Screen_Width,
                        BackgroundColor = Media.colorWhite                        
                    }
                },
                {
                    "Page_Logout",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page,
                        Height = Styles.Height_Page_With_Nav_Bar,
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
                    Name = "pnlFacebook",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.TOP_CENTER,
                        Width = Styles.Width_Page,
                        Height = Styles.Width_Page * 0.25f,
                        Top = Styles.Height_Bar_Short,
                        BackgroundColor = Media.colorTransparent
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIImage()
                        {
                            Name = "imgFacebook",
                            Idle = new UIIdleImage()
                            {
                                Float = Float.BOTTOM_CENTER,
                                Width = Styles.Width_Page * 0.25f,
                                Height = Styles.Width_Page * 0.25f,                                
                                BackgroundColor = Media.colorWhite,
                                Url = "Images/icon_facebook"
                            }
                        }
                    }
                },
                new UIPanel()
                {
                    Name = "pnlInfo",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.MIDDLE_CENTER,
                        Width = Styles.Screen_Width_Three_Quarters,
                        Height = Styles.Height_Page * 0.5f,                        
                        BackgroundColor = Media.colorTransparent
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIText()
                        {
                            Name = "txtInfo",
                            Idle = new UIIdleText()
                            {
                                Float = Float.MIDDLE_CENTER,
                                Width = Styles.Screen_Width_Three_Quarters,
                                Alignment = TextAnchor.MiddleCenter,
                                Font = Media.fontExoLight,
                                FontColor = Media.colorGrey,
                                FontSize = Styles.Font_Size_Large,
                                LineHeight = 1.25f,
                                Text = "Mojo uses Facebook Login...\n\nThat means, even if we log you out from the Mojo app, Facebook will re-login you as " + Variables.Self.name + " when you come back as long as you stay logged-in to Facebook on this device.\n\nIf you want to switch to another Mojo account, you need to log out from the Mojo app, then log out from Facebook and open Mojo again...\n\nCheers...:)"
                            }
                        }
                    }
                },
                new UIPanel()
                {
                    Name = "clckLogmeout",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.BOTTOM_CENTER,
                        Width = Styles.Screen_Width_Half,
                        Height = Styles.Height_Bar_Medium,
                        Bottom = Styles.Height_Bar_Tall,
                        BackgroundColor = Media.colorBlack
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIText()
                        {
                            Name = "txtLogmeout",
                            Idle = new UIIdleText()
                            {
                                Float = Float.MIDDLE_CENTER,
                                Width = Styles.Screen_Width_Half,
                                Height = Styles.Height_Bar_Medium,
                                Alignment = TextAnchor.MiddleCenter,
                                Font = Media.fontExoLight,
                                FontColor = Media.colorWhite,
                                FontSize = Styles.Font_Size_Medium,
                                LineHeight = 1,
                                Text = "Log me out"
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
                                    Action ifInitialized = () =>
                                    {
                                        FB.LogOut();
                                        File.Delete(Config.Directories.Identity);

                                        new HttpPostRequestSender()
                                        {
                                            Material = new HttpPostRequestSenderMaterial()
                                            {
                                                Url = Config.URLs.Logout,
                                                Fields = new Dictionary<string,string>()
                                                {
                                                    { "appInstanceKey", Variables.App_Instance_Key },
                                                }                                                
                                            },
                                            OnSuccess = (www) =>
                                            {
                                                new Suspender()
                                                {
                                                    Suspension = 1,
                                                    OnFinish = () =>
                                                    {
                                                        Application.Quit();
                                                    }
                                                }
                                                .Suspend();
                                            }
                                        }
                                        .Send();                                        
                                    };

                                    (Variables.UI["txtTopRight"] as UIText).Element.text = "Logging you out...";

                                    if (FB.IsInitialized)
                                        ifInitialized();
                                    else
                                        FB.Init(() => { ifInitialized(); });
                                }
                            }
                        };
                    }
                },
                new UIPanel()
                {
                    Name = "clckLogoutBack",
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
