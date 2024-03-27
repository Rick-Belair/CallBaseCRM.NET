using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DataAccess;
using System.Security.Cryptography;
using System.Text;

namespace CallBaseMock
{
    public static class Utility
    {
        public static string setLanguage(string currLanguage)
        {
            string lang = "";

            if (!string.IsNullOrEmpty(currLanguage))
            {
                if (currLanguage.Equals("FR"))
                    lang = "EN";
                else
                    lang = "FR";
            }

            else
                lang = "FR";

            return lang;

        }//setLanguage

        public static string shortenLabel(string label, int labelLength)
        {
            return label.Substring(0, labelLength) + "...";

        }//shortenLabel

        public static string getDate(string calDate)
        {
            calDate = calDate.Replace(".", "");
            if (calDate.Contains("janv"))
                calDate = calDate.Replace("janv", "jan");
            else if (calDate.Contains("févr"))
                calDate = calDate.Replace("févr", "feb");
            else if (calDate.Contains("mars"))
                calDate = calDate.Replace("mars", "mar");
            else if (calDate.Contains("avr"))
                calDate = calDate.Replace("avr", "apr");
            else if (calDate.Contains("mai"))
                calDate = calDate.Replace("mai", "may");
            else if (calDate.Contains("juin"))
                calDate = calDate.Replace("juin", "jun");
            else if (calDate.Contains("juil"))
                calDate = calDate.Replace("juil", "jul");
            else if (calDate.Contains("août"))
                calDate = calDate.Replace("août", "aug");
            else if (calDate.Contains("sept"))
                calDate = calDate.Replace("sept", "sep");
            else if (calDate.Contains("déc"))
                calDate = calDate.Replace("déc", "dec");

            return calDate.ToUpper();

        }//getDate

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }//GetBytes

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static string GetKey()
        {
            string key = "221";
            return Util2.GetKey(key);
        }

        public static string GetIV()
        {
            string iv = "59722";
            return Util6.GetIV(iv);
        }


        public static string Decrypt(string input, string key, string iv)
        {

            byte[] inputArray = Convert.FromBase64String(input);
            var rijndaelManagedCipher = new RijndaelManaged();
            rijndaelManagedCipher.BlockSize = 256;
            rijndaelManagedCipher.IV = UTF8Encoding.UTF8.GetBytes(iv);
            rijndaelManagedCipher.Key = UTF8Encoding.UTF8.GetBytes(key);
            rijndaelManagedCipher.Mode = CipherMode.ECB;
            rijndaelManagedCipher.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rijndaelManagedCipher.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            rijndaelManagedCipher.Clear();

            return UTF8Encoding.UTF8.GetString(resultArray);

        }

    }//class

}//namespace