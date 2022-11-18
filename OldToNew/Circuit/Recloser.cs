using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    [GlmName("recloser")]
    public class Recloser : Link
    {
        [GlmName("retry_time")]
        public double RetryTime { get; set; }

        [GlmName("max_number_of_tries ")]
        public double MaximumNumberOfTries { get; set; }
    }
}
