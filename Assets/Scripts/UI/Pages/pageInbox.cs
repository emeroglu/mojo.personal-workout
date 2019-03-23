using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Repository;
using Assets.Scripts.UI.Lists;
using UnityEngine;

namespace Assets.Scripts.UI.Pages
{
    public class pageInbox : UIPage
    {        
        public pageInbox(UIDirection direction, UIPageHeight height) : base(direction, height) { }

        protected override string GiveName()
        {
            return "pageInbox";
        }

        protected override string GiveTitle()
        {
            return "Inbox";
        }

        protected override Dictionary<string, UIState> GenerateStates()
        {
            return new Dictionary<string, UIState>()
            {
                {
                    "Page_Inbox",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page,
                        Height = Styles.Height_Page,
                        Top = Styles.Top_Page,                        
                        BackgroundColor = Media.colorTransparent                        
                    }
                },
                {
                    "Page_Profile,Page_Target,Page_Main,Page_Soundtracks,Page_Error",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page,
                        Height = Styles.Height_Page,
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
                new listInbox()
            };
        }
    }
}
