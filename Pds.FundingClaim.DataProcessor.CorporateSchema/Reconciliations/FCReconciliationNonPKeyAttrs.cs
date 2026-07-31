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
    [XmlRoot(ElementName = "fcreconciliationnonpkeyattrs")]
    public class FCReconciliationNonPKeyAttrs
    {
        /// <remarks/>
        [XmlElement("status")]
        public ReconciliationStatus Status { get; set; }

        /// <remarks/>
        [XmlElement("statuslastupdateddate", DataType = "date")]
        public DateTime StatusLastUpdatedDate { get; set; }

        /// <remarks/>
        [XmlElement("amountattrs")]
        public AmountAttrs AmountAttrs { get; set; }

        /// <remarks/>
        [XmlElement("contractallocation")]
        public ContractAllocation[] ContractAllocation { get; set; }
    }
}