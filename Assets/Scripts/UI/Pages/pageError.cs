using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Repository;
using UnityEngine;

namespace Assets.Scripts.UI.Pages
{
    public class pageError : UIPage
    {
        public pageError(UIDirection direction, UIPageHeight height) : base(direction, height) { }

        protected override string GiveName()
        {
            return "pageError";
        }

        protected override string GiveTitle()
        {
            return "";
        }

        protected override Dictionary<string, UIState> GenerateStates()
        {
            return new Dictionary<string, UIState>()
            {                
                {
                    "Page_Error",
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
                new UIImage()
                {
                    Name = "imgError",
                    Idle = new UIIdleImage()
                    {
                        Float = Float.MIDDLE_CENTER,
                        Width = Styles.Screen_Width_Three_Quarters,
                        Height = Styles.Screen_Width_Three_Quarters, 
                        Bottom = Styles.Screen_Width_Three_Quarters * 0.1f, 
                        BackgroundColor = Media.colorBlackOneTenthTransparent,
                        Url = "Images/icon_shock"
                    }
                },
                new UIText()
                {
                    Name = "txtSorry",
                    Idle = new UIIdleText()
                    {
                        Float = Float.MIDDLE_CENTER,
                        Width = Styles.Screen_Width_Four_Fifth,
                        Bottom = Styles.Height_Bar_Medium,
                        Alignment = TextAnchor.MiddleCenter,
                        Font = Media.fontExoRegular,
                        FontColor = Media.colorBlack,
                        FontSize = Styles.Font_Size_Largest,
                        LineHeight = 1,
                        Text = "Something went wrong..."
                    }
                },
                new UIText()
                {
                    Name = "txtIssue",
                    Idle = new UIIdleText()
                    {
                        Float = Float.MIDDLE_CENTER,
                        Width = Styles.Screen_Width_Four_Fifth,     
                        Height = Styles.Height_Bar_Medium * 2,
                        Alignment = TextAnchor.MiddleCenter,
                        Font = Media.fontExoLight,
                        FontColor = Media.colorGreyDark,
                        FontSize = Styles.Font_Size_Larger,
                        LineHeight = 1.25f,
                        Text = Variables.Exception_Message,
                        FurtherAccess = true
                    }
                },
                new UIPanel()
                {
                    Name = "clckRestart",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.BOTTOM_CENTER,
                        Width = Styles.Screen_Width_One_Thirds,
                        Height = Styles.Height_Bar_Medium,
                        Bottom = Styles.Height_Bar_Tall,
                        BackgroundColor = Media.colorBlack
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIText()
                        {
                            Name = "txtRestart",
                            Idle = new UIIdleText()
                            {
                                Float = Float.MIDDLE_CENTER,
                                Width = Styles.Screen_Width_One_Thirds,
                                Alignment = TextAnchor.MiddleCenter,
                                Font = Media.fontExoLight,
                                FontColor = Media.colorWhite,
                                FontSize = Styles.Font_Size_Medium,
                                LineHeight = 1,
                                Text = "Restart App"
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
                                    Application.Quit();
                                }
                            }
                        };                        
                    }
                }
            };
        }
    }
}
