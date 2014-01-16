using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceTester.HelperClasses
{
    public static class DictionaryExtensions
    {
        public static void AddRange<T, S>(this Dictionary<T, S> source, IEnumerable<KeyValuePair<T, S>> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection is null");
            }

            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                {
                    source.Add(item.Key, item.Value);
                }
                else
                {
                    // handle duplicate key issue here
                }
            }
        }
    }
}
