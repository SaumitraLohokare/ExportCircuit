using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// For overhead lines, the overheadline configuration object must specify the overhead line conductor types used in the particular setup.
    /// </summary>
    [GlmName("overhead_line_conductor")]
    public class OverheadLineConductor : Conductor
    {
        [GlmName("diameter")]
        public double Diameter { get; set; }

        [GlmName("geometric_mean_radius")]
        public double GeometricMeanRadius { get; set; }

        [GlmName("resistance")]
        public double Resistance { get; set; }
    }
}
