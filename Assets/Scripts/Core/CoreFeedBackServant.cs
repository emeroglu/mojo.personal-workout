using System;
using Assets.Scripts.Repository;

namespace Assets.Scripts.Core
{
    public abstract class CoreFeedBackServant<MaterialType, ResultType>
        where MaterialType : CoreMaterial
        where ResultType : CoreResult
    {
        public MaterialType Material { get; set; }

        public Action<ResultType> OnFinish { get; set; }
        public Action<Exception> OnFail { get; set; }

        protected void Perform()
        {
            try
            {
                Job();
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
