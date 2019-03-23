using System;

namespace Assets.Scripts.Abstract.Tools
{
    public class Task
    {
        public string Mission { get; set; }
        public Action<Action<object, object>, Action<object>, Action<object>, object> Action { get; set; }
        public bool Registered { get; set; }
    }
}
