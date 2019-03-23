using Assets.Scripts.Abstract.Json;
using Assets.Scripts.Core;
using Assets.Scripts.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Abstract.UI;

namespace Assets.Scripts.Agents.Tools
{
    public class ContentDownloader : CoreServant<ContentDownloaderMaterial>
    {
        public void Download()
        {
            Perform();
        }

        protected override void Job()
        {
            Start();
        }

        private void Start()
        {
            if (Directory.Exists(Material.ExtractUrl))
                Directory.Delete(Material.ExtractUrl, true);

            new HttpRequestSender()
            {
                Material = new HttpRequestSenderMaterial()
                {
                    Url = Material.DownloadUrl,
                    Template = Material.Template,
                    UI_Text = (string.IsNullOrEmpty(Material.UI_Text)) ? null : (Variables.UI[Material.UI_Text] as UIText).Element
                },
                OnSuccess = (www) =>
                {
                    string directory = Material.ExtractUrl;

                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    FileStream stream = new FileStream(directory + "/content.zip", FileMode.CreateNew, FileAccess.Write);
                    BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(www.bytes);
                    writer.Flush();
                    writer.Close();
                    stream.Close();

                    Extract();
                }                
            }
            .Send();
        }

        private void Extract()
        {
            string directory = Material.ExtractUrl;

            new ZipExtractor()
            {
                Material = new ZipExtractorMaterial()
                {
                    ReadUrl = directory + "/content.zip",
                    ExtractUrl = directory
                },
                OnFinish = () =>
                {
                    try
                    {
                        File.Delete(directory + "/content.zip");
                    }
                    catch (Exception) { }
                    finally
                    {
                        OnFinish();
                        Dispose();
                    }
                }                
            }
            .Extract();
        }
    }

    public class ContentDownloaderMaterial : CoreMaterial
    {
        public string Template { get; set; }
        public string UI_Text { get; set; }

        public string DownloadUrl { get; set; }
        public string ExtractUrl { get; set; }
    }
}
