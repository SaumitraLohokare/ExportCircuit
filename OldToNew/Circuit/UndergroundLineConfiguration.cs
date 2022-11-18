using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Underground line objects take line configuration information to describe the particular line being implemented
    /// </summary>
    [GlmName("line_configuration")]
    public class UndergroundLineConfiguration : LineConfiguration
    {
        // navigation properties
        [GlmName("conductor_A")]
        public virtual UndergroundLineConductor? ConductorA { get; set; } = default!;
        [GlmName("conductor_B")]
        public virtual UndergroundLineConductor? ConductorB { get; set; } = default!;
        [GlmName("conductor_C")]
        public virtual UndergroundLineConductor? ConductorC { get; set; } = default!;
        [GlmName("conductor_N")]
        public virtual UndergroundLineConductor? ConductorN { get; set; } = default!;
    }
}
