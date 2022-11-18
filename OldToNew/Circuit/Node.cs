using Prosumergrid.Domain.Attributes;
using Prosumergrid.Domain.Entities.BaseClasses;

namespace Prosumergrid.Domain.Entities.Circuit
{
    public enum NodeType { Node, Meter }
    public enum BusType
    { 
        /// <summary>For a constant power bus (default)</summary>
        PQ,
        /// <summary>For a voltage-controlled (magnitude) bus</summary>
        PV,
        /// <summary>For the infinite bus of a system.</summary>
        Swing,
    }

    /// <summary>
    /// The node object is equivalent to a bus of the distribution system. 
    /// It provides a connection point for link-based objects and a point of known voltages on the system. 
    /// </summary>
    [GlmName("node")]
    public class Node : DisplayableElement
    {
        private static readonly double[] latLongConversion = { 1394647.502, 15291348.34, -75.9059, 42.0958, 1393832.688, 15294013.54, -75.909, 42.1031 };

        private double _latitude;
        public double Latitude { 
            get {
                return GetConvertedLatitude();
            }
            set
            {
                _latitude = value;
            }
        }

        private double _longitude;
        public double Longitude { 
            get
            {
                return GetConvertedLongitude();
            }
            set
            {
                _longitude = value;
            }
        }
        public NodeType NodeType { get; set; }
        
        [GlmName("bustype")]
        public BusType BusType { get; set; }

        [GlmName("nominal_voltage")]
        public double NominalVoltage { get; set; }
        public bool ConsistentPath { get; set; }
        public double DistToSlack { get; set; }

        // navigation properties
        public virtual List<Capacitor> Capacitors { get; set; } = new();
        public virtual List<Load> Loads { get; set; } = new();

        // DERs
        public virtual List<Generator> Generators { get; set; } = new();
        public virtual List<Solar> Solars { get; set; } = new();
        public virtual List<Storage> Storages { get; set; } = new();
        public virtual List<HeatPump> HeatPumps { get; set; } = new();
        public virtual List<EVChargingStation> EVChargingStations { get; set; } = new();

        public double GetConvertedLatitude()
        {
            var point1Y = latLongConversion[1];
            var point1Lat = latLongConversion[3];

            var point2Y = latLongConversion[5];
            var point2Lat = latLongConversion[7];

            var latConversionFactor = (_latitude - point1Y) * Math.Abs(point2Lat - point1Lat) / Math.Abs(point2Y - point1Y);

            var latitude = point1Lat + latConversionFactor;

            if (latitude > 90)
                throw new FormatException($"Node {Name} latitude can't be greater than 90 deg.");

            if (latitude < -90)
                throw new FormatException($"Node {Name} latitude can't be less than -90 deg.");

            return latitude;
        }

        public double GetConvertedLongitude()
        {
            var point1X = latLongConversion[0];
            var point1Lon = latLongConversion[2];

            var point2X = latLongConversion[4];
            var point2Lon = latLongConversion[6];

            var lonConversionFactor = (_longitude - point1X) * Math.Abs(point2Lon - point1Lon) / Math.Abs(point2X - point1X);

            var longitude = point1Lon + lonConversionFactor;

            if (longitude > 180)
                throw new FormatException($"Node {Name} longitud can't be greater than 180 deg.");

            if (longitude < -180)
                throw new FormatException($"Node {Name} longitud can't be less than -180 deg.");

            return longitude;
        }

    }
}
