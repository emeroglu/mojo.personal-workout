using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Json;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Agents.Renderers;
using Assets.Scripts.Agents.Tools;
using Assets.Scripts.Repository;
using Assets.Scripts.UI.ListItems;
using Assets.Scripts.UI.Pages;
using Newtonsoft.Json;
using UnityEngine;


namespace Assets.Scripts.UI.Panels
{
    public class pnlFrame : UIPanel
    {
        public pnlFrame(Action<Exception> onFail)
        {
            this.Name = "pnlFrame";

            this.Idle = new UIIdlePanel()
            {
                Float = Float.MIDDLE_CENTER,
                Width = Styles.Screen_Width,
                Height = Styles.Screen_Height * 1.5f,
                BackgroundColor = Media.colorTransparent
            };

            this.States = new Dictionary<string, UIState>()
            {
                {
                    "View",
                    new UIStatePanel()
                    {
                        Width = Styles.Screen_Width,
                        Height = Styles.Screen_Height,
                        BackgroundColor = Media.colorTransparent                        
                    }
                }
            };

            this.Components = new List<UIComponent>()
            {
                new UIImage()
                {
                    Name = "imgFade",
                    Idle = new UIIdleImage()
                    {
                        Float = Float.MIDDLE_CENTER,
                        Width = Styles.Screen_Width,
                        Height = Styles.Screen_Height * 1.5f,                                           
                        BackgroundColor = Color.black,
                        Url = "Images/faded_background"                        
                    },
                    States = new Dictionary<string,UIState>()
                    {
                        {
                            "View",
                            new UIStateImage()
                            {
                                Width = Styles.Screen_Width,
                                Height = Styles.Screen_Height,
                                BackgroundColor = Color.black
                            }
                        }
                    }
                },
                new UIImage()
                {
                    Name = "imgBottomIcon",
                    Idle = new UIIdleImage()
                    {
                        Float = Float.BOTTOM_CENTER,
                        Width = Styles.Side_Tall_Bar_Icon,
                        Height = Styles.Side_Tall_Bar_Icon, 
                        Bottom = Styles.Padding_For_Anything,                                
                        BackgroundColor = Media.colorOneThirdsTransparent,                        
                        Url = "Images/icon_mj"
                    }
                },
                new UIPanel()
                {
                    Name = "clckFrameBack",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.BOTTOM_LEFT,
                        Width = Styles.Width_Bar_Single,
                        Height = Styles.Height_Bar_Medium,                        
                        BackgroundColor = Media.colorTransparent
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIText()
                        {
                            Name = "txtBack",
                            Idle = new UIIdleText()
                            {
                                Float = Float.MIDDLE_CENTER,
                                Width = Styles.Width_Bar_Single,
                                Alignment = TextAnchor.MiddleCenter,
                                Font = Media.fontExoLight,
                                FontColor = Media.colorGreyLight,
                                FontSize = Styles.Font_Size_Larger,
                                LineHeight = 1,
                                Text = "Back"
                            }
                        }
                    },
                    OnTouchInitialization = (component) =>
                    {
                        return new List<OnTouchListener>()
                        {
                            new OnTouchListener()
                            {
                                Owner = component.Name,
                                Target = component.Name,
                                Enabled = true,
                                Released = true,
                                OnRelease = (go) =>
                                {
                                    Directives.Sense_Touch = false;

                                    if (Variables.Current_Mojo.soundtrack.touch)
                                    {
                                        Events.Listeners.FirstOrDefault(l => l.Name == "On_Swiping_Horizontally").Enabled = false;
                                        Events.Listeners.FirstOrDefault(l => l.Name == "Swipe_Listener").Remove = true;
                                    }

                                    new StateBroadcaster()
                                    {
                                        Material = new StateBroadcasterMaterial()
                                        {
                                            State = "Back"
                                        },
                                        OnFinish = () =>
                                        {                                                                
                                            Variables.UI["pnlApp"].Object.transform.SetAsLastSibling();

                                            new StateBroadcaster()
                                            {
                                                Material = new StateBroadcasterMaterial()
                                                {
                                                    State = "App"
                                                },
                                                OnFinish = () =>
                                                {
                                                    MonoBridge.Destroy(Objects.Environment);
                                                    MonoBridge.Destroy(Objects.Avatar);
                                                    Objects.AudioSource.Stop();

                                                    Variables.UI["pnlFrame"].Destroy();

                                                    Variables.UI["pnlKeyboard"].Object.transform.SetAsLastSibling();

                                                    Directives.Sense_Touch = true;
                                                },
                                                OnFail = onFail
                                            }
                                            .Broadcast();                               
                                        }
                                    }
                                    .Broadcast();
                                }
                            }
                        };                                                
                    }
                },
                new UIPanel()
                {
                    Name = "pnlCountdown",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.BOTTOM_RIGHT,
                        Width = Styles.Width_Bar_Single,
                        Height = Styles.Height_Bar_Medium,                        
                        BackgroundColor = Media.colorTransparent
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIText()
                        {
                            Name = "txtCountdown",
                            Idle = new UIIdleText()
                            {
                                Float = Float.MIDDLE_CENTER,
                                Width = Styles.Width_Bar_Single,
                                Alignment = TextAnchor.MiddleCenter,
                                Font = Media.fontExoLight,
                                FontColor = Media.colorGreyLight,
                                FontSize = Styles.Font_Size_Larger,
                                LineHeight = 1,
                                Text = "00:00",
                                FurtherAccess = true
                            }
                        }                                                                     
                    }
                },
                new UIPanel()
                {
                    Name = "pnlContent",
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.TOP_CENTER,
                        Width = Styles.Screen_Width,
                        Height = Styles.Height_Bar_Tall,
                        Bottom = Styles.Height_Bar_Tall,
                        BackgroundColor = Media.colorTransparent
                    },
                    States = new Dictionary<string,UIState>()
                    {                                               
                        {
                            "Delayed_View",
                            new UIStatePanel()
                            {
                                Width = Styles.Screen_Width,
                                Height = Styles.Height_Bar_Tall,
                                BackgroundColor = Media.colorTransparent                                
                            }
                        }
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIImage()
                        {
                            Name = "imgProfile",
                            Idle = new UIIdleImage()
                            {
                                Float = Float.TOP_LEFT,
                                Width = Styles.Side_Frame_Profile,
                                Height = Styles.Side_Frame_Profile,
                                Top = Styles.Top_Frame_Profile,
                                Left =Styles.Left_Frame_Profile,
                                BackgroundColor = Media.colorOpaque,
                                Url = (Variables.Current_Mojo.pk == 0)?"Images/icon_app":Variables.Current_Mojo.user.picture,
                                LazyLoad = true,
                                LazyLoadSuspension = 1
                            },
                            Components = new List<UIComponent>()
                            {
                                new UIImage()
                                {
                                    Name = "imgProfileMaskRounded",
                                    Idle = new UIIdleImage()
                                    {
                                        Float = Float.TOP_LEFT,
                                        Width = Styles.Side_Frame_Profile,
                                        Height = Styles.Side_Frame_Profile,
                                        BackgroundColor = Media.colorBlack,
                                        Url = "Images/in_rounded"
                                    }
                                },
                                new UIImage()
                                {
                                    Name = "imgProfileMask",
                                    Idle = new UIIdleImage()
                                    {
                                        Float = Float.TOP_LEFT,
                                        Width = Styles.Side_Frame_Profile,
                                        Height = Styles.Side_Frame_Profile,
                                        BackgroundColor = Media.colorBlackOneThirdsTransparent,
                                        Url = "Images/solid"
                                    }
                                }
                            }
                        },
                        new UIText()
                        {
                            Name = "txtTopMessage",
                            Idle = new UIIdleText()
                            {
                                Float = Float.MIDDLE_LEFT,
                                Width = Styles.Width_Frame_Message,
                                Height = Styles.Height_Bar_Tall,
                                Left =Styles.Left_Frame_Message,
                                Alignment = TextAnchor.MiddleLeft,
                                Font = Media.fontExoLight,
                                FontColor = Media.colorWhite,
                                FontSize = Styles.Font_Size_Smaller,
                                LineHeight = 1.25f,
                                Text = (Variables.Current_Mojo.message.Length == 0)?Variables.Current_Mojo.user.name+ ":\n" + Variables.Current_Mojo.soundtrack.name + " - " + Variables.Current_Mojo.soundtrack.belongsTo
                                        :(Variables.Current_Mojo.message.Length <= 40)?Variables.Current_Mojo.user.name + ":\n" + Variables.Current_Mojo.message
                                        :Variables.Current_Mojo.message
                            }
                        }
                    }
                }                
            };
        }
    }
}
