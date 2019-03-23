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
    public class listFriendPicker : UIList<listFriendsItem>
    {
        public listFriendPicker()
        {
            this.Name = "listFriendPicker";

            this.Idle = new UIIdleList()
            {
                Float = Float.TOP_CENTER,
                Width = Styles.Screen_Width * 0.96f,
                Height = Styles.Height_Page - Styles.Height_Bar_Medium,
                Top = Styles.Height_Bar_Medium,
                BackgroundColor = Media.colorGreyExtraLight,
                MaxItems = 50,
                FurtherAccess = true
            };

            this.OnPopulate = (index, item) =>
            {
                return new UIPanel()
                {
                    Name = "pnlUser_" + index,
                    Idle = new UIIdlePanel()
                    {
                        Float = Float.TOP_CENTER,
                        Width = Styles.Width_List_Item,
                        Height = Styles.Height_List_Item * 0.75f,
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
                                Width = Styles.Height_List_Item * 0.5f,
                                Height = Styles.Height_List_Item * 0.5f, 
                                Left = Styles.Height_List_Item * 0.25f,                               
                                BackgroundColor = Media.colorOpaque,
                                Url = item.user.picture,
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
                                        Width = Styles.Height_List_Item * 0.5f,
                                        Height = Styles.Height_List_Item * 0.5f,                                       
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
                                Width = Styles.Width_List_Item - Styles.Height_List_Item,                                
                                Left = Styles.Height_List_Item,
                                Alignment = TextAnchor.MiddleLeft,
                                Font = Media.fontExoRegular,
                                FontColor = Media.colorGreyDark,
                                FontSize = Styles.Font_Size_Large,
                                LineHeight = 1,
                                Text = item.user.name
                            }                        
                        },
                        new UIPanel()
                        {
                            Name = "pnlTick" + index,
                            Idle = new UIIdlePanel()
                            {
                                Float = Float.MIDDLE_RIGHT,
                                Width = Styles.Height_List_Item * 0.75f,
                                Height = Styles.Height_List_Item * 0.75f,                                                             
                                BackgroundColor = Media.colorTransparent                               
                            },
                            Components = new List<UIComponent>()
                            {
                                new UIImage()
                                {
                                    Name = "imgTick_" + index,
                                    Idle = new UIIdleImage()
                                    {
                                        Float = Float.MIDDLE_CENTER,
                                        Width = Styles.Height_List_Item * 0.35f,
                                        Height = Styles.Height_List_Item * 0.35f,
                                        BackgroundColor = Media.colorTransparent,
                                        Url = "Images/icon_tick"
                                    },
                                    States = new Dictionary<string,UIState>()
                                    {
                                        {
                                            "Tick_Hide",
                                            new UIStateImage()
                                            {
                                                Width = Styles.Height_List_Item * 0.35f,
                                                Height = Styles.Height_List_Item * 0.35f,
                                                BackgroundColor = Media.colorTransparent
                                            }
                                        },
                                        {
                                            "Tick_Show_" + index,
                                            new UIStateImage()
                                            {
                                                Width = Styles.Height_List_Item * 0.35f,
                                                Height = Styles.Height_List_Item * 0.35f,
                                                BackgroundColor = Media.colorGrey
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
            };

            this.OnTouchInitialization = (list) =>
            {
                return new List<OnTouchListener>()
                {
                    new OnTouchListener()
                    {
                        Owner = list.Name,
                        Target = "%listFriendPickerItem_",
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
                                    listFriendsItem item = list.Data[index];                                    

                                    new StateBroadcaster()
                                    {
                                        Material = new StateBroadcasterMaterial()
                                        {
                                            State = "Tick_Hide"
                                        },
                                        OnFinish = () =>
                                        {
                                            new StateBroadcaster()
                                            {
                                                Material = new StateBroadcasterMaterial()
                                                {
                                                    State = "Tick_Show_" + index
                                                },
                                                OnFinish = () =>
                                                {
                                                    UIText txtFriendsNext = Variables.UI["txtFriendsNext"] as UIText;

                                                    txtFriendsNext.Element.text = "OK, let's Mojo...";

                                                    Variables.Current_Mojo.user = item.user;

                                                    Directives.Sense_Touch = true;
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
    }
}
