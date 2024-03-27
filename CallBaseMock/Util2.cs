using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CallBaseMock
{
    public static class Util2
    {
        public static string GetKey(string key)
        {
            key = "2234597" + key;
            return Util3.GetKey(key);
        }

        public static string GetIV(string iv)
        {
            iv += "523491";
            return Util4.GetIV(iv);
        }
    }
}