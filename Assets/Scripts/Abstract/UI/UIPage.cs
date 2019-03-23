using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Repository;
using UnityEngine;

namespace Assets.Scripts.Abstract.UI
{
    public abstract class UIPage : UIComponent
    {
        protected bool centered;        

        public UIPage(UIDirection direction,UIPageHeight height)
        {
            this.Name = GiveName();

            if (direction == UIDirection.FROM_LEFT)
            {
                this.Idle = new UIIdlePage()
                {
                    Float = Float.TOP_CENTER,
                    Width = Styles.Width_Page,                    
                    Top = Styles.Top_Page,
                    Right = Styles.Screen_Width,
                    BackgroundColor = Media.colorWhite
                };
            }
            else if (direction == UIDirection.FROM_RIGHT)
            {
                this.Idle = new UIIdlePage()
                {
                    Float = Float.TOP_CENTER,
                    Width = Styles.Width_Page,                    
                    Top = Styles.Top_Page,
                    Left = Styles.Screen_Width,
                    BackgroundColor = Media.colorWhite
                };
            }
            else if (direction == UIDirection.CENTER)
            {
                this.Idle = new UIIdlePage()
                {
                    Float = Float.TOP_CENTER,
                    Width = Styles.Width_Page,                    
                    Top = Styles.Top_Page,                    
                    BackgroundColor = Media.colorWhite
                };
            }

            if (height == UIPageHeight.NORMAL)
                this.Idle.Height = Styles.Height_Page;
            else if (height == UIPageHeight.TALL)
                this.Idle.Height = Styles.Height_Page_With_Nav_Bar;

            this.States = GenerateStates();
            this.Components = GenerateComponents();
        }       

        public string Title { get { return GiveTitle(); } }

        protected abstract string GiveName();
        protected abstract string GiveTitle();        
        protected abstract Dictionary<string, UIState> GenerateStates();
        protected abstract List<UIComponent> GenerateComponents();        
    }
}
