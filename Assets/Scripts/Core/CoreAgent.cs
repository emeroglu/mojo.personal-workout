using System;
using Assets.Scripts.Repository;

namespace Assets.Scripts.Core
{
    public abstract class CoreAgent<MaterialType> where MaterialType : CoreMaterial
    {
        public MaterialType Material { get; set; }

        public Action OnFinish { get; set; }
        public Action<Exception> OnFail { get; set; }

        protected void Perform()
        {
            try
            {
                Job();

                if (OnFinish != null)
                    OnFinish();

                Dispose();
            }
            catch (Exception ex)
            {
                if (OnFail == null)
                    OnFail(ex);
                else
                    Events.Exception(ex);
            }
        }

        protected abstract void Job();

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
