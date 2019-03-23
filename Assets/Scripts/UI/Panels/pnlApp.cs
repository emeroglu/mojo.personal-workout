using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Repository;
using Assets.Scripts.UI.Pages;
using Assets.Scripts.UI.Panels;
using UnityEngine;

namespace Assets.Scripts.UI.Panels
{
    public class pnlApp : UIPanel
    {
        public pnlApp(Action<Exception> onFail)
        {
            this.Name = "pnlApp";

            this.Idle = new UIIdlePanel()
            {
                Float = Float.MIDDLE_CENTER,
                Width = Styles.Screen_Width,
                Height = Styles.Screen_Height,
                BackgroundColor = Media.colorGreyExtraLight                
            };

            this.States = new Dictionary<string, UIState>()
            {
                {
                    "View,Previewing",
                    new UIStatePanel()
                    {
                        Width = Styles.Screen_Width,
                        Height = Styles.Screen_Height,
                        Right = Styles.Screen_Width,
                        BackgroundColor = Media.colorGreyExtraLight
                    }
                },
                {
                    "App",
                    new UIStatePanel()
                    {
                        Width = Styles.Screen_Width,
                        Height = Styles.Screen_Height,                        
                        BackgroundColor = Media.colorGreyExtraLight
                    }
                }
            };

            this.Components = new List<UIComponent>()
            {                        
                new pnlTopBar(),
                new pnlBottomBar()
            };
        }
    }
}
