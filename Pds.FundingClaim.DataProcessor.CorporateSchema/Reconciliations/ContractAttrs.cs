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
    [XmlRoot(ElementName = "contractattrs")]
    public class ContractAttrs
    {
        /// <remarks/>
        [XmlElement("contractnumber")]
        public string ContractNumber { get; set; }

        /// <remarks/>
        [XmlElement("contractversionnumber")]
        public int ContractVersionNumber { get; set; }

        /// <remarks/>
        [XmlElement("contractsubversionnumber")]
        public int ContractSubVersionNumber { get; set; }

        /// <remarks/>
        [XmlIgnore]
        public bool ContractSubVersionNumberSpecified { get; set; }

        /// <remarks/>
        [XmlElement("fundingtypeallattrs")]
        public FundingTypeAllAttrs FundingTypeAllAttrs { get; set; }

        /// <remarks/>
        [XmlElement("contractapprovaldate", DataType = "date", IsNullable = true)]
        public DateTime? ContractApprovalDate { get; set; }

        /// <remarks/>
        [XmlIgnore]
        public bool ContractApprovalDateSpecified { get; set; }
    }
}