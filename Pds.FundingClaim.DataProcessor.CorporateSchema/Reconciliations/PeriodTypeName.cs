using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Pds.FundingClaim.CorporateSchema.Reconciliations
{
    /// <remarks/>
    [GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable]
    [XmlType(Namespace = "urn:sfa:schemas:FCReconciliation")]
    [XmlRoot(ElementName = "periodtypename")]
    public enum PeriodTypeName
    {

        /// <remarks/>
        [XmlEnum("ACADEMIC YEAR")]
        ACADEMICYEAR,

        /// <remarks/>
        [XmlEnum("CALENDAR YEAR")]
        CALENDARYEAR,

        /// <remarks/>
        [XmlEnum("FINANCIAL YEAR")]
        FINANCIALYEAR,

        /// <remarks/>
        [XmlEnum("CALENDAR MONTH")]
        CALENDARMONTH,

        /// <remarks/>
        [XmlEnum("CALENDAR QUARTER")]
        CALENDARQUARTER
    }
}