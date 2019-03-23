using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Abstract.Json;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Agents.Tools;
using Assets.Scripts.Core;
using Assets.Scripts.Repository;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Agents.Renderers
{
    public class UIRenderer : CoreProcessor<UIRendererMaterial>
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
                Add_Rect(),
                Anchor(),
                Render_Page(),
                Render_Panel(),
                Render_Image(),
                Render_Text(),
                Render_List(),
                Click_Event(),
                Render_Components()
            };
        }

        private Task Add_Rect()
        {
            return new Task()
            {
                Mission = "Add_Rect",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    Material.Component.Object = new GameObject(Material.Component.Name);
                    Material.Component.Object.transform.parent = Material.Parent.transform;

                    if (Material.FirstSibling)
                        Material.Component.Object.transform.SetAsFirstSibling();

                    Material.Component.Rect = Material.Component.Object.AddComponent<RectTransform>();

                    nextTask(null);
                }
            };
        }

        private Task Anchor()
        {
            return new Task()
            {
                Mission = "Anchor",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (Material.Component.Idle.Float == Float.TOP_LEFT)
                    {
                        Material.Component.Rect.anchorMin = new Vector2(0, 1);
                        Material.Component.Rect.anchorMax = new Vector2(0, 1);
                        Material.Component.Rect.pivot = new Vector2(0, 1);
                    }
                    else if (Material.Component.Idle.Float == Float.TOP_CENTER)
                    {
                        Material.Component.Rect.anchorMin = new Vector2(0.5f, 1);
                        Material.Component.Rect.anchorMax = new Vector2(0.5f, 1);
                        Material.Component.Rect.pivot = new Vector2(0.5f, 1);
                    }
                    else if (Material.Component.Idle.Float == Float.TOP_RIGHT)
                    {
                        Material.Component.Rect.anchorMin = new Vector2(1, 1);
                        Material.Component.Rect.anchorMax = new Vector2(1, 1);
                        Material.Component.Rect.pivot = new Vector2(1, 1);
                    }
                    else if (Material.Component.Idle.Float == Float.MIDDLE_LEFT)
                    {
                        Material.Component.Rect.anchorMin = new Vector2(0, 0.5f);
                        Material.Component.Rect.anchorMax = new Vector2(0, 0.5f);
                        Material.Component.Rect.pivot = new Vector2(0, 0.5f);
                    }
                    else if (Material.Component.Idle.Float == Float.MIDDLE_CENTER)
                    {
                        Material.Component.Rect.anchorMin = new Vector2(0.5f, 0.5f);
                        Material.Component.Rect.anchorMax = new Vector2(0.5f, 0.5f);
                        Material.Component.Rect.pivot = new Vector2(0.5f, 0.5f);
                    }
                    else if (Material.Component.Idle.Float == Float.MIDDLE_RIGHT)
                    {
                        Material.Component.Rect.anchorMin = new Vector2(1, 0.5f);
                        Material.Component.Rect.anchorMax = new Vector2(1, 0.5f);
                        Material.Component.Rect.pivot = new Vector2(1, 0.5f);
                    }
                    else if (Material.Component.Idle.Float == Float.BOTTOM_LEFT)
                    {
                        Material.Component.Rect.anchorMin = new Vector2(0, 0);
                        Material.Component.Rect.anchorMax = new Vector2(0, 0);
                        Material.Component.Rect.pivot = new Vector2(0, 0);
                    }
                    else if (Material.Component.Idle.Float == Float.BOTTOM_CENTER)
                    {
                        Material.Component.Rect.anchorMin = new Vector2(0.5f, 0);
                        Material.Component.Rect.anchorMax = new Vector2(0.5f, 0);
                        Material.Component.Rect.pivot = new Vector2(0.5f, 0);
                    }
                    else if (Material.Component.Idle.Float == Float.BOTTOM_RIGHT)
                    {
                        Material.Component.Rect.anchorMin = new Vector2(1, 0);
                        Material.Component.Rect.anchorMax = new Vector2(1, 0);
                        Material.Component.Rect.pivot = new Vector2(1, 0);
                    }

                    nextTask(null);
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
                    if (Material.Component is UIPage)
                    {
                        Material.Component.Type = UIType.PAGE;

                        UIPage page = Material.Component as UIPage;
                        UIIdlePage idle = page.Idle as UIIdlePage;

                        page.Rect.anchoredPosition = new Vector2(idle.Left - idle.Right, idle.Bottom - idle.Top);
                        page.Rect.sizeDelta = new Vector2(idle.Width, idle.Height);

                        page.Background = page.Object.AddComponent<RawImage>();
                        page.Background.texture = Media.texSolid;
                        page.Background.color = idle.BackgroundColor;
                    }

                    nextTask(null);
                }
            };
        }

        private Task Render_Panel()
        {
            return new Task()
            {
                Mission = "Render_Panel",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (Material.Component is UIPanel)
                    {
                        Material.Component.Type = UIType.PANEL;

                        UIPanel panel = Material.Component as UIPanel;
                        UIIdlePanel idle = panel.Idle as UIIdlePanel;

                        panel.Rect.anchoredPosition = new Vector2(idle.Left - idle.Right, idle.Bottom - idle.Top);
                        panel.Rect.sizeDelta = new Vector2(idle.Width, idle.Height + idle.Padding);

                        panel.Background = panel.Object.AddComponent<RawImage>();
                        panel.Background.texture = Media.texSolid;
                        panel.Background.color = idle.BackgroundColor;
                    }

                    nextTask(null);
                }
            };
        }

        private Task Render_Image()
        {
            return new Task()
            {
                Mission = "Render_Image",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (Material.Component is UIImage)
                    {
                        Material.Component.Type = UIType.IMAGE;

                        UIImage image = Material.Component as UIImage;
                        UIIdleImage idle = image.Idle as UIIdleImage;

                        image.Rect.anchoredPosition = new Vector2(idle.Left - idle.Right, idle.Bottom - idle.Top);
                        image.Rect.sizeDelta = new Vector2(idle.Width, idle.Height);

                        if (idle.PredefinedWidth != 0 && idle.PredefinedHeight != 0)
                        {
                            image.Rect.sizeDelta = new Vector2(idle.Width, idle.PredefinedHeight * (idle.Width / idle.PredefinedWidth));
                            idle.Height = image.Rect.sizeDelta.y;
                        }

                        new ImageManager()
                        {
                            Material = new ImageManagerMaterial()
                            {
                                Url = idle.Url,
                                LazyLoad = idle.LazyLoad,
                                Suspension = idle.LazyLoadSuspension
                            },
                            OnFinish = (result) =>
                            {
                                image.Background = image.Object.AddComponent<RawImage>();
                                image.Background.texture = result.Texture;
                                image.Background.color = idle.BackgroundColor;

                                if (idle.Height == 0)
                                {
                                    image.Rect.sizeDelta = new Vector2(idle.Width, image.Background.texture.height * (idle.Width / image.Background.texture.width));
                                    idle.Height = image.Rect.sizeDelta.y;
                                }
                            }
                        }
                        .Retrieve();
                    }

                    nextTask(null);
                }
            };
        }

        private Task Render_Text()
        {
            return new Task()
            {
                Mission = "Render_Text",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (Material.Component is UIText)
                    {
                        Material.Component.Type = UIType.TEXT;

                        UIText text = Material.Component as UIText;
                        UIIdleText idle = text.Idle as UIIdleText;

                        text.Rect.anchoredPosition = new Vector2(idle.Left - idle.Right, idle.Bottom - idle.Top);
                        text.Rect.sizeDelta = new Vector2(idle.Width, 0);

                        text.Element = Material.Component.Object.AddComponent<Text>();

                        text.Element.alignment = idle.Alignment;
                        text.Element.font = idle.Font;
                        text.Element.color = idle.FontColor;
                        text.Element.fontSize = int.Parse(Math.Floor(Screen.width * idle.FontSize).ToString());
                        text.Element.lineSpacing = idle.LineHeight;
                        text.Element.text = idle.Text;

                        if (idle.Height == 0)
                        {
                            TextGenerator generator = new TextGenerator();

                            float height = generator.GetPreferredHeight(text.Element.text, text.Element.GetGenerationSettings(text.Rect.sizeDelta));
                            idle.Height = height;
                        }

                        text.Rect.sizeDelta = new Vector2(text.Rect.sizeDelta.x, idle.Height);
                    }

                    nextTask(null);
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
                    if (Material.Component is UIScrollable)
                    {
                        Material.Component.Type = UIType.LIST;

                        UIScrollable list = Material.Component as UIScrollable;
                        UIIdleList idle = list.Idle as UIIdleList;

                        list.Rect.anchoredPosition = new Vector2(idle.Left - idle.Right, idle.Bottom - idle.Top);
                        list.Rect.sizeDelta = new Vector2(idle.Width, idle.Height);

                        list.Background = list.Object.AddComponent<RawImage>();
                        list.Background.texture = Media.texSolid;
                        list.Background.color = idle.BackgroundColor;

                        list.Container = new UIContainer();
                        list.Container.Object = new GameObject(list.Name + "Container");
                        list.Container.Object.transform.parent = list.Object.transform;

                        list.Container.Rect = list.Container.Object.AddComponent<RectTransform>();
                        list.Container.Rect.anchorMin = new Vector2(0.5f, 1);
                        list.Container.Rect.anchorMax = new Vector2(0.5f, 1);
                        list.Container.Rect.pivot = new Vector2(0.5f, 1);
                        list.Container.Rect.sizeDelta = new Vector2(0, 0);
                        list.Container.Rect.anchoredPosition = new Vector2(0, 0);

                        UIScroll scroll = list.Object.AddComponent<UIScroll>();
                        scroll.vertical = true;
                        scroll.horizontal = false;
                        scroll.movementType = UIScroll.MovementType.Elastic;
                        scroll.content = list.Container.Rect;
                        scroll.elasticity = 0.1f;
                        scroll.decelerationRate = 0.005f;
                        scroll.scrollSensitivity = 0.0001f;
                        scroll.scrollFactor = 2f;

                        scroll.gameObject.AddComponent<Mask>();

                        new EventRegistrar()
                        {
                            Material = new EventRegistrarMaterial()
                            {
                                Listener = new EventListener()
                                {
                                    Owner = list.Name,
                                    Name = list.Name + "OnScrollUp",
                                    Enabled = true,
                                    Event = () =>
                                    {
                                        return (list.Container.Rect.anchoredPosition.y > list.ScrollUp);
                                    },
                                    OnFired = (listener) =>
                                    {
                                        list.ScrollingUp = true;
                                        list.WasScrollingUp = true;
                                        list.WasScrollingDown = false;

                                        if (!list.ScrollUpCalled)
                                        {
                                            list.ScrollUpCalled = true;

                                            if (list.OnScrollUp != null)
                                                list.OnScrollUp(list);
                                        }

                                        if (list.OnScrollingUp != null)
                                            list.OnScrollingUp(list);
                                    },
                                    OnMiss = (listener) =>
                                    {
                                        list.ScrollingUp = false;
                                        list.ScrollUpCalled = false;

                                        if (list.OnNoScrollingUp != null)
                                            list.OnNoScrollingUp(list);
                                    },
                                    OnEither = (listener) =>
                                    {
                                        list.ScrollUp = list.Container.Rect.anchoredPosition.y;
                                    }
                                }
                            }
                        }
                        .Register();

                        new EventRegistrar()
                        {
                            Material = new EventRegistrarMaterial()
                            {
                                Listener = new EventListener()
                                {
                                    Owner = list.Name,
                                    Name = list.Name + "OnScrollDown",
                                    Enabled = true,
                                    Event = () =>
                                    {
                                        return (list.Container.Rect.anchoredPosition.y < list.ScrollDown);
                                    },
                                    OnFired = (listener) =>
                                    {
                                        list.ScrollingDown = true;
                                        list.WasScrollingDown = true;
                                        list.WasScrollingUp = false;

                                        if (!list.ScrollDownCalled)
                                        {
                                            list.ScrollDownCalled = true;

                                            if (list.OnScrollDown != null)
                                                list.OnScrollDown(list);
                                        }

                                        if (list.OnScrollingDown != null)
                                            list.OnScrollingDown(list);
                                    },
                                    OnMiss = (listener) =>
                                    {
                                        list.ScrollingDown = false;
                                        list.ScrollDownCalled = false;

                                        if (list.OnNoScrollingDown != null)
                                            list.OnNoScrollingDown(list);
                                    },
                                    OnEither = (listener) =>
                                    {
                                        list.ScrollDown = list.Container.Rect.anchoredPosition.y;
                                    }
                                }
                            }
                        }
                        .Register();

                        if (list.OnRefresh != null)
                        {
                            new EventRegistrar()
                            {
                                Material = new EventRegistrarMaterial()
                                {
                                    OnTouchListener = new OnTouchListener()
                                    {
                                        Owner = list.Name,
                                        Target = list.Name,
                                        Enabled = true,
                                        Released = true,
                                        OnRelease = (go) =>
                                        {
                                            if (list.Container.Rect.anchoredPosition.y < -Screen.height * 0.15f)
                                            {
                                                if (!list.Refreshing)
                                                {
                                                    list.Refreshing = true;

                                                    (Variables.UI["txtTopRight"] as UIText).Element.text = "Refreshing...";

                                                    new Suspender()
                                                    {
                                                        Suspension = 1f,
                                                        OnFinish = () =>
                                                        {
                                                            if (list.OnRefresh != null)
                                                                list.OnRefresh(list);
                                                        }
                                                    }
                                                    .Suspend();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            .Register();
                        }
                    }

                    nextTask(null);
                }
            };
        }

        private Task Click_Event()
        {
            return new Task()
            {
                Mission = "Click_Event",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (Material.Component.OnEventInitialization != null)
                    {
                        foreach (EventListener listener in Material.Component.OnEventInitialization(Material.Component))
                        {
                            if (!Events.Listeners.Exists(l => l.Name == listener.Name))
                                Events.Listeners.Add(listener);
                        }
                    }

                    if (Material.Component.OnTouchInitialization != null)
                    {
                        foreach (OnTouchListener listener in Material.Component.OnTouchInitialization(Material.Component))
                        {
                            if (!Events.OnTouch_Listeners.Exists(l => l.Target == listener.Target))
                                Events.OnTouch_Listeners.Add(listener);
                        }
                    }

                    nextTask(null);
                }
            };
        }

        private Task Render_Components()
        {
            return new Task()
            {
                Mission = "Render_Components",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (Material.Component.Idle.FurtherAccess)
                        Variables.UI[Material.Component.Name] = Material.Component;

                    if (Material.Component.States == null)
                        Material.Component.States = new Dictionary<string, UIState>();
                    else
                        Variables.UI[Material.Component.Name] = Material.Component;

                    if (Material.Component.Components != null)
                    {
                        Action<int> renderComponent = null;

                        renderComponent = (i) =>
                        {
                            if (i != Material.Component.Components.Count)
                            {
                                UIComponent component = Material.Component.Components[i];

                                if (component == null)
                                    renderComponent(i + 1);
                                else
                                    new UIRenderer()
                                    {
                                        Material = new UIRendererMaterial()
                                        {
                                            Component = component,
                                            Parent = Material.Component.Object
                                        },
                                        OnFinish = () =>
                                        {
                                            if (Material.Component.Idle.Span)
                                            {
                                                component.Rect.anchoredPosition = new Vector2(component.Rect.anchoredPosition.x, component.Rect.anchoredPosition.y - Material.Component.Idle.Height);
                                                Material.Component.Idle.Height += component.Rect.sizeDelta.y + component.Idle.Bottom + component.Idle.Top;
                                                Material.Component.Rect.sizeDelta = new Vector2(Material.Component.Idle.Width, Material.Component.Idle.Height);
                                            }

                                            renderComponent(i + 1);
                                        }
                                    }
                                    .Render();
                            }
                            else
                            {
                                if (OnFinish != null)
                                    OnFinish();

                                Dispose();
                            }
                        };

                        renderComponent(0);
                    }
                    else
                    {
                        Material.Component.Components = new List<UIComponent>();

                        if (OnFinish != null)
                            OnFinish();

                        Dispose();
                    }
                }
            };
        }

    }

    public class UIRendererMaterial : CoreMaterial
    {
        public UIComponent Component { get; set; }
        public bool FirstSibling { get; set; }
        public GameObject Parent { get; set; }
    }
}
