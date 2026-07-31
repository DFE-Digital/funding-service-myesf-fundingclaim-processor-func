using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Pds.FundingClaim.CorporateSchema.Reconciliations
{
    /// <remarks/>
    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "urn:sfa:schemas:FCReconciliation")]
    [XmlRoot(ElementName = "fcreconciliationallattrs")]
    public class FCReconciliationAllAttrs
    {
        /// <remarks/>
        [XmlElement("fcreconciliationpkeyattrs")]
        public FCReconciliationPKeyAttrs FCReconciliationPKeyAttrs { get; set; }

        /// <remarks/>
        [XmlElement("fcreconciliationnonpkeyattrs")]
        public FCReconciliationNonPKeyAttrs FCReconciliationNonPKeyAttrs { get; set; }
    }
}