using System;
using System.Collections.Generic;
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

namespace Assets.Scripts.UI.Lists
{
    public class listSoundtracks : UIList<listSoundtracksItem>
    {
        public listSoundtracks()
        {
            this.Name = "listSoundtracks";

            this.Idle = new UIIdleList()
            {
                Float = Float.TOP_CENTER,
                Width = Styles.Width_Page,
                Height = Styles.Height_Page,
                BackgroundColor = Media.colorGreyExtraLight,
                MaxItems = 50,
                FurtherAccess = true
            };

            this.OnPopulate = (index, item) =>
            {
                if (item.type == "heading")
                    return Item_Heading(index, item);

                if (item.type == "soundtrack")
                    return Item_Soundtrack(index, item);

                return null;
            };

            this.OnTouchInitialization = (list) =>
            {
                return new List<OnTouchListener>()
                {
                    new OnTouchListener()
                    {
                        Owner = list.Name,
                        Target = "%listSoundtracksItem_",
                        Enabled = true,
                        Released = true,
                        OnRelease = (go) =>
                        {                                                      
                            new Suspender()
                            {
                                Suspension = 0.05f,
                                OnFinish = () =>
                                {
                                    if (list.ScrollingDown || list.ScrollingUp)
                                        return;

                                    Directives.Sense_Touch = false;

                                    int index = int.Parse(go.name.Split('_')[1]);
                                    listSoundtracksItem item = list.Data[index];

                                    new StateBroadcaster()
                                    {
                                        Material = new StateBroadcasterMaterial()
                                        {
                                            State = "Tick_Hide"
                                        },
                                        OnFinish = () =>
                                        {
                                            UIText txtAudiosNext = Variables.UI["txtAudiosNext"] as UIText;

                                            if (Variables.Current_Mojo.character == null)
                                                txtAudiosNext.Element.text = "OK, let's select character...";
                                            else
                                                txtAudiosNext.Element.text = "OK, let's leave a message...";

                                            new StateBroadcaster()
                                            {
                                                Material = new StateBroadcasterMaterial()
                                                {
                                                    State = "Tick_Show_" + index
                                                },
                                                OnFinish = () =>
                                                {                                                    
                                                    Variables.Current_Mojo.soundtrack = item.soundtrack;                                                    

                                                    if (Variables.Current_Mojo.character == null)
                                                    {
                                                        new UIPageRenderer<listCharactersItem>()
                                                        {
                                                            Material = new UIPageRendererMaterial<listCharactersItem>()
                                                            {
                                                                Page = new pageCharacters(UIDirection.FROM_RIGHT, UIPageHeight.TALL),
                                                                List = "listCharacters",
                                                                ListUrl = Config.URLs.Characters_Page,
                                                                OnConversion = Conversions.Characters,
                                                                FromCache = true
                                                            }                                                            
                                                        }
                                                        .Render();
                                                    }
                                                    else
                                                    {
                                                        new UIPageRenderer<listItem>()
                                                        {
                                                            Material = new UIPageRendererMaterial<listItem>()
                                                            {
                                                                Page = new pageMessage(UIDirection.FROM_RIGHT, UIPageHeight.TALL)                                                                
                                                            }                                                            
                                                        }
                                                        .Render();
                                                    }
                                                }                                                
                                            }
                                            .Broadcast();
                                        }                                        
                                    }
                                    .Broadcast();
                                }
                            }
                            .Suspend();
                        }
                    }
                };
            };
        }

        private UIComponent Item_Heading(int index, listSoundtracksItem item)
        {
            return new UIPanel()
            {
                Name = "pnlHeading_" + index,
                Idle = new UIIdlePanel()
                {
                    Float = Float.TOP_CENTER,
                    Width = Styles.Width_List_Item,
                    Height = Styles.Height_Bar_Tall,
                    BackgroundColor = Media.colorTransparent
                },
                Components = new List<UIComponent>()
                {
                    new UIPanel()
                    {
                        Name = "pnlHeadingContent_" + index,
                        Idle = new UIIdlePanel()
                        {
                            Float = Float.BOTTOM_CENTER,
                            Width = Styles.Width_List_Item,
                            Height = Styles.Height_Bar_Short,                                                        
                            BackgroundColor = Media.colorBlack
                        },
                        Components = new List<UIComponent>()
                        {
                            new UIText()
                            {
                                Name = "txtHeadingLeft_" + index,
                                Idle = new UIIdleText()
                                {
                                    Float = Float.MIDDLE_CENTER,
                                    Width = Styles.Width_List_Item * 0.94f,
                                    Alignment = TextAnchor.MiddleLeft,
                                    Font = Media.fontExoLight,
                                    FontColor = Media.colorGreyExtraLight,
                                    FontSize = Styles.Font_Size_Large,
                                    LineHeight = 1,
                                    Text = item.text
                                }
                            }                          
                        }
                    }                    
                }
            };
        }

        private UIComponent Item_Soundtrack(int index, listSoundtracksItem item)
        {
            return new UIPanel()
            {
                Name = "pnlAudio_" + index,
                Idle = new UIIdlePanel()
                {
                    Float = Float.TOP_CENTER,
                    Width = Styles.Width_List_Item,
                    Height = Styles.Height_Bar_Tall,
                    Top = Styles.Padding_For_Anything * 0.5f,
                    BackgroundColor = Media.colorWhite
                },
                Components = new List<UIComponent>()
                {
                    new UIImage()
                    {
                        Name = "imgAudio_" + index,
                        Idle = new UIIdleImage()
                        {
                            Float = Float.MIDDLE_LEFT,
                            Width = Styles.Height_Bar_Tall * 0.5f,
                            Height = Styles.Height_Bar_Tall * 0.5f,
                            Left = Styles.Height_Bar_Tall * 0.25f,
                            BackgroundColor = Media.colorGreyDark,
                            Url = (item.soundtrack.type == "song")?"Images/icon_music":(item.soundtrack.type == "voice")?"Images/icon_microphone":(item.soundtrack.type == "movie")?"Images/icon_movie":""
                        }
                    },
                    new UIPanel()
                    {
                        Name = "pnlAudioName_" + index,
                        Idle = new UIIdlePanel()
                        {
                            Float = Float.TOP_LEFT,
                            Width = Styles.Width_List_Item - Styles.Height_Bar_Tall,
                            Height = Styles.Height_Bar_Tall * 0.66f,
                            Left = Styles.Height_Bar_Tall,
                            BackgroundColor = Media.colorTransparent
                        },
                        Components = new List<UIComponent>()
                        {
                            new UIText()
                            {
                                Name = "txtAudioName_" + index,
                                Idle = new UIIdleText()
                                {
                                    Float = Float.MIDDLE_CENTER,
                                    Width = Styles.Width_List_Item - Styles.Height_Bar_Tall,                                    
                                    Alignment = TextAnchor.MiddleLeft,
                                    Font = Media.fontExoRegular,
                                    FontColor = Media.colorGreyDark,
                                    FontSize = Styles.Font_Size_Large,
                                    LineHeight = 1,
                                    Text = item.soundtrack.name
                                }
                            }
                        }
                    },
                    new UIPanel()
                    {
                        Name = "pnlAudioName_" + index,
                        Idle = new UIIdlePanel()
                        {
                            Float = Float.BOTTOM_LEFT,
                            Width = Styles.Width_List_Item - Styles.Height_Bar_Tall,
                            Height = Styles.Height_Bar_Tall * 0.66f,
                            Left = Styles.Height_Bar_Tall,
                            BackgroundColor = Media.colorTransparent
                        },
                        Components = new List<UIComponent>()
                        {
                            new UIText()
                            {
                                Name = "txtAudioBelongsTo_" + index,
                                Idle = new UIIdleText()
                                {
                                    Float = Float.MIDDLE_CENTER,
                                    Width = Styles.Width_List_Item - Styles.Height_Bar_Tall,                                    
                                    Alignment = TextAnchor.UpperLeft,
                                    Font = Media.fontExoLight,
                                    FontColor = Media.colorGrey,
                                    FontSize = Styles.Font_Size_Small,
                                    LineHeight = 1,
                                    Text = item.soundtrack.belongsTo
                                }
                            }
                        }
                    },
                    new UIImage()
                    {
                        Name = "imgAudioTick_" + index,
                        Idle = new UIIdleImage()
                        {
                            Float = Float.MIDDLE_RIGHT,
                            Width = Styles.Height_Bar_Tall * 0.5f,
                            Height = Styles.Height_Bar_Tall * 0.5f,
                            Right = Styles.Height_Bar_Tall * 0.25f,
                            BackgroundColor = Media.colorTransparent,
                            Url = "Images/icon_tick"
                        },
                        States = new Dictionary<string,UIState>()
                        {
                            {
                                "Tick_Hide",
                                new UIStateImage()
                                {
                                    Width = Styles.Height_Bar_Tall * 0.5f,
                                    Height = Styles.Height_Bar_Tall * 0.5f,
                                    Right = Styles.Height_Bar_Tall * 0.25f,
                                    BackgroundColor = Media.colorTransparent
                                }
                            },
                            {
                                "Tick_Show_" + index,
                                new UIStateImage()
                                {
                                    Width = Styles.Height_Bar_Tall * 0.5f,
                                    Height = Styles.Height_Bar_Tall * 0.5f,
                                    Right = Styles.Height_Bar_Tall * 0.25f,
                                    BackgroundColor = Media.colorGrey
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
