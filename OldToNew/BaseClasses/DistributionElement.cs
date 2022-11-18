using Prosumergrid.Domain.Entities.Circuit;

namespace Prosumergrid.Domain.Entities.BaseClasses
{
    public abstract class DistributionElement : Element
    {
        public Guid FeederId { get; set; }
        public virtual Feeder Feeder { get; set; } = default!;

    }
}
