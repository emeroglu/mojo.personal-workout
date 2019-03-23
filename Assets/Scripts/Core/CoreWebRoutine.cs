using Assets.Scripts.Repository;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public abstract class CoreWebRoutine<MaterialType>
    {
        public MaterialType Material { get; set; }

        public Action<Exception> OnFail { get; set; }

        public Action<WWW> OnSuccess { get; set; }             

        protected void Perform()
        {
            MonoBridge.StartCoroutine(Routine());
        }

        protected abstract IEnumerator Routine();

        protected void Dispose()
        {
            Material = default(MaterialType);

            OnFail = null;

            OnSuccess = null;                        

            GC.SuppressFinalize(this);
            GC.Collect();
        }
    }
}
