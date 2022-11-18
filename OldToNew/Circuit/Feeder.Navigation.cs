// using Prosumergrid.Domain.Entities.ProjectManagement;

namespace Prosumergrid.Domain.Entities.Circuit
{
    /// <summary>
    /// Electrical feeder circuit.
    /// </summary>
    public partial class Feeder
    {
        // navigation properties

        // public virtual Scenario Scenario { get; set; } = default!;
        public virtual List<Node> Nodes { get; set; } = new();

        // switches

        public virtual List<Switch> Switches { get; set; } = new();

        public virtual List<Sectionalizer> Sectionalizers { get; set; } = new();

        // lines

        public virtual List<LineSpacing> LineSpacings { get; set; } = new();

        public virtual List<OverheadLine> OverheadLines { get; set; } = new();

        public virtual List<OverheadLineConductor> OverheadLineConductors { get; set; } = new();

        public virtual List<OverheadLineConfiguration> OverheadLineConfigurations { get; set; } = new();

        public virtual List<UndergroundLine> UndergroundLines { get; set; } = new();

        public virtual List<UndergroundLineConductor> UndergroundLineConductors { get; set; } = new();

        public virtual List<UndergroundLineConfiguration> UndergroundLineConfigurations { get; set; } = new();

        // shunts
        public virtual List<Capacitor> Capacitors { get; set; } = new();
        public virtual List<Load> Loads { get; set; } = new();

        // transformer
        public virtual List<Transformer> Transformers { get; set; } = new();
        public virtual List<TransformerConfiguration> TransformerConfigurations { get; set; } = new();

        // regulator
        public virtual List<Regulator> Regulators { get; set; } = new();
        public virtual List<RegulatorConfiguration> RegulatorConfigurations { get; set; } = new();

        // recloser
        public virtual List<Recloser> Reclosers { get; set; } = new();

        // fuses
        public virtual List<Fuse> Fuses { get; set; } = new();

        // DER
        public virtual List<Generator> Generators { get; set; } = new();
        public virtual List<Solar> Solars { get; set; } = new();
        public virtual List<Storage> Storages { get; set; } = new();
        public virtual List<HeatPump> HeatPumps { get; set; } = new();
        public virtual List<EVChargingStation> EVChargingStations { get; set; } = new();

    }
}
