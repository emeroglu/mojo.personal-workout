using Assets.Scripts.Abstract.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Repository;
using Assets.Scripts.Abstract.UI;

namespace Assets.Scripts.Core
{
    public abstract class CoreFeedBackProcessor<MaterialType, ResultType> : CoreFeedBackServant<MaterialType, ResultType>
        where MaterialType : CoreMaterial
        where ResultType : CoreResult
    {
        private List<Task> listTasks { get; set; }

        protected abstract List<Task> Tasks();
        protected abstract bool Condition();
        protected abstract void OnInterruption();        
        protected abstract bool DebugLog();

        protected override void Job()
        {
            listTasks = Tasks();

            TakeAction(0, null);
        }

        private void TakeAction(int i, object pass)
        {
            if (DebugLog())
                Debug.LogWarning("Task " + (i + 1) + " / " + listTasks.Count + " - " + listTasks[i].Mission);

            if (Condition())
            {
                try
                {
                    if (i != listTasks.Count)
                    {
                        listTasks[i].Action
                        (
                            (task, package) =>
                            {
                                try
                                {
                                    int index = (int)task;
                                    TakeAction(index, package);
                                }
                                catch (Exception)
                                {
                                    string mission = (string)task;

                                    Task desiredTask = listTasks.Where(t => t.Mission == mission).FirstOrDefault();

                                    if (desiredTask != null)
                                        TakeAction(listTasks.IndexOf(listTasks.Where(t => t.Mission == mission).FirstOrDefault()), package);
                                }
                            },
                            (package) => { TakeAction(i, package); },
                            (package) => { TakeAction(i + 1, package); },
                            pass
                        );
                    }                    
                }
                catch (Exception ex)
                {
                    if (OnFail == null)
                        OnFail(ex);
                    else
                        Events.Exception(ex);

                    Dispose();
                }
            }
            else
            {
                OnInterruption();
            }
        }
    }
}
