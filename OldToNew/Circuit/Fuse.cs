using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Status of the fuse on current phase
    public enum PhaseFuseStatus
    {
        /// <summary>
        /// The fuse on current phase is still conducting and has not exceeded its current limit.
        /// </summary>
        Good,
        /// <summary>
        /// The fuse on current phase has exceeded its current limit and is no longer conducting.
        /// </summary>
        Blown
    }

    /// <summary>
    /// Distribution to be used after a fuse has blown to restore it.
    /// </summary>
    public enum RepairDistributionType
    {
        /// <summary>
        /// No distribution is used and the value in MeanReplacementTime is taken directly.
        /// </summary>
        None,
        /// <summary>
        /// An exponential distribution is used with MeanReplacementTime taken as one over the lambda value.
        /// </summary>
        Exponential
    }

    /// <summary>
    /// Electric fuse element
    /// </summary>
    [GlmName("fuse")]
    public class Fuse : SwitchElement
    {
        [GlmName("mean_replacement_time")]
        public double MeanReplacementTime { get; set; }
        /// <summary>
        /// Gets or sets the phase A status.
        /// </summary>
        [GlmName("phase_A_status")]
        public PhaseFuseStatus PhaseAStatus { get; set; }
        /// <summary>
        /// Gets or sets the phase B status.
        /// </summary>
        [GlmName("phase_B_status")]
        public PhaseFuseStatus PhaseBStatus { get; set; }
        /// <summary>
        /// Gets or sets the phase C status.
        /// </summary>
        [GlmName("phase_C_status")]
        public PhaseFuseStatus PhaseCStatus { get; set; }

        [GlmName("repair_dist_type")]
        public RepairDistributionType RepairDistributionType { get; set; }

        [GlmName("current_limit")]
        public double CurrentLimit { get; set; }

    }
}
