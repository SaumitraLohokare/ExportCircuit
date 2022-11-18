using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;
using System.Numerics;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Describes the electrical connection between the high and low side of the transformer. 
    /// These may be referenced by keyword or number.
    /// </summary>
    public enum TransformerConnectType
    {
        /// <summary>An unknown transformer that will throw an error when used.</summary>
        Unknown,
        /// <summary>A wye to wye connected transformer.</summary>
        WyeWye,
        /// <summary>A delta to delta connected transformer.</summary>
        DeltaDelta,
        /// <summary>A delta to grounded-wye connected transformer.</summary>
        DeltaGwye,
        /// <summary>A single leg of a wye to wye connected transformer.</summary>
        SinglePhase,
        /// <summary>
        /// A single-phase, center-tapped transformer or split-phase transformer.
        /// Used to connect three-phase distribution to triplex-distribution.
        /// </summary>
        SinglePhaseCenterTapped

    }
    
    /// <summary>
    /// Describes the type of transformer the object represents. 
    /// Used for informational purposes only.
    /// Valid types may be referenced by keyword or number
    /// </summary>
    public enum InstallType
    {
        /// <summary>No information on the transformer physical type.</summary>
        Unknown,
        /// <summary>A pole-mounted transformer.</summary>
        Poletop,
        /// <summary>A pad, or ground level transformer.</summary>
        Padmount,
        /// <summary>An enclosed transformer "building," either underground or above ground.</summary>
        Vault
    }

    /// <summary>
    /// The type of coolant used in the transformer. Valid types may be referenced by keyword or number
    /// </summary>
    public enum CoolantType
    {
        /// <summary>An unknown and unknown coolant type that will throw an error when used.</summary>
        Unknown,
        /// <summary>A transformer immersed in mineral oil.</summary>
        MineralOil,
        /// <summary>A transformer with air as its coolant. This type is not handled yet.</summary>
        Dry
    }

    /// <summary>
    /// The type of cooling used in the transformer. Valid types may be referenced by keyword or number
    /// </summary>
    public enum CoolingType
    {
        /// <summary>An unknown and unknown cooling type that will throw an error when used.</summary>
        Unknown,
        /// <summary>A liquid immersed self cooled transformer.</summary>
        OA,
        /// <summary>A forced air cooled liquid immersed transformer.</summary>
        FA,
        /// <summary>A transformer with non-direction forced oil and air flow.</summary>
        NDFOA,
        /// <summary>A transformer with non-direction forced oil and water flow.</summary>
        NDFOW,
        /// <summary>A transformer with direction forced oil and air flow.</summary>
        DFOA,
        /// <summary>A transformer with direction forced oil and water flow.</summary>
        DFOW
    }

    /// <summary>
    /// The transformer configuration object describes the details of a particular transformer implementation. 
    /// It includes information like the power rating, connection type, and nominal voltage on each side.
    /// </summary>
    [GlmName("transformer_configuration")]
    public class TransformerConfiguration : DistributionElement
    {
        [GlmName("connect_type")]
        public TransformerConnectType ConnectType { get; set; }

        [GlmName("no_load_loss")]
        public double NoLoadLoss { get; set; }

        [GlmName("power_rating")]
        public double PowerRating { get; set; }

        [GlmName("reactance_resistance_ratio")]
        public double ReactanceResistanceRatio { get; set; }

        [GlmName("full_load_loss")]
        public double FullLoadLoss { get; set; }

        [GlmName("powerA_rating")]
        public double PowerARating { get; set; }
        [GlmName("powerB_rating")]
        public double PowerBRating { get; set; }
        [GlmName("powerC_rating")]
        public double PowerCRating { get; set; }

        public double ShuntResistance { get; set; }
        public double ShuntReactance { get; set; }

        [GlmName("shunt_impedance")]
        public Complex ShuntImpedance
        {
            get => new(ShuntResistance, ShuntReactance);
        }

        [GlmName("resistance")]
        public double Resistance { get; set; }
        [GlmName("reactance")]
        public double Reactance { get; set; }
        [GlmName("impedance")]
        public Complex Impedance
        {
            get => new(Resistance, Reactance);
        }

        [GlmName("primary_voltage")]
        public double PrimaryVoltage { get; set; }

        [GlmName("secondary_voltage")]
        public double SecondaryVoltage { get; set; }

        [GlmName("core_coil_weight")]
        public double CoreCoilWeight { get; set; }

        [GlmName("installed_insulation_life")]
        public double InstalledInsulationLife { get; set; }

        [GlmName("install_type")]
        public InstallType InstallType { get; set; }

        [GlmName("coolant_type")]
        public CoolantType CoolantType { get; set; }

        [GlmName("cooling_type")]
        public CoolingType CoolingType { get; set; }
    }
}

