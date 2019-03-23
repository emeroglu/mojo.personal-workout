using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Agents.Renderers;
using Assets.Scripts.Repository;
using Assets.Scripts.UI.ListItems;
using Assets.Scripts.UI.Lists;
using UnityEngine;

namespace Assets.Scripts.UI.Pages
{
    public class pageFriendRequests : UIPage
    {        
        public pageFriendRequests(UIDirection direction, UIPageHeight height) : base(direction, height) { }

        protected override string GiveName()
        {
            return "pageFriendRequests";
        }

        protected override string GiveTitle()
        {
            return "Friend Requests";
        }

        protected override Dictionary<string, UIState> GenerateStates()
        {
            return new Dictionary<string, UIState>()
            {
                {
                    "Page_FriendRequests",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page_Full,
                        Height = Styles.Height_Page_With_Nav_Bar,
                        Top = Styles.Top_Page,                        
                        BackgroundColor = Media.colorTransparent                        
                    }
                },
                {
                    "Page_Profile",
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
                    "Page_Error",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page_Full,
                        Height = Styles.Height_Page_With_Nav_Bar,
                        Top = Styles.Top_Page, 
                        Right = Styles.Screen_Width,
                        BackgroundColor = Media.colorTransparent                        
                    }
                }
            };
        }

        protected override List<UIComponent> GenerateComponents()
        {
            return new List<UIComponent>()
            {
                new listFriendRequests(),
                new UIPanel()
                {
                    Name = "clckBack",
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
