using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Sectionalizer objects provide a means to isolate faulted portions of a system. 
    /// Sectionalizer objects work in conjuction with recloser objects. 
    /// </summary>
    [GlmName("sectionalizer")]
    public class Sectionalizer : SwitchElement
    {
        [GlmName("operating_mode")]
        public OperatingMode OperatingMode { get; set; }

        [GlmName("phase_A_state")]
        public Status PhaseAState { get; set; }
        [GlmName("phase_B_state")]
        public Status PhaseBState { get; set; }
        [GlmName("phase_C_state")]
        public Status PhaseCState { get; set; }
    }
}
