using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Overhead line element
    /// </summary>
    [GlmName("overhead_line")]
    public class OverheadLine : Link
    {
        public Guid ConfigurationId { get; set; }

        [GlmName("configuration")]
        public virtual OverheadLineConfiguration Configuration { get; set; } = default!;

        [GlmName("length")]
        public override double Length { get { return base.Length; } }
    }
}
