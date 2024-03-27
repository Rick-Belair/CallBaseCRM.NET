using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CallBaseMock
{
    public static class Util4
    {
        public static string GetKey(string key)
        {
            key += "491564";
            return Util5.GetKey(key);
        }

        public static string GetIV(string iv)
        {
            iv = "038113" + iv;
            return Util5.GetIV(iv);
        }
    }
}