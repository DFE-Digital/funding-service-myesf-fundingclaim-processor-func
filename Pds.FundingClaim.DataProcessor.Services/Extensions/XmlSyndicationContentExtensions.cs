using System.ServiceModel.Syndication;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Pds.FundingClaim.DataProcessor.Services.Extensions
{
    /// <summary>
    /// Extension methods on XmlSyndicationContent.
    /// </summary>
    public static class XmlSyndicationContentExtensions
    {
        /// <summary>
        /// Deserializes the xml content.
        /// </summary>
        /// <typeparam name="T">The type to be deserialized into.</typeparam>
        /// <param name="xmlContent">The content to be deserialized.</param>
        /// <returns>The type content.</returns>
        public static T Deserialize<T>(this XmlSyndicationContent xmlContent)
        {
            var serializer = new XmlSerializer(typeof(T));
            var contentXml = new XDocument();
            using (var writer = contentXml.CreateWriter())
            {
                xmlContent.WriteTo(writer, "content", string.Empty);
            }

            contentXml.LowerCaseAllElementAndAttributeNames();

            using (var reader = contentXml.Root.FirstNode.CreateReader())
            {
                var item = (T)serializer.Deserialize(reader);
                return item;
            }
        }
    }
}
