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
    [XmlRoot(ElementName = "contractallocation")]
    public class ContractAllocation
    {
        /// <remarks/>
        [XmlElement("contractallocationpkeyattrs")]
        public ContractAllocationPKeyAttrs ContractAllocationPKeyAttrs { get; set; }

        /// <remarks/>
        [XmlElement("contractallocationnonpkeyattrs")]
        public ContractAllocationNonPKeyAttrs ContractAllocationNonPKeyAttrs { get; set; }

        /// <remarks/>
        [XmlElement("contractallocationdnattrs")]
        public ContractAllocationDNAttrs ContractAllocationDNAttrs { get; set; }
    }
}