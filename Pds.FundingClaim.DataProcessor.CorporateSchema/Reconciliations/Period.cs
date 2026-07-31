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
    [XmlRoot(ElementName = "period")]
    public class Period
    {
        /// <remarks/>
        [XmlElement("periodtypepkeyattrs")]
        public PeriodTypePKeyAttrs PeriodTypePKeyAttrs { get; set; }

        /// <remarks/>
        [XmlElement("periodtypenonpkeyattrs")]
        public PeriodTypeNonPKeyAttrs PeriodTypeNonPKeyAttrs { get; set; }

        /// <remarks/>
        [XmlElement(ElementName = "period")]
        public string PeriodValue { get; set; }
    }
}