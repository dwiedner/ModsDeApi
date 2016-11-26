using System;
using System.Collections.Generic;
using System.Net;

namespace ModsDeApi.Services
{
    internal class CookieHelper
    {
        public static CookieCollection GetAllCookiesFromHeader(string strHeader, string strHost)
        {
            if (strHeader == string.Empty)
                return new CookieCollection();

            var al = ConvertCookieHeaderToList(strHeader);
            return ConvertCookieListToCookieCollection(al, strHost);
        }


        private static IList<string> ConvertCookieHeaderToList(string strCookHeader)
        {
            strCookHeader = strCookHeader.Replace("\r", "");
            strCookHeader = strCookHeader.Replace("\n", "");
            var strCookTemp = strCookHeader.Split(',');
            var al = new List<string>();

            var i = 0;
            while (i < strCookTemp.Length)
            {
                if (strCookTemp[i].IndexOf("expires=", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    al.Add(strCookTemp[i] + "," + strCookTemp[i + 1]);
                    i = i + 1;
                }
                else
                {
                    al.Add(strCookTemp[i]);
                }
                i = i + 1;
            }
            return al;
        }


        private static CookieCollection ConvertCookieListToCookieCollection(IList<string> al, string strHost)
        {
            var cc = new CookieCollection();

            var alcount = al.Count;
            for (var i = 0; i < alcount; i++)
            {
                var strEachCook = al[i];
                var strEachCookParts = strEachCook.Split(';');
                var intEachCookPartsCount = strEachCookParts.Length;
                var cookTemp = new Cookie();

                for (var j = 0; j < intEachCookPartsCount; j++)
                {
                    if (j == 0)
                    {
                        var strCNameAndCValue = strEachCookParts[j];
                        if (!string.IsNullOrEmpty(strCNameAndCValue))
                        {
                            var firstEqual = strCNameAndCValue.IndexOf("=", StringComparison.Ordinal);
                            var firstName = strCNameAndCValue.Substring(0, firstEqual);
                            var allValue = strCNameAndCValue.Substring(firstEqual + 1, strCNameAndCValue.Length - (firstEqual + 1));
                            cookTemp.Name = firstName;
                            cookTemp.Value = allValue;
                        }
                        continue;
                    }
                    string strPNameAndPValue;
                    string[] nameValuePairTemp;
                    if (strEachCookParts[j].IndexOf("path", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        strPNameAndPValue = strEachCookParts[j];
                        if (!string.IsNullOrEmpty(strPNameAndPValue))
                        {
                            nameValuePairTemp = strPNameAndPValue.Split('=');
                            if (!string.IsNullOrEmpty(nameValuePairTemp[1]))
                            {
                                cookTemp.Path = nameValuePairTemp[1];
                            }
                            else
                            {
                                cookTemp.Path = "/";
                            }
                        }
                        continue;
                    }

                    if (strEachCookParts[j].IndexOf("domain", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        strPNameAndPValue = strEachCookParts[j];
                        if (!string.IsNullOrEmpty(strPNameAndPValue))
                        {
                            nameValuePairTemp = strPNameAndPValue.Split('=');

                            if (!string.IsNullOrEmpty(nameValuePairTemp[1]))
                            {
                                cookTemp.Domain = nameValuePairTemp[1];
                            }
                            else
                            {
                                cookTemp.Domain = strHost;
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(cookTemp.Path))
                {
                    cookTemp.Path = "/";
                }
                if (string.IsNullOrEmpty(cookTemp.Domain))
                {
                    cookTemp.Domain = strHost;
                }
                cc.Add(cookTemp);
            }
            return cc;
        }
    }
}
