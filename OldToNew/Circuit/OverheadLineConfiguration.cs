using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Overhead line objects take line configuration information to describe the particular line being implemented
    /// </summary>
    [GlmName("line_configuration")]
    public class OverheadLineConfiguration : LineConfiguration
    {
        // navigation properties
        [GlmName("conductor_A")]
        public virtual OverheadLineConductor? ConductorA { get; set; }
        [GlmName("conductor_B")]
        public virtual OverheadLineConductor? ConductorB { get; set; }
        [GlmName("conductor_C")]
        public virtual OverheadLineConductor? ConductorC { get; set; }
        [GlmName("conductor_N")]
        public virtual OverheadLineConductor? ConductorN { get; set; }
    }
}
