using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Agents.Initializers;
using Assets.Scripts.Agents.Tools;
using Assets.Scripts.Repository;
using Assets.Scripts.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.UI.ListItems;
using Assets.Scripts.Agents.Renderers;
using Assets.Scripts.UI.Pages;
using Assets.Scripts.Abstract.Exceptions;
using System.IO;

namespace Assets.Scripts
{
    public class App : MonoBehaviour
    {
        public void Start()
        {
            MonoBridge.StartCoroutine = (routine) => { StartCoroutine(routine); };
            MonoBridge.Destroy = (gameObject) => { Destroy(gameObject); };
            MonoBridge.DestroyComponent = (component) => { Destroy(component); };
            MonoBridge.Instantiate = (name, gameObject) =>
            {
                GameObject go = Instantiate(gameObject);
                go.name = name;

                return go;
            };

            new AppInitializer().Initialize();
        }

        private void FixedUpdate()
        {
            if (Directives.Events_Initialized)
            {
                List<Switch> listAnimationRemoval = new List<Switch>();

                foreach (Switch animation in Events.Switches.ToList())
                {
                    try
                    {
                        if (animation.Enabled)
                        {
                            animation.Cycle++;

                            if (animation.Cycle != animation.Limit)
                            {
                                if (animation.Condition())
                                    animation.OnAnimate(animation);
                                else
                                {
                                    listAnimationRemoval.Add(animation);
                                    animation.OnFinish();
                                }
                            }
                            else
                            {
                                listAnimationRemoval.Add(animation);
                                animation.OnFinish();
                            }
                        }
                        else
                        {
                            listAnimationRemoval.Add(animation);
                            animation.OnFinish();
                        }
                    }
                    catch (Exception)
                    {
                        listAnimationRemoval.Add(animation);
                    }
                }

                foreach (Switch animation in listAnimationRemoval)
                {
                    Events.Switches.Remove(animation);
                }

                List<EventListener> listRemoval = new List<EventListener>();

                foreach (EventListener listener in Events.Listeners.ToList())
                {
                    if (listener.Remove)
                    {
                        if (listener.OnExit != null)
                            listener.OnExit();

                        listRemoval.Add(listener);
                    }
                    else
                    {
                        if (listener.Enabled)
                        {
                            if (listener.Event())
                            {
                                if (listener.OnFired != null)
                                    listener.OnFired(listener);
                            }
                            else
                            {
                                if (listener.OnMiss != null)
                                    listener.OnMiss(listener);
                            }

                            if (listener.OnEither != null)
                                listener.OnEither(listener);
                        }
                    }
                }

                foreach (EventListener listener in listRemoval)
                {
                    Events.Listeners.Remove(listener);
                }
            }
        }

        private void OnApplicationQuit()
        {
            new HttpPostRequestSender()
            {
                Material = new HttpPostRequestSenderMaterial()
                {
                    Url = Config.URLs.Instance_Termination,
                    Fields = new Dictionary<string, string>()
                    {
                        { "appInstanceKey", Variables.App_Instance_Key },
                    }
                }
            }
            .Send();
        }
    }
}
