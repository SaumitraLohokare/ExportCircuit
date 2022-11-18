using System.Numerics;

namespace Prosumergrid.Domain.Entities.BaseClasses
{
    /// <summary>
    /// Switch operating mode.
    /// </summary>
    public enum OperatingMode { Individual, Bank }
    /// <summary>
    /// Base class for all switch elements (switch, sectionalizers, and fuses).
    /// </summary>
    public abstract class SwitchElement : Link
    {
        // input currents

        /// <summary>
        /// Gets or sets the real part in the input current for the phase A.
        /// </summary>
        public double CurrentInAReal { get; set; }
        /// <summary>
        /// Gets or sets the imaginary part in the input current for the phase A.
        /// </summary>
        public double CurrentInAImag { get; set; }

        /// <summary>
        /// Gets the input current in phase A.
        /// </summary>
        public Complex CurrentInA
        {
            get => new(CurrentInAReal, CurrentInAImag);
        }

        /// <summary>
        /// Gets or sets the real part in the input current for the phase B.
        /// </summary>
        public double CurrentInBReal { get; set; }
        /// <summary>
        /// Gets or sets the imaginary part in the input current for the phase B.
        /// </summary>
        public double CurrentInBImag { get; set; }

        /// <summary>
        /// Gets the input current in phase B.
        /// </summary>
        public Complex CurrentInB
        {
            get => new(CurrentInBReal, CurrentInBImag);
        }

        /// <summary>
        /// Gets or sets the real part in the input current for the phase C.
        /// </summary>
        public double CurrentInCReal { get; set; }
        /// <summary>
        /// Gets or sets the imaginary part in the input current for the phase C.
        /// </summary>
        public double CurrentInCImag { get; set; }

        /// <summary>
        /// Gets the input current in phase C.
        /// </summary>
        public Complex CurrentInC
        {
            get => new(CurrentInCReal, CurrentInCImag);
        }

        // output currents

        /// <summary>
        /// Gets or sets the real part in the output current for the phase A.
        /// </summary>
        public double CurrentOutAReal { get; set; }
        /// <summary>
        /// Gets or sets the imaginary part in the output current for the phase A.
        /// </summary>
        public double CurrentOutAImag { get; set; }

        /// <summary>
        /// Gets the output current in phase A.
        /// </summary>
        public Complex CurrentOutA
        {
            get => new(CurrentOutAReal, CurrentOutAImag);
        }

        /// <summary>
        /// Gets or sets the real part in the output current for the phase B.
        /// </summary>
        public double CurrentOutBReal { get; set; }
        /// <summary>
        /// Gets or sets the imaginary part in the output current for the phase B.
        /// </summary>
        public double CurrentOutBImag { get; set; }

        /// <summary>
        /// Gets the output current in phase B.
        /// </summary>
        public Complex CurrentOutB
        {
            get => new(CurrentOutBReal, CurrentOutBImag);
        }

        /// <summary>
        /// Gets or sets the real part in the output current for the phase C.
        /// </summary>
        public double CurrentOutCReal { get; set; }
        /// <summary>
        /// Gets or sets the imaginary part in the output current for the phase C.
        /// </summary>
        public double CurrentOutCImag { get; set; }

        /// <summary>
        /// Gets the output current in phase C.
        /// </summary>
        public Complex CurrentOutC
        {
            get => new(CurrentOutCReal, CurrentOutCImag);
        }

        // input power

        /// <summary>
        /// Gets or sets the input active power in phase A.
        /// </summary>
        public double ActivePowerInA { get; set; }
        /// <summary>
        /// Gets or sets the input reactive power in phase A.
        /// </summary>
        public double ReactivePowerInA { get; set; }

        /// <summary>
        /// Gets the input power in phase A.
        /// </summary>
        public Complex PowerInA
        {
            get => new(ActivePowerInA, ReactivePowerInA);
        }

        /// <summary>
        /// Gets or sets the input active power in phase B.
        /// </summary>
        public double ActivePowerInB { get; set; }
        /// <summary>
        /// Gets or sets the input reactive power in phase B.
        /// </summary>
        public double ReactivePowerInB { get; set; }

        /// <summary>
        /// Gets the input power in phase B.
        /// </summary>
        public Complex PowerInB
        {
            get => new(ActivePowerInB, ReactivePowerInB);
        }

        /// <summary>
        /// Gets or sets the input active power in phase C.
        /// </summary>
        public double ActivePowerInC { get; set; }
        /// <summary>
        /// Gets or sets the input reactive power in phase C.
        /// </summary>
        public double ReactivePowerInC { get; set; }

        /// <summary>
        /// Gets the nput power in phase C.
        /// </summary>
        public Complex PowerInC
        {
            get => new(ActivePowerInC, ReactivePowerInC);
        }

        // output power

        /// <summary>
        /// Gets or sets the output active power in phase A.
        /// </summary>
        public double ActivePowerOutA { get; set; }
        /// <summary>
        /// Gets or sets the output reactive power in phase A.
        /// </summary>
        public double ReactivePowerOutA { get; set; }

        /// <summary>
        /// Gets the output power in phase A.
        /// </summary>
        public Complex PowerOutA
        {
            get => new(ActivePowerOutA, ReactivePowerOutA);
        }

        /// <summary>
        /// Gets or sets the output active power in phase B.
        /// </summary>
        public double ActivePowerOutB { get; set; }
        /// <summary>
        /// Gets or sets the output reactive power in phase B.
        /// </summary>
        public double ReactivePowerOutB { get; set; }

        /// <summary>
        /// Gets the output power in phase B.
        /// </summary>
        public Complex PowerOutB
        {
            get => new(ActivePowerOutB, ReactivePowerOutB);
        }

        /// <summary>
        /// Gets or sets the output active power in phase C.
        /// </summary>
        public double ActivePowerOutC { get; set; }
        /// <summary>
        /// Gets or sets the output reactive power in phase C.
        /// </summary>
        public double ReactivePowerOutC { get; set; }

        /// <summary>
        /// Gets the output power in phase C
        /// </summary>
        public Complex PowerOutC
        {
            get => new(ActivePowerOutC, ReactivePowerOutC);
        }

    }
}
