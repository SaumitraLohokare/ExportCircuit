using Prosumergrid.Domain.Attributes;

namespace Prosumergrid.Domain.Entities.BaseClasses
{
    /// <summary>
    /// Base class for all shunt elements
    /// </summary>
    public abstract class Shunt : DistributedResource
    {
        // for capacitor the previous name was cap_nominal_voltage
        [GlmName("nominal_voltage")]
        public virtual double NominalVoltage { get; set; }
        public Status Status { get; set; }
    }
}
