using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.Agents.Tools;
using Assets.Scripts.Repository;

namespace Assets.Scripts.Abstract.UI
{
    public class UIState : UISwitch
    {        
        
    }

    public class UIStatePanel : UIState
    {
        public float Height { get; set; }

        public Color32 BackgroundColor { get; set; }
    }

    public class UIStatePage : UIStatePanel { }
    public class UIStateList : UIStatePanel { }
    public class UIStateImage : UIStatePanel { }    

    public class UIStateText : UIState
    {
        public Color32 FontColor { get; set; }
    }
}

