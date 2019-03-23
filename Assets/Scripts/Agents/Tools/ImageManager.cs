using Assets.Scripts.Core;
using Assets.Scripts.Repository;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Abstract.Json;
using Assets.Scripts.Agents.Tools;
using System.IO;
using Newtonsoft.Json;
using System;

namespace Assets.Scripts.Agents.Tools
{
    public class ImageManager : CoreFeedBackCoroutine<ImageManagerMaterial, ImageManagerResult>
    {
        public void Retrieve()
        {
            Perform();
        }

        protected override IEnumerator Routine()
        {
            JsonImage image = Cache.Image_Cache.FirstOrDefault(i => i.url == Material.Url);

            if (image == null)
            {
                if (Material.Url.Contains("http://") || Material.Url.Contains("https://"))
                {
                    Variables.Image_Cache_Inc++;

                    image = new JsonImage()
                    {
                        pk = Variables.Image_Cache_Inc,
                        suffix = "." + Material.Url.Split('/').Last().Split('.').Last().Split('?').First(),
                        url = Material.Url,
                        loading = true
                    };

                    Cache.Image_Cache.Add(image);

                    if (Material.LazyLoad)
                        yield return new WaitForSeconds(Material.Suspension);

                    WWW www = new WWW(Material.Url);
                    yield return www;

                    image.texture = www.texture;
                    image.loading = false;

                    FileStream stream = new FileStream(Config.Directories.Image.Replace("{0}", image.pk.ToString()).Replace("{1}", image.suffix), FileMode.Create, FileAccess.Write);
                    BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(www.bytes);
                    writer.Flush();
                    writer.Close();
                    stream.Close();

                    FileStream stream2 = new FileStream(Config.Directories.Images_Cache_File, FileMode.Create, FileAccess.Write);
                    StreamWriter writer2 = new StreamWriter(stream2);
                    writer2.Write(JsonConvert.SerializeObject(Cache.Image_Cache.Where(i => i.pk != 0).Select(i => new { pk = i.pk, url = i.url, suffix = i.suffix })));
                    writer2.Flush();
                    writer2.Close();
                    stream2.Close();
                }
                else
                {
                    image = new JsonImage()
                    {
                        pk = 0,
                        suffix = "." + Material.Url.Split('/').Last().Split('.').Last().Split('?').First(),
                        url = Material.Url,
                        loading = false
                    };

                    Cache.Image_Cache.Add(image);

                    image.texture = Resources.Load<Texture>(Material.Url);
                }

                OnFinish(new ImageManagerResult() { Texture = image.texture });
            }
            else
            {
                if (image.texture == null)
                {
                    if (Material.Url.Contains("http://") || Material.Url.Contains("https://"))
                    {
                        if (Material.LazyLoad)
                            yield return new WaitForSeconds(Material.Suspension);

                        if (image.loading)
                            yield return Listen(image);
                        else
                        {
                            WWW www = new WWW("file:///" + Config.Directories.Image.Replace("{0}", image.pk.ToString()).Replace("{1}", image.suffix));
                            yield return www;

                            image.texture = www.texture;
                        }
                    }
                    else
                    {
                        image.texture = Resources.Load<Texture>(Material.Url);
                    }

                    OnFinish(new ImageManagerResult() { Texture = image.texture });
                }
                else
                {
                    OnFinish(new ImageManagerResult() { Texture = image.texture });
                }
            }

            Dispose();
        }

        private IEnumerator Listen(JsonImage image)
        {
            if (image.loading)
            {
                yield return new WaitForSeconds(0.25f);
                yield return Listen(image);
            }
            else
            {
                WWW www = new WWW("file:///" + Config.Directories.Image.Replace("{0}", image.pk.ToString()).Replace("{1}", image.suffix));
                yield return www;

                image.texture = www.texture;
            }
        }
    }

    public class ImageManagerMaterial : CoreMaterial
    {
        public string Url { get; set; }

        public bool LazyLoad { get; set; }
        public float Suspension { get; set; }
    }

    public class ImageManagerResult : CoreResult
    {
        public Texture Texture { get; set; }
    }
}
