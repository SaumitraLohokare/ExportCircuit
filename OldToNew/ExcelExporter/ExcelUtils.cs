using System.Numerics;
using Prosumergrid.Domain.Entities.BaseClasses;
using NPOI.SS.UserModel;
using Prosumergrid.Infrastructure.Utils;
using FileConverter.Extensions;

namespace ProsumerGridExporter.ExcelExporter;

public class ExcelUtils
{
    private static HashSet<string> fieldsToSkip = new () {
        "Id", // NOTE: Temporarily added Id to skip until web app supports it
        "Susceptance",
        "Resistance",
        "Reactance",
        // also other r11 ... r33 properties
        "Reactance11",
        "Resistance11",
        "Reactance12",
        "Resistance12",
        "Reactance13",
        "Resistance13",
        "Reactance13",
        "Resistance13",
        "Reactance21",
        "Resistance21",
        "Reactance22",
        "Resistance22",
        "Reactance23",
        "Resistance23",
        "Reactance31",
        "Resistance31",
        "Reactance32",
        "Resistance32",
        "Reactance33",
        "Resistance33",

        "FeederId",
        "NodeType",
        "PrimaryPhases",
        "FromId",
        "ToId",
        "Scenario",
        "ScenarioId",
        "NumPhases",
        "PrimaryPhases",
        "ParentNodeId",
        "LineSpacingId",
        "ConductorAId",
        "ConductorBId",
        "ConductorCId",
        "ConductorNId",
        "ConfigurationId",
        "SenseNodeId",
        "RemoteSenseId",
        "RemoteSenseBId"
    };

    public static List<string> GetColumnNames(string name)
    {
        var columnNames = new List<string>();
        var type = CircuitUtils.GetClassType(name);

        // return empty cuz we dont need sheet??????
        if (type == null) return columnNames;

        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            if (property.PropertyType.IsGenericType && (property.PropertyType.GetGenericTypeDefinition() == typeof(List<>)))
                continue;

            if (fieldsToSkip.Contains(property.Name))
                continue;

            if (property.Name.Contains("Reactance")) Console.WriteLine("Something failed...");
            columnNames.Add(property.Name);
        }

        columnNames.MoveToTop("Feeder");
        //columnNames.MoveToTop("Phases");
        //columnNames.MoveToTop("ParentNode");
        columnNames.MoveToTop("GroupId");
        columnNames.MoveToTop("Name");
        //columnNames.MoveToTop("Id");

        return columnNames;
    }

    public static void FillColumnNames(string name, ref IRow row, out List<String> columnNames)
    {
        columnNames = new List<string>();
        var type = CircuitUtils.GetClassType(name);

        // return empty cuz we dont need sheet??????
        if (type == null) return;

        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            if (property.PropertyType.IsGenericType && (property.PropertyType.GetGenericTypeDefinition() == typeof(List<>)))
                continue;

            if (fieldsToSkip.Contains(property.Name))
                continue;

            columnNames.Add(property.Name);
        }

        columnNames.MoveToTop("GroupId");
        columnNames.MoveToTop("Name");
        columnNames.MoveToTop("Id");

        var cellIndex = 0;
        foreach (var column in columnNames)
        {
            var cell = row.CreateCell(cellIndex++);
            cell.SetCellValue(column);
            
        }
    }

    public static void FillRow<E>(E element, List<string> columnNames, ref IRow row) where E : Element
    {
        var cellIndex = 0;
        foreach (var propertyName in columnNames)
        {
            var cell = row.CreateCell(cellIndex++);
            var property = element.GetType().GetProperty(propertyName);

            if (property == null)
            {
                cell.SetCellValue("");
                // NOTE: Debugging, later uncomment
                throw new NotImplementedException($"Skipping property {propertyName} as it does not exist in {element.GetType().Name}");
                //Console.WriteLine($"Skipping property {propertyName} as it does not exist in {element.GetType().Name}");
                //continue;
            }
            if (property.PropertyType.IsAssignableTo(typeof(Element)))
            {
                // Found Element...
                var val = (Element?)property.GetValue(element);
                if (val == null)
                {
                    cell.SetCellValue("");
                }
                else
                {
                    cell.SetCellValue(val.Name);
                }
                continue;
            }
            if (property.PropertyType == typeof(Complex))
            {
                var complexValue = (Complex)(property.GetValue(element) ?? new Complex());
                //if (complexValue.Real != 0 && complexValue.Imaginary != 0)
                    cell.SetCellValue($"{complexValue.Real}+{complexValue.Imaginary}j");
                continue;
            }
            if (property.PropertyType.IsEnum)
            {
                cell.SetCellValue((property.GetValue(element) ?? "").ToString()!.ToUpper());
                continue;
            }
            // TODO: Discuss
            //if (IsNumericType(property.PropertyType))
            //{
            //    var value = property.GetValue(element);
            //    if (IsNotDefault(value ?? 0) || property.Name.Contains("TapPos") || property.Name.Contains("NominalVoltage"))
            //    {
            //        cell.SetCellValue((value ?? "").ToString());
            //    }
            //    else
            //    {
            //        cell.SetCellValue("");
            //    }
            //    continue;
            //}
            cell.SetCellValue((property.GetValue(element) ?? "").ToString());
        }
    }

    private static bool IsNumericType(Type t)
    {
        HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(int),  typeof(double),  typeof(decimal),
            typeof(long), typeof(short),   typeof(sbyte),
            typeof(byte), typeof(ulong),   typeof(ushort),
            typeof(uint), typeof(float)
        };

        return NumericTypes.Contains(Nullable.GetUnderlyingType(t) ?? t);
    }

    private static bool IsNotDefault(object o)
    {
        Dictionary<Type, object> NumericTypes = new Dictionary<Type, object>
        {
            { typeof(int), default(int) },
            { typeof(double), default(double) },
            { typeof(decimal), default(decimal) },
            { typeof(long), default(long) },
            { typeof(short), default(short) },
            { typeof(sbyte), default(sbyte) },
            { typeof(byte), default(byte) },
            { typeof(ulong), default(ulong) },
            { typeof(ushort), default(ushort) },
            { typeof(uint), default(uint) },
            { typeof(float), default(float) },
        };
        Type t = o.GetType();
        bool found = NumericTypes.TryGetValue(Nullable.GetUnderlyingType(t) ?? t, out var value);
        if (!found) throw new ArgumentOutOfRangeException("Not a numeric type.");

        return !object.Equals(value, o);
    }
}

