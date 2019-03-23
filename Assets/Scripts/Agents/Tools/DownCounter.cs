using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Core;
using Assets.Scripts.Repository;
using UnityEngine;

namespace Assets.Scripts.Agents.Tools
{
    public class DownCounter : CoreRecursive<DownCounterMaterial>
    {
        public void Start()
        {
            Perform();
        }

        protected override void Job()
        {
            Material.Seconds--;

            string text = "00:";

            if (Material.Seconds < 10)
                text += "0" + Material.Seconds;
            else
                text += Material.Seconds;

            (Variables.UI["txtCountdown"] as UIText).Element.text = text;
        }
    }

    public class DownCounterMaterial : CoreMaterial
    {
        public int Seconds { get; set; }
    }
}
