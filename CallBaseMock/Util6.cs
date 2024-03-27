using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CallBaseMock
{
    public static class Util6
    {
        public static string GetKey(string key)
        {
            key = "11335" + key;
            return key;
        }

        public static string GetIV(string iv)
        {
            iv += "1974";
            return Util3.GetIV(iv);
        }
    }
}