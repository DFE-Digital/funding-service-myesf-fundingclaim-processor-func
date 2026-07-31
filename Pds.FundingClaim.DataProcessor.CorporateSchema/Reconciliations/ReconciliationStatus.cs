using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Pds.FundingClaim.CorporateSchema.Reconciliations
{
    /// <remarks/>
    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable]
    [XmlType(Namespace = "urn:sfa:schemas:FCReconciliation")]
    [XmlRoot(ElementName = "status")]
    public enum ReconciliationStatus
    {

        /// <remarks/>
        Draft,

        /// <remarks/>
        Published
    }
}