using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Json;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Repository;
using Assets.Scripts.UI.Lists;
using UnityEngine;

namespace Assets.Scripts.UI.Pages
{
    public class pageMain : UIPage
    {        
        public pageMain(UIDirection direction, UIPageHeight height) : base(direction, height) { }

        protected override string GiveName()
        {
            Events.OnTouch_Listeners.FirstOrDefault(e => e.Target == "%clckBottom_").Enabled = true;

            Variables.Current_Mojo = new JsonMojo();

            return "pageMain";
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
                    "Page_Main",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page,
                        Height = Styles.Height_Page,
                        Top = Styles.Top_Page,                        
                        BackgroundColor = Media.colorWhite                        
                    }
                },
                {
                    "Page_Inbox",
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
                    "Page_Profile,Page_Target,Page_Characters,Page_Soundtracks,Page_Logout,Page_Error,Page_FriendRequests,Page_About_1",
                    new UIStatePage()
                    {
                        Width = Styles.Width_Page,
                        Height = Styles.Height_Page,
                        Top = Styles.Top_Page, 
                        Right = Styles.Screen_Width,
                        BackgroundColor = Media.colorWhite                        
                    }
                }
            };
        }

        protected override List<UIComponent> GenerateComponents()
        {
            return new List<UIComponent>()
            {                
                new listMain()                
            };
        }       
    }
}
