using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Xml.Linq;

namespace ModsDeApi.Services
{
    static class ExtensionMethods
    {
        public static bool EqualsIgnoreCase(this string source, string compare)
        {
            return string.Equals(source, compare, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool EqualsIgnoreCase(this XName source, string compare)
        {
            if (source == null)
                return compare == null;

            return string.Equals(source.LocalName, compare, StringComparison.CurrentCultureIgnoreCase);
        }

        public static DateTime ToDateTime(this int unixTimestamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimestamp).ToLocalTime();
            return dateTime;
        }

        public static string ToUnsecureString(this SecureString input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(input);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
