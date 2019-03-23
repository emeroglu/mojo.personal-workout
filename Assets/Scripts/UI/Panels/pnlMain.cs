using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Repository;
using Assets.Scripts.UI.Panels;
using UnityEngine;

namespace Assets.Scripts.UI.Panels
{
    public class pnlMain : UIPanel
    {
        public pnlMain(Action<Exception> onFail)
        {
            this.Name = "pnlMain";

            this.Idle = new UIIdlePanel()
            {
                Float = Float.MIDDLE_CENTER,
                Width = Styles.Screen_Width,
                Height = Styles.Screen_Height,
                BackgroundColor = Media.colorTransparent,
                FurtherAccess = true
            };

            this.Components = new List<UIComponent>()
            {                
                new pnlKeyboard(onFail),                
                new UIPanel()
                {
                    Name = "pnlMask",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.MIDDLE_CENTER,
                        Width = Styles.Screen_Width,
                        Height = Styles.Screen_Height,
                        BackgroundColor = Media.colorTransparent
                    },
                    States = new Dictionary<string,UIState>()
                    {
                        {
                            "Keyboard_Open",
                            new UIStatePanel()
                            {
                                Width = Styles.Screen_Width,
                                Height = Styles.Screen_Height,
                                BackgroundColor = Media.colorBlackTwoThirdsTransparent
                            }
                        },
                        {
                            "Keyboard_Closed",
                            new UIStatePanel()
                            {
                                Width = Styles.Screen_Width,
                                Height = Styles.Screen_Height,
                                BackgroundColor = Media.colorTransparent
                            }
                        }
                    }
                },
                new pnlApp(onFail),
                new pnlSplash()
            };
        }
    }
}
