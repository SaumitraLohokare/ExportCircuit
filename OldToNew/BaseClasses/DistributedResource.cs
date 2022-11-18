using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.Circuit;

namespace Prosumergrid.Domain.Entities.BaseClasses
{
    public abstract class DistributedResource : DisplayableElement
    {
        public Guid ParentNodeId { get; set; }

        [GlmName("parent")]
        public virtual Node ParentNode { get; set; } = default!;
    }
}
