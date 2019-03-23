using System;
using Assets.Scripts.Core;
using UnityEngine;
using Assets.Scripts.Repository;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Abstract.UI;
using UnityEngine.UI;
using System.Linq;

namespace Assets.Scripts.Agents.Tools
{
    public class StateSwitcher : CoreServant<SwitcherMaterial>
    {
        public void Switch()
        {
            Perform();
        }

        protected override void Job()
        {            
            Material.Component.Switching = true;            

            Events.Switches.Add
            (
                new Switch()
                {
                    ID = Material.ID,
                    Enabled = true,
                    Cycle = 0,
                    Limit = (100 / (int)Math.Floor(100 * Styles.Default_Velocity) * 10),
                    Condition = () =>
                    {
                        bool theEvent = false;

                        if (Material.Component is UIText)
                        {
                            UIText txt = Material.Component as UIText;
                            UIStateText state = Material.State as UIStateText;

                            theEvent = theEvent || Math.Abs(txt.Rect.anchoredPosition.x - state.Left + state.Right) > 0.5f;
                            theEvent = theEvent || Math.Abs(txt.Rect.anchoredPosition.y - state.Bottom + state.Top) > 0.5f;

                            theEvent = theEvent || Math.Abs(txt.Rect.sizeDelta.x - state.Width) > 0.1f;

                            theEvent = theEvent || Math.Abs(txt.Element.color.r - state.FontColor.r) > 1f;
                            theEvent = theEvent || Math.Abs(txt.Element.color.g - state.FontColor.g) > 1f;
                            theEvent = theEvent || Math.Abs(txt.Element.color.b - state.FontColor.b) > 1f;
                            theEvent = theEvent || Math.Abs(txt.Element.color.a - state.FontColor.a) > 1f;
                        }
                        else
                        {
                            UIStatePanel state = Material.State as UIStatePanel;

                            theEvent = theEvent || Math.Abs(Material.Component.Rect.anchoredPosition.x - state.Left + state.Right) > 0.5f;
                            theEvent = theEvent || Math.Abs(Material.Component.Rect.anchoredPosition.y - state.Bottom + state.Top) > 0.5f;

                            theEvent = theEvent || Math.Abs(Material.Component.Rect.sizeDelta.x - state.Width) > 0.1f;
                            theEvent = theEvent || Math.Abs(Material.Component.Rect.sizeDelta.y - state.Height) > 0.1f;

                            theEvent = theEvent || Math.Abs(Material.Component.Background.color.r - state.BackgroundColor.r) > 1f;
                            theEvent = theEvent || Math.Abs(Material.Component.Background.color.g - state.BackgroundColor.g) > 1f;
                            theEvent = theEvent || Math.Abs(Material.Component.Background.color.b - state.BackgroundColor.b) > 1f;
                            theEvent = theEvent || Math.Abs(Material.Component.Background.color.a - state.BackgroundColor.a) > 1f;
                        }

                        return theEvent;
                    },
                    OnAnimate = (switchh) =>
                    {
                        bool enabled = false;

                        if (Material.Component is UIText)
                        {
                            UIText txt = Material.Component as UIText;
                            UIStateText state = Material.State as UIStateText;

                            Vector2 pos = new Vector2(state.Left - state.Right, state.Bottom - state.Top);
                            Vector2 lerpPosition = Vector2.Lerp(txt.Rect.anchoredPosition, pos, Styles.Default_Velocity);

                            if (Vector2.Distance(lerpPosition, txt.Rect.anchoredPosition) < 0.1f)
                            {
                                enabled = enabled | false;
                            }
                            else
                            {
                                enabled = true;

                                txt.Rect.anchoredPosition = lerpPosition;
                            }

                            Vector2 lerpSize = Vector2.Lerp(Material.Component.Rect.sizeDelta, new Vector2(state.Width, txt.Idle.Height), Styles.Default_Velocity);

                            if (Vector2.Distance(lerpSize, Material.Component.Rect.sizeDelta) < 0.1f)
                            {
                                enabled = enabled | false;
                            }
                            else
                            {
                                enabled = true;

                                Material.Component.Rect.sizeDelta = lerpSize;
                            }

                            Color32 lerpColor = Color32.Lerp(txt.Element.color, state.FontColor, Styles.Default_Velocity);

                            if (lerpColor == txt.Element.color)
                            {
                                enabled = enabled | false;
                            }
                            else
                            {
                                enabled = true;

                                txt.Element.color = lerpColor;
                            }
                        }
                        else
                        {
                            UIStatePanel state = Material.State as UIStatePanel;

                            Vector2 pos = new Vector2(state.Left - state.Right, state.Bottom - state.Top);
                            Vector2 lerpPosition = Vector2.Lerp(Material.Component.Rect.anchoredPosition, pos, Styles.Default_Velocity);

                            if (Vector2.Distance(lerpPosition, Material.Component.Rect.anchoredPosition) < 0.1f)
                            {
                                enabled = enabled | false;
                            }
                            else
                            {
                                enabled = true;

                                Material.Component.Rect.anchoredPosition = lerpPosition;
                            }

                            Vector2 lerpSize = Vector2.Lerp(Material.Component.Rect.sizeDelta, new Vector2(state.Width, state.Height), Styles.Default_Velocity);

                            if (Vector2.Distance(lerpSize, Material.Component.Rect.sizeDelta) < 0.1f)
                            {
                                enabled = enabled | false;
                            }
                            else
                            {
                                enabled = true;

                                Material.Component.Rect.sizeDelta = lerpSize;
                            }

                            Color32 lerpColor = Color32.Lerp(Material.Component.Background.color, state.BackgroundColor, Styles.Default_Velocity);

                            if (lerpColor == Material.Component.Background.color)
                            {
                                enabled = enabled | false;
                            }
                            else
                            {
                                enabled = true;

                                Material.Component.Background.color = lerpColor;
                            }
                        }

                        switchh.Enabled = enabled;
                    },
                    OnFinish = () =>
                    {
                        if (Material.Component is UIText)
                        {
                            UIText txt = Material.Component as UIText;
                            UIStateText state = Material.State as UIStateText;

                            Vector2 pos = new Vector2(state.Left - state.Right, state.Bottom - state.Top);

                            txt.Rect.anchoredPosition = pos;
                            txt.Rect.sizeDelta = new Vector2(state.Width, txt.Idle.Height);
                            txt.Element.color = state.FontColor;
                        }
                        else
                        {
                            UIStatePanel state = Material.State as UIStatePanel;

                            Vector2 pos = new Vector2(state.Left - state.Right, state.Bottom - state.Top);

                            Material.Component.Rect.anchoredPosition = pos;
                            Material.Component.Rect.sizeDelta = new Vector2(state.Width, state.Height);
                            Material.Component.Background.color = state.BackgroundColor;
                        }
                                                
                        Material.Component.Switching = false;                      

                        if (OnFinish != null)
                            OnFinish();

                        Dispose();
                    }                    
                }
            );
        }
    }

    public class SwitcherMaterial : CoreMaterial
    {
        public int ID { get; set; }
        public UIComponent Component { get; set; }
        public UIState State { get; set; }
    }
}

