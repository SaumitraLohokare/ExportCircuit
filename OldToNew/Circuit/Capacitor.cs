using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Defines the control scheme the capacitor will utilize to perform 
    /// switching operations.
    /// </summary>
    public enum CapacitorControl
    {
        /// <summary>
        /// Capacitor switching is controlled manually through SwitchA, SwitchB, and SwitchC.
        /// </summary>
        Manual,
        /// <summary>
        /// Voltage controlled mode. The capacitor parent node itself or a node specified by RemoteSense 
        /// or RemoteSenseB has its voltage checked against VoltageSetHigh and VoltageSetLow.
        /// </summary>
        Volt
    }

    /// <summary>
    /// Specifies how the switching action occurs for all phases of the capacitor.
    /// </summary>
    public enum CapacitorControlLevel
    {
        /// <summary>
        /// All capacitors are switched based on the control scheme and ParticipatingPhases property.
        /// </summary>
        Bank,
        /// <summary>
        /// Capacitors are switched individually based on the control scheme and ParticipatingPhases property.
        /// </summary>
        Individual
    }

    /// <summary>
    /// Capacitors are used for reactive power compensation and voltage support scenarios.
    /// The capacitor implements a switchable set of capacitors. 
    /// </summary>
    [GlmName("capacitor")]
    public class Capacitor : Shunt
    {
        [GlmName("cap_nominal_voltage")]
        public override double NominalVoltage { get; set; }

        public Guid? RemoteSenseId { get; set; }
        [GlmName("remote_sense")]
        public virtual Node? RemoteSense { get; set; }

        public Guid? RemoteSenseBId { get; set; }
        [GlmName("remote_sense_B")]
        public virtual Node? RemoteSenseB { get; set; }

        /// <summary>
        /// Gets or sets the participating phases. (pt_phases).
        /// </summary>
        [GlmName("pt_phase")]
        public string? ParticipatingPhases { get; set; }
        [GlmName("phases_connected")]
        public string? PhasesConnected { get; set; }

        /// <summary>
        /// Gets or sets the switch for the phase A.
        /// </summary>
        [GlmName("switchA")]
        public Status SwitchA { get; set; }
        /// <summary>
        /// Gets or sets the switch for the phase B.
        /// </summary>
        [GlmName("switchB")]
        public Status SwitchB { get; set; }
        /// <summary>
        /// Gets or sets the switch for the phase C.
        /// </summary>
        [GlmName("switchC")]
        public Status SwitchC { get; set; }

        public CapacitorControl Control { get; set; }
        public CapacitorControlLevel ControlLevel { get; set; }

        [GlmName("voltage_set_high")]
        public double VoltageSetHigh { get; set; }

        [GlmName("voltage_set_low")]
        public double VoltageSetLow { get; set; }

        /// <summary>
        /// Gets or sets the reactive power set high. (VAr_set_high)
        /// </summary>
        [GlmName("VAr_set_high")]
        public double ReactivePowerSetHigh { get; set; }
        /// <summary>
        /// Gets or sets the reactive power set low. (VAr_set_low)
        /// </summary>
        [GlmName("VAr_set_low")]
        public double ReactivePowerSetLow { get; set; }

        [GlmName("capacitor_A")]
        public double CapacitorA { get; set; }
        [GlmName("capacitor_B")]
        public double CapacitorB { get; set; }
        [GlmName("capacitor_C")]
        public double CapacitorC { get; set; }

        [GlmName("time_delay")]
        public double TimeDelay { get; set; }
        [GlmName("dwell_time")]
        public double DwellTime { get; set; }
        [GlmName("lockout_time")]
        public double LockoutTime { get; set; }

        // properties not in gridlab d

        public double CurrentSetHigh { get; set; }
        public double CurrentSetLow { get; set; }

    }
}
