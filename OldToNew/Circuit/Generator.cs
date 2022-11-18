using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Energy generator element
    /// </summary>
    [GlmName("load")]
    public class Generator : DERElement
    {
        public decimal ColdStartupCost { get; set; }
        public double ColdStartupTime { get; set; }
        public decimal HotStartupCost { get; set; }
        public double HotStartupStatus { get; set; }
        public decimal ShutdownCost { get; set; }

        public bool CommitmentStatus { get; set; }

        public int ConstructionTime { get; set; }
        public double CalendarLife { get; set; }
        public int YearOfInstallation { get; set; }

        /// <summary>
        /// Gets or sets the reactive power. (kVAr)
        /// </summary>
        public double ReactivePower { get; set; }

        /// <summary>
        /// Gets or sets the active power. (kW).
        /// </summary>
        public double ActivePower { get; set; }

        public double MaxPowerOutput { get; set; }
        public double MinPowerOutput { get; set; }

        public int MinimumDowntime { get; set; }
        public int MinimumUptime { get; set; }

        public double NoLoadCost { get; set; }
        public double NonSpinningReserve { get; set; }
        public double SpinningReserve { get; set; }
        public double OperatingReserve { get; set; }

        public double RampUpLimit { get; set; }
        public double RampDownLimit { get; set; }

        public double StartupCapability { get; set; }
        public double ShutdownCapability { get; set; }

        public bool ShutdownStatus { get; set; }
        public bool StartupStatus { get; set; }


    }
}
