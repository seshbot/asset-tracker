using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Utils
{
    public static class CollectionExtensions
    {
        public static ICollection<T> ToCollection<T>(this IEnumerable<T> xs)
        {
            return new Collection<T>(xs.ToList());
        }
    }
}
