using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Transformers provide a means to change the voltage from one node to another in the distribution system.
    /// Similar to the different line objects, a transformer object requires a configuration object to specify the details of the implementation.
    /// </summary>
    [GlmName("transformer")]
    public class Transformer : Link
    {
        public Guid ConfigurationId { get; set; }

        [GlmName("configuration")]
        public virtual TransformerConfiguration Configuration { get; set; } = default!;

        public double AgingConstant { get; set; }

        [GlmName("aging_granularity")]
        public double AgingGranularity { get; set; }

        [GlmName("climate")]
        public string? Climate { get; set; }

        [GlmName("use_thermal_model")]
        public bool UseThermalModel { get; set; }
    }
}
