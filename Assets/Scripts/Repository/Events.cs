using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts.Abstract.Exceptions;
using Assets.Scripts.Abstract.Tools;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Agents.Renderers;
using Assets.Scripts.UI.ListItems;
using Assets.Scripts.UI.Pages;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Repository
{
    public static class Events
    {
        public static List<Switch> Switches;
        public static List<EventListener> Listeners;

        public static List<OnTouchListener> OnTouch_Listeners;
        public static List<GameObject> Touched_Objects;

        public static void Message(string text)
        {
            if (GameObject.Find("imgError") != null)
                MonoBridge.Destroy(GameObject.Find("imgError"));

            if (GameObject.Find("txtError") != null)
                MonoBridge.Destroy(GameObject.Find("txtError"));

            GameObject go = new GameObject("imgError");
            go.transform.parent = GameObject.Find("Canvas").transform;

            RawImage img = go.AddComponent<RawImage>();
            img.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            img.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            img.rectTransform.anchoredPosition = new Vector2(0, 0);
            img.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height * 0.5f);
            img.color = Color.black;

            go = new GameObject("txtError");
            go.transform.parent = GameObject.Find("Canvas").transform;

            Text txt = go.AddComponent<Text>();
            txt.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            txt.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            txt.rectTransform.anchoredPosition = new Vector2(0, 0);
            txt.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height * 0.5f);
            txt.alignment = TextAnchor.MiddleCenter;
            txt.font = Resources.Load<Font>("Fonts/Exo 2.0/Exo2.0-Light");
            txt.fontSize = 25;
            txt.color = Color.white;
            txt.text = text;
        }

        public static void Exception(Exception ex)
        {
            throw ex;

            FileStream stream = new FileStream(Config.Directories.Root + "/error.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(ex.Message);
            writer.Flush();
            writer.Close();
            stream.Close();

            Variables.Exception_Message = "";

            if (Directives.App_Initialized)
            {
                if (ex is NoConnectionException)
                {
                    Variables.Exception_Message = "No Internet Connection...";
                }
                else if (ex is ServerException)
                {
                    Variables.Exception_Message = "Our server seems to be down :(";
                }
                else if (ex is ConnectionDropException)
                {
                    Variables.Exception_Message = "Connection dropped...";
                }
                else if (ex is NotFoundException)
                {
                    Variables.Exception_Message = "404 Not Found...";
                }
                else if (ex is InternalServerException)
                {
                    Variables.Exception_Message = "Something went wrong on our servers...";
                }
                else if (ex is InstanceException)
                {
                    Variables.Exception_Message = "Your session has expired...";
                }

                new UIPageRenderer<listItem>()
                {
                    Material = new UIPageRendererMaterial<listItem>()
                    {
                        Page = new pageError(UIDirection.FROM_RIGHT, UIPageHeight.NORMAL)
                    },
                    OnFinish = () =>
                    {                        
                        Events.OnTouch_Listeners.FirstOrDefault(e => e.Target == "%clckBottom_").Enabled = false;
                    }
                }
                .Render();

            }
            else
            {
                if (Directives.UI_Initialized)
                {
                    (Variables.UI["txtConsole"] as UIText).Element.text = "Sorry, something went wrong...";
                }
            }
        }
    }
}
