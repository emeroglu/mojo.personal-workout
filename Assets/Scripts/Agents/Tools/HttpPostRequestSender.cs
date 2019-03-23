using Assets.Scripts.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Abstract.Exceptions;
using Assets.Scripts.Repository;

namespace Assets.Scripts.Agents.Tools
{
    public class HttpPostRequestSender : CoreWebRoutine<HttpPostRequestSenderMaterial>
    {
        public void Send()
        {
            Perform();
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

            Debug.Log(url);

            WWW www;

            if (Material.Fields != null)
            {
                WWWForm form = new WWWForm();

                foreach (string key in Material.Fields.Keys)
                {
                    form.AddField(key, Material.Fields[key]);
                }

                www = new WWW(url, form);
            }
            else
                www = new WWW(url);

            yield return www;

            Debug.Log(www.text);

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
                    if (www.text == "not recognized")
                        OnFail(new InstanceException());
                    else
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

    public class HttpPostRequestSenderMaterial : CoreMaterial
    {
        public string Url { get; set; }
        public List<string> Parameters { get; set; }

        public Dictionary<string, string> Fields { get; set; }

    }
}
