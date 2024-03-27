using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CallBaseMock
{
    public class Util5
    {
        public static string GetKey(string key)
        {
            key += "91038";
            return Util6.GetKey(key);
        }

        public static string GetIV(string iv)
        {
            iv += "56491";
            return iv;
        }
    }
}