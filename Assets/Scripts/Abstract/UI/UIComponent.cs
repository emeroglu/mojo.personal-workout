using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Json;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Agents.Tools;
using Assets.Scripts.Repository;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Abstract.UI
{
    public abstract class UIComponent
    {
        public string Name { get; set; }

        public UIType Type { get; set; }

        public GameObject Object { get; set; }
        public RectTransform Rect { get; set; }
        public RawImage Background { get; set; }

        public UIIdle Idle { get; set; }

        public bool Switching { get; set; }        
        public Dictionary<string, UIState> States { get; set; }

        public List<UIComponent> Components { get; set; }

        public Func<UIComponent, List<EventListener>> OnEventInitialization { get; set; }
        public Func<UIComponent, List<OnTouchListener>> OnTouchInitialization { get; set; }

        public void Destroy()
        {
            try
            {                
                Events.Listeners.Where(l => l.Owner == Name).ToList().ForEach
                (
                    (l) => { l.Remove = true; }
                );

                Events.OnTouch_Listeners.Where(o => o.Owner == Name).ToList().ForEach
                (
                    (o) => { o.Remove = true; }
                );

                if (this.Type == UIType.LIST)
                {
                    List<string> keys = Variables.UI.Keys.Where(k => k.Contains(this.Name + "Item_")).ToList();

                    foreach (string key in keys)
                    {
                        Variables.UI[key].Destroy();
                    }
                }
                else
                {
                    foreach (UIComponent component in this.Components)
                    {
                        component.Destroy();
                    }
                }

                Variables.UI.Remove(Name);

                MonoBridge.Destroy(Object);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
