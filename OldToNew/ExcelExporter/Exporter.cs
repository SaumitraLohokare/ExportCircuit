using Prosumergrid.Domain.Entities.BaseClasses;
using Prosumergrid.Domain.Entities.Circuit;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using Prosumergrid.Infrastructure.Utils;

namespace ProsumerGridExporter.ExcelExporter;

public class Exporter
{
    public static void Export(Feeder feeder, string fileName)
    {
        FileStream? inStream = null;
        HSSFWorkbook workbook;
        if (File.Exists(fileName))
        {
            Console.WriteLine("File Exists, Opening in Append mode...");
            inStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            workbook = new HSSFWorkbook(inStream);
        }
        else
        {
            workbook = new HSSFWorkbook();
        }

        var data = CircuitUtils.GetFeederNavigationProperties<DistributionElement>();
        // for each sheet name we put in the sheet
        foreach (var list in data)
        {
            var key = CircuitUtils.SingularizeProperty(list.Name);
            var sheet = workbook.CreateSheet(key);
            var rowCount = sheet.LastRowNum; // dunno if it will start at 0 or last row num???

            List<String> columnNames;
            if (rowCount > 0)
            {
                var firstRow = sheet.CreateRow(0);
                ExcelUtils.FillColumnNames(key, ref firstRow, out columnNames);
            } else
            {
                columnNames = ExcelUtils.GetColumnNames(key);
            }

            var items = (list.GetValue(feeder) ?? throw new ArgumentNullException("Given list is null"));
            if (items == null) continue;
            Convert.ChangeType(items, list.PropertyType);

            // create rows
            var rowIndex = rowCount + 1;
            foreach (var element in (IEnumerable<Element>)items)
            {
                var row = sheet.CreateRow(rowIndex++);
                ExcelUtils.FillRow(element, columnNames, ref row);
            }
        }

        if (inStream != null)
            inStream.Close();

        var outStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
        workbook.Write(outStream);
    }

    public static void Export(Dictionary<string, IEnumerable<Element>> dict, string fileName)
    {
        var workbook = new XSSFWorkbook();

        foreach (var (key, items) in dict)
        {
            var sheet = workbook.CreateSheet(key);
            var rowCount = sheet.LastRowNum; // dunno if it will start at 0 or last row num???

            List<String> columnNames;
            var firstRow = sheet.CreateRow(0);
            ExcelUtils.FillColumnNames(key, ref firstRow, out columnNames);

            // TODO:
            // Empty column names means sheet cannot be converted yet.. So we skip it
            if (columnNames.Count == 0) continue;
            
            if (items == null) continue;

            // create rows
            var rowIndex = rowCount + 1;
            foreach (var element in items)
            {
                var row = sheet.CreateRow(rowIndex++);
                ExcelUtils.FillRow(element, columnNames, ref row);
            }
        }

        var outStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
        workbook.Write(outStream);
    }
}


