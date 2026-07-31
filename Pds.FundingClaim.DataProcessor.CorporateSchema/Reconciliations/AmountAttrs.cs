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
    [XmlRoot(ElementName = "amountattrs")]
    public class AmountAttrs
    {
        /// <remarks/>
        [XmlElement("plannedvalue", IsNullable = true)]
        public decimal? PlannedValue { get; set; }

        /// <remarks/>
        [XmlIgnore]
        public bool PlannedValueSpecified { get; set; }

        /// <remarks/>
        [XmlElement("claimedvalue", IsNullable = true)]
        public decimal? ClaimedValue { get; set; }

        /// <remarks/>
        [XmlIgnore]
        public bool ClaimedValueSpecified { get; set; }

        /// <remarks/>
        [XmlElement("proposedreconciliationvalue", IsNullable = true)]
        public decimal? ProposedReconciliationValue { get; set; }

        /// <remarks/>
        [XmlIgnore]
        public bool ProposedReconciliationValueSpecified { get; set; }

        /// <remarks/>
        [XmlElement("reconciliationvalue", IsNullable = true)]
        public decimal? ReconciliationValue { get; set; }

        /// <remarks/>
        [XmlIgnore]
        public bool ReconciliationValueSpecified { get; set; }

        /// <remarks/>
        [XmlElement("cappedclaimedvalue", IsNullable = true)]
        public decimal? CappedClaimedValue { get; set; }

        /// <remarks/>
        [XmlIgnore]
        public bool CappedClaimedValueSpecified { get; set; }

        /// <remarks/>
        [XmlElement("adjustedcappedclaimedvalue", IsNullable = true)]
        public decimal? AdjustedCappedClaimedValue { get; set; }

        /// <remarks/>
        [XmlIgnore]
        public bool AdjustedCappedClaimedValueSpecified { get; set; }

        /// <remarks/>
        [XmlElement("claimadjustments", IsNullable = true)]
        public ClaimAdjustmentAttrs ClaimAdjustments { get; set; }
		
		 /// <remarks/>
        [XmlElement("earningsboostvalue", IsNullable = true)]
        public decimal? EarningsBoostValue { get; set; }
    }
}