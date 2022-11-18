using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Heat pump
    /// </summary>
    [GlmName("load")]
    public class HeatPump : DERElement
    {
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

        public double RampUpLimit { get; set; }
        public double RampDownLimit { get; set; }
    }
}
