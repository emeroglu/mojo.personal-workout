using Assets.Scripts.Repository;
using System;
using System.Collections;

namespace Assets.Scripts.Core
{
    public abstract class CoreCoroutine<MaterialType>
    {
        public MaterialType Material { get; set; }

        public Action<Exception> OnFail { get; set; }

        public Action OnStart { get; set; }
        public Action OnFinish { get; set; }

        protected void Perform()
        {
            try
            {
                if (OnStart != null)
                    OnStart();
            }
            catch (Exception ex)
            {
                if (OnFail == null)
                    OnFail(ex);
                else
                    Events.Exception(ex);
            }

            MonoBridge.StartCoroutine(Routine());
        }

        protected abstract IEnumerator Routine();

        protected void Dispose()
        {
            Material = default(MaterialType);

            OnStart = null;
            OnFinish = null;
            OnFail = null;

            GC.SuppressFinalize(this);
            GC.Collect();
        }
    }
}
