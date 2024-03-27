using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CallBaseMock
{
    public static class Util3
    {
        public static string GetKey(string key)
        {
            key += "974523";
            return Util4.GetKey(key);
        }

        public static string GetIV(string iv)
        {
            iv = "352234" + iv;
            return Util2.GetIV(iv);
        }
    }
}