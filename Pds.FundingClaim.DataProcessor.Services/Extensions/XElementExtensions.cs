using System;
using System.Xml.Linq;

namespace Pds.FundingClaim.DataProcessor.Services.Extensions
{
    /// <summary>
    /// Extension methods xelement.
    /// </summary>
    public static class XElementExtensions
    {
        /// <summary>
        /// Lower case the element name of the current element and all its child elements.
        /// </summary>
        /// <param name="xElement">The caller object.</param>
        /// <returns>The element.</returns>
        public static XElement LowerCaseAllElementNames(this XElement xElement)
        {
            if (xElement == null)
            {
                throw new ArgumentNullException(nameof(xElement));
            }

            xElement.Name = xElement.Name.Namespace + xElement.Name.LocalName.ToLower();

            foreach (var element in xElement.Elements())
            {
                element.LowerCaseAllElementNames();
            }

            return xElement;
        }

        /// <summary>
        /// Lower case the element name of the current element and all its child elements.
        /// </summary>
        /// <param name="xElement">The caller object.</param>
        /// <returns>The XElement.</returns>
        public static XElement LowerCaseAllAttributeNames(this XElement xElement)
        {
            if (xElement == null)
            {
                throw new ArgumentNullException(nameof(xElement));
            }

            // Replacing attributes here as XAttribute.Name has no setter.
            foreach (var attribute in xElement.Attributes())
            {
                if (!attribute.IsNamespaceDeclaration && string.IsNullOrEmpty(attribute.Name.NamespaceName))
                {
                    var newAttribute = new XAttribute(attribute.Name.LocalName.ToLower(), attribute.Value);
                    attribute.Remove();
                    xElement.Add(newAttribute);
                }
            }

            foreach (var element in xElement.Elements())
            {
                element.LowerCaseAllAttributeNames();
            }

            return xElement;
        }
    }
}
