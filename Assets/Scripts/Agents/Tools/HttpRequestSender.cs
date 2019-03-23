using Assets.Scripts.Core;
using Assets.Scripts.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Abstract.Exceptions;

namespace Assets.Scripts.Agents.Tools
{
    public class HttpRequestSender : CoreWebRoutine<HttpRequestSenderMaterial>
    {
        public void Send()
        {
            Perform();
        }

        private IEnumerator Progress(WWW www)
        {
            yield return new WaitForSeconds(.1f);

            if (Material != null)
            {
                if (Material.UI_Text != null)
                    Material.UI_Text.text = Material.Template.Replace("{0}", Math.Round(www.progress * 100) + "%");
            }

            if (!www.isDone)
                MonoBridge.StartCoroutine(Progress(www));
        }

        protected override IEnumerator Routine()
        {
            string url = Material.Url;

            if (Material.Parameters != null)
            {
                foreach (string param in Material.Parameters)
                {
                    url += "/" + param;
                }
            }

            WWW www = new WWW(url);

            if (Material.UI_Text != null)
                MonoBridge.StartCoroutine(Progress(www));

            yield return www;

            while (!www.isDone) { }

            if (Material.UI_Text != null)
                Material.UI_Text.text = "";

            if (www.error != null)
            {
                Exception ex = null;

                if (www.error.Contains("java.net.UnknownHostException"))
                    ex = new NoConnectionException();
                else if (www.error.Contains("Failed to connect to"))
                    ex = new ServerException();
                else if (www.error.Contains("Recv failure: Connection was reset"))
                    ex = new ConnectionDropException();
                else
                {
                    try
                    {
                        string status = www.responseHeaders["STATUS"];

                        if (status.Contains("404 Not Found"))
                            ex = new NotFoundException();
                        else if (status.Contains("500 Internal Server Error"))
                            ex = new InternalServerException();
                    }
                    catch (Exception)
                    {
                        ex = new Exception(www.error);

                        Debug.LogAssertion(www.error);
                    }
                }

                if (OnFail != null)
                    OnFail(ex);
                else
                    Events.Exception(ex);
            }
            else
            {
                try
                {
                    OnSuccess(www);
                }
                catch (Exception ex)
                {
                    if (OnFail != null)
                        OnFail(ex);
                    else
                        Events.Exception(ex);
                }
            }

            Dispose();
        }
    }

    public class HttpRequestSenderMaterial : CoreMaterial
    {
        public string Template { get; set; }

        public Text UI_Text { get; set; }

        public string Url { get; set; }
        public List<string> Parameters { get; set; }
    }
}
