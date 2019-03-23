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
    public class listInbox : UIList<listInboxItem>
    {
        public listInbox()
        {
            this.Name = "listInbox";

            this.Idle = new UIIdleList()
            {
                Float = Float.TOP_CENTER,
                Width = Styles.Screen_Width * 0.96f,
                Height = Styles.Height_Page,
                BackgroundColor = Media.colorGreyExtraLight,
                MaxItems = 50,
                FurtherAccess = true
            };

            this.OnPopulate = (index, item) =>
            {
                return new UIPanel()
                {
                    Name = "pnlMojo_" + index,
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.TOP_CENTER,
                        Width = Styles.Width_List_Item,
                        Height = Styles.Height_List_Item,
                        Top = (index == 0) ? Styles.Top_List_Item * 2f : Styles.Top_List_Item,
                        BackgroundColor = Media.colorWhite
                    },
                    Components = new List<UIComponent>()
                    {
                        new UIImage()
                        {
                            Name = "imgProfile_" + index,
                            Idle = new UIIdleImage()
                            {
                                Float = Float.MIDDLE_LEFT,
                                Width = Styles.Side_Inbox_Mojo_Item_Profile,
                                Height = Styles.Side_Inbox_Mojo_Item_Profile, 
                                Left = Styles.Left_Inbox_Mojo_Item_Profile,                               
                                BackgroundColor = Media.colorOpaque,
                                Url = (item.mojo.pk == 0)?"Images/icon_app":item.mojo.user.picture,
                                LazyLoad = true,
                                LazyLoadSuspension = 1.5f
                            },
                            Components = new List<UIComponent>()
                            {
                                new UIImage()
                                {
                                    Name = "imgMask_" + index,
                                    Idle = new UIIdleImage()
                                    {
                                        Float = Float.TOP_LEFT,
                                        Width = Styles.Side_Inbox_Mojo_Item_Profile,
                                        Height = Styles.Side_Inbox_Mojo_Item_Profile,                                       
                                        BackgroundColor = Media.colorWhite,
                                        Url = "Images/in_rounded"
                                    }
                                }
                            }
                        },
                        new UIText()
                        {
                            Name = "txtUser_" + index,
                            Idle = new UIIdleText()
                            {
                                Float = Float.MIDDLE_LEFT,
                                Width = Styles.Width_Inbox_Mojo_Item_Username,
                                Bottom = Screen.width * 0.05f,
                                Left = Styles.Side_Inbox_Mojo_Item_Profile + Styles.Left_Inbox_Mojo_Item_Profile * 2,
                                Alignment = TextAnchor.MiddleLeft,
                                Font = Media.fontExoRegular,
                                FontColor = Media.colorGreyDark,
                                FontSize = Styles.Font_Size_Large,
                                LineHeight = 1,
                                Text = item.mojo.user.name
                            }
                        },
                        new UIText()
                        {
                            Name = "txtMessage_" + index,
                            Idle = new UIIdleText()
                            {
                                Float = Float.TOP_LEFT,
                                Width = Styles.Width_Inbox_Mojo_Item_Message,
                                Top = Styles.Top_Inbox_Mojo_Item_Message * 3.25f,
                                Left = Styles.Side_Inbox_Mojo_Item_Profile + Styles.Left_Inbox_Mojo_Item_Profile * 2,
                                Alignment = TextAnchor.MiddleLeft,
                                Font = Media.fontExoLight,
                                FontColor = Media.colorGrey,
                                FontSize = Styles.Font_Size_Medium,
                                LineHeight = 1.25f,
                                Text = (item.mojo.pk == 0)?"sent your very first mojo with a " + item.mojo.character.name:"sent you a mojo with " + item.mojo.character.name
                            }
                        },
                        new UIImage()
                        {
                            Name = "clckAvatar_" + index,
                            Idle = new UIIdleImage()
                            {
                                Float = Float.MIDDLE_RIGHT,
                                Width = Styles.Side_Inbox_Mojo_Item_Target,
                                Height = Styles.Side_Inbox_Mojo_Item_Target,                                                             
                                BackgroundColor = Media.colorOpaque,
                                Url = item.mojo.character.picture,
                                LazyLoad = true,
                                LazyLoadSuspension = 1.5f
                            },
                            Components = new List<UIComponent>()
                            {
                                new UIImage()
                                {
                                    Name = "imgAvatarMask_" + index,
                                    Idle = new UIIdleImage()
                                    {
                                        Float = Float.MIDDLE_RIGHT,
                                        Width = Styles.Side_Inbox_Mojo_Item_Target,
                                        Height = Styles.Side_Inbox_Mojo_Item_Target,                                       
                                        BackgroundColor = Media.colorBlackTwoThirdsTransparent,
                                        Url = "Images/solid"
                                    },
                                    States = new Dictionary<string,UIState>()
                                    {
                                        {
                                            "View_" + index,
                                            new UIStateImage()
                                            {
                                                Width = Styles.Width_List_Item,
                                                Height = Styles.Height_List_Item,                                       
                                                BackgroundColor = Media.colorBlack                                                
                                            }
                                        },
                                        {
                                            "Delayed_View",
                                            new UIStateImage()
                                            {
                                                Width = Styles.Side_Inbox_Mojo_Item_Target,
                                                Height = Styles.Side_Inbox_Mojo_Item_Target,                                       
                                                BackgroundColor = Media.colorBlackTwoThirdsTransparent,                                                
                                            }
                                        }
                                    }
                                },
                                new UIText()
                                {
                                    Name = "txtView_" + index,
                                    Idle = new UIIdleText()
                                    {
                                        Float = Float.MIDDLE_RIGHT,
                                        Width = Styles.Side_Inbox_Mojo_Item_Target,                                        
                                        Alignment = TextAnchor.MiddleCenter,
                                        Font = Media.fontExoLight,
                                        FontColor = Media.colorWhite,
                                        FontSize = Styles.Font_Size_Large,
                                        LineHeight = 1,
                                        Text = "View"                                        
                                    },
                                    States = new Dictionary<string,UIState>()
                                    {
                                        {
                                            "View_" + index,
                                            new UIStateText()
                                            {
                                                Width = Styles.Width_List_Item,                                                                                      
                                                FontColor = Media.colorWhite                                                
                                            }
                                        },
                                        {
                                            "Delayed_View",
                                            new UIStateText()
                                            {
                                                Width = Styles.Side_Inbox_Mojo_Item_Target,                                                                                      
                                                FontColor = Media.colorWhite                                                
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
            };

            this.OnRefresh = (list) =>
            {
                Directives.Sense_Touch = false;

                new UIListRenderer<listInboxItem>()
                {
                    Material = new UIListRendererMaterial<listInboxItem>()
                    {
                        List = this.Name,
                        ListUrl = Config.URLs.Inbox_Page,
                        OnConversion = Conversions.Inbox
                    },
                    OnFinish = () =>
                    {
                        list.Refreshing = false;

                        Directives.Sense_Touch = true;

                        new Suspender()
                        {
                            Suspension = 1,
                            OnFinish = () =>
                            {
                                (Variables.UI["txtTopRight"] as UIText).Element.text = "Inbox";
                            }
                        }
                        .Suspend();
                    }                    
                }
                .Render();
            };

            this.OnTouchInitialization = (list) =>
            {
                return new List<OnTouchListener>()
                {
                    new OnTouchListener()
                    {
                        Owner = this.Name,
                        Target = "%clckAvatar_",
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

                                    Variables.Current_Mojo = list.Data[index].mojo;

                                    new Preloader()
                                    {
                                        Material = new PreloaderMaterial()
                                        {
                                            Index = index                                            
                                        },
                                        OnFinish = ()=> { Directives.Sense_Touch = true; }                                        
                                    }
                                    .Preload();
                                }
                            }
                            .Suspend();                                              
                        }                        
                    }
                };
            };
        }
    }
}
