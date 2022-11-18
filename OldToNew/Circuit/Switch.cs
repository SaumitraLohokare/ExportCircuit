using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Switch objects are used to change topology and add or remove elements from a powerflow system.
    /// When a switch is opened, no current flow is permitted and the downstream objects will be effectively removed from the system.
    /// </summary>
    [GlmName("switch")]
    public class Switch : SwitchElement
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
