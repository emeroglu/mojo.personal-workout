using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Repository;
using UnityEngine;

namespace Assets.Scripts.UI.Panels
{
    public class pnlTopBar : UIPanel
    {
        public pnlTopBar()
        {
            this.Name = "pnlTopBar";

            this.Idle = new UIIdlePanel()
            {
                Float = Float.TOP_CENTER,
                Width = Styles.Screen_Width,
                Height = Styles.Height_Bar_Medium,
                BackgroundColor = Media.colorBlack
            };

            this.Components = new List<UIComponent>()
            {   
                new UIText()
                {
                    Name = "txtMojo",
                    Idle = new UIIdleText()
                    {
                        Float = Float.MIDDLE_CENTER,
                        Width = Styles.Screen_Width_Half,                           
                        Alignment = TextAnchor.MiddleCenter,
                        Font = Media.fontExoRegular,
                        FontColor = Media.colorWhite,
                        FontSize = Styles.Font_Size_Largest * 1.25f,
                        LineHeight = 1,
                        Text = "mojo"
                    },
                    States = new Dictionary<string,UIState>()
                    {
                        {
                            "Mojo_Center",
                            new UIStateText()
                            {
                                Width = Styles.Screen_Width_Half,                                                                
                                FontColor = Media.colorWhite                                
                            }
                        },
                        {
                            "Mojo_To_Left",
                            new UIStateText()
                            {
                                Width = Styles.Screen_Width_Half, 
                                Right = Styles.Screen_Width_Quarter * 1.5f,                                
                                FontColor = Media.colorWhite                                
                            }
                        }
                    }
                },
                new UIText()
                {
                    Name = "txtTopRight",
                    Idle = new UIIdleText()
                    {
                        Float = Float.MIDDLE_RIGHT,
                        Width = Styles.Screen_Width_Half,
                        Left = Styles.Screen_Width_Half,   
                        Alignment = TextAnchor.MiddleRight,
                        Font = Media.fontExoLight,
                        FontColor = Media.colorTransparent,
                        FontSize = Styles.Font_Size_Medium,
                        LineHeight = 1,
                        Text = ""
                    },
                    States = new Dictionary<string,UIState>()
                    {
                        {
                            "Top_Right_Text_Hide",
                            new UIStateText()
                            {
                                Width = Styles.Screen_Width_Half,  
                                Left = Styles.Screen_Width_Half,                              
                                FontColor = Media.colorTransparent                                
                            }
                        },
                        {
                            "Top_Right_Text_Show",
                            new UIStateText()
                            {
                                Width = Styles.Screen_Width_Half, 
                                Right = Styles.Right_Medium_Bar_Icon,                                
                                FontColor = Media.colorWhite                                
                            }
                        }
                    }
                }
            };
        }
    }
}
