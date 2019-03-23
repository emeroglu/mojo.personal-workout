using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Core;
using Assets.Scripts.Repository;

namespace Assets.Scripts.Agents.Tools
{
    public class KeyboardOpener : CoreServant<KeyboardOpenerMaterial>
    {
        public void Open()
        {
            Perform();
        }

        protected override void Job()
        {
            List<OnTouchListener> listenersToShut = Events.OnTouch_Listeners.Where(l => Material.Listeners.Contains(l.Target)).ToList();

            foreach (OnTouchListener listener in listenersToShut)
            {
                listener.Enabled = false;
            }

            Events.OnTouch_Listeners.FirstOrDefault(o => o.Target == "%pnlKey_").Enabled = true;

            UIPanel pnlMask = Variables.UI["pnlMask"] as UIPanel;
            UIPanel pnlKeyboard = Variables.UI["pnlKeyboard"] as UIPanel;            

            pnlMask.Object.transform.SetAsLastSibling();
            pnlKeyboard.Object.transform.SetAsLastSibling();            

            new StateBroadcaster()
            {
                Material = new StateBroadcasterMaterial()
                {
                    State = "Keyboard_Open"
                },
                OnFinish = () =>
                {
                    Directives.Keyboard_Open = true;
                    Directives.Sense_Touch = true;

                    Variables.Keyboard_Text = Material.Initial_Text;

                    Material.Model.Element.text = Variables.Keyboard_Text;

                    new EventRegistrar()
                    {
                        Material = new EventRegistrarMaterial()
                        {
                            Listener = new EventListener()
                            {
                                Owner = Material.Owner,
                                Name = Material.Owner + "_Keyboard",
                                Enabled = true,
                                Event = () => { return Directives.Keyboard_Open; },
                                OnFired = (listener) =>
                                {
                                    Variables.Keyboard_Text = Material.OnMask(Variables.Keyboard_Text);

                                    Material.Model.Element.text = Variables.Keyboard_Text;
                                },
                                OnMiss = (listener) =>
                                {
                                    listener.Enabled = false;
                                    listener.Remove = true;
                                },
                                OnExit = () =>
                                {
                                    new StateBroadcaster()
                                    {
                                        Material = new StateBroadcasterMaterial()
                                        {
                                            State = "Keyboard_Closed"
                                        },
                                        OnFinish = () =>
                                        {
                                            UIPanel pnlApp = Variables.UI["pnlApp"] as UIPanel;

                                            pnlApp.Object.transform.SetAsLastSibling();

                                            Material.OnClose();

                                            foreach (OnTouchListener listener in listenersToShut)
                                            {
                                                listener.Enabled = true;
                                            }

                                            Dispose();
                                        }
                                    }
                                    .Broadcast();
                                }
                            }
                        }
                    }
                    .Register();
                }                
            }
            .Broadcast();
        }
    }

    public class KeyboardOpenerMaterial : CoreMaterial
    {
        public string Owner { get; set; }
        public List<string> Listeners { get; set; }

        public UIText Model { get; set; }
        public string Initial_Text { get; set; }

        public Func<string,string> OnMask { get; set; }
        public Action OnClose { get; set; }
    }
}
