using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.Agents.Tools;
using Assets.Scripts.Repository;

namespace Assets.Scripts.Abstract.UI
{
    public class UIIdle : UISwitch
    {
        public float Height { get; set; }

        public Float Float { get; set; }
        public bool Span { get; set; }

        public bool FurtherAccess { get; set; }
    }

    public class UIIdlePanel : UIIdle
    {
        public Color32 BackgroundColor { get; set; }
    }

    public class UIIdlePage : UIIdlePanel { }
    
    public class UIIdleList : UIIdlePanel 
    {        
        public int MaxItems { get; set; }
    }

    public class UIIdleImage : UIIdlePanel
    {
        public int PredefinedWidth { get; set; }
        public int PredefinedHeight { get; set; }

        public string Url { get; set; }

        public bool LazyLoad { get; set; }
        public float LazyLoadSuspension { get; set; }
    }

    public class UIIdleText : UIIdle
    {
        public Color32 FontColor { get; set; }
        public float FontSize { get; set; }

        public TextAnchor Alignment { get; set; }

        public Font Font { get; set; }

        public float LineHeight { get; set; }

        public string Text { get; set; }
    }
}

