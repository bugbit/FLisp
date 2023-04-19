using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLisp.Core
{
    public class SMath
    {
        public static bool TryAdd(object n1, object n2, out object? result)
        {
            object? r = (n1, n2) switch
            {
                (int i1, int i2) => i1 + i2,
                (int i1, double i2) => i1 + i2,
                (double i1, int i2) => i1 + i2,
                (double i1, double i2) => i1 + i2,
                _ => null
            };

            result = r;

            return r != null;
        }
    }
}