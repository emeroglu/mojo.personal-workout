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
    public class listMain : UIList<listMainItem>
    {
        public listMain()
        {
            this.Name = "listMain";

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
                        Target = "%clckHeadingRight_",
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

                                    Variables.Current_Mojo = new JsonMojo() { message = "" };

                                    if (index == 0)
                                    {
                                        Events.OnTouch_Listeners.FirstOrDefault(e => e.Target == "%clckBottom_").Enabled = false;

                                        new UIPageRenderer<listCharactersItem>()
                                        {
                                            Material = new UIPageRendererMaterial<listCharactersItem>()
                                            {
                                                Page = new pageCharacters(UIDirection.FROM_RIGHT, UIPageHeight.TALL),
                                                List = "listCharacters",
                                                ListUrl = Config.URLs.Characters_Page,
                                                ListFields = new Dictionary<string,string>()
                                                {
                                                    { "soundtrackFk" , "0" }
                                                },
                                                OnConversion = Conversions.Characters,
                                                FromCache = true
                                            }                                            
                                        }
                                        .Render();
                                    }
                                    else
                                    {
                                        Events.OnTouch_Listeners.FirstOrDefault(e => e.Target == "%clckBottom_").Enabled = false;

                                        new UIPageRenderer<listSoundtracksItem>()
                                        {
                                            Material = new UIPageRendererMaterial<listSoundtracksItem>()
                                            {
                                                Page = new pageSoundtracks(UIDirection.FROM_RIGHT, UIPageHeight.TALL),
                                                List = "listSoundtracks",
                                                ListUrl = Config.URLs.Soundtracks_Page,
                                                ListFields = new Dictionary<string, string>()
                                                {
                                                    { "characterFk" , "0" }
                                                },
                                                OnConversion = Conversions.Soundtracks,
                                                FromCache = true
                                            }                                            
                                        }
                                        .Render();
                                    }
                                }
                            }
                            .Suspend();
                        }
                    },
                    new OnTouchListener()
                    {
                        Owner = list.Name,
                        Target = "%imgMainAvatar",
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
                                    listMainItem item = list.Data[index];

                                    JsonCharacter character = null;

                                    if (go.name.Contains("Left"))
                                        character = item.character;
                                    else if (go.name.Contains("Right"))
                                        character = item.character2;

                                    Variables.Current_Mojo = new JsonMojo() { character = character, message = "" };

                                    new UIPageRenderer<listSoundtracksItem>()
                                    {
                                        Material = new UIPageRendererMaterial<listSoundtracksItem>()
                                        {
                                            Page = new pageSoundtracks(UIDirection.FROM_RIGHT, UIPageHeight.TALL),
                                            List = "listSoundtracks",
                                            ListUrl = Config.URLs.Soundtracks_Page,
                                            ListFields = new Dictionary<string, string>()
                                            {
                                                { "characterFk" , character.pk.ToString() }
                                            },
                                            OnConversion = Conversions.Soundtracks,
                                            FromCache = true
                                        }                                        
                                    }
                                    .Render();
                                }
                            }
                            .Suspend();                                                                                        
                        }
                    },
                    new OnTouchListener()
                    {
                        Owner = list.Name,
                        Target = "%listMainItem_",
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

                                    if (index <= 3)
                                        return;

                                    listMainItem item = list.Data[index];

                                    Variables.Current_Mojo = new JsonMojo() { soundtrack = item.soundtrack, message = "" };

                                    new UIPageRenderer<listCharactersItem>()
                                    {
                                        Material = new UIPageRendererMaterial<listCharactersItem>()
                                        {
                                            Page = new pageCharacters(UIDirection.FROM_RIGHT, UIPageHeight.TALL),
                                            List = "listCharacters",
                                            ListUrl = Config.URLs.Characters_Page,
                                            ListFields = new Dictionary<string, string>()
                                            {
                                                { "soundtrackFk" , item.soundtrack.pk.ToString() }
                                            },
                                            OnConversion = Conversions.Characters,
                                            FromCache = true
                                        }                                        
                                    }
                                    .Render();
                                }
                            }
                            .Suspend(); 
                        }
                    }
                };
            };
        }

        private UIComponent Item_Heading(int index, listMainItem item)
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
                            },
                            new UIPanel()
                            {
                                Name = "clckHeadingRight_" + index,
                                Idle = new UIIdlePanel()
                                {
                                    Float = Float.MIDDLE_RIGHT,
                                    Width = Styles.Width_Bar_Single * 0.6f,
                                    Height = Styles.Height_Bar_Short,
                                    BackgroundColor = Media.colorTransparent
                                },
                                Components = new List<UIComponent>()
                                {
                                    new UIText()
                                    {
                                        Name = "txtHeadingRight_" + index,
                                        Idle = new UIIdleText()
                                        {
                                            Float = Float.MIDDLE_CENTER,
                                            Width = Styles.Width_Bar_Single * 0.6f,
                                            Alignment = TextAnchor.MiddleCenter,
                                            Font = Media.fontExoLight,
                                            FontColor = Media.colorWhite,
                                            FontSize = Styles.Font_Size_Small,
                                            LineHeight = 1,
                                            Text = "See all"
                                        }
                                    }
                                }
                            }                            
                        }
                    }                    
                }
            };
        }

        private UIComponent Item_Character(int index, listMainItem item)
        {
            return new UIPanel()
            {
                Name = "pnlAvatar_" + index,
                Idle = new UIIdlePanel()
                {
                    Float = Float.TOP_CENTER,
                    Width = Styles.Width_List_Item,
                    Height = Styles.Width_List_Item * 0.5f,
                    BackgroundColor = Media.colorWhite
                },
                Components = new List<UIComponent>()
                {
                    new UIImage()
                    {
                        Name = "imgMainAvatarLeft_" + index,
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
                                Name = "imgMainAvatarLeftMask_" + index,
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
                                Name = "txtMainAvatarLeftName_" + index,
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
                            }
                        }
                    },
                    new UIImage()
                    {
                        Name = "imgMainAvatarRight_" + index,
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
                                Name = "imgMainAvatarRightMask_" + index,
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
                                Name = "txtMainAvatarRightName_" + index,
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
                            }
                        }
                    }
                }
            };
        }

        private UIComponent Item_Soundtrack(int index, listMainItem item)
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
                        Name = "imgAudioArrow_" + index,
                        Idle = new UIIdleImage()
                        {
                            Float = Float.MIDDLE_RIGHT,
                            Width = Styles.Height_Bar_Tall * 0.3f,
                            Height = Styles.Height_Bar_Tall * 0.3f,
                            Right = Styles.Height_Bar_Tall * 0.15f,
                            BackgroundColor = Media.colorGrey,
                            Url = "Images/icon_arrow"
                        }
                    }
                }
            };
        }
    }
}
