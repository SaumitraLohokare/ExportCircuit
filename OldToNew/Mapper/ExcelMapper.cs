using Prosumergrid.Domain.Entities.Circuit;
using Prosumergrid.Domain.Entities.BaseClasses;
using NPOI.SS.UserModel;
using System.Linq.Expressions;
using System.Reflection;
using FileConverter.Utility;

namespace FileConverter.Mapper
{
    public record ColumnInfo(int Index, PropertyInfo PropertyInfo);

    public class ExcelMapper
    {
        public List<Node> MapNodes(ISheet sheet)
        {
            List<Node> nodes = new List<Node>();

            var headerRow = sheet.GetRow(0);

            for (int i = 2; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                var node = new Node();
            }

            return nodes;
        }

        // Code from internship resources
        private Dictionary<string, Action<PropertyInfo, object, object>>? _propertyResolver;
        private Dictionary<string, Action<string, object>>? _readOnlyPropertyResolver;
        private List<string> _ignoredProperties;

        public ExcelMapper(IWorkbook workbook)
        {
            Workbook = workbook ?? throw new ArgumentNullException(nameof(workbook));
            Columns = new Dictionary<string, ColumnInfo>();
            _ignoredProperties = new List<string>();
        }

        public IWorkbook Workbook { get; init; }
        public PropertyInfo[]? TargetProperties { get; private set; }
        public Dictionary<string, ColumnInfo> Columns { get; private set; }

        private string sheetOwner;

        private void GetColumnsInfo(ISheet sheet)
        {
            Columns.Clear();

            var ownerRow = sheet.GetRow(2);
            sheetOwner = ownerRow.GetCell(1).ToString() ?? "No Owner";

            // NOTE: header row is not the first row... find header row
            // Header row in old fprmat is on row 5... So that is first row + 4
            var headerRow = sheet.GetRow(4);

            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                var cell = headerRow.GetCell(i);
                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                {
                    var cellName = cell.ToString();
                    // NOTE: map old cellNames to new Property names
                    var propertyInfo = TargetProperties?
                        //.FirstOrDefault(x => string.Compare(x.Name, cellName, true) == 0); // NOTE: here cell name != property name probably
                        .FirstOrDefault(x => UtilFunctions.CompareNewCellName(x.Name, cellName!));

                    if (propertyInfo == null) continue;

                    // ignored properties
                    if (_ignoredProperties.Contains(propertyInfo.Name)) continue;

                    Columns[cellName!] = new ColumnInfo(i, propertyInfo);
                }
            }
        }

        /// <summary>
        /// Performs the type conversion for each value in the columns.
        /// </summary>
        /// <param name="value">Cell value</param>
        /// <param name="type">Target type</param>
        /// <returns>Converted value</returns>
        private object? ConvertValue(object value, Type type)
        {
            // if it doesn't implement IConvertible
            if (!type.GetInterfaces().Contains(typeof(IConvertible)))
                return value;

            // if is an enumerator
            if (type.IsEnum && value.GetType() == typeof(string))
            {
                if (type.Name == "LoadClass")
                {
                    var strValue = (string)value;
                    return strValue switch
                    {
                        "Residential" => LoadClass.R,
                        "NonResidential" => LoadClass.C,
                        _ => LoadClass.U
                    };
                }
                var convertedResult = Enum.TryParse(type, (string)value, true, out var convertedValue);
                if (!convertedResult) return Activator.CreateInstance(type);// throw new Exception("Failed to convert value of : " + (string)value + " to type " + type.Name); // QUESTION: Should it not fail?
                    return convertedValue;
            }

            var result = Convert.ChangeType(value, type);
            if (result == null) return Activator.CreateInstance(type);

            return result;
        }

        // Take OverHeadLineConfigurations
        public IEnumerable<OverheadLineConfiguration> TakeOverheadLineConfigurations()
        {
            var sheet = Workbook.GetSheet("line_configuration");
            var elements = new List<OverheadLineConfiguration>();

            if (sheet == null) return elements;

            TargetProperties = typeof(OverheadLineConfiguration).GetProperties();

            GetColumnsInfo(sheet);

            // get Conductors column index
            int[] conductorColumnIndices = { -1, -1, -1, -1 };
            int conductorIndex = 0;
            foreach (var cell in sheet.GetRow(4).Cells)
            {
                if (cell == null) continue;
                if (cell.ToString()!.StartsWith("conductor"))
                {
                    conductorColumnIndices[conductorIndex++] = cell.ColumnIndex;
                }
            }

            for (int i = (sheet.FirstRowNum + 1 + 4); i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                // need to check Conductor A, B, C, N here for name starting with "oh_"
                var isOverheadLineConfiguration = false;
                foreach (var conductorI in conductorColumnIndices)
                {
                    if (conductorI < 0) continue;

                    var conductorCell = row.GetCell(conductorI);
                    if (conductorCell == null) continue;

                    var conductorName = conductorCell.ToString();
                    if (conductorName == null || string.IsNullOrEmpty(conductorName)) continue;
                    isOverheadLineConfiguration |= conductorName.StartsWith("oh_");
                }

                if (!isOverheadLineConfiguration) continue;

                var element = SetTargetProperties<OverheadLineConfiguration>(row);

                if (element != null)
                {
                    // TODO: Discuss
                    /* var idProp = element.GetType().GetProperty("Id");
                     if (idProp != null && idProp.GetValue(element) != null && idProp.GetValue(element)!.Equals(Guid.Empty))
                     { // If Id property exists.. and is not null.. and is all zeros... Create a new Guid
                         idProp.SetValue(element, Guid.NewGuid());
                     }*/

                    // HACK
                    if (element.GetType().IsAssignableTo(typeof(DistributionElement)))
                    {
                        if (element.Feeder == null)
                        {
                            element.Feeder = new Feeder();
                            element.Feeder.Name = sheetOwner;
                        }
                    }

                    elements.Add(element);
                }
            }

            return elements;
        }

        // Take UndergroundLineConfigurations
        public IEnumerable<UndergroundLineConfiguration> TakeUndergroundLineConfigurations()
        {
            var sheet = Workbook.GetSheet("line_configuration");
            var elements = new List<UndergroundLineConfiguration>();

            if (sheet == null) return elements;

            TargetProperties = typeof(UndergroundLineConfiguration).GetProperties();

            GetColumnsInfo(sheet);

            // get Conductors column index
            int[] conductorColumnIndices = { -1, -1, -1, -1 };
            int conductorIndex = 0;
            foreach (var cell in sheet.GetRow(4).Cells)
            {
                if (cell == null) continue;
                if (cell.ToString()!.StartsWith("conductor"))
                {
                    conductorColumnIndices[conductorIndex++] = cell.ColumnIndex;
                }
            }

            for (int i = (sheet.FirstRowNum + 1 + 4); i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                // need to check Conductor A, B, C, N here for name starting with "oh_"
                var isOverheadLineConfiguration = false;
                foreach (var conductorI in conductorColumnIndices)
                {
                    if (conductorI < 0) continue;

                    var conductorCell = row.GetCell(conductorI);
                    if (conductorCell == null) continue;

                    var conductorName = conductorCell.ToString();
                    if (conductorName == null || string.IsNullOrEmpty(conductorName)) continue;
                    isOverheadLineConfiguration |= conductorName.StartsWith("oh_");
                }

                if (isOverheadLineConfiguration) continue;

                var element = SetTargetProperties<UndergroundLineConfiguration>(row);

                if (element != null)
                {
                    // TODO: Discuss
                    /* var idProp = element.GetType().GetProperty("Id");
                     if (idProp != null && idProp.GetValue(element) != null && idProp.GetValue(element)!.Equals(Guid.Empty))
                     { // If Id property exists.. and is not null.. and is all zeros... Create a new Guid
                         idProp.SetValue(element, Guid.NewGuid());
                     }*/
                    
                    if (element.GetType().IsAssignableTo(typeof(DistributionElement)))
                    {
                        if (element.Feeder == null)
                        {
                            element.Feeder = new Feeder();
                            element.Feeder.Name = sheetOwner;
                        }
                    }

                    elements.Add(element);
                }
            }

            return elements;
        }

        public IEnumerable<TTarget> Take<TTarget>(string sheetName) where TTarget : DistributionElement
        {
            var sheet = Workbook.GetSheet(sheetName);
            var elements = new List<TTarget>();

            if (sheet == null) return elements;

            TargetProperties = typeof(TTarget).GetProperties();

            GetColumnsInfo(sheet);

            // for each sheet
            // NOTE: +4 because we need to skip 4 rows after first row to get to the header row
            for (int i = (sheet.FirstRowNum + 1 + 4); i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                var element = SetTargetProperties<TTarget>(row);

                if (element != null)
                {
                    // TODO: Discuss
                    /* var idProp = element.GetType().GetProperty("Id");
                     if (idProp != null && idProp.GetValue(element) != null && idProp.GetValue(element)!.Equals(Guid.Empty))
                     { // If Id property exists.. and is not null.. and is all zeros... Create a new Guid
                         idProp.SetValue(element, GuiI.NewGuid());
                     }*/

                    // HACK... should probably find the actual feeder from the results and set that as the feeder for this element...
                    // if feeder is empty, take it from the header of the sheet
                    if (element.GetType().IsAssignableTo(typeof(DistributionElement)))
                    {
                        if (element.Feeder == null)
                        {
                            element.Feeder = new Feeder();
                            element.Feeder.Name = sheetOwner;
                        }
                    }

                    elements.Add(element);
                }
            }

            return elements;

        }

        public IEnumerable<TTarget> TakeFeeder<TTarget>(string sheetName) where TTarget : class
        {
            var sheet = Workbook.GetSheet(sheetName);
            var elements = new List<TTarget>();

            if (sheet == null) return elements;

            TargetProperties = typeof(TTarget).GetProperties();

            GetColumnsInfo(sheet);

            // for each sheet
            // NOTE: +4 because we need to skip 4 rows after first row to get to the header row
            for (int i = (sheet.FirstRowNum + 1 + 4); i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                var element = SetTargetProperties<TTarget>(row);

                if (element != null)
                {
                    // TODO: Discuss
                    /* var idProp = element.GetType().GetProperty("Id");
                     if (idProp != null && idProp.GetValue(element) != null && idProp.GetValue(element)!.Equals(Guid.Empty))
                     { // If Id property exists.. and is not null.. and is all zeros... Create a new Guid
                         idProp.SetValue(element, GuiI.NewGuid());
                     }*/

                    elements.Add(element);
                }
            }

            return elements;

        }

        /// <summary>
        /// Set target object properties.
        /// </summary>
        /// <typeparam name="TTarget">Type of the target object.</typeparam>
        /// <param name="row">Excel row</param>
        /// <returns>Target object with properties populated.</returns>
        private TTarget SetTargetProperties<TTarget>(IRow row)
        {
            var targetObject = Activator.CreateInstance(typeof(TTarget));

            // for each object properties
            foreach (var column in Columns)
            {
                var property = column.Value.PropertyInfo;
                var type = property.PropertyType;
                var index = column.Value.Index;
                var cellValue = row.GetCell(index)?.ToString();

                if (cellValue == null) continue;

                var convertedValue = ConvertValue(cellValue, type);

                if (property != null && convertedValue != null)
                    ResolveValue<TTarget>(property, convertedValue, (TTarget)targetObject!);
            }
            return (TTarget)targetObject!;

        }

        public ExcelMapper MapColumn(string columnName,
            Action<PropertyInfo, object, object> resolver)
        {
            if (_propertyResolver == null)
                _propertyResolver = new();

            _propertyResolver[columnName] = resolver;
            return this;
        }

        public ExcelMapper MapReadOnlyColumn(string columnName,
            Action<string, object> resolver)
        {
            if (_readOnlyPropertyResolver == null)
                _readOnlyPropertyResolver = new();

            _readOnlyPropertyResolver[columnName] = resolver;
            return this;
        }

        public ExcelMapper Ignore<TTarget>(Expression<Func<TTarget, object>> propertySelector)
        {
            MemberExpression? expression = null;

            //this line is necessary, because sometimes the expression comes in as Convert(originalexpression)
            if (propertySelector.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)propertySelector.Body;
                if (unaryExpression.Operand is MemberExpression)
                {
                    expression = (MemberExpression)unaryExpression.Operand;
                }
                else
                    throw new ArgumentException();
            }
            else if (propertySelector.Body is MemberExpression)
            {
                expression = (MemberExpression)propertySelector.Body;
            }
            else
            {
                throw new ArgumentException();
            }

            var propertyInfo = (PropertyInfo)expression.Member;
            var propertyName = propertyInfo.Name;

            _ignoredProperties.Add(propertyName);

            return this;
        }

        /// <summary>
        /// Method that resolves value assignment to target properties.
        /// </summary>
        /// <typeparam name="TTarget">Type of the target object.</typeparam>
        /// <param name="property">Property information.</param>
        /// <param name="value">Value to be assigned to property.</param>
        /// <param name="target">Target object.</param>
        private void ResolveValue<TTarget>(PropertyInfo property, object value, TTarget target)
        {
            if (property.CanWrite)
            {
                // specific property resolver has priority
                if (_propertyResolver != null && target != null)
                {
                    var propertyName = property.Name;
                    bool resolverExists = _propertyResolver.TryGetValue(propertyName, out var resolver);

                    if (resolverExists && resolver != null)
                    {
                        resolver(property, value, target);
                        return;
                    }
                }

                // in case there is no custom resolver;
                property.SetValue(target, value, null);
                return;
            }

            if (_readOnlyPropertyResolver != null && target != null)
            {
                var propertyName = property.Name;
                bool resolverExists = _readOnlyPropertyResolver.TryGetValue(propertyName, out var resolver);

                if (resolverExists && resolver != null)
                {
                    resolver((string)value, target);
                    return;
                }
            }

        }
    }
}
