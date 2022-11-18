using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Storage element
    /// </summary>
    [GlmName("load")]
    public class Storage : EnergyStorageElement
    {
        public double InitStateOfCharge { get; set; }

        public double SpinningReserveCharge { get; set; }
        public double SpinningReserveDischarge { get; set; }

        public double NonSpinningReserveCharge { get; set; }
        public double NonSpinningReserveDischarge { get; set; }


        public double RegulationUpCharge { get; set; }
        public double RegulationUpDischarge { get; set; }
        public double RegulationDownCharge { get; set; }
        public double RegulationDownDischarge { get; set; }

    }
}
