using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Core;
using Assets.Scripts.Repository;
using Assets.Scripts.UI.Pages;
using Assets.Scripts.Abstract.Json;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Agents.Renderers;
using Assets.Scripts.UI.ListItems;
using Assets.Scripts.UI.Panels;

namespace Assets.Scripts.Agents.Tools
{
    public class Preloader : CoreProcessor<PreloaderMaterial>
    {
        private JsonCache animatorCache;
        private JsonCache skyboxCache;
        private JsonCache avatarCache;
        private JsonCache audioCache;
        private JsonCache environmentCache;

        private bool downloadAnimator;
        private bool downloadSkybox;
        private bool downloadAvatar;
        private bool downloadAudio;
        private bool downloadEnvironment;

        private Text txtView;

        public void Preload()
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

        protected override bool DebugLog()
        {
            return false;
        }

        protected override List<Task> Tasks()
        {
            return new List<Task>()
            {                
                UI_Downloading(),
                Mojo_Open_Interaction(),                
                Check_Cache(),
                Download_Skybox(),
                Download_Environment(),
                Download_Animator(),                
                Download_Avatar(),
                Download_Audio(),
                Update_Cache(),
                Retrieve_Skybox(),
                Retrieve_Environment(),
                Retrieve_Animator(),
                Retrieve_Avatar(),                
                Retrieve_Audio(),
                Instantiate(),
                Initialize_Frame(),
                UI_Animation()
            };
        }

        private Task UI_Downloading()
        {
            return new Task()
            {
                Mission = "UI_Downloading",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    txtView = (Variables.UI["txtView_" + Material.Index] as UIText).Element;
                    txtView.text = "Downloading...";

                    new StateBroadcaster()
                    {
                        Material = new StateBroadcasterMaterial()
                        {
                            State = "View_" + Material.Index
                        },
                        OnFinish = () => { nextTask(null); }
                    }
                    .Broadcast();
                }
            };
        }

        private Task Mojo_Open_Interaction()
        {
            return new Task()
            {
                Mission = "Mojo_Open_Interaction",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (Variables.Current_Mojo.pk == 0)
                        nextTask(null);
                    else
                        new HttpPostRequestSender()
                        {
                            Material = new HttpPostRequestSenderMaterial()
                            {
                                Url = Config.URLs.Mojo_Viewed,
                                Fields = new Dictionary<string, string>()
                                {
                                    { "appInstanceKey", Variables.App_Instance_Key },
                                    { "mojoFk", Variables.Current_Mojo.pk.ToString() }
                                }
                            },
                            OnSuccess = (www) => { nextTask(null); }
                        }
                        .Send();
                }
            };
        }

        private Task Check_Cache()
        {
            return new Task()
            {
                Mission = "Check_Cache",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    skyboxCache = Cache.Skybox_Cache.FirstOrDefault(c => c.pk == Variables.Current_Mojo.soundtrack.skybox.pk);

                    if (skyboxCache == null)
                    {
                        downloadSkybox = true;

                        skyboxCache = new JsonCache() { pk = Variables.Current_Mojo.soundtrack.skybox.pk, updateDate = DateTime.Now.ToString(Config.Variables.Date_Format) };
                        Cache.Skybox_Cache.Add(skyboxCache);
                    }
                    else
                    {
                        if (DateTime.ParseExact(skyboxCache.updateDate, Config.Variables.Date_Format, null) < DateTime.ParseExact(Variables.Current_Mojo.soundtrack.skybox.lastUpdate, Config.Variables.Date_Format, null))
                        {
                            downloadSkybox = true;

                            skyboxCache.updateDate = DateTime.Now.ToString(Config.Variables.Date_Format);
                        }
                        else
                        {
                            downloadSkybox = false;
                        }
                    }

                    animatorCache = Cache.Animator_Cache.FirstOrDefault(c => c.pk == Variables.Current_Mojo.soundtrack.animator.pk);

                    if (animatorCache == null)
                    {
                        downloadAnimator = true;

                        animatorCache = new JsonCache() { pk = Variables.Current_Mojo.soundtrack.animator.pk, updateDate = DateTime.Now.ToString(Config.Variables.Date_Format) };
                        Cache.Animator_Cache.Add(animatorCache);
                    }
                    else
                    {
                        if (DateTime.ParseExact(animatorCache.updateDate, Config.Variables.Date_Format, null) < DateTime.ParseExact(Variables.Current_Mojo.soundtrack.animator.lastUpdate, Config.Variables.Date_Format, null))
                        {
                            downloadAnimator = true;

                            animatorCache.updateDate = DateTime.Now.ToString(Config.Variables.Date_Format);
                        }
                        else
                        {
                            downloadAnimator = false;
                        }
                    }

                    avatarCache = Cache.Avatar_Cache.FirstOrDefault(c => c.pk == Variables.Current_Mojo.character.pk);

                    if (avatarCache == null)
                    {
                        downloadAvatar = true;

                        avatarCache = new JsonCache() { pk = Variables.Current_Mojo.character.pk, updateDate = DateTime.Now.ToString(Config.Variables.Date_Format) };
                        Cache.Avatar_Cache.Add(avatarCache);
                    }
                    else
                    {
                        if (DateTime.ParseExact(avatarCache.updateDate, Config.Variables.Date_Format, null) < DateTime.ParseExact(Variables.Current_Mojo.character.lastUpdate, Config.Variables.Date_Format, null))
                        {
                            downloadAvatar = true;

                            avatarCache.updateDate = DateTime.Now.ToString(Config.Variables.Date_Format);
                        }
                        else
                        {
                            downloadAvatar = false;
                        }
                    }

                    audioCache = Cache.Music_Cache.FirstOrDefault(c => c.pk == Variables.Current_Mojo.soundtrack.pk);

                    if (audioCache == null)
                    {
                        downloadAudio = true;

                        audioCache = new JsonCache() { pk = Variables.Current_Mojo.soundtrack.pk, updateDate = DateTime.Now.ToString(Config.Variables.Date_Format) };
                        Cache.Music_Cache.Add(audioCache);
                    }
                    else
                    {
                        if (DateTime.ParseExact(audioCache.updateDate, Config.Variables.Date_Format, null) < DateTime.ParseExact(Variables.Current_Mojo.soundtrack.lastUpdate, Config.Variables.Date_Format, null))
                        {
                            downloadAudio = true;

                            audioCache.updateDate = DateTime.Now.ToString(Config.Variables.Date_Format);
                        }
                        else
                        {
                            downloadAudio = false;
                        }
                    }

                    environmentCache = Cache.Environment_Cache.FirstOrDefault(c => c.pk == Variables.Current_Mojo.soundtrack.environment.pk);

                    if (environmentCache == null)
                    {
                        downloadEnvironment = true;

                        environmentCache = new JsonCache() { pk = Variables.Current_Mojo.soundtrack.environment.pk, updateDate = DateTime.Now.ToString(Config.Variables.Date_Format) };
                        Cache.Environment_Cache.Add(environmentCache);
                    }
                    else
                    {
                        if (DateTime.ParseExact(environmentCache.updateDate, Config.Variables.Date_Format, null) < DateTime.ParseExact(Variables.Current_Mojo.soundtrack.environment.lastUpdate, Config.Variables.Date_Format, null))
                        {
                            downloadEnvironment = true;

                            environmentCache.updateDate = DateTime.Now.ToString(Config.Variables.Date_Format);
                        }
                        else
                        {
                            downloadEnvironment = false;
                        }
                    }

                    nextTask(null);
                }
            };
        }

        private Task Download_Skybox()
        {
            return new Task()
            {
                Mission = "Download_Skybox",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (downloadSkybox)
                    {
                        new ContentDownloader()
                        {
                            Material = new ContentDownloaderMaterial()
                            {
                                DownloadUrl = Config.URLs.Skyboxes.Replace("{0}", Variables.Current_Mojo.soundtrack.skybox.pk.ToString()),
                                ExtractUrl = Config.Directories.Skybox_Extract.Replace("{0}", Variables.Current_Mojo.soundtrack.skybox.pk.ToString())
                            },
                            OnFinish = () => { nextTask(null); }
                        }
                        .Download();
                    }
                    else
                        nextTask(null);
                }
            };
        }

        private Task Download_Environment()
        {
            return new Task()
            {
                Mission = "Download_Environment",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (downloadEnvironment)
                    {
                        new ContentDownloader()
                        {
                            Material = new ContentDownloaderMaterial()
                            {
                                DownloadUrl = Config.URLs.Environments.Replace("{0}", Variables.Current_Mojo.soundtrack.environment.pk.ToString()),
                                ExtractUrl = Config.Directories.Environment_Extract.Replace("{0}", Variables.Current_Mojo.soundtrack.environment.pk.ToString()),
                                Template = "Downloading environment {0}",
                                UI_Text = "txtView_" + Material.Index
                            },
                            OnFinish = () =>
                            {
                                txtView.text = "Environment downloaded...";

                                new Suspender()
                                {
                                    Suspension = 0.5f,
                                    OnFinish = () =>
                                    {
                                        nextTask(null);
                                    }
                                }
                                .Suspend();
                            }
                        }
                        .Download();
                    }
                    else
                    {
                        txtView.text = "Environment already downloaded...";

                        new Suspender()
                        {
                            Suspension = 0.5f,
                            OnFinish = () =>
                            {
                                nextTask(null);
                            }
                        }
                        .Suspend();
                    }
                }
            };
        }

        private Task Download_Animator()
        {
            return new Task()
            {
                Mission = "Download_Animator",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (downloadAnimator)
                    {
                        new ContentDownloader()
                        {
                            Material = new ContentDownloaderMaterial()
                            {
                                DownloadUrl = Config.URLs.Animator.Replace("{0}", Variables.Current_Mojo.soundtrack.animator.pk.ToString()),
                                ExtractUrl = Config.Directories.Animator_Extract.Replace("{0}", Variables.Current_Mojo.soundtrack.animator.pk.ToString())
                            },
                            OnFinish = () => { nextTask(null); }
                        }
                        .Download();
                    }
                    else
                        nextTask(null);
                }
            };
        }

        private Task Download_Avatar()
        {
            return new Task()
            {
                Mission = "Download_Avatar",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (downloadAvatar)
                    {
                        new ContentDownloader()
                        {
                            Material = new ContentDownloaderMaterial()
                            {
                                DownloadUrl = Config.URLs.Avatar.Replace("{0}", Variables.Current_Mojo.character.pk.ToString()),
                                ExtractUrl = Config.Directories.Character_Extract.Replace("{0}", Variables.Current_Mojo.character.pk.ToString()),
                                Template = "Downloading character {0}",
                                UI_Text = "txtView_" + Material.Index
                            },
                            OnFinish = () =>
                            {
                                txtView.text = "Character downloaded...";

                                new Suspender()
                                {
                                    Suspension = 0.5f,
                                    OnFinish = () =>
                                    {
                                        nextTask(null);
                                    }
                                }
                                .Suspend();
                            }
                        }
                        .Download();
                    }
                    else
                    {
                        txtView.text = "Character already downloaded...";

                        new Suspender()
                        {
                            Suspension = 0.5f,
                            OnFinish = () =>
                            {
                                nextTask(null);
                            }
                        }
                        .Suspend();
                    }
                }
            };
        }

        private Task Download_Audio()
        {
            return new Task()
            {
                Mission = "Download_Audio",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    if (downloadAudio)
                    {
                        new ContentDownloader()
                        {
                            Material = new ContentDownloaderMaterial()
                            {
                                DownloadUrl = Config.URLs.Music.Replace("{0}", Variables.Current_Mojo.soundtrack.pk.ToString()),
                                ExtractUrl = Config.Directories.Soundtrack_Extract.Replace("{0}", Variables.Current_Mojo.soundtrack.pk.ToString()),
                                Template = "Downloading soundtrack {0}",
                                UI_Text = "txtView_" + Material.Index
                            },
                            OnFinish = () =>
                            {
                                txtView.text = "Soundtrack downloaded...";

                                new Suspender()
                                {
                                    Suspension = 0.5f,
                                    OnFinish = () =>
                                    {
                                        txtView.text = "Loading...";

                                        nextTask(null);
                                    }
                                }
                                .Suspend();
                            }
                        }
                        .Download();
                    }
                    else
                    {
                        txtView.text = "Soundtrack already downloaded...";

                        new Suspender()
                        {
                            Suspension = 0.5f,
                            OnFinish = () =>
                            {
                                txtView.text = "Loading...";

                                nextTask(null);
                            }
                        }
                        .Suspend();
                    }
                }
            };
        }

        private Task Update_Cache()
        {
            return new Task()
            {
                Mission = "Update_Cache",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    string avatarCache = JsonConvert.SerializeObject(Cache.Avatar_Cache);

                    FileStream stream = new FileStream(Config.Directories.Characters_Cache_File, FileMode.Open, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(stream);
                    writer.Write(avatarCache);
                    writer.Flush();
                    writer.Close();
                    stream.Close();

                    string musicCache = JsonConvert.SerializeObject(Cache.Music_Cache);

                    FileStream stream2 = new FileStream(Config.Directories.Soundtracks_Cache_File, FileMode.Open, FileAccess.Write);
                    StreamWriter writer2 = new StreamWriter(stream2);
                    writer2.Write(musicCache);
                    writer2.Flush();
                    writer2.Close();
                    stream2.Close();

                    string animatorCache = JsonConvert.SerializeObject(Cache.Animator_Cache);

                    FileStream stream3 = new FileStream(Config.Directories.Animators_Cache_File, FileMode.Open, FileAccess.Write);
                    StreamWriter writer3 = new StreamWriter(stream3);
                    writer3.Write(animatorCache);
                    writer3.Flush();
                    writer3.Close();
                    stream3.Close();

                    string skyboxCache = JsonConvert.SerializeObject(Cache.Skybox_Cache);

                    FileStream stream4 = new FileStream(Config.Directories.Skyboxes_Cache_File, FileMode.Open, FileAccess.Write);
                    StreamWriter writer4 = new StreamWriter(stream4);
                    writer4.Write(skyboxCache);
                    writer4.Flush();
                    writer4.Close();
                    stream4.Close();

                    string environmentCache = JsonConvert.SerializeObject(Cache.Environment_Cache);

                    FileStream stream5 = new FileStream(Config.Directories.Environments_Cache_File, FileMode.Open, FileAccess.Write);
                    StreamWriter writer5 = new StreamWriter(stream5);
                    writer5.Write(environmentCache);
                    writer5.Flush();
                    writer5.Close();
                    stream5.Close();

                    nextTask(null);
                }
            };
        }

        private Task Retrieve_Skybox()
        {
            return new Task()
            {
                Mission = "Retrieve_Skybox",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    JsonBundle skyboxBundle = Cache.Bundle_Cache.FirstOrDefault(b => b.pk == Variables.Current_Mojo.soundtrack.skybox.pk && b.type == BundleType.SKYBOX);

                    if (skyboxBundle == null)
                    {
                        new HttpRequestSender()
                        {
                            Material = new HttpRequestSenderMaterial()
                            {
                                Url = "file:///" + Config.Directories.Skybox.Replace("{0}", Variables.Current_Mojo.soundtrack.skybox.pk.ToString())
                            },
                            OnSuccess = (www) =>
                            {
                                skyboxBundle = new JsonBundle()
                                {
                                    pk = Variables.Current_Mojo.soundtrack.skybox.pk,
                                    type = BundleType.SKYBOX,
                                    bundle = www.assetBundle
                                };

                                Cache.Bundle_Cache.Add(skyboxBundle);

                                nextTask(null);
                            }
                        }
                        .Send();
                    }
                    else
                        nextTask(null);
                }
            };
        }

        private Task Retrieve_Environment()
        {
            return new Task()
            {
                Mission = "Retrieve_Environment",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    JsonBundle environmentBundle = Cache.Bundle_Cache.FirstOrDefault(b => b.pk == Variables.Current_Mojo.soundtrack.environment.pk && b.type == BundleType.ENVIRONMENT);

                    if (environmentBundle == null)
                    {
                        new HttpRequestSender()
                        {
                            Material = new HttpRequestSenderMaterial()
                            {
                                Url = "file:///" + Config.Directories.Environment.Replace("{0}", Variables.Current_Mojo.soundtrack.environment.pk.ToString())
                            },
                            OnSuccess = (www) =>
                            {
                                environmentBundle = new JsonBundle()
                                {
                                    pk = Variables.Current_Mojo.soundtrack.environment.pk,
                                    type = BundleType.ENVIRONMENT,
                                    bundle = www.assetBundle
                                };

                                Cache.Bundle_Cache.Add(environmentBundle);

                                nextTask(null);
                            }
                        }
                        .Send();
                    }
                    else
                        nextTask(null);
                }
            };
        }

        private Task Retrieve_Animator()
        {
            return new Task()
            {
                Mission = "Retrieve_Animator",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    JsonBundle animatorBundle = Cache.Bundle_Cache.FirstOrDefault(b => b.pk == Variables.Current_Mojo.soundtrack.animator.pk && b.type == BundleType.ANIMATOR);

                    if (animatorBundle == null)
                    {
                        new HttpRequestSender()
                        {
                            Material = new HttpRequestSenderMaterial()
                            {
                                Url = "file:///" + Config.Directories.Animator.Replace("{0}", Variables.Current_Mojo.soundtrack.animator.pk.ToString())
                            },
                            OnSuccess = (www) =>
                            {
                                animatorBundle = new JsonBundle()
                                {
                                    pk = Variables.Current_Mojo.soundtrack.animator.pk,
                                    type = BundleType.ANIMATOR,
                                    bundle = www.assetBundle
                                };

                                Cache.Bundle_Cache.Add(animatorBundle);

                                nextTask(null);
                            }
                        }
                        .Send();
                    }
                    else
                        nextTask(null);
                }
            };
        }

        private Task Retrieve_Avatar()
        {
            return new Task()
            {
                Mission = "Retrieve_Avatar",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    JsonBundle avatarBundle = Cache.Bundle_Cache.FirstOrDefault(b => b.pk == Variables.Current_Mojo.character.pk && b.type == BundleType.AVATAR);

                    if (avatarBundle == null)
                    {
                        new HttpRequestSender()
                        {
                            Material = new HttpRequestSenderMaterial()
                            {
                                Url = "file:///" + Config.Directories.Character.Replace("{0}", Variables.Current_Mojo.character.pk.ToString())
                            },
                            OnSuccess = (www) =>
                            {
                                avatarBundle = new JsonBundle()
                                {
                                    pk = Variables.Current_Mojo.character.pk,
                                    type = BundleType.AVATAR,
                                    bundle = www.assetBundle
                                };

                                Cache.Bundle_Cache.Add(avatarBundle);

                                nextTask(null);
                            }
                        }
                        .Send();

                    }
                    else
                        nextTask(null);
                }
            };
        }

        private Task Retrieve_Audio()
        {
            return new Task()
            {
                Mission = "Retrieve_Audio",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    JsonAudioClip audioCache = Cache.Audio_Cache.FirstOrDefault(a => a.pk == Variables.Current_Mojo.soundtrack.pk);

                    if (audioCache == null)
                    {
                        new HttpRequestSender()
                        {
                            Material = new HttpRequestSenderMaterial()
                            {
                                Url = "file:///" + Config.Directories.Soundtrack.Replace("{0}", Variables.Current_Mojo.soundtrack.pk.ToString())
                            },
                            OnSuccess = (www) =>
                            {
                                audioCache = new JsonAudioClip() { pk = Variables.Current_Mojo.soundtrack.pk, clip = www.GetAudioClip() };

                                Cache.Audio_Cache.Add(audioCache);

                                nextTask(null);
                            }
                        }
                        .Send();
                    }
                    else
                        nextTask(null);

                }
            };
        }

        private Task Instantiate()
        {
            return new Task()
            {
                Mission = "Instantiate",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    JsonBundle avatarBundle = Cache.Bundle_Cache.FirstOrDefault(b => b.pk == Variables.Current_Mojo.character.pk && b.type == BundleType.AVATAR);
                    JsonBundle animatorBundle = Cache.Bundle_Cache.FirstOrDefault(b => b.pk == Variables.Current_Mojo.soundtrack.animator.pk && b.type == BundleType.ANIMATOR);
                    JsonBundle skyboxBundle = Cache.Bundle_Cache.FirstOrDefault(b => b.pk == Variables.Current_Mojo.soundtrack.skybox.pk && b.type == BundleType.SKYBOX);
                    JsonBundle environmentBundle = Cache.Bundle_Cache.FirstOrDefault(b => b.pk == Variables.Current_Mojo.soundtrack.environment.pk && b.type == BundleType.ENVIRONMENT);

                    Objects.Environment = MonoBridge.Instantiate("Environment", environmentBundle.bundle.LoadAllAssets<GameObject>()[0]);

                    Objects.Environment.transform.localPosition = new Vector3(0, 0, 0);
                    Objects.Environment.transform.localEulerAngles = new Vector3(0, 0, 0);
                    Objects.Environment.transform.localScale = new Vector3(1, 1, 1);

                    Objects.Avatar = MonoBridge.Instantiate("Avatar", avatarBundle.bundle.LoadAllAssets<GameObject>()[0]);

                    Objects.Avatar.transform.localPosition = new Vector3(0, 0, 0);
                    Objects.Avatar.transform.localEulerAngles = new Vector3(0, 0, 0);
                    Objects.Avatar.transform.localScale = new Vector3(1, 1, 1);

                    Objects.Transform = GameObject.Find("Transform");

                    JsonVector position = Variables.Current_Mojo.soundtrack.position;
                    JsonVector rotation = Variables.Current_Mojo.soundtrack.rotation;
                    JsonVector scale = Variables.Current_Mojo.soundtrack.scale;

                    Objects.Transform.transform.localPosition = new Vector3(position.x, position.y, position.z);
                    Objects.Transform.transform.localEulerAngles = new Vector3(rotation.x, rotation.y, rotation.z);
                    Objects.Transform.transform.localScale = new Vector3(scale.x, scale.y, scale.z);

                    Objects.Body = GameObject.Find("Body");

                    Objects.Animator = Objects.Body.GetComponent<Animator>();
                    if (Objects.Animator == null)
                        Objects.Animator = Objects.Body.AddComponent<Animator>();

                    Objects.Animator.runtimeAnimatorController = animatorBundle.bundle.LoadAllAssets<RuntimeAnimatorController>()[0];
                    Objects.Animator.applyRootMotion = true;
                    Objects.Animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;

                    RenderSettings.skybox = skyboxBundle.bundle.LoadAllAssets<Material>()[0];

                    AudioClip audio = Cache.Audio_Cache.FirstOrDefault(a => a.pk == Variables.Current_Mojo.soundtrack.pk).clip;

                    Objects.AudioSource.clip = audio;
                    Objects.AudioSource.loop = Variables.Current_Mojo.soundtrack.loop;
                    Objects.AudioSource.Play();

                    new DownCounter()
                    {
                        Material = new DownCounterMaterial()
                        {
                            Seconds = Variables.Current_Mojo.soundtrack.length
                        },
                        Delay = 1,
                        Condition = (material) =>
                        {
                            if (Objects.Body == null)
                                return false;

                            if (Variables.Current_Mojo.soundtrack.loop)
                            {
                                if (material.Seconds == 0)
                                    material.Seconds = Variables.Current_Mojo.soundtrack.length;

                                return true;
                            }
                            else
                                return material.Seconds != 0;
                        },
                        OnFinish = () =>
                        {
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
                                        }
                                    }
                                    .Broadcast();
                                }
                            }
                            .Broadcast();
                        }
                    }
                    .Start();

                    Objects.Mojo_Camera.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    nextTask(null);
                }
            };
        }

        private Task Initialize_Frame()
        {
            return new Task()
            {
                Mission = "Initialize_Frame",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    new UIRenderer()
                    {
                        Material = new UIRendererMaterial()
                        {
                            Parent = Variables.UI["pnlMain"].Object,
                            Component = new pnlFrame(OnFail)
                        },
                        OnFinish = () =>
                        {
                            new StateBroadcaster()
                            {
                                Material = new StateBroadcasterMaterial()
                                {
                                    State = "View"
                                },
                                OnFinish = () => { nextTask(null); },
                            }
                            .Broadcast();
                        }
                    }
                    .Render();
                }
            };
        }

        private Task UI_Animation()
        {
            return new Task()
            {
                Mission = "UI_Animation",
                Action = (seekTo, currentTask, nextTask, delivery) =>
                {
                    new Suspender()
                    {
                        Suspension = 1,
                        OnFinish = () =>
                        {
                            new StateBroadcaster()
                            {
                                Material = new StateBroadcasterMaterial()
                                {
                                    State = "Delayed_View"
                                },
                                OnFinish = () => { txtView.text = "View"; }
                            }
                            .Broadcast();

                            if (Variables.Current_Mojo.soundtrack.touch)
                            {
                                Variables.Swipe_Left = 0;
                                Variables.Swipe_Right = 0;
                                Variables.Swipe_Horizontal = 0;

                                Events.Listeners.FirstOrDefault(l => l.Name == "On_Swiping_Horizontally").Enabled = true;

                                new EventRegistrar()
                                {
                                    Material = new EventRegistrarMaterial()
                                    {
                                        Listener = new EventListener()
                                        {
                                            Owner = "pnlFrame",
                                            Name = "Swipe_Listener",
                                            Enabled = true,
                                            Event = () => { return true; },
                                            OnEither = (listener) =>
                                            {
                                                GameObject go = Objects.Mojo_Camera;

                                                Quaternion target = Quaternion.Euler(go.transform.localRotation.x, go.transform.localRotation.y + Variables.Swipe_Horizontal, 0);

                                                go.transform.localRotation = Quaternion.Lerp(go.transform.localRotation, target, 0.1f);
                                            }
                                        }
                                    }
                                }
                                .Register();
                            }

                            if (OnFinish != null)
                                OnFinish();
                        }
                    }
                    .Suspend();
                }
            };
        }

    }

    public class PreloaderMaterial : CoreMaterial
    {
        public int Index { get; set; }
    }
}
