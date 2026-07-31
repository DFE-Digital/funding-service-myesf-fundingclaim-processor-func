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
    [XmlRoot(ElementName = "deliverable")]
    public class Deliverable
    {
        /// <remarks/>
        [XmlElement("deliverablecode")]
        public int DeliverableCode { get; set; }

        /// <remarks/>
        [XmlElement("deliverablename")]
        public string DeliverableName { get; set; }
    }
}