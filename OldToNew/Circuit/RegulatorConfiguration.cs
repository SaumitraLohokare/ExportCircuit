using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Selection method for the electrical connection type of the regulator implemented. 
    /// Valid types may be referred to by number or keyword
    /// </summary>
    public enum RegulatorConnectType
    {
        /// <summary>Unknown regulator implementation that will throw an error if used</summary>
        Unknown,
        /// <summary>Wye connected regulator implementation.</summary>
        WyeWye,
        /// <summary>
        /// Open delta connected regulator with CA open
        /// </summary>
        /// <remarks>Unimplemented in Gridlab-D at this time.</remarks>
        OpenDeltaABBC,
        /// <summary>
        /// Open delta connected regulator with AB open
        /// </summary>
        /// <remarks>Unimplemented in Gridlab-D at this time.</remarks>
        OpenDeltaBCAC,
        /// <summary>
        /// Open delta connected regulator with BC open
        /// </summary>
        /// <remarks>Unimplemented in Gridlab-D at this time.</remarks>
        OpenDeltaCABA,
        /// <summary>
        /// Closed delta connected regulator implementation
        /// </summary>
        /// <remarks>Unimplemented in Gridlab-D at this time.</remarks>
        ClosedDelta

    }

    /// <summary>
    /// Defines how automatic controls influence the tap settings of the regulator.
    /// /// </summary>
    public enum RegulatorControlLevel
    {
        /// <summary>Each phase is controlled individually.</summary>
        Individual,
        /// <summary>
        /// All phases are controlled identically.
        /// Using the PowerTransducerPhase (PT_phase) property, the regulator determines any control actions and applies it to all phases identically.
        /// </summary>
        Bank
    }

    /// <summary>
    /// Defines the control scheme the regulator will use to operate. 
    /// </summary>
    public enum RegulatorControlScheme
    {
        /// <summary>Manual control mode. User specifies all tap changes.</summary>
        Manual,
        /// <summary>
        /// Output node of the regulator's voltage is examined. Tap changes are performed based on BandCenter and BandWidth.
        /// </summary>
        OutputVoltage,
        /// <summary>
        /// Line drop compensator control mode. Utilizes compensator information in addition to band_center and BandWidth to determine tap changes.
        /// </summary>
        LineDropComp,
        /// <summary>
        /// Voltage of a remote node (specified by sense_node in the regulator object) in the system is examined.
        /// Tap changes are performed based on BandCenter and BandWidth.
        /// </summary>
        RemoteNode
    }

    public enum StepVoltageRegulatorType
    {
        /// <summary>Type A step-voltage regulator</summary>
        TypeA,
        /// <summary>Type B step-voltage regulator</summary>
        TypeB
    }


    /// <summary>
    /// The regulator configuration object describes the details of a particular regulator object implementation.
    /// This includes details such as the control scheme, regulator type, sensing information, and time delays.
    /// </summary>
    [GlmName("regulator_configuration")]
    public class RegulatorConfiguration : DistributionElement
    {
        /// <summary>
        /// Current transducer connection phase. (CT_phase)
        /// </summary>
        /// <remarks>This function is not implemented at this time in Gridlab-D.</remarks>
        [GlmName("CT_phase")]
        public string? CurrentTransducerPhase { get; set; }

        /// <summary>
        /// Power transducer connection phase. (PT_phase)
        /// </summary>
        /// <remarks>This function is not implemented at this time in Gridlab-D.</remarks>
        [GlmName("PT_phase")]
        public string? PowerTransducerPhase { get; set; }

        [GlmName("regulation")]
        public double Regulation { get; set; }

        [GlmName("Control")]
        public RegulatorControlScheme Control { get; set; }
        [GlmName("Type")]
        public StepVoltageRegulatorType Type { get; set; }

        [GlmName("compensator_r_setting_A")]
        public double CompensatorResistiveSettingA { get; set; }
        [GlmName("compensator_r_setting_B")]
        public double CompensatorResistiveSettingB { get; set; }
        [GlmName("compensator_r_setting_C")]
        public double CompensatorResistiveSettingC { get; set; }

        [GlmName("compensator_x_setting_A")]
        public double CompensatorReactiveSettingA { get; set; }
        [GlmName("compensator_x_setting_B")]
        public double CompensatorReactiveSettingB { get; set; }
        [GlmName("compensator_x_setting_C")]
        public double CompensatorReactiveSettingC { get; set; }

        [GlmName("band_center")]
        public double BandCenter { get; set; }
        [GlmName("band_width")]
        public double BandWidth { get; set; }

        [GlmName("current_transducer_ratio")]
        public double CurrentTransducerRatio { get; set; }
        [GlmName("power_transducer_ratio")]
        public double PowerTransducerRatio { get; set; }

        [GlmName("connect_type")]
        public RegulatorConnectType ConnectType { get; set; }

        [GlmName("control_level")]
        public RegulatorControlLevel ControlLevel { get; set; }

        [GlmName("dwell_time")]
        public double DwellTime { get; set; }
        [GlmName("time_delay")]
        public double TimeDelay { get; set; }

        [GlmName("lower_taps")]
        public int LowerTaps { get; set; }
        [GlmName("raise_taps")]
        public int RaiseTaps { get; set; }

        [GlmName("tap_pos_A")]
        public int TapPositionA { get; set; }

        [GlmName("tap_pos_B")]
        public int TapPositionB { get; set; }

        [GlmName("tap_pos_C")]
        public int TapPositionC { get; set; }


    }
}
