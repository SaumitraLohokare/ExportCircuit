using Prosumergrid.Domain.Entities.Circuit;
using Geolocation;
using Prosumergrid.Domain.Attributes;

namespace Prosumergrid.Domain.Entities.BaseClasses
{
    /// <summary>
    /// Link status
    /// </summary>
    public enum Status { Open, Closed }
    /// <summary>
    /// Base class for all link objects
    /// </summary>
    public abstract class Link : DisplayableElement
    {
        public Guid FromId { get; set; }

        public Guid ToId { get; set; }
        public Status Status { get; set; }


        /// <summary>
        /// Navigation property for link origin Node.
        /// </summary>
        [GlmName("from")]
        public virtual Node From { get; set; } = default!;
        /// <summary>
        /// Navigation property for link destination Node.
        /// </summary>
        [GlmName("to")]
        public virtual Node To { get; set; } = default!;

        /// <summary>
        /// Gets the link length
        /// </summary>

        public virtual double Length
        {
            get
            {
                var from = new Coordinate
                {
                    Latitude = From.GetConvertedLatitude(),
                    Longitude = From.GetConvertedLongitude()
                };

                var to = new Coordinate
                {
                    Latitude = To.GetConvertedLatitude(),
                    Longitude = To.GetConvertedLongitude()
                };

                double distanceMiles;

                try
                {
                    distanceMiles = GeoCalculator.GetDistance(from, to, 2, DistanceUnit.Miles);
                }
                catch (Exception)
                {
                    return 0;
                }

                var distanceFeet = distanceMiles * 5280;

                return distanceFeet;
            }
        }
    }
}
