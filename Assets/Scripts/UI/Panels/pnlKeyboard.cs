using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Agents.Tools;
using Assets.Scripts.Repository;
using UnityEngine;

namespace Assets.Scripts.UI.Panels
{
    public class pnlKeyboard : UIPanel
    {
        public pnlKeyboard(Action<Exception> onFail)
        {
            this.Name = "pnlKeyboard";

            this.Idle = new UIIdlePanel()
            {
                Float = Float.BOTTOM_CENTER,
                Width = Screen.width,
                Height = Screen.height * 0.275f,
                Top = Screen.height * 0.275f,
                BackgroundColor = Media.colorGreyExtraLight
            };

            this.States = new Dictionary<string, UIState>()
            {
                {
                    "Keyboard_Open",
                    new UIStatePanel()
                    {
                        Width = Screen.width,
                        Height = Screen.height * 0.275f,  
                        Top = 0,
                        BackgroundColor = Media.colorGreyExtraLight                        
                    }
                },
                {
                    "Keyboard_Closed",
                    new UIStatePanel()
                    {
                        Width = Screen.width,
                        Height = Screen.height * 0.275f,   
                        Top = Screen.height * 0.275f,
                        BackgroundColor = Media.colorGreyExtraLight                        
                    }
                }
            };

            this.Components = new List<UIComponent>()
            {                    
                topRowKey(1,"Q"),
                topRowKey(2,"W"),
                topRowKey(3,"E"),
                topRowKey(4,"R"),
                topRowKey(5,"T"),
                topRowKey(6,"Y"),
                topRowKey(7,"U"),
                topRowKey(8,"I"),
                topRowKey(9,"O"),
                topRowKey(10,"P"),
                middleRowKey(1,"A"),
                middleRowKey(2,"S"),
                middleRowKey(3,"D"),
                middleRowKey(4,"F"),
                middleRowKey(5,"G"),
                middleRowKey(6,"H"),
                middleRowKey(7,"J"),
                middleRowKey(8,"K"),
                middleRowKey(9,"L"),
                bottomRowKey(1,"Z"),
                bottomRowKey(2,"X"),
                bottomRowKey(3,"C"),
                bottomRowKey(4,"V"),
                bottomRowKey(5,"B"),
                bottomRowKey(6,"N"),
                bottomRowKey(7,"M"),
                backspace(),
                space(),
                enter()                
            };

            new EventRegistrar()
            {
                Material = new EventRegistrarMaterial()
                {
                    OnTouchListener = new OnTouchListener()
                    {
                        Owner = this.Name,
                        Target = "%pnlKey_",
                        Enabled = false,
                        Released = true,
                        OnTouch = (go) =>
                        {
                            string key = go.name.Split('_')[1].ToLower();

                            Objects.ClickSource.Play();

                            if (key == "backspace")
                            {
                                if (!string.IsNullOrEmpty(Variables.Keyboard_Text))
                                    Variables.Keyboard_Text = Variables.Keyboard_Text.Substring(0, Variables.Keyboard_Text.Length - 1);
                            }
                            else if (key == "space")
                            {
                                Variables.Keyboard_Text += " ";
                            }
                            else if (key == "enter")
                            {
                                Directives.Keyboard_Open = false;

                                Events.OnTouch_Listeners.FirstOrDefault(o => o.Target == "%pnlKey_").Enabled = false;
                            }
                            else
                            {
                                Variables.Keyboard_Text += key;
                            }
                        }
                    }
                }                
            }
            .Register();

        }

        private UIPanel topRowKey(int index, string letter)
        {
            return new UIPanel()
            {
                Name = "pnlKey_" + letter,
                Idle = new UIIdlePanel()
                {
                    Float = Float.TOP_LEFT,
                    Width = Screen.width * 0.085f,
                    Height = Screen.width * 0.085f,
                    Top = Screen.width * 0.05f,
                    Left = (index * Screen.width * 0.014f) + ((index - 1) * Screen.width * 0.085f),
                    BackgroundColor = Media.colorWhite
                },
                Components = new List<UIComponent>()
                {
                    new UIText()
                    {
                        Name = "txt_" + letter,
                        Idle = new UIIdleText()
                        {
                            Float = Float.MIDDLE_CENTER,
                            Width = Screen.width * 0.1f,
                            Alignment = TextAnchor.MiddleCenter,
                            Font = Media.fontExoLight,
                            FontColor = Media.colorGreyDark,
                            FontSize = 0.045f,
                            LineHeight = 1,
                            Text = letter
                        }
                    }
                }
            };
        }

        private UIPanel middleRowKey(int index, string letter)
        {
            return new UIPanel()
            {
                Name = "pnlKey_" + letter,
                Idle = new UIIdlePanel()
                {
                    Float = Float.TOP_LEFT,
                    Width = Screen.width * 0.085f,
                    Height = Screen.width * 0.085f,
                    Top = Screen.width * 0.15f,
                    Left = Screen.width * 0.055f + (index * Screen.width * 0.014f) + ((index - 1) * Screen.width * 0.085f),
                    BackgroundColor = Media.colorWhite
                },
                Components = new List<UIComponent>()
                {
                    new UIText()
                    {
                        Name = "txt_" + letter,
                        Idle = new UIIdleText()
                        {
                            Float = Float.MIDDLE_CENTER,
                            Width = Screen.width * 0.1f,
                            Alignment = TextAnchor.MiddleCenter,
                            Font = Media.fontExoLight,
                            FontColor = Media.colorGreyDark,
                            FontSize = 0.045f,
                            LineHeight = 1,
                            Text = letter
                        }
                    }
                }
            };
        }

        private UIPanel bottomRowKey(int index, string letter)
        {
            return new UIPanel()
            {
                Name = "pnlKey_" + letter,
                Idle = new UIIdlePanel()
                {
                    Float = Float.TOP_LEFT,
                    Width = Screen.width * 0.085f * ((index == 8) ? 2 : 1),
                    Height = Screen.width * 0.085f,
                    Top = Screen.width * 0.25f,
                    Left = Screen.width * 0.082f + (index * Screen.width * 0.014f) + ((index - 1) * Screen.width * 0.085f),
                    BackgroundColor = Media.colorWhite
                },
                Components = new List<UIComponent>()
                {
                    new UIText()
                    {
                        Name = "txt_" + letter,
                        Idle = new UIIdleText()
                        {
                            Float = Float.MIDDLE_CENTER,
                            Width = Screen.width * 0.1f,
                            Alignment = TextAnchor.MiddleCenter,
                            Font = Media.fontExoLight,
                            FontColor = Media.colorGreyDark,
                            FontSize = 0.045f,
                            LineHeight = 1,
                            Text = letter
                        }
                    }
                }
            };
        }

        private UIPanel backspace()
        {
            return new UIPanel()
            {
                Name = "pnlKey_backspace",
                Idle = new UIIdlePanel()
                {
                    Float = Float.TOP_LEFT,
                    Width = Screen.width * 0.085f * 1.5f,
                    Height = Screen.width * 0.085f,
                    Top = Screen.width * 0.25f,
                    Left = Screen.width * 0.082f + (8 * Screen.width * 0.014f) + (7 * Screen.width * 0.085f),
                    BackgroundColor = Media.colorWhite
                },
                Components = new List<UIComponent>()
                {
                    new UIImage()
                    {
                        Name = "img_backspace",
                        Idle = new UIIdleImage()
                        {
                            Float = Float.MIDDLE_CENTER,
                            Width = Screen.width * 0.045f,
                            Height = Screen.width * 0.045f,
                            BackgroundColor = Media.colorGreyDark,
                            Url = "Images/icon_backspace"
                        }
                    }
                }
            };
        }

        private UIPanel enter()
        {
            return new UIPanel()
            {
                Name = "pnlKey_enter",
                Idle = new UIIdlePanel()
                {
                    Float = Float.TOP_LEFT,
                    Width = Screen.width * 0.085f * 1.5f,
                    Height = Screen.width * 0.085f,
                    Top = Screen.width * 0.35f,
                    Left = Screen.width * 0.725f,
                    BackgroundColor = Media.colorWhite
                },
                Components = new List<UIComponent>()
                {
                    new UIImage()
                    {
                        Name = "img_enter",
                        Idle = new UIIdleImage()
                        {
                            Float = Float.MIDDLE_CENTER,
                            Width = Screen.width * 0.045f,
                            Height = Screen.width * 0.045f,
                            BackgroundColor = Media.colorGreyDark,
                            Url = "Images/icon_enter"
                        }
                    }
                }
            };
        }

        private UIPanel space()
        {
            return new UIPanel()
            {
                Name = "pnlKey_space",
                Idle = new UIIdlePanel()
                {
                    Float = Float.TOP_LEFT,
                    Width = Screen.width * 0.4f,
                    Height = Screen.width * 0.085f,
                    Top = Screen.width * 0.35f,
                    Left = Screen.width * 0.3f,
                    BackgroundColor = Media.colorWhite
                },
                Components = new List<UIComponent>()
                {
                    new UIText()
                    {
                        Name = "txt_space",
                        Idle = new UIIdleText()
                        {
                            Float = Float.MIDDLE_CENTER,
                            Width = Screen.width * 0.45f,
                            Alignment = TextAnchor.MiddleCenter,
                            Font = Media.fontExoLight,
                            FontColor = Media.colorGreyDark,
                            FontSize = 0.045f,
                            LineHeight = 1,
                            Text = "space"
                        }
                    }
                }
            };
        }
    }
}
