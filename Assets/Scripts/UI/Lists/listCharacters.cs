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
    public class listCharacters : UIList<listCharactersItem>
    {
        public listCharacters()
        {
            this.Name = "listCharacters";

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

                if (item.type == "character")
                    return Item_Character(index, item);

                return null;
            };

            this.OnTouchInitialization = (list) =>
            {
                return new List<OnTouchListener>()
                {
                    new OnTouchListener()
                    {
                        Owner = list.Name,
                        Target = "%imgAvatar",
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
                                    listCharactersItem item = list.Data[index];

                                    JsonCharacter avatar = null;

                                    if (go.name.Contains("Left"))
                                        avatar = item.character;
                                    else if (go.name.Contains("Right"))
                                        avatar = item.character2;

                                    new StateBroadcaster()
                                    {
                                        Material = new StateBroadcasterMaterial()
                                        {
                                            State = "Tick_Hide"
                                        },
                                        OnFinish = () =>
                                        {
                                            UIText txtAvatarsNext = Variables.UI["txtAvatarsNext"] as UIText;

                                            if (Variables.Current_Mojo.soundtrack == null)
                                                txtAvatarsNext.Element.text = "OK, let's select sound...";
                                            else
                                                txtAvatarsNext.Element.text = "OK, let's leave a message...";

                                            new StateBroadcaster()
                                            {
                                                Material = new StateBroadcasterMaterial()
                                                {
                                                    State = "Tick_Show_" + index + "_" + avatar.pk
                                                },
                                                OnFinish = () =>
                                                {                                                    
                                                    Variables.Current_Mojo.character = avatar;                                                    

                                                    if (Variables.Current_Mojo.soundtrack == null)
                                                    {
                                                        new UIPageRenderer<listSoundtracksItem>()
                                                        {
                                                            Material = new UIPageRendererMaterial<listSoundtracksItem>()
                                                            {
                                                                Page = new pageSoundtracks(UIDirection.FROM_RIGHT, UIPageHeight.TALL),
                                                                List = "listSoundtracks",
                                                                ListUrl = Config.URLs.Soundtracks_Page,
                                                                OnConversion = Conversions.Soundtracks,
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

        private UIComponent Item_Heading(int index, listCharactersItem item)
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

        private UIComponent Item_Character(int index, listCharactersItem item)
        {
            return new UIPanel()
            {
                Name = "pnlAvatar_" + index,
                Idle = new UIIdlePanel()
                {
                    Float = Float.TOP_CENTER,
                    Width = Styles.Width_List_Item,
                    Height = Styles.Width_List_Item * 0.5f,
                    BackgroundColor = Media.colorTransparent
                },
                Components = new List<UIComponent>()
                {
                    new UIImage()
                    {
                        Name = "imgAvatarLeft_" + index + "_" + item.character.pk,
                        Idle = new UIIdleImage()
                        {
                            Float = Float.MIDDLE_LEFT,
                            Width = Styles.Width_List_Item * 0.5f,
                            Height = Styles.Width_List_Item * 0.5f,
                            BackgroundColor = Media.colorOpaque,
                            Url = item.character.picture,
                            LazyLoad = true,
                            LazyLoadSuspension = 1
                        },
                        Components = new List<UIComponent>()
                        {
                            new UIImage()
                            {
                                Name = "imgAvatarLeftMask_" + index + "_" + item.character.pk,
                                Idle = new UIIdleImage()
                                {
                                    Float = Float.MIDDLE_CENTER,
                                    Width = Styles.Width_List_Item * 0.5f,
                                    Height = Styles.Width_List_Item * 0.5f,
                                    BackgroundColor = Media.colorBlackSemiTransparent,
                                    Url = "Images/solid"
                                }
                            },
                            new UIText()
                            {
                                Name = "txtAvatarLeftName_" + index + "_" + item.character.pk,
                                Idle = new UIIdleText()
                                {
                                    Float = Float.BOTTOM_CENTER,
                                    Width = Styles.Width_List_Item * 0.5f,
                                    Bottom = Styles.Padding_For_Anything,
                                    Alignment = TextAnchor.MiddleCenter,
                                    Font = Media.fontExoLight,
                                    FontColor = Media.colorWhite,
                                    FontSize = Styles.Font_Size_Large,
                                    LineHeight = 1,
                                    Text = item.character.name
                                }
                            },
                            new UIImage()
                            {
                                Name = "imgTick_" + index + "_" + item.character.pk,
                                Idle = new UIIdleImage()
                                {
                                    Float = Float.MIDDLE_CENTER,
                                    Width = Styles.Width_Bar_Single * 0.75f,
                                    Height = Styles.Width_Bar_Single * 0.75f,
                                    BackgroundColor = Media.colorTransparent,
                                    Url = "Images/icon_tick"
                                },
                                States = new Dictionary<string,UIState>()
                                {
                                    {
                                        "Tick_Hide",
                                        new UIStateImage()
                                        {
                                            Width = Styles.Width_Bar_Single * 0.75f,
                                            Height = Styles.Width_Bar_Single * 0.75f,
                                            BackgroundColor = Media.colorTransparent
                                        }
                                    },
                                    {
                                        "Tick_Show_" + index + "_" + item.character.pk,
                                        new UIStateImage()
                                        {
                                            Width = Styles.Width_Bar_Single * 0.75f,
                                            Height = Styles.Width_Bar_Single * 0.75f,
                                            BackgroundColor = Media.colorWhite
                                        }
                                    }
                                }
                            }
                        }
                    },
                    (item.character2 == null)?null:
                    new UIImage()
                    {
                        Name = "imgAvatarRight_" + index + "_" + item.character2.pk,
                        Idle = new UIIdleImage()
                        {
                            Float = Float.MIDDLE_RIGHT,
                            Width = Styles.Width_List_Item * 0.5f,
                            Height = Styles.Width_List_Item * 0.5f,
                            BackgroundColor = Media.colorOpaque,
                            Url = item.character2.picture,
                            LazyLoad = true,
                            LazyLoadSuspension = 1
                        },
                        Components = new List<UIComponent>()
                        {
                            new UIImage()
                            {
                                Name = "imgAvatarRightMask_" + index + "_" + item.character2.pk,
                                Idle = new UIIdleImage()
                                {
                                    Float = Float.MIDDLE_CENTER,
                                    Width = Styles.Width_List_Item * 0.5f,
                                    Height = Styles.Width_List_Item * 0.5f,
                                    BackgroundColor = Media.colorBlackSemiTransparent,
                                    Url = "Images/solid"
                                }
                            },
                            new UIText()
                            {
                                Name = "txtAvatarRightName_" + index + "_" + item.character2.pk,
                                Idle = new UIIdleText()
                                {
                                    Float = Float.BOTTOM_CENTER,
                                    Width = Styles.Width_List_Item * 0.5f,
                                    Bottom = Styles.Padding_For_Anything,
                                    Alignment = TextAnchor.MiddleCenter,
                                    Font = Media.fontExoLight,
                                    FontColor = Media.colorWhite,
                                    FontSize = Styles.Font_Size_Large,
                                    LineHeight = 1,
                                    Text = item.character2.name
                                }
                            },
                            new UIImage()
                            {
                                Name = "imgTick_" + index + "_" + item.character2.pk,
                                Idle = new UIIdleImage()
                                {
                                    Float = Float.MIDDLE_CENTER,
                                    Width = Styles.Width_Bar_Single * 0.75f,
                                    Height = Styles.Width_Bar_Single * 0.75f,
                                    BackgroundColor = Media.colorTransparent,
                                    Url = "Images/icon_tick"
                                },
                                States = new Dictionary<string,UIState>()
                                {
                                    {
                                        "Tick_Hide",
                                        new UIStateImage()
                                        {
                                            Width = Styles.Width_Bar_Single * 0.75f,
                                            Height = Styles.Width_Bar_Single * 0.75f,
                                            BackgroundColor = Media.colorTransparent
                                        }
                                    },
                                    {
                                        "Tick_Show_" + index + "_" + item.character2.pk,
                                        new UIStateImage()
                                        {
                                            Width = Styles.Width_Bar_Single * 0.75f,
                                            Height = Styles.Width_Bar_Single * 0.75f,
                                            BackgroundColor = Media.colorWhite
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
