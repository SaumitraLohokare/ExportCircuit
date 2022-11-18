using Prosumergrid.Domain.Attributes;

namespace Prosumergrid.Domain.Entities.BaseClasses
{
    public abstract class Element
    {
        public Guid Id { get; set; }

        [GlmName("name")]
        public string Name { get; set; } = default!;
        public string? Comments { get; set; }
    }
}
