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
    public class listAddFriend : UIList<listForeignFriendsItem>
    {
        public listAddFriend()
        {
            this.Name = "listAddFriend";

            this.Idle = new UIIdleList()
            {
                Float = Float.TOP_CENTER,
                Width = Styles.Width_Page_Wide,
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
                        Height = Styles.Height_List_Item_Short,
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
                                Width = Styles.Height_List_Item_Shorter,
                                Height = Styles.Height_List_Item_Shorter, 
                                Left = Styles.Height_List_Item_Shorter * 0.5f,                               
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
                                        Width = Styles.Height_List_Item_Shorter,
                                        Height = Styles.Height_List_Item_Shorter,                                       
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
                                Width = Styles.Width_List_Item - Styles.Height_List_Item_Short,                                
                                Left = Styles.Height_List_Item_Short,
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
                            Name = "clckAdd_" + index,
                            Idle = new UIIdlePanel()
                            {
                                Float = Float.MIDDLE_RIGHT,
                                Width = Styles.Width_List_Item_Action,
                                Height = Styles.Height_List_Item_Short,                                                             
                                BackgroundColor = Media.colorBlack                               
                            },
                            States = new Dictionary<string,UIState>()
                            {
                                {
                                    "Add_" + index,
                                    new UIStatePanel()
                                    {
                                        Width = Styles.Width_List_Item,
                                        Height = Styles.Height_List_Item_Short,
                                        BackgroundColor = Media.colorBlack
                                    }
                                },
                                {
                                    "Added",
                                    new UIStatePanel()
                                    {
                                        Width = Styles.Width_List_Item_Action,
                                        Height = Styles.Height_List_Item_Short,
                                        BackgroundColor = Media.colorBlack
                                    }
                                }
                            },
                            Components = new List<UIComponent>()
                            {
                                new UIText()
                                {
                                    Name = "txtAdd_" + index,
                                    Idle = new UIIdleText()
                                    {
                                        Float = Float.MIDDLE_CENTER,
                                        Width = Styles.Width_List_Item_Action,
                                        Height = Styles.Height_List_Item_Short,
                                        Alignment = TextAnchor.MiddleCenter,
                                        Font = Media.fontExoLight,
                                        FontColor = Media.colorWhite,
                                        FontSize = Styles.Font_Size_Smaller,
                                        LineHeight = 1,
                                        Text = (item.user.requested)?"Requested":"Add",
                                        FurtherAccess = true
                                    },
                                    States = new Dictionary<string,UIState>()
                                    {
                                        {
                                            "Add_" + index,
                                            new UIStateText()
                                            {
                                                Width = Styles.Width_List_Item,                                                
                                                FontColor = Media.colorWhite
                                            }
                                        },
                                        {
                                            "Added",
                                            new UIStateText()
                                            {
                                                Width = Styles.Width_List_Item_Action,
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

            this.OnTouchInitialization = (list) =>
            {
                return new List<OnTouchListener>()
                {
                    new OnTouchListener()
                    {
                        Owner = list.Name,
                        Target = "%clckAdd_",
                        Enabled = true,
                        Released = true,
                        OnRelease = (go) =>
                        {
                            Directives.Sense_Touch = false;

                            int index = int.Parse(go.name.Split('_')[1]);
                            listForeignFriendsItem item = list.Data[index];

                            new StateBroadcaster()
                            {
                                Material = new StateBroadcasterMaterial()
                                {
                                    State = "Add_" + index
                                },
                                OnFinish = ()=>
                                {
                                    (Variables.UI["txtSearch"] as UIText).Element.text = "Find a friend by Name...";
                                    (Variables.UI["txtAdd_" + index] as UIText).Element.text = "Sending request...";

                                    if (item.user.requested)
                                    {
                                        (Variables.UI["txtAdd_" + index] as UIText).Element.text = "You already requested this user...";

                                        new Suspender()
                                        {
                                            Suspension = 1,
                                            OnFinish = () =>
                                            {
                                                (Variables.UI["txtAdd_" + index] as UIText).Element.text = (item.user.requested) ? "Requested" : "Add";

                                                new StateBroadcaster()
                                                {
                                                    Material = new StateBroadcasterMaterial()
                                                    {
                                                        State = "Added"
                                                    },
                                                    OnFinish = () => { Directives.Sense_Touch = true; }                                                    
                                                }
                                                .Broadcast();
                                            }
                                        }
                                        .Suspend();  

                                        return;
                                    }

                                    new HttpPostRequestSender()
                                    {
                                        Material = new HttpPostRequestSenderMaterial()
                                        {
                                            Url = Config.URLs.Friend_Request,
                                            Fields = new Dictionary<string,string>()
                                            {
                                                { "appInstanceKey", Variables.App_Instance_Key },
                                                { "friendFk", item.user.pk.ToString() }
                                            }
                                        },
                                        OnSuccess = (www) =>
                                        {
                                            (Variables.UI["txtAdd_" + index] as UIText).Element.text = "Request sent...";

                                            new Suspender()
                                            {
                                                Suspension = 1,
                                                OnFinish = () =>
                                                {
                                                    (Variables.UI["txtAdd_" + index] as UIText).Element.text = "Requested";

                                                    new StateBroadcaster()
                                                    {
                                                        Material = new StateBroadcasterMaterial()
                                                        {
                                                            State = "Added"
                                                        },
                                                        OnFinish = () => { Directives.Sense_Touch = true; }
                                                    }
                                                    .Broadcast();
                                                }
                                            }
                                            .Suspend(); 
                                        }                                        
                                    }
                                    .Send();                                                                          
                                }
                            }
                            .Broadcast();
                        }
                    }
                };
            };

        }
    }
}
