using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Pds.FundingClaim.CorporateSchema.Reconciliations
{
    /// <remarks/>
    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "urn:sfa:schemas:FCReconciliation")]
    [XmlRoot(Namespace = "urn:sfa:schemas:FCReconciliation", ElementName = "fcreconciliation")]
    public class FCReconciliation
    {
        public FCReconciliation()
        {
            SchemaVersion = 3.00m;
        }

        /// <remarks/>
        [XmlElement("fcreconciliationallattrs")]
        public FCReconciliationAllAttrs FCReconciliationAllAttrs { get; set; }

        /// <remarks/>
        [XmlAttribute]
        public decimal SchemaVersion { get; set; }

        public XDocument ToXml()
        {
            XmlSerializer serializer = null;

            try
            {
                serializer = new XmlSerializer(typeof(FCReconciliation));
            }
            catch (Exception)
            {
            }

            var document = new XDocument();

            using (var stream = document.CreateWriter())
            {
                serializer.Serialize(stream, this);
            }
            return document;
        }

        public static FCReconciliation FromXml(string rawXml)
        {
            XmlSerializer serializer = null;

            try
            {
                serializer = new XmlSerializer(typeof(FCReconciliation));
            }
            catch (Exception)
            {
            }

            var xdoc = XDocument.Parse(rawXml);

            using (var stream = xdoc.CreateReader())
            {
                return (FCReconciliation)serializer.Deserialize(stream);
            }
        }
    }
}