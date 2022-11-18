
namespace Prosumergrid.Domain.Entities.BaseClasses
{
    public abstract class EnergyStorageElement : DERElement
    {
        public double ChargeEfficiency { get; set; }
        public double DischargeEfficiency { get; set; }

        public bool CommitmentStatus { get; set; }
        public int ConstructionTime { get; set; }
        public double CalendarLife { get; set; }
        public int YearOfInstallation { get; set; }

        /// <summary>
        /// DOD
        /// </summary>
        public decimal DepthOfDischarge { get; set; } = new();

        //kvar
        public double ReactivePower { get; set; }
        // kvarA
        public double ReactivePowerA { get; set; }
        // kvarB
        public double ReactivePowerB { get; set; }
        // kvarC
        public double ReactivePowerC { get; set; }

        // kW
        public double ActivePower { get; set; }
        // kwA
        public double ActivePowerA { get; set; }
        // kwB
        public double ActivePowerB { get; set; }
        // kwC
        public double ActivePowerC { get; set; }

        // kwh
        public double Energy { get; set; }
        // max_kwh
        public double MaxEnergy { get; set; }


        public double PowerCharge { get; set; }
        public double PowerDischarge { get; set; }

        public double RampUpLimit { get; set; }
        public double RampDownLimit { get; set; }

    }
}
