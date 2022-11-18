using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Regulators are essentially tap-changing transformers that attempt to maintain a voltage level at a specified point in the system.
    /// Regulators are one of two objects in the powerflow module that incorporate a form of automatic control.
    /// To take full advantage of this functionality, simulations of greater than one time step (time-varying simulations) are recommended.
    /// Similar to transformer and line objects, regulators require a regulator_configuration to determine many of their operating parameters.
    /// </summary>
    [GlmName("regulator")]
    public class Regulator : Link
    {
        public Guid ConfigurationId { get; set; }

        [GlmName("configuration")]
        public virtual RegulatorConfiguration Configuration { get; set; } = default!;

        public Guid? SenseNodeId { get; set; }

        [GlmName("sense_node")]
        public Node? SenseNode { get; set; }
    }
}
