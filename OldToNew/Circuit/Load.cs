using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;
using System.Numerics;

namespace Prosumergrid.Domain.Entities.Circuit
{

    /// <summary>
    /// Describes the type of load the object represents. 
    /// Used for informational purposes only.
    /// </summary>
    public enum LoadClass
    {
        /// <summary>Unknown load type.</summary>
        U,
        /// <summary>Residential load</summary>
        R,
        /// <summary>Commercial load</summary>
        C,
        /// <summary>Industrial load</summary>
        I,
        /// <summary>Agricultural load</summary>
        A
    }

    /// <summary>
    /// Electric load element
    /// </summary>
    [GlmName("load")]
    public class Load : Shunt
    {
        public int PriorityLevel { get; set; }

        [GlmName("load_class")]
        public LoadClass LoadClass { get; set; }

        /// <summary>
        /// Gets or sets the base reactive power A. (kVAr)
        /// </summary>
        public double BaseReactivePowerA { get; set; }
        /// <summary>
        /// Gets or sets the base reactive power B. (kVAr)
        /// </summary>
        public double BaseReactivePowerB { get; set; }
        /// <summary>
        /// Gets or sets the base reactive power C. (kVAr)
        /// </summary>
        public double BaseReactivePowerC { get; set; }

        /// <summary>
        /// Gets or sets the base active power A. (kW).
        /// </summary>
        public double BaseActivePowerA { get; set; }
        /// <summary>
        /// Gets or sets the base active power B. (kW).
        /// </summary>
        public double BaseActivePowerB { get; set; }
        /// <summary>
        /// Gets or sets the base active power C. (kW).
        /// </summary>
        public double BaseActivePowerC { get; set; }

        /// <summary>
        /// Gets or sets the base power A.
        /// </summary>
        [GlmName("base_power_A")]
        public double BasePowerA { get; set; }
        /// <summary>
        /// Gets or sets the base power B.
        /// </summary>
        [GlmName("base_power_B")]
        public double BasePowerB { get; set; }
        /// <summary>
        /// Gets or sets the base power C.
        /// </summary>
        [GlmName("base_power_C")]
        public double BasePowerC { get; set; }

        public double ConstantCurrentAReal { get; set; }
        public double ConstantCurrentAImag { get; set; }
        [GlmName("constant_current_A")]
        public Complex ConstantCurrentA
        {
            get => new(ConstantCurrentAReal, ConstantCurrentAImag);
        }

        public double ConstantCurrentBReal { get; set; }
        public double ConstantCurrentBImag { get; set; }
        [GlmName("constant_current_B")]
        public Complex ConstantCurrentB
        {
            get => new(ConstantCurrentBReal, ConstantCurrentBImag);
        }

        public double ConstantCurrentCReal { get; set; }
        public double ConstantCurrentCImag { get; set; }

        [GlmName("constant_current_C")]
        public Complex ConstantCurrentC
        {
            get => new(ConstantCurrentCReal, ConstantCurrentCImag);
        }

        public double ConstantResistanceA { get; set; }
        public double ConstantReactanceA { get; set; }

        [GlmName("constant_impedance_A")]
        public Complex ConstantImpedanceA
        {
            get => new(ConstantResistanceA, ConstantReactanceA);
        }

        public double ConstantResistanceB { get; set; }
        public double ConstantReactanceB { get; set; }

        [GlmName("constant_impedance_B")]
        public Complex ConstantImpedanceB
        {
            get => new(ConstantResistanceB, ConstantReactanceB);
        }

        public double ConstantResistanceC { get; set; }
        public double ConstantReactanceC { get; set; }

        [GlmName("constant_impedance_C")]
        public Complex ConstantImpedanceC
        {
            get => new(ConstantResistanceC, ConstantReactanceC);
        }

        public double ConstantActivePowerA { get; set; }
        public double ConstantReactivePowerA { get; set; }

        [GlmName("constant_power_A")]
        public Complex ConstantPowerA
        {
            get => new(ConstantActivePowerA, ConstantReactivePowerA);
        }

        public double ConstantActivePowerB { get; set; }
        public double ConstantReactivePowerB { get; set; }

        [GlmName("constant_power_B")]
        public Complex ConstantPowerB
        {
            get => new(ConstantActivePowerB, ConstantReactivePowerB);
        }

        public double ConstantActivePowerC { get; set; }
        public double ConstantReactivePowerC { get; set; }

        [GlmName("constant_power_C")]
        public Complex ConstantPowerC
        {
            get => new(ConstantActivePowerC, ConstantReactivePowerC);
        }

        [GlmName("current_fraction_A")]
        public double CurrentFractionA { get; set; }

        [GlmName("current_fraction_B")]
        public double CurrentFractionB { get; set; }

        [GlmName("current_fraction_C")]
        public double CurrentFractionC { get; set; }


        [GlmName("current_pf_A")]
        public double CurrentPfA { get; set; }
        [GlmName("current_pf_B")]
        public double CurrentPfB { get; set; }
        [GlmName("current_pf_C")]
        public double CurrentPfC { get; set; }


        [GlmName("impedance_fraction_A")]
        public double ImpedanceFractionA { get; set; }
        [GlmName("impedance_fraction_B")]
        public double ImpedanceFractionB { get; set; }
        [GlmName("impedance_fraction_C")]
        public double ImpedanceFractionC { get; set; }


        [GlmName("impedance_pf_A")]
        public double ImpedancePfA { get; set; }
        [GlmName("impedance_pf_B")]
        public double ImpedancePfB { get; set; }
        [GlmName("impedance_pf_C")]
        public double ImpedancePfC { get; set; }


        [GlmName("power_fraction_A")]
        public double PowerFractionA { get; set; }
        [GlmName("power_fraction_B")]
        public double PowerFractionB { get; set; }
        [GlmName("power_fraction_C")]
        public double PowerFractionC { get; set; }


        [GlmName("power_pf_A")]
        public double PowerPfA { get; set; }
        [GlmName("power_pf_B")]
        public double PowerPfB { get; set; }
        [GlmName("power_pf_C")]
        public double PowerPfC { get; set; }

        // dem_resp_lb
        public double DemandResponseLowerBound { get; set; }
        // dem_resp_shortage_cost
        public decimal DemandResponseShortageCost { get; set; }
        // dem_resp_surplus_cost
        public decimal DemandResponseSurplusCost { get; set; }
        // dem_resp_ub
        public double DemandResponseUpperBound { get; set; }

        // kw
        public double ActivePowerA { get; set; }
        public double ActivePowerB { get; set; }
        public double ActivePowerC { get; set; }

        // kvar

        public double ReactivePowerA { get; set; }
        public double ReactivePowerB { get; set; }
        public double ReactivePowerC { get; set; }


    }
}
