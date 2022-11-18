using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    [GlmName("load")]
    public class Solar : DERElement
    {
        public decimal SpillageCost { get; set; }
    }
}

