using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Electrical feeder circuit.
    /// </summary>
    public partial class Feeder : Element
    {
        public string? Description { get; set; }

        public Guid ScenarioId { get; set; }
    }
}
