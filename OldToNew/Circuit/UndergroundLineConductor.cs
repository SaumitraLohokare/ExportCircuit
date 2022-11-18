using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    [GlmName("underground_line_conductor")]
    public class UndergroundLineConductor : Conductor
    {
        [GlmName("outer_diameter")]
        public double OuterDiameter { get; set; }

        [GlmName("conductor_gmr")]
        public double ConductorGMR { get; set; }

        [GlmName("conductor_diameter")]
        public double ConductorDiameter { get; set; }

        [GlmName("conductor_resistance")]
        public double ConductorResistance { get; set; }
        [GlmName("conductor_gmr")]
        public double NeutralGMR { get; set; }
        [GlmName("neutral_diameter")]
        public double NeutralDiameter { get; set; }
        [GlmName("neutral_resistance")]
        public double NeutralResistance { get; set; }
        [GlmName("neutral_strands")]
        public int NeutralStrands { get; set; }

        [GlmName("insultation_relative_permitivitty")]
        public double InsulationRelativePermittivity { get; set; }

        [GlmName("shield_gmr")]
        public double ShieldGMR { get; set; }

        [GlmName("shield_resistance")]
        public double ShieldResitance { get; set; }

        public double? Susceptance { get; set; }
        public double? Resistance { get; set; }
        public double? Reactance { get; set; }
    }
}
