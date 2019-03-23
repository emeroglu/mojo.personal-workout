
using Assets.Scripts.Abstract.Json;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Agents.Tools;
using Assets.Scripts.Core;
using Assets.Scripts.Repository;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.UI.Panels;
using Assets.Scripts.UI.ListItems;
using Assets.Scripts.Agents.Renderers;
using Assets.Scripts.UI.Pages;
using Facebook.Unity;

namespace Assets.Scripts.Agents.Initializers
{
    public class AppInitializer : CoreProcessor<CoreMaterial>
    {
        public void Initialize()
        {
            Perform();
        }

        protected override bool Condition()
        {
            return true;
        }

        protected override void OnInterruption()
        {

        }

        protected override bool DebugLog() { return false; }

        protected override List<Task> Tasks()
        {
            return new List<Task>()
            {                
                Initialize_System(),                                
                Initiate_Listeners(),                 
                Initialize_UI(),                                 
#if !UNITY_EDITOR
                Register_For_Notification(),  
#endif                               
                Initialize_Root(),   
                Retrieve_Version(),
                Reset_If_Needed(),
                Initialize_Directories(),  
                Initialize_Image_Cache(),     
                Facebook_Init(),
                Needs_Login(),                                 
                Save_Identity(),   
                App_Instance(),                                                                                                                 
                Retrieve_Cache(),                                              
                Start_App()
            };
        }

        private Task Initialize_System()
        {
            return new Task()
            {
                Mission = "Initialize_System",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    new SystemInitializer()
                    {
                        OnFinish = () => { nextTask(null); }
                    }
                    .Initialize();
                }
            };
        }

        private Task Initiate_Listeners()
        {
            return new Task()
            {
                Mission = "Initiate_Listeners",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    new EventInitializer()
                    {
                        OnFinish = () =>
                        {
                            Directives.Events_Initialized = true;

                            nextTask(null);
                        }
                    }
                    .Initialize();
                }
            };
        }

        private Task Initialize_UI()
        {
            return new Task()
            {
                Mission = "Initialize_UI",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    new UIRenderer()
                    {
                        Material = new UIRendererMaterial()
                        {
                            Parent = Objects.Canvas,
                            Component = new pnlMain(OnFail)
                        },
                        OnFinish = () =>
                        {
                            new StateBroadcaster()
                            {
                                Material = new StateBroadcasterMaterial()
                                {
                                    State = "Initializing"
                                },
                                OnFinish = () =>
                                {
                                    Directives.UI_Initialized = true;

                                    nextTask(null);
                                }
                            }
                            .Broadcast();
                        }
                    }
                    .Render();
                }
            };
        }

        private Task Register_For_Notification()
        {
            return new Task()
            {
                Mission = "Register_For_Notification",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    OneSignal.StartInit("bc3cdf55-f7c1-437a-a9ed-7585fb756f66")
                    .HandleNotificationOpened(HandleNotificationOpened)
                    .HandleNotificationReceived(HandleNotificationReceived)
                    .InFocusDisplaying(OneSignal.OSInFocusDisplayOption.Notification)
                    .EndInit();

                    nextTask(null);
                }
            };
        }

        private void HandleNotificationReceived(OSNotification notification)
        {
            Directives.App_Notification = true;

            Variables.Notification_Type = notification.payload.additionalData["type"] as string;
        }

        private void HandleNotificationOpened(OSNotificationOpenedResult result)
        {
            Directives.App_Notification = true;

            Variables.Notification_Type = result.notification.payload.additionalData["type"] as string;

            if (Directives.App_Opened)
            {
                if (Variables.Notification_Type == "mojo")
                {
                    new UIPageRenderer<listInboxItem>()
                    {
                        Material = new UIPageRendererMaterial<listInboxItem>()
                        {
                            Page = new pageInbox(UIDirection.FROM_LEFT, UIPageHeight.NORMAL),
                            List = "listInbox",
                            ListUrl = Config.URLs.Inbox_Page,
                            OnConversion = Conversions.Inbox
                        }
                    }
                    .Render();
                }
                else if (Variables.Notification_Type == "request")
                {
                    new UIPageRenderer<listFriendRequestsItem>()
                    {
                        Material = new UIPageRendererMaterial<listFriendRequestsItem>()
                        {
                            Page = new pageFriendRequests(UIDirection.FROM_RIGHT, UIPageHeight.TALL),
                            List = "listFriendRequests",
                            ListUrl = Config.URLs.Friend_Requests_Page,
                            OnConversion = Conversions.Friend_Requests
                        }
                    }
                    .Render();
                }
            }
        }

        private Task Initialize_Root()
        {
            return new Task()
            {
                Mission = "Initialize_Root",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (!Directory.Exists(Config.Directories.Root))
                    {
                        Directives.App_First = true;

                        Directory.CreateDirectory(Config.Directories.Root);
                    }

                    nextTask(null);
                }
            };
        }

        private Task Retrieve_Version()
        {
            return new Task()
            {
                Mission = "Retrieve_Version",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (File.Exists(Config.Directories.Version))
                    {
                        FileStream stream = new FileStream(Config.Directories.Version, FileMode.Open, FileAccess.Read);
                        StreamReader reader = new StreamReader(stream);

                        int version = int.Parse(reader.ReadToEnd());

                        reader.Close();
                        stream.Close();

                        nextTask(version);
                    }
                    else
                        nextTask(0);
                }
            };
        }

        private Task Reset_If_Needed()
        {
            return new Task()
            {
                Mission = "Reset_If_Needed",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    int version = (int)delivery;

                    if (version != Config.Variables.App_Version)
                    {
                        Directory.Delete(Config.Directories.Root, true);

                        Directory.CreateDirectory(Config.Directories.Root);

                        FileStream stream = new FileStream(Config.Directories.Version, FileMode.Create, FileAccess.Write);
                        StreamWriter writer = new StreamWriter(stream);

                        writer.Write(Config.Variables.App_Version);
                        writer.Flush();
                        writer.Close();
                        stream.Close();
                    }

                    nextTask(null);
                }
            };
        }

        private Task Initialize_Directories()
        {
            return new Task()
            {
                Mission = "Initialize_Directories",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (!Directory.Exists(Config.Directories.Animators_Folder))
                    {
                        Directory.CreateDirectory(Config.Directories.Animators_Folder);
                    }

                    if (!Directory.Exists(Config.Directories.Characters_Folder))
                    {
                        Directory.CreateDirectory(Config.Directories.Characters_Folder);
                    }

                    if (!Directory.Exists(Config.Directories.Soundtracks_Folder))
                    {
                        Directory.CreateDirectory(Config.Directories.Soundtracks_Folder);
                    }

                    if (!Directory.Exists(Config.Directories.Skyboxes_Folder))
                    {
                        Directory.CreateDirectory(Config.Directories.Skyboxes_Folder);
                    }

                    if (!Directory.Exists(Config.Directories.Environments_Folder))
                    {
                        Directory.CreateDirectory(Config.Directories.Environments_Folder);
                    }

                    if (!File.Exists(Config.Directories.Animators_Cache_File))
                    {
                        FileStream stream = new FileStream(Config.Directories.Animators_Cache_File, FileMode.CreateNew, FileAccess.Write);
                        stream.Close();
                    }

                    if (!File.Exists(Config.Directories.Characters_Cache_File))
                    {
                        FileStream stream = new FileStream(Config.Directories.Characters_Cache_File, FileMode.CreateNew, FileAccess.Write);
                        stream.Close();
                    }

                    if (!File.Exists(Config.Directories.Soundtracks_Cache_File))
                    {
                        FileStream stream = new FileStream(Config.Directories.Soundtracks_Cache_File, FileMode.CreateNew, FileAccess.Write);
                        stream.Close();
                    }

                    if (!File.Exists(Config.Directories.Skyboxes_Cache_File))
                    {
                        FileStream stream = new FileStream(Config.Directories.Skyboxes_Cache_File, FileMode.CreateNew, FileAccess.Write);
                        stream.Close();
                    }

                    if (!File.Exists(Config.Directories.Environments_Cache_File))
                    {
                        FileStream stream = new FileStream(Config.Directories.Environments_Cache_File, FileMode.CreateNew, FileAccess.Write);
                        stream.Close();
                    }

                    nextTask(null);
                }
            };
        }

        private Task Initialize_Image_Cache()
        {
            return new Task()
            {
                Mission = "Initialize_Image_Cache",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (!Directory.Exists(Config.Directories.Images))
                    {
                        Directory.CreateDirectory(Config.Directories.Images);
                    }

                    FileStream stream = new FileStream(Config.Directories.Images_Cache_File, FileMode.OpenOrCreate, FileAccess.Read);
                    StreamReader reader = new StreamReader(stream);

                    string imageCache = reader.ReadToEnd();

                    reader.Close();
                    stream.Close();

                    if (!string.IsNullOrEmpty(imageCache))
                    {
                        Cache.Image_Cache = JsonConvert.DeserializeObject<List<JsonImage>>(imageCache);
                        Variables.Image_Cache_Inc = Cache.Image_Cache.Count;
                    }

                    nextTask(null);
                }
            };
        }

        private Task Facebook_Init()
        {
            return new Task()
            {
                Mission = "Facebook_Init",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    FB.Init(() => { nextTask(null); });
                }
            };
        }

        private Task Needs_Login()
        {
            return new Task()
            {
                Mission = "Needs_Login",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (FB.IsLoggedIn)
                    {
                        if (File.Exists(Config.Directories.Identity))
                        {
                            FileStream stream = new FileStream(Config.Directories.Identity, FileMode.Open, FileAccess.Read);
                            StreamReader reader = new StreamReader(stream);

                            string json = reader.ReadToEnd();

                            reader.Close();
                            stream.Close();

                            Variables.Self = JsonConvert.DeserializeObject<JsonSelf>(json);

                            if (Variables.Self.fbId != AccessToken.CurrentAccessToken.UserId)
                            {
                                File.Delete(Config.Directories.Identity);

                                currentTask(null);
                            }
                            else
                                nextTask(null);
                        }
                        else
                        {
                            FB.API("/me?fields=name,email,picture.type(large)", HttpMethod.POST, (graphResult) =>
                            {
                                Variables.Self = new JsonSelf()
                                {
                                    key = "",
                                    fbId = graphResult.ResultDictionary["id"].ToString(),
                                    email = graphResult.ResultDictionary["email"].ToString(),
                                    name = graphResult.ResultDictionary["name"].ToString(),
                                    picture = ((graphResult.ResultDictionary["picture"] as Dictionary<string, object>)["data"] as Dictionary<string, object>)["url"].ToString()
                                };

                                (Variables.UI["txtConsole"] as UIText).Element.text = "Logged you in...";


#if UNITY_EDITOR
                                nextTask(null);
#endif
#if !UNITY_EDITOR
                                OneSignal.IdsAvailable(HandleID);                                        
                                Variables.nextTask = nextTask;
#endif

                            });

                        }
                    }
                    else
                    {
                        Directives.App_First = true;

                        (Variables.UI["txtConsole"] as UIText).Element.text = "Need to log you in...";

                        List<string> perms = new List<string>() { "public_profile", "email", "user_friends" };
                        FB.LogInWithReadPermissions(perms, (result) =>
                        {
                            if (result.Cancelled)
                                Application.Quit();

                            if (FB.IsLoggedIn)
                            {
                                FB.API("/me?fields=name,email,picture.type(large)", HttpMethod.POST, (graphResult) =>
                                {
                                    Variables.Self = new JsonSelf()
                                    {
                                        key = "",
                                        fbId = graphResult.ResultDictionary["id"].ToString(),
                                        email = graphResult.ResultDictionary["email"].ToString(),
                                        name = graphResult.ResultDictionary["name"].ToString(),
                                        picture = ((graphResult.ResultDictionary["picture"] as Dictionary<string, object>)["data"] as Dictionary<string, object>)["url"].ToString()
                                    };

                                    (Variables.UI["txtConsole"] as UIText).Element.text = "Logged you in...";


#if UNITY_EDITOR
                                    nextTask(null);
#endif
#if !UNITY_EDITOR
                                        OneSignal.IdsAvailable(HandleID);                                        
                                        Variables.nextTask = nextTask;
#endif

                                });
                            }
                        });
                    }
                }
            };
        }

        private void HandleID(string playerID, string pushToken)
        {
            Variables.Self.key = playerID;
            Variables.nextTask(null);
        }

        private Task Save_Identity()
        {
            return new Task()
            {
                Mission = "Save_Identity",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    FileStream stream = new FileStream(Config.Directories.Identity, FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(stream);

                    string json = JsonConvert.SerializeObject(Variables.Self);

                    writer.Write(json);
                    writer.Flush();
                    writer.Close();
                    stream.Close();

                    nextTask(null);
                }
            };
        }

        private Task App_Instance()
        {
            return new Task()
            {
                Mission = "App_Instance",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (Directives.App_First)
                        (Variables.UI["txtConsole"] as UIText).Element.text = "Getting ready for first use...";
                    else
                        (Variables.UI["txtConsole"] as UIText).Element.text = "Initializing...";

                    new Suspender()
                    {
                        Suspension = 10,
                        OnFinish = () =>
                        {
                            if (!Directives.App_Opened)
                            {
                                (Variables.UI["txtConsole"] as UIText).Element.text = "Might take a bit long...";
                            }
                        }
                    }
                    .Suspend();

                    new HttpPostRequestSender()
                    {
                        Material = new HttpPostRequestSenderMaterial()
                        {
                            Url = Config.URLs.App_Instance,
                            Fields = new Dictionary<string, string>()
                            {
                                { "fbId" , Variables.Self.fbId },                                                                                                                               
                                { "email" , Variables.Self.email },
                                { "name" , Variables.Self.name },
                                { "picture" , Variables.Self.picture },
                                { "uniqueKey" , SystemInfo.deviceUniqueIdentifier },
                                { "notificationKey" , Variables.Self.key },
                                { "deviceName" , (SystemInfo.deviceName.ToLower().Contains("unknown") ? "Unknown" : SystemInfo.deviceName) },
                                { "devicePlatform" , Application.platform.ToString() },
                                { "deviceType" , SystemInfo.deviceType.ToString() },
                                { "deviceScreenWidth" , Styles.Screen_Width.ToString() },
                                { "deviceScreenHeight" , Styles.Screen_Height.ToString() },
                                { "deviceScreenRatio" , (Styles.Screen_Ratio.ToString() + "0000000").Replace(".",",").Substring(0,7) },
                                { "appVersion" , Config.Variables.App_Version.ToString() },
                            }
                        },
                        OnSuccess = (www) =>
                        {
                            Directives.App_Opened = true;

                            if (www.text == "signup")
                            {
                                seekTo("Needs_Login", null);
                            }
                            else if (www.text == "update")
                            {
                                string[] urls = www.text.Split(',');

                                if (Application.platform == RuntimePlatform.Android)
                                    Application.OpenURL(urls[1]);

                                if (Application.platform == RuntimePlatform.IPhonePlayer)
                                    Application.OpenURL(urls[2]);
                            }
                            else
                            {
                                Variables.App_Instance_Key = www.text;

                                nextTask(null);
                            }
                        }
                    }
                    .Send();
                }
            };
        }

        private Task Retrieve_Cache()
        {
            return new Task()
            {
                Mission = "Retrieve_Cache",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    FileStream stream = new FileStream(Config.Directories.Skyboxes_Cache_File, FileMode.Open, FileAccess.Read);
                    StreamReader reader = new StreamReader(stream);

                    string skyboxCache = reader.ReadToEnd();
                    reader.Close();
                    stream.Close();

                    if (!string.IsNullOrEmpty(skyboxCache))
                        Cache.Skybox_Cache = JsonConvert.DeserializeObject<List<JsonCache>>(skyboxCache);

                    FileStream stream2 = new FileStream(Config.Directories.Characters_Cache_File, FileMode.Open, FileAccess.Read);
                    StreamReader reader2 = new StreamReader(stream2);

                    string avatarCache = reader2.ReadToEnd();
                    reader2.Close();
                    stream2.Close();

                    if (!string.IsNullOrEmpty(avatarCache))
                        Cache.Avatar_Cache = JsonConvert.DeserializeObject<List<JsonCache>>(avatarCache);

                    FileStream stream3 = new FileStream(Config.Directories.Soundtracks_Cache_File, FileMode.Open, FileAccess.Read);
                    StreamReader reader3 = new StreamReader(stream3);

                    string audioCache = reader3.ReadToEnd();

                    reader3.Close();
                    stream3.Close();

                    if (!string.IsNullOrEmpty(audioCache))
                        Cache.Music_Cache = JsonConvert.DeserializeObject<List<JsonCache>>(audioCache);

                    FileStream stream4 = new FileStream(Config.Directories.Animators_Cache_File, FileMode.Open, FileAccess.Read);
                    StreamReader reader4 = new StreamReader(stream4);

                    string animatorCache = reader4.ReadToEnd();

                    reader4.Close();
                    stream4.Close();

                    if (!string.IsNullOrEmpty(animatorCache))
                        Cache.Animator_Cache = JsonConvert.DeserializeObject<List<JsonCache>>(animatorCache);

                    FileStream stream5 = new FileStream(Config.Directories.Environments_Cache_File, FileMode.Open, FileAccess.Read);
                    StreamReader reader5 = new StreamReader(stream5);

                    string environmentCache = reader5.ReadToEnd();

                    reader5.Close();
                    stream5.Close();

                    if (!string.IsNullOrEmpty(environmentCache))
                        Cache.Environment_Cache = JsonConvert.DeserializeObject<List<JsonCache>>(environmentCache);

                    nextTask(null);
                }
            };
        }

        private Task Start_App()
        {
            return new Task()
            {
                Mission = "Start_App",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (Directives.App_Notification)
                    {
                        if (Variables.Notification_Type == "mojo")
                        {
                            new UIPageRenderer<listInboxItem>()
                            {
                                Material = new UIPageRendererMaterial<listInboxItem>()
                                {
                                    Page = new pageInbox(UIDirection.CENTER, UIPageHeight.NORMAL),
                                    List = "listInbox",
                                    ListUrl = Config.URLs.Inbox_Page,
                                    OnConversion = Conversions.Inbox
                                },
                                OnFinish = () => { Directives.App_Initialized = true; }
                            }
                            .Render();
                        }
                        else if (Variables.Notification_Type == "request")
                        {
                            new UIPageRenderer<listMainItem>()
                            {
                                Material = new UIPageRendererMaterial<listMainItem>()
                                {
                                    Page = new pageMain(UIDirection.CENTER, UIPageHeight.NORMAL),
                                    List = "listMain",
                                    ListUrl = Config.URLs.Main_Page,
                                    OnConversion = Conversions.Main
                                },
                                OnFinish = () =>
                                {
                                    Directives.App_Initialized = true;

                                    new UIPageRenderer<listFriendRequestsItem>()
                                    {
                                        Material = new UIPageRendererMaterial<listFriendRequestsItem>()
                                        {
                                            Page = new pageFriendRequests(UIDirection.FROM_RIGHT, UIPageHeight.TALL),
                                            List = "listFriendRequests",
                                            ListUrl = Config.URLs.Friend_Requests_Page,
                                            OnConversion = Conversions.Friend_Requests
                                        },
                                        OnFinish = () => { Directives.App_Initialized = true; }
                                    }
                                    .Render();
                                }
                            }
                            .Render();
                        }
                    }
                    else
                    {
                        new UIPageRenderer<listMainItem>()
                        {
                            Material = new UIPageRendererMaterial<listMainItem>()
                            {
                                Page = new pageMain(UIDirection.CENTER, UIPageHeight.NORMAL),
                                List = "listMain",
                                ListUrl = Config.URLs.Main_Page,
                                OnConversion = Conversions.Main
                            },
                            OnFinish = () =>
                            {
                                Directives.App_Initialized = true;

                                if (Directives.App_First)
                                {
                                    new UIPageRenderer<listItem>()
                                    {
                                        Material = new UIPageRendererMaterial<listItem>()
                                        {
                                            Page = new pageAbout_1(UIDirection.FROM_RIGHT, UIPageHeight.TALL)
                                        }
                                    }
                                    .Render();
                                }
                            }
                        }
                        .Render();
                    }
                }
            };
        }
    }
}