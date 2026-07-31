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
    [XmlType(AnonymousType = true, Namespace = "urn:sfa:schemas:FCReconciliation")]
    [XmlRoot(ElementName = "contractdeliverable")]
    public class ContractDeliverable
    {
        /// <remarks/>
        [XmlElement("deliverable")]
        public Deliverable Deliverable { get; set; }

        /// <remarks/>
        [XmlElement("amountattrs")]
        public AmountAttrs AmountAttrs { get; set; }
    }
}