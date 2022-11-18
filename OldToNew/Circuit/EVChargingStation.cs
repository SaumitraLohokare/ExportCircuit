using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Electric Vehicle Charging Station
    /// </summary>
    [GlmName("load")]
    public class EVChargingStation : EnergyStorageElement
    {
        public double ServiceCharge { get; set; }
        public double ServiceDischarge { get; set; }
        
    }
}
