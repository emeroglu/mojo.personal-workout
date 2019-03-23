using Assets.Scripts.Repository;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public abstract class CoreRecursive<MaterialType> where MaterialType : CoreMaterial
    {
        public MaterialType Material { get; set; }

        public Action OnStart { get; set; }
        public Action OnFinish { get; set; }
        public Action<Exception> OnFail { get; set; }

        public Func<MaterialType,bool> Condition { get; set; }
        public float Delay { get; set; }

        protected void Perform()
        {
            if (OnStart != null)
            {
                try
                {
                    OnStart();
                }
                catch (Exception ex)
                {
                    if (OnFail != null)
                        OnFail(ex);
                }
            }

            MonoBridge.StartCoroutine(Routine());
        }

        private IEnumerator Routine()
        {
            if (Condition == null)
                Condition = (material) => { return true; };

            if (Condition(Material))
            {
                try
                {
                    Job();
                }
                catch (Exception ex)
                {
                    if (OnFail != null)
                        OnFail(ex);
                }

                yield return new WaitForSeconds(Delay);

                MonoBridge.StartCoroutine(Routine());
            }
            else
            {
                try
                {
                    if (OnFinish != null)
                        OnFinish();
                }
                catch (Exception ex)
                {
                    if (OnFail != null)
                        OnFail(ex);
                }

                Dispose();
            }
        }

        protected abstract void Job();

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
