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
    [XmlRoot(ElementName = "allocationgroup")]
    public class AllocationGroup
    {
        /// <remarks/>
        [XmlElement("allocationgrouppkeyattrs")]
        public AllocationGroupPKeyAttrs AllocationGroupPKeyAttrs { get; set; }

        /// <remarks/>
        [XmlElement("allocationgroupnonpkeyattrs")]
        public AllocationGroupNonPKeyAttrs AllocationGroupNonPKeyAttrs { get; set; }
    }
}