using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Json;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Agents.Tools;
using Assets.Scripts.Core;
using Assets.Scripts.Repository;
using Assets.Scripts.UI.ListItems;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Agents.Renderers
{
    public class UIListRenderer<ItemType> : CoreServant<UIListRendererMaterial<ItemType>> where ItemType : listItem
    {
        public void Render()
        {
            Perform();
        }

        protected override void Job()
        {
            UIList<ItemType> list = Variables.UI[Material.List] as UIList<ItemType>;
            UIComponent item;

            if (list.Items == null)
                list.Items = new List<UIComponent>();
            else
            {
                foreach (UIComponent component in list.Items)
                {
                    component.Destroy();
                }

                list.Items = new List<UIComponent>();
            }

            UIIdleList idle = list.Idle as UIIdleList;

            Action<int> renderItem = null;

            renderItem = (i) =>
            {
                if (i != idle.MaxItems && i < list.Data.Count)
                {
                    item = list.OnPopulate(i, list.Data[i]);
                    item.Name = list.Name + "Item_" + i;                    
                    list.Items.Add(item);

                    new UIRenderer()
                    {
                        Material = new UIRendererMaterial()
                        {
                            Parent = list.Container.Object,
                            Component = item                            
                        },
                        OnFinish = () => { renderItem(i + 1); }                        
                    }
                    .Render();
                }
                else
                {
                    float offset = 0, height = 0, prevHeight = 0, totalHeight = 0;

                    for (int j = 0;j < list.Items.Count;j++)
                    {
                        height = list.Items[j].Idle.Height + list.Items[j].Idle.Padding;

                        if (j == 0)
                        {
                            prevHeight = 0;
                            offset = 0;
                        }
                        else
                        {
                            prevHeight = list.Items[j - 1].Rect.sizeDelta.y;
                            offset = list.Items[j - 1].Rect.anchoredPosition.y - prevHeight;
                        }

                        list.Items[j].Rect.sizeDelta = new Vector2(list.Items[j].Rect.sizeDelta.x, height);
                        list.Items[j].Rect.anchoredPosition = new Vector2(0, offset + list.Items[j].Idle.Bottom - list.Items[j].Idle.Top);
                    }

                    totalHeight = -offset + height + Screen.height * 0.25f;

                    list.Container.Rect.sizeDelta = new Vector2(Screen.width, totalHeight);
                    list.Container.Rect.anchoredPosition = new Vector2(0, -list.Items[0].Rect.anchoredPosition.y - Screen.height * 0.01f);

                    if (OnFinish != null)
                        OnFinish();
                }
            };

            bool fromCache = false;

            string url = Material.ListUrl;

            if (Material.ListFields != null)
                foreach (string key in Material.ListFields.Keys)
                {
                    url += "/" + Material.ListFields[key];
                }

            if (Material.FromCache)
            {
                fromCache = Cache.Data.ToList().Exists(c => c.Key == url);
            }

            if (fromCache)
            {
                string json = Cache.Data[url];

                list.Data = Material.OnConversion(json);

                if (list.Data.Count != 0)
                    renderItem(0);
                else
                {
                    if (OnFinish != null)
                        OnFinish();
                }
            }
            else
            {
                Dictionary<string, string> fields = new Dictionary<string, string>();

                fields["appInstanceKey"] = Variables.App_Instance_Key;

                if (Material.ListFields != null)
                    foreach (string key in Material.ListFields.Keys)
                    {
                        fields[key] = Material.ListFields[key];
                    }

                new HttpPostRequestSender()
                {
                    Material = new HttpPostRequestSenderMaterial()
                    {
                        Url = Material.ListUrl,
                        Fields = fields
                    },
                    OnSuccess = (www) =>
                    {
                        Cache.Data[url] = www.text;

                        list.Data = Material.OnConversion(www.text);

                        if (list.Data.Count != 0)
                            renderItem(0);
                        else
                        {
                            if (OnFinish != null)
                                OnFinish();
                        }
                    }                    
                }
                .Send();
            }

            if (list.OnEventInitialization != null)
            {
                foreach (EventListener listener in list.OnEventInitialization(list))
                {
                    if (!Events.Listeners.Exists(l => l.Name == listener.Name))
                        Events.Listeners.Add(listener);
                }
            }

            if (list.OnTouchInitialization != null)
            {
                foreach (OnTouchListener listener in list.OnTouchInitialization(list))
                {
                    if (!Events.OnTouch_Listeners.Exists(l => l.Target == listener.Target))
                        Events.OnTouch_Listeners.Add(listener);
                }
            }
        }
    }

    public class UIListRendererMaterial<ItemType> : CoreMaterial where ItemType : listItem
    {
        public string List { get; set; }

        public string ListUrl { get; set; }
        public Dictionary<string, string> ListFields { get; set; }
        public Func<string, List<ItemType>> OnConversion { get; set; }

        public bool FromCache { get; set; }
    }
}
