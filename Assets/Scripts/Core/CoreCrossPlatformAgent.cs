using System;
using Assets.Scripts.Repository;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public abstract class CoreCrossPlatformAgent<MaterialType> where MaterialType : CoreMaterial
    {
        public MaterialType Material { get; set; }

        public Action OnFinish { get; set; }
        public Action<Exception, RuntimePlatform> OnFail { get; set; }

        protected void Perform()
        {
            RuntimePlatform platform = Application.platform;

            try
            {
                Shared_Start();

                if (platform == RuntimePlatform.Android)
                {
                    Android();
                }
                else if (platform == RuntimePlatform.IPhonePlayer)
                {
                    IOS();
                }
                else if (platform == RuntimePlatform.WindowsEditor)
                {
                    Editor();
                }

                Shared_End();

                if (OnFinish != null)
                    OnFinish();
            }
            catch (Exception ex)
            {
                if (OnFail != null)
                    OnFail(ex, platform);                
                else
                    Events.Exception(ex);
            }

            Dispose();
        }

        protected abstract void Shared_Start();

        protected abstract void Android();
        protected abstract void IOS();
        protected abstract void Editor();

        protected abstract void Shared_End();

        protected void Dispose()
        {
            Material = default(MaterialType);

            OnFinish = null;
            OnFail = null;

            GC.SuppressFinalize(this);
            GC.Collect();
        }
    }
}
