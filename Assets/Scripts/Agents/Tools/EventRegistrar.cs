using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Core;
using Assets.Scripts.Repository;

namespace Assets.Scripts.Agents.Tools
{
    public class EventRegistrar : CoreAgent<EventRegistrarMaterial>
    {
        public void Register()
        {
            Perform();
        }

        protected override void Job()
        {
            if (Material.Listener != null)
                Events.Listeners.Add(Material.Listener);

            if (Material.OnTouchListener != null)
                Events.OnTouch_Listeners.Add(Material.OnTouchListener);
        }
    }

    public class EventRegistrarMaterial : CoreMaterial
    {
        public EventListener Listener { get; set; }
        public OnTouchListener OnTouchListener { get; set; }
    }
}
