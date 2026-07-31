using System;
using System.Xml.Linq;

namespace Pds.FundingClaim.DataProcessor.Services.Extensions
{
    /// <summary>
    /// Extension methods on XDocument.
    /// </summary>
    public static class XDocumentExtensions
    {
        /// <summary>
        /// Lower case the name of all the elements and attributes for the current XDocument.
        /// </summary>
        /// <param name="xDoc">The caller object.</param>
        public static void LowerCaseAllElementAndAttributeNames(this XDocument xDoc)
        {
            xDoc.LowerCaseAllElementNames();
            xDoc.LowerCaseAllAttributeNames();
        }

        /// <summary>
        /// Lower case the name of all the elements for the current XDocument.
        /// </summary>
        /// <param name="xDoc">The caller object.</param>
        public static void LowerCaseAllElementNames(this XDocument xDoc)
        {
            if (xDoc == null)
            {
                throw new ArgumentNullException(nameof(xDoc));
            }

            foreach (var element in xDoc.Elements())
            {
                element.LowerCaseAllElementNames();
            }
        }

        /// <summary>
        /// Lower case the name of all the attributes for the current XDocument.
        /// </summary>
        /// <param name="xDoc">The caller object.</param>
        public static void LowerCaseAllAttributeNames(this XDocument xDoc)
        {
            if (xDoc == null)
            {
                throw new ArgumentNullException(nameof(xDoc));
            }

            foreach (var element in xDoc.Elements())
            {
                element.LowerCaseAllAttributeNames();
            }
        }
    }
}
