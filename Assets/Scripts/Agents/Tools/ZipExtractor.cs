using Assets.Scripts.Core;
using Ionic.Zip;

namespace Assets.Scripts.Agents.Tools
{
    public class ZipExtractor : CoreAgent<ZipExtractorMaterial>
    {
        public void Extract()
        {
            Perform();
        }

        protected override void Job()
        {
            ZipFile zip = ZipFile.Read(Material.ReadUrl);

            zip.ExtractAll(Material.ExtractUrl);
        }
    }

    public class ZipExtractorMaterial : CoreMaterial
    {
        public string ReadUrl { get; set; }
        public string ExtractUrl { get; set; }
    }
}