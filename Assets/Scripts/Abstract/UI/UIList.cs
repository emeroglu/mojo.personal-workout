using System;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Abstract.Json;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Agents.Tools;
using Assets.Scripts.Repository;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.UI.ListItems;

namespace Assets.Scripts.Abstract.UI
{
    public class UIScrollable : UIComponent
    {
        public bool Scrolling { get; set; }

        public bool ScrollingUp { get; set; }
        public bool WasScrollingUp { get; set; }
        public bool ScrollingDown { get; set; }
        public bool WasScrollingDown { get; set; }

        public float ScrollUp { get; set; }
        public bool ScrollUpCalled { get; set; }
        public float ScrollDown { get; set; }
        public bool ScrollDownCalled { get; set; }

        public Action<UIScrollable> OnTop { get; set; }
        public Action<UIScrollable> OnBottom { get; set; }
        public Action<UIScrollable> OnScrollingUp { get; set; }
        public Action<UIScrollable> OnScrollingDown { get; set; }
        public Action<UIScrollable> OnNoScrollingUp { get; set; }
        public Action<UIScrollable> OnNoScrollingDown { get; set; }
        public Action<UIScrollable> OnScrollUp { get; set; }
        public Action<UIScrollable> OnScrollDown { get; set; }

        public bool Refreshing { get; set; }

        public Action<UIScrollable> OnRefresh { get; set; }

        public UIContainer Container { get; set; }
    }

    public class UIList<ItemType> : UIScrollable where ItemType : listItem
    {
        public List<UIComponent> Items { get; set; }

        public List<ItemType> Data { get; set; }

        public Func<int, ItemType, UIComponent> OnPopulate { get; set; }
        public new Func<UIList<ItemType>, List<EventListener>> OnEventInitialization { get; set; }
        public new Func<UIList<ItemType>, List<OnTouchListener>> OnTouchInitialization { get; set; }

        public void ScrollToTop()
        {
            ScrollTo(-this.Items[0].Rect.anchoredPosition.y - Screen.height * 0.01f, () => { });
        }

        public void ScrollTo(float position, Action onFinish)
        {
            if (!Scrolling)
            {
                Scrolling = true;

                if (Math.Abs(Container.Rect.anchoredPosition.y - position) > Screen.height * 0.001f)
                {
                    this.Object.GetComponent<UIScroll>().enabled = false;

                    new EventRegistrar()
                    {
                        Material = new EventRegistrarMaterial()
                        {
                            Listener = new EventListener()
                            {
                                Name = Name + "ToScroll",
                                Enabled = true,
                                Event = () => { return Math.Abs(Container.Rect.anchoredPosition.y - position) > Screen.height * 0.001f; },
                                OnFired = (listener) =>
                                {
                                    Container.Rect.anchoredPosition = Vector2.Lerp(Container.Rect.anchoredPosition, new Vector2(0, position), 0.1f);

                                    if (Math.Abs(Container.Rect.anchoredPosition.y - position) <= Screen.height * 0.001f)
                                    {
                                        listener.Remove = true;
                                    }
                                },
                                OnExit = () =>
                                {
                                    Container.Rect.anchoredPosition = new Vector2(0, position);
                                    this.Object.GetComponent<UIScroll>().enabled = true;
                                    Scrolling = false;
                                    WasScrollingUp = false;
                                    WasScrollingDown = false;

                                    onFinish();
                                }
                            }
                        }
                    }
                    .Register();
                }
                else
                {
                    Container.Rect.anchoredPosition = new Vector2(0, position);
                    this.Object.GetComponent<UIScroll>().enabled = true;
                    Scrolling = false;
                    WasScrollingUp = false;
                    WasScrollingDown = false;

                    onFinish();
                }
            }
        }
    }
}
