using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Core;
using Assets.Scripts.Repository;
using System.Linq;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Abstract.Tools;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Agents.Tools
{
    public class StateBroadcaster : CoreServant<StateBroadcasterMaterial>
    {
        public void Broadcast()
        {
            Perform();
        }

        protected override void Job()
        {
            UIState state;

            if (Material.States == null)
                Material.States = new List<string>() { Material.State };

            Variables.Switch_Inc++;

            int switchId = Variables.Switch_Inc;

            foreach (string s in Material.States)
            {
                foreach (UIComponent component in Variables.UI.Values)
                {
                    if (!component.Switching)
                    {
                        foreach (string key in component.States.Keys)
                        {
                            if (key.Split(',').Contains(s))
                            {
                                state = component.States[key];

                                new StateSwitcher()
                                {
                                    Material = new SwitcherMaterial()
                                    {
                                        ID = switchId,
                                        Component = component,
                                        State = state
                                    }                                    
                                }
                                .Switch();
                            }
                        }
                    }
                }
            }

            if (OnFinish != null)
            {
                new Suspender()
                {
                    Suspension = 0.1f,
                    OnFinish = () =>
                    {
                        new EventRegistrar()
                        {
                            Material = new EventRegistrarMaterial()
                            {
                                Listener = new EventListener()
                                {
                                    Owner = "StateBroadcaster",
                                    Name = Material.State + "BroadcastListener",
                                    Enabled = true,
                                    Event = () => { return Events.Switches.Where(s => s.ID == switchId).Count() == 0; },
                                    OnFired = (listener) =>
                                    {
                                        listener.Remove = true;

                                        if (OnFinish != null)
                                        {
                                            try
                                            {
                                                OnFinish();
                                            }
                                            catch (Exception ex)
                                            {
                                                if (OnFail != null)
                                                    OnFail(ex);
                                            }
                                        }

                                        Dispose();
                                    }
                                }
                            }                            
                        }
                        .Register();
                    }
                }
                .Suspend();
            }
        }
    }

    public class StateBroadcasterMaterial : CoreMaterial
    {
        public string State { get; set; }
        public List<string> States { get; set; }
    }
}
