using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Json;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Agents.Renderers;
using Assets.Scripts.Repository;
using Assets.Scripts.UI.ListItems;
using Assets.Scripts.UI.Lists;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.UI.Pages
{
    public class pageSoundtracks : UIPage
    {
        public pageSoundtracks(UIDirection direction, UIPageHeight height) : base(direction, height) { }

        protected override string GiveName()
        {
            return "pageSoundtracks";
        }

        protected override string GiveTitle()
        {
            return "Select a Soundtrack";
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
                        Height = Styles.Height_Page_With_Nav_Bar,
                        Top = Styles.Top_Page,
                        Left = Styles.Screen_Width,
                        BackgroundColor = Media.colorWhite                        
                    }
                },
                {
                    "Page_Profile,Page_Characters,Page_Message,Page_Error",
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
                    "Page_Soundtracks",
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
                new listSoundtracks(),
                new UIPanel()
                {
                    Name = "pnlAudiosPageBottomBar",
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
                            Name = "clckAudiosCancel",
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
                                    Name = "txtAudiosCancel",
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
                            Name = "clckAudiosNext",
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
                                    Name = "txtAudiosNext",
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
                                    Name = "imgAudiosNextArrow",
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
                            }
                        }
                    }
                }
            };
        }
    }
}
