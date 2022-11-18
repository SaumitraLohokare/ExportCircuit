using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// The line spacing object describe how the individual conductors of a 
    /// distribution line are arranged underground or on the support pole.
    /// </summary>
    [GlmName("line_spacing")]
    public class LineSpacing : DistributionElement
    {
        [GlmName("distance_AB")]
        public double DistanceAB { get; set; }

        [GlmName("distance_AC")]
        public double DistanceAC { get; set; }

        [GlmName("distance_AE")]
        public double DistanceAE { get; set; }

        [GlmName("distance_AN")]
        public double DistanceAN { get; set; }

        [GlmName("distance_BC")]
        public double DistanceBC { get; set; }

        [GlmName("distance_BE")]
        public double DistanceBE { get; set; }

        [GlmName("distance_BN")]
        public double DistanceBN { get; set; }

        [GlmName("distance_CE")]
        public double DistanceCE { get; set; }

        [GlmName("distance_CN")]
        public double DistanceCN { get; set; }

        [GlmName("distance_NE")]
        public double DistanceNE { get; set; }
    }
}
