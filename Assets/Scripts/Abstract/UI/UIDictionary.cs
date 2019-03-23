using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Abstract.UI
{
    public class UIDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public new TValue this[TKey key]
        {
            get
            {
                TValue val;
                if (base.TryGetValue(key, out val))
                {
                    return val;
                }
                else
                {
                    throw new KeyNotFoundException(string.Format("The given key ({0}) was not present in the dictionary.", key));
                }
            }
            set
            {
                base[key] = value;
            }
        }
    }
}
