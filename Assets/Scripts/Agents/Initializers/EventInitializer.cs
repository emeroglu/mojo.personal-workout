using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Core;
using Assets.Scripts.Repository;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Agents.Initializers
{
    public class EventInitializer : CoreAgent<CoreMaterial>
    {
        public void Initialize()
        {
            Perform();
        }

        protected override void Job()
        {
            Events.Listeners = new List<EventListener>();
            Events.Touched_Objects = new List<GameObject>();
            Events.OnTouch_Listeners = new List<OnTouchListener>();
            Events.Switches = new List<Switch>();

            Events.Listeners.Add(On_Touch());
            Events.Listeners.Add(On_Release());
            Events.Listeners.Add(While_Touching());
            Events.Listeners.Add(On_Swiping_Horizontally());
        }

        private EventListener On_Touch()
        {
            return new EventListener()
            {
                Name = "On_Touch",
                Enabled = true,
                Event = () => { return Input.GetMouseButtonDown(0); },
                OnFired = (listener) =>
                {
                    Directives.Touching = true;

                    Variables.Touch_Position_Start = Input.mousePosition;
                }
            };
        }

        private EventListener On_Release()
        {
            return new EventListener()
            {
                Name = "On_Release",
                Enabled = true,
                Event = () => { return Input.GetMouseButtonUp(0); },
                OnFired = (listener) =>
                {
                    Directives.Touching = false;

                    Variables.Touch_Position_Delta_X = 0;
                }
            };
        }

        private EventListener While_Touching()
        {
            return new EventListener()
            {
                Name = "While_Touching",
                Enabled = true,
                Event = () => { return Directives.Sense_Touch && Directives.Touching; },
                OnFired = (listener) =>
                {
                    Variables.Touch_Position_End = Input.mousePosition;

                    PointerEventData pointer = new PointerEventData(EventSystem.current);
                    pointer.position = Input.mousePosition;

                    List<RaycastResult> listRayResults = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointer, listRayResults);

                    List<OnTouchListener> listRemoval = new List<OnTouchListener>();

                    GameObject go;                    

                    foreach (OnTouchListener onTouchListener in Events.OnTouch_Listeners.ToList())
                    {                                                    
                        if (onTouchListener.Remove)
                        {
                            listRemoval.Add(onTouchListener);
                        }
                        else
                        {
                            if (onTouchListener.Enabled)
                            {
                                if (onTouchListener.Target.Contains('%'))
                                {
                                    go = listRayResults.FirstOrDefault(r => r.gameObject.name.Contains(onTouchListener.Target.Replace("%", ""))).gameObject;
                                }
                                else
                                {
                                    go = listRayResults.FirstOrDefault(r => r.gameObject.name == onTouchListener.Target).gameObject;
                                }

                                if (go != null)
                                {
                                    if (onTouchListener.Released)
                                    {
                                        onTouchListener.Released = false;

                                        Events.Touched_Objects.Add(go);

                                        if (onTouchListener.OnTouch != null)
                                            onTouchListener.OnTouch(go);
                                    }
                                    else
                                    {
                                        if (onTouchListener.OnTouching != null)
                                            onTouchListener.OnTouching(go);
                                    }
                                }
                            }
                        }
                    }

                    foreach (OnTouchListener onTouchListener in listRemoval)
                    {
                        Events.OnTouch_Listeners.Remove(onTouchListener);
                    }
                },
                OnMiss = (listener) =>
                {                    
                    List<OnTouchListener> listRemoval = new List<OnTouchListener>();

                    GameObject go;

                    foreach (OnTouchListener onTouchListener in Events.OnTouch_Listeners.ToList())
                    {
                        if (onTouchListener.Remove)
                        {
                            listRemoval.Add(onTouchListener);
                        }
                        else
                        {
                            if (onTouchListener.Enabled)
                            {
                                if (onTouchListener.Target.Contains('%'))
                                {
                                    go = Events.Touched_Objects.FirstOrDefault(r => r.name.Contains(onTouchListener.Target.Replace("%", "")));
                                }
                                else
                                {
                                    go = Events.Touched_Objects.FirstOrDefault(r => r.name == onTouchListener.Target);
                                }

                                if (go != null)
                                {
                                    if (!onTouchListener.Released)
                                    {
                                        onTouchListener.Released = true;

                                        Events.Touched_Objects.Remove(go);

                                        if (onTouchListener.OnRelease != null)
                                            onTouchListener.OnRelease(go);
                                    }
                                }
                            }
                        }
                    }

                    foreach (OnTouchListener onTouchListener in listRemoval)
                    {
                        Events.OnTouch_Listeners.Remove(onTouchListener);
                    }
                }
            };
        }

        private EventListener On_Swiping_Horizontally()
        {
            return new EventListener()
            {
                Name = "On_Swiping_Horizontally",
                Enabled = false,
                Event = () =>
                {
                    if (Directives.Touching)
                    {
                        float deltaPrev_X = Variables.Touch_Position_Delta_X;
                        float deltaX = Variables.Touch_Position_End.x - Variables.Touch_Position_Start.x;

                        return Math.Abs(deltaX - deltaPrev_X) > Screen.width * 0.01f;
                    }

                    return false;
                },
                OnFired = (listener) =>
                {
                    float deltaPrev_X = Variables.Touch_Position_Delta_X;
                    Variables.Touch_Position_Delta_X = Variables.Touch_Position_End.x - Variables.Touch_Position_Start.x;

                    if (Variables.Touch_Position_Delta_X - deltaPrev_X > 0)
                    {
                        Variables.Swipe_Right += Math.Abs(Variables.Touch_Position_Delta_X - deltaPrev_X) * 0.5f;
                    }
                    else
                    {
                        Variables.Swipe_Left += Math.Abs(Variables.Touch_Position_Delta_X - deltaPrev_X) * 0.5f;
                    }

                    Variables.Swipe_Horizontal = Variables.Swipe_Left - Variables.Swipe_Right;
                    Variables.Swipe_Horizontal += 360;
                    Variables.Swipe_Horizontal %= 360;
                }
            };
        }
    }
}
