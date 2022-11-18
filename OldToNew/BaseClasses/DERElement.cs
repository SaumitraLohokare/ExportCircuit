namespace Prosumergrid.Domain.Entities.BaseClasses
{
    /// <summary>
    /// The element planning status.
    /// </summary>
    public enum PlanningStatus { Installed, Optimized }
    /// <summary>
    /// Output schedule enumeration.
    /// </summary>
    public enum OutputSchedule { Optimized, UnitPredefined, FeederPredefined }

    /// <summary>
    /// Base class for all DER elements.
    /// </summary>
    public abstract class DERElement : DistributedResource
    {
        public decimal CapitalCost { get; set; } = new();
        public decimal FixedOperatingCost { get; set; } = new();
        public decimal VariableOperatingCost { get; set; } = new();

        /// <summary>
        /// Gets or sets the max reactive power. (max_kvar before).
        /// </summary>
        public double MaxReactivePower { get; set; }
        /// <summary>
        /// Gets or sets the max active power. (max_kw before).
        /// </summary>
        public double MaxActivePower { get; set; }

        public OutputSchedule OutputSchedule { get; set; }
        public PlanningStatus PlanningStatus { get; set; }

        public string UnitType { get; set; } = default!;
        public string UnitSubtype { get; set; } = default!;
    }
}
