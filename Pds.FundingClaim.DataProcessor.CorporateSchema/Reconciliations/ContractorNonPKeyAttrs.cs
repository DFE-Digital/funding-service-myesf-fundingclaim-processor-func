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
    [XmlRoot(ElementName = "contractornonpkeyattrs")]
    public class ContractorNonPKeyAttrs
    {
        /// <remarks/>
        [XmlElement(ElementName = "ukprn")]
        public int UKPRN { get; set; }

        /// <remarks/>
        [XmlElement(ElementName = "legalname")]
        public string LegalName { get; set; }
    }
}