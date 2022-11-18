using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Underground lines represent burial distribution cables in a powerflow system
    /// </summary>
    [GlmName("underground_line")]
    public class UndergroundLine : Link
    {
        public Guid ConfigurationId { get; set; }

        [GlmName("configuration")]
        public virtual UndergroundLineConfiguration Configuration { get; set; } = default!;
        public int NumParallelCables { get; set; }

        [GlmName("length")]
        public override double Length { get { return base.Length; } }
    }
}
