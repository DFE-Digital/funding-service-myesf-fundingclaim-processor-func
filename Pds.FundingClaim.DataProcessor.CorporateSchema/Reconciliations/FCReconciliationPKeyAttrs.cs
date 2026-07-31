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
    [XmlRoot(ElementName = "fcreconciliationpkeyattrs")]
    public class FCReconciliationPKeyAttrs
    {
        /// <remarks/>
        [XmlElement("contractor")]
        public Contractor Contractor { get; set; }

        /// <remarks/>
        [XmlElement("period")]
        public Period Period { get; set; }

        /// <remarks/>
        [XmlElement("claimversionnumber")]
        public int ClaimVersionNumber { get; set; }

        /// <remarks/>
        [XmlElement("claimtype")]
        public ClaimType ClaimType { get; set; }

        /// <remarks/>
        [XmlElement("allocationgroup")]
        public AllocationGroup AllocationGroup { get; set; }
    }
}