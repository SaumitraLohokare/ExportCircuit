using Prosumergrid.Domain.Entities.BaseClasses;
using Prosumergrid.Domain.Entities.Circuit;
using System.Reflection;

namespace Prosumergrid.Infrastructure.Utils;

/// <summary>
/// Utility methods to perform circuit common tasks
/// </summary>
public static class CircuitUtils
{
    /// <summary>
    /// Gets all feeder navigation properties.
    /// </summary>
    /// <typeparam name="T">Type that filters elements by type.</typeparam>
    /// <returns>Enumerable of property info objects.</returns>
    public static IEnumerable<PropertyInfo> GetFeederNavigationProperties<T>()
    {
        var elementProperties = typeof(Feeder).GetProperties()
            .Where(p =>
            {
                var generics = p.PropertyType.GenericTypeArguments;
                if (generics.Length > 0)
                {
                    var generic = generics[0];
                    if (generic.IsSubclassOf(typeof(T))) return true;
                }
                return false;
            });
        return elementProperties;
    }

    /// <summary>
    /// Singularizes a type name. ie: "Nodes" => "Node"  
    /// </summary>
    /// <param name="typeName">Type name in plural.</param>
    /// <returns></returns>
    public static string SingularizeProperty(string typeName)
    {
        var lastCharIdx = typeName.Length - 1;

        // if the name does not have an s at the end then we consider it singular
        if (typeName[lastCharIdx] != 's')
            return typeName;

        var singularizedName = typeName.Remove(lastCharIdx);
        if (singularizedName == "Switche") singularizedName = "Switch";

        return singularizedName;
    }

    public static Type? GetClassType(string name)
    {
        return name switch
        {
        "Node" => typeof(Node),
        "Link" => typeof(Link),
        "OverheadLine" => typeof(OverheadLine),
        "UndergroundLine" => typeof(UndergroundLine),
        "OverheadLineConfiguration" => typeof(OverheadLineConfiguration),
        "UndergroundLineConfiguration" => typeof(UndergroundLineConfiguration),
        "LineSpacing" => typeof(LineSpacing),
        "OverheadLineConductor" => typeof(OverheadLineConductor),
        "UndergroundLineConductor" => typeof(UndergroundLineConductor),
        "Transformer" => typeof(Transformer),
        "TransformerConfiguration" => typeof(TransformerConfiguration),
        "Load" => typeof(Load),
        "Regulator" => typeof(Regulator),
        "RegulatorConfiguration" => typeof(RegulatorConfiguration),
        "Capacitor" => typeof(Capacitor),
        "Fuse" => typeof(Fuse),
        "Switch" => typeof(Switch),
        "Recloser" => typeof(Recloser),
        "Sectionalizer" => typeof(Sectionalizer),
        "Generator" => typeof(Generator),
        "Solar" => typeof(Solar),
        "Storage" => typeof(Storage),
        "HeatPump" => typeof(HeatPump),
        "EVChargingStation" => typeof(EVChargingStation),
        "Feeder" => typeof(Feeder),
        var x => throw new NotImplementedException($"ERROR: {x} does not have a class mapping.")//typeof(Exception) // null
        };
    }
}


