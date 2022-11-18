
using Prosumergrid.Domain.Attributes;

namespace Prosumergrid.Domain.Entities.BaseClasses
{
    /// <summary>
    /// Base class for all conductor elements.
    /// </summary>
    public abstract class Conductor : DistributionElement
    {
        public double RatingSummerContinuous { get; set; }

        public double RatingSummerEmergency { get; set; }

        public double RatingWinterContinuous { get; set; }

        public double RatingWinterEmergency { get; set; }

    }
}
