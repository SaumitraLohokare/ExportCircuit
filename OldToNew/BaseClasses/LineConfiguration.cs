using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.Circuit;
using System.Numerics;

namespace Prosumergrid.Domain.Entities.BaseClasses
{
    /// <summary>
    /// Base class for all line configuration objects
    /// </summary>
    public abstract class LineConfiguration : DistributionElement
    {
        public Guid? LineSpacingId { get; set; }

        [GlmName("spacing")]
        public virtual LineSpacing? LineSpacing { get; set; }

        // conductors

        public Guid? ConductorAId { get; set; }
        public Guid? ConductorBId { get; set; }
        public Guid? ConductorCId { get; set; }
        public Guid? ConductorNId { get; set; }

        [GlmName("c11")]
        public double Capacitance11 { get; set; }

        [GlmName("c12")]
        public double Capacitance12 { get; set; }

        [GlmName("c13")]
        public double Capacitance13 { get; set; }

        [GlmName("c21")]
        public double Capacitance21 { get; set; }

        [GlmName("c22")]
        public double Capacitance22 { get; set; }

        [GlmName("c23")]
        public double Capacitance23 { get; set; }

        [GlmName("c31")]
        public double Capacitance31 { get; set; }

        [GlmName("c32")]
        public double Capacitance32 { get; set; }

        [GlmName("c33")]
        public double Capacitance33 { get; set; }

        // impedances

        public double Resistance11{ get; set; }
        public double Reactance11{ get; set; }

        [GlmName("z11")]
        public Complex Impedance11
        {
            get => new(Resistance11, Reactance11);
        }

        public double Resistance12{ get; set; }
        public double Reactance12{ get; set; }

        [GlmName("z12")]
        public Complex Impedance12
        {
            get => new(Resistance12, Reactance12);
        }

        public double Resistance13{ get; set; }
        public double Reactance13{ get; set; }
        [GlmName("z13")]
        public Complex Impedance13
        {
            get => new(Resistance13, Reactance13);
        }

        public double Resistance21{ get; set; }
        public double Reactance21{ get; set; }
        [GlmName("z21")]
        public Complex Impedance21
        {
            get => new(Resistance21, Reactance21);
        }

        public double Resistance22{ get; set; }
        public double Reactance22{ get; set; }
        [GlmName("z22")]
        public Complex Impedance22
        {
            get => new(Resistance22, Reactance22);
        }

        public double Resistance23{ get; set; }
        public double Reactance23{ get; set; }
        [GlmName("z23")]
        public Complex Impedance23
        {
            get => new(Resistance23, Reactance23);
        }
        public double Resistance31{ get; set; }
        public double Reactance31{ get; set; }
        [GlmName("z31")]
        public Complex Impedance31
        {
            get => new(Resistance31, Reactance31);
        }

        public double Resistance32{ get; set; }
        public double Reactance32{ get; set; }
        [GlmName("z32")]
        public Complex Impedance32
        {
            get => new(Resistance32, Reactance32);
        }

        public double Resistance33{ get; set; }
        public double Reactance33{ get; set; }

        [GlmName("z33")]
        public Complex Impedance33
        {
            get => new(Resistance33, Reactance33);
        }

    }
}
