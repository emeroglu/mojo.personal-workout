using Assets.Scripts.Core;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Agents.Tools
{
    public class Suspender : CoreCoroutine<CoreMaterial>
    {
        public float Suspension { get; set; }

        public void Suspend()
        {
            Perform();
        }

        protected override IEnumerator Routine()
        {
            yield return new WaitForSeconds(Suspension);

            try
            {
                if (OnFinish != null)
                    OnFinish();

                Dispose();
            }
            catch (Exception ex)
            {
                OnFail(ex);
            }
        }
    } 
}
