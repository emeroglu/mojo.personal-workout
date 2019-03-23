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
using UnityEngine;

namespace Assets.Scripts.Agents.Renderers
{
    public class UIPageRenderer<ItemType> : CoreProcessor<UIPageRendererMaterial<ItemType>> where ItemType : listItem
    {
        public void Render()
        {
            Perform();
        }

        protected override bool Condition()
        {
            return true;
        }

        protected override void OnInterruption()
        {

        }

        protected override bool DebugLog()
        {
            return false;
        }

        protected override List<Task> Tasks()
        {
            return new List<Task>()
            {
                Check_Page(),
                Logo_To_Left(),
                Load_Page(),
                Render_Page(),                
                Render_List(),
                Wait(),
                Immediate_Destroy(),
                Broadcast(),
                Destroy()
            };
        }

        private Task Check_Page()
        {
            return new Task()
            {
                Mission = "Check_Page",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (Variables.Current_Page != Material.Page.Name)
                        nextTask(null);
                }
            };
        }

        private Task Logo_To_Left()
        {
            return new Task()
            {
                Mission = "Logo_To_Left",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    (Variables.UI["txtTopRight"] as UIText).Element.text = "Loading...";

                    if (Material.Page.Title == "")
                    {
                        new StateBroadcaster()
                        {
                            Material = new StateBroadcasterMaterial()
                            {
                                States = new List<string>()
                                {
                                    "Mojo_Center",
                                    "Top_Right_Text_Hide"
                                }
                            },
                            OnFinish = () => { nextTask(null); }                            
                        }
                        .Broadcast();
                    }
                    else
                    {
                        new StateBroadcaster()
                        {
                            Material = new StateBroadcasterMaterial()
                            {
                                States = new List<string>()
                                {
                                    "Mojo_To_Left",
                                    "Top_Right_Text_Show"
                                }
                            },
                            OnFinish = () => { nextTask(null); }                            
                        }
                        .Broadcast();
                    }
                }
            };
        }

        private Task Load_Page()
        {
            return new Task()
            {
                Mission = "Load_Page",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (string.IsNullOrEmpty(Material.Url))
                    {
                        nextTask(null);
                    }
                    else
                    {
                        Dictionary<string, string> fields = new Dictionary<string, string>();

                        fields["appInstanceKey"] = Variables.App_Instance_Key;

                        if (Material.Fields != null)
                            foreach (string key in Material.Fields.Keys)
                            {
                                fields[key] = Material.Fields[key];
                            }

                        new HttpPostRequestSender()
                        {
                            Material = new HttpPostRequestSenderMaterial()
                            {
                                Url = Material.Url,
                                Fields = fields
                            },
                            OnSuccess = (www) =>
                            {
                                if (Material.OnPageLoad != null)
                                    Material.OnPageLoad(www.text);

                                if (Material.OnPageRegeneration != null)
                                    Material.Page = Material.OnPageRegeneration();

                                nextTask(null);
                            }
                        }
                        .Send();
                    }
                }
            };
        }

        private Task Render_Page()
        {
            return new Task()
            {
                Mission = "Render_Page",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {                 
                    new UIRenderer()
                    {
                        Material = new UIRendererMaterial()
                        {
                            Parent = Variables.UI["pnlApp"].Object,
                            Component = Material.Page                            
                        },
                        OnFinish = () =>
                        {
                            if (Material.OnPageRender != null)
                                Material.OnPageRender(Material.Page);                            

                            nextTask(null);
                        }                        
                    }
                    .Render();
                }
            };
        }

        private Task Render_List()
        {
            return new Task()
            {
                Mission = "Render_List",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (string.IsNullOrEmpty(Material.List))
                    {
                        nextTask(null);
                    }
                    else
                    {
                        new UIListRenderer<ItemType>()
                        {
                            Material = new UIListRendererMaterial<ItemType>()
                            {
                                List = Material.List,
                                ListUrl = Material.ListUrl,
                                ListFields = (Material.ListFields == null) ? null : Material.ListFields,
                                OnConversion = (Material.OnConversion == null) ? null : Material.OnConversion,
                                FromCache = Material.FromCache
                            },
                            OnFinish = () =>
                            {
                                if (Material.OnListRender != null)
                                    Material.OnListRender(Material.Page, Variables.UI[Material.List] as UIList<ItemType>);

                                nextTask(null);
                            }                            
                        }
                        .Render();
                    }
                }
            };
        }

        private Task Wait()
        {
            return new Task()
            {
                Mission = "Wait",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    new Suspender()
                    {
                        Suspension = 0.25f,
                        OnFinish = () => { nextTask(null); }
                    }
                    .Suspend();
                }
            };
        }

        private Task Immediate_Destroy()
        {
            return new Task()
            {
                Mission = "Immediate_Destroy",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (Material.ImmediateDestroy)
                    {
                        if (Variables.UI.Keys.Contains(Variables.Current_Page))
                            Variables.UI[Variables.Current_Page].Destroy();
                    }

                    nextTask(null);
                }
            };
        }

        private Task Broadcast()
        {
            return new Task()
            {
                Mission = "Broadcast",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    new StateBroadcaster()
                    {
                        Material = new StateBroadcasterMaterial()
                        {
                            State = "Page_" + Material.Page.Name.Replace("page", "")
                        },
                        OnFinish = () =>
                        {
                            nextTask(null);
                        }                        
                    }
                    .Broadcast();
                }
            };
        }

        private Task Destroy()
        {
            return new Task()
            {
                Mission = "Destroy",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (!Material.ImmediateDestroy)
                    {
                        if (Variables.UI.Keys.Contains(Variables.Current_Page))
                            Variables.UI[Variables.Current_Page].Destroy();
                    }

                    Variables.Current_Page = Material.Page.Name;

                    Directives.Sense_Touch = true;

                    (Variables.UI["txtTopRight"] as UIText).Element.text = Material.Page.Title;

                    if (OnFinish != null)
                        OnFinish();

                    Dispose();
                }
            };
        }

    }

    public class UIPageRendererMaterial<ItemType> : UIListRendererMaterial<ItemType> where ItemType : listItem
    {
        public UIPage Page { get; set; }
        public string Url { get; set; }
        public Dictionary<string, string> Fields { get; set; }        

        public Action<string> OnPageLoad { get; set; }
        public Func<UIPage> OnPageRegeneration { get; set; }
        public Action<UIPage> OnPageRender { get; set; }
        public Action<UIPage, UIList<ItemType>> OnListRender { get; set; }

        public bool ImmediateDestroy { get; set; }
    }
}
