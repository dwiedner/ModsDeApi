using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ModsDeApi.Services
{
    class XmlHelper
    {
        public static async Task<XDocument> LoadUrl(string url)
        {
            using (var client = new WebClient())
            {
                using (var stream = await client.OpenReadTaskAsync(url))
                {
                    return XDocument.Load(stream);
                }
            }
        }

        public static string GetAttributeValue(XElement element, string attributeName)
        {
            var idAttribute = element.Attributes().FirstOrDefault(x => x.Name.EqualsIgnoreCase(attributeName));
            if (idAttribute == null)
                throw new Exception($"[{element.Name}] element has no [{attributeName}] attribute");

            return idAttribute.Value;
        }

        public static int GetAttributeValueAsInt(XElement element, string attributeName)
        {
            var value = GetAttributeValue(element, attributeName);

            int number;
            if (!int.TryParse(value, out number))
                throw new Exception($"Invalid value for [{attributeName}] attribute [{value}]");

            return number;
        }

        public static string GetChildValue(XElement element, string childName)
        {
            var descriptionElement = element.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(childName));
            if (descriptionElement == null)
                throw new Exception($"[{element.Name}] element doesn't contain [{childName}] element");

            return descriptionElement.Value;
        }

        public static string GetChildAttributeValue(XElement element, string childName, string attributeName)
        {
            var childElement = element.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(childName));
            if (childElement == null)
                return string.Empty;

            return GetAttributeValue(childElement, attributeName);
        }

        public static int GetChildAttributeValueAsInt(XElement element, string childName, string attributeName)
        {
            int number;
            if (!int.TryParse(GetChildAttributeValue(element, childName, attributeName), out number))
                return 0;

            return number;
        }
    }
}
