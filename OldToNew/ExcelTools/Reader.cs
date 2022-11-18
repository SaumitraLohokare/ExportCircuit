using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Prosumergrid.Domain.Entities.BaseClasses;
using Prosumergrid.Domain.Entities.Circuit;
using System.Reflection;
using FileConverter.Mapper;
using FileConverter.Utility;
using System.Numerics;
using System.Text.RegularExpressions;
using FileConverter.Extensions;

namespace ExcelTools
{
    public class Reader
    {
        /// <summary>
        /// Given a path, tries to read a book and returns it.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Nullable IWorkbook</returns>
        public static IWorkbook? ReadWorkbook(string path)
        {
            IWorkbook? book = null;

            try
            {
                FileStream fs = File.OpenRead(path);
                try
                {
                    book = new XSSFWorkbook(fs);
                }
                catch { }

                if (book == null)
                {
                    book = new HSSFWorkbook(fs);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return null;
            }
            return book;
        }
        
        /// <summary>
        /// Reads all the sheets of the IWorkbook into a List and returns it.
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public List<ISheet> GetSheets(IWorkbook book)
        {
            if (book == null) return new List<ISheet>();

            List<ISheet> sheets = new List<ISheet>();
            for (int i = 0; i < book.NumberOfSheets; i++)
            {
                sheets.Add(book.GetSheetAt(i));
            }
            return sheets;
        }

        private ExcelMapper? _mapper;
        private Guid _scenarioId;

        public Dictionary<string, IEnumerable<Element>> Results { get; private set; } = new();

        public Dictionary<string, IEnumerable<Element>> ReadData(IWorkbook book, Guid scenarioId)
        {
            if (book == null) throw new ArgumentNullException("book is null");

            ConfigureMapper(book);

            _scenarioId = scenarioId;

            Results["Feeder"] = TakeFeeders(); // NOTE: change Feeder to dcircuit in TakeFeeders

            // gets all circuit classes
            var classes = typeof(Feeder).Assembly.GetTypes()
                .Where(t =>
                    t.Namespace == "Prosumergrid.Domain.Entities.Circuit"
                    && t.BaseType?.FullName != "System.Enum"
                    && t.Name != "Feeder")
                .ToList();

            // move dependant classes to the end
            classes.MoveToTop(typeof(Node));
            classes.MoveToBottom(typeof(Regulator));
            classes.MoveToBottom(typeof(Transformer));
            classes.MoveToBottom(typeof(OverheadLine));
            classes.MoveToBottom(typeof(UndergroundLine));

            foreach (var elementType in classes)
            {
                if (elementType == typeof(UndergroundLineConfiguration))
                {
                    Results["UndergroundLineConfiguration"] = _mapper!.TakeUndergroundLineConfigurations();
                    continue;
                }

                if (elementType == typeof(OverheadLineConfiguration))
                {
                    Results["OverheadLineConfiguration"] = _mapper!.TakeOverheadLineConfigurations();
                    continue;
                }

                // gets the take elements method
                MethodInfo? takeElementsMethod = this
                    .GetType()
                    .GetMethod("TakeElements", BindingFlags.NonPublic | BindingFlags.Instance)?
                    .MakeGenericMethod(elementType);

                // invokes the take elements method
                Results[elementType.Name.ToString()] = (IEnumerable<Element>)takeElementsMethod?
                    .Invoke(this, new object[] { UtilFunctions.ConvertSheetName(elementType.Name.ToString()) })!; // NOTE: parameter should be old name of the sheet
            }

            return Results;
        }

        private IEnumerable<T> TakeElements<T>(string sheetName) where T : DistributionElement
        {
            IEnumerable<T> elements;
            elements = _mapper!.Take<T>(UtilFunctions.ConvertSheetName(sheetName));
            return elements;
        }

        private IEnumerable<Feeder> TakeFeeders()
        {
            var feeders = _mapper!.TakeFeeder<Feeder>(UtilFunctions.ConvertSheetName("Feeder")); // NOTE: change "Feeder" to  dcircuit

            foreach (var feeder in feeders)
                feeder.ScenarioId = _scenarioId;

            return feeders;
        }


        // TODO: SPLIT TO PARTIAL CLASS HERE

        private void ConfigureMapper(IWorkbook book)
        { // TODO: need to change column names to snake case
            _mapper = new ExcelMapper(book);

            _mapper
               .MapColumn("Feeder", (property, value, target) =>
               {
                   var feeders = (IEnumerable<Feeder>)Results["Feeder"];
                   var feeder = feeders.FirstOrDefault(f => f.Name == (string)value);
                   if (feeder != null)
                       property.SetValue(target, feeder, null);
                   else
                       throw new ArgumentNullException();
               })
               .MapColumn("LineSpacing", (property, value, target) =>
               {
                   var lineSpacings = (IEnumerable<LineSpacing>)Results["LineSpacing"];
                   var lineSpacing = lineSpacings.FirstOrDefault(s => s.Name == (string)value);
                   if (lineSpacing != null)
                       property.SetValue(target, lineSpacing, null);
               })
               .MapColumn("ConductorA", SetConductorProperty)
               .MapColumn("ConductorB", SetConductorProperty)
               .MapColumn("ConductorC", SetConductorProperty)
               .MapColumn("ConductorN", SetConductorProperty)
               .MapColumn("Configuration", SetConfigurationProperty<DistributionElement>())
               .MapColumn("From", SetNodeProperty)
               .MapColumn("To", SetNodeProperty)
               .MapColumn("ParentNode", SetNodeProperty)
               .MapColumn("SenseNode", SetNodeProperty)
               .MapColumn("RemoteSense", SetNodeProperty)
               .MapColumn("RemoteSenseB", SetNodeProperty)
               .MapReadOnlyColumn("Impedance", SetImpedanceProperty)
               .MapReadOnlyColumn("ShuntImpedance", SetShuntImpedanceProperty);

            // line configurations complex props

            var lineProperties = typeof(LineConfiguration).GetProperties()
                .Where(p => p.PropertyType == typeof(Complex));

            foreach (var lineProperty in lineProperties)
            {
                _mapper.MapReadOnlyColumn(lineProperty.Name,
                    SetLineImpedanceProperties<LineConfiguration>(lineProperty.Name));
            }

            // switches complex props
            var switchProperties = typeof(SwitchElement).GetProperties()
                .Where(p => p.PropertyType == typeof(Complex));

            foreach (var switchProperty in switchProperties)
            {
                _mapper.MapReadOnlyColumn(switchProperty.Name,
                    SetSwitchComplexProperties<SwitchElement>(switchProperty.Name));
            }

            // load complex props
            var loadProperties = typeof(Load).GetProperties()
                .Where(p => p.PropertyType == typeof(Complex));

            foreach (var loadProperty in loadProperties)
            {
                _mapper.MapReadOnlyColumn(loadProperty.Name,
                    SetLoadComplexProperties<Load>(loadProperty.Name));
            }
        }

        private Action<string, object> SetLoadComplexProperties<T>(string propertyName) where T : Load
        {
            var action = (string value, object target) =>
            {
                var targetObject = (T)target;
                string complexString = value;
                var complexValue = complexString.ToComplexNumber();

                var pattern = @"^Constant(Current|Power|Impedance)(A|B|C)$";
                var regex = new Regex(pattern);

                var match = regex.Match(propertyName);

                if (match.Success)
                {
                    var parameterName = match.Groups[1].Value;
                    var phase = match.Groups[2].Value;

                    var realProperty = propertyName + "Real";
                    var imagProperty = propertyName + "Imag";

                    if (parameterName == "Power")
                    {
                        realProperty = "ConstantActive" + parameterName + phase;
                        imagProperty = "ConstantReactive" + parameterName + phase;
                    }

                    if (parameterName == "Impedance")
                    {
                        realProperty = "ConstantResistance" + phase;
                        imagProperty = "ConstantReactance" + phase;
                    }

                    typeof(T).GetProperty(realProperty)?.SetValue(target, complexValue.Real);
                    typeof(T).GetProperty(imagProperty)?.SetValue(target, complexValue.Imaginary);
                }
            };
            return action;

        }

        private Action<string, object> SetSwitchComplexProperties<T>(string propertyName) where T : SwitchElement
        {
            var action = (string value, object target) =>
            {
                var targetObject = (T)target;
                string complexString = value;
                var complexValue = complexString.ToComplexNumber();

                var pattern = @"^(Current|Power)(In|Out)(A|B|C)$";
                var regex = new Regex(pattern);

                var match = regex.Match(propertyName);

                if (match.Success)
                {
                    var parameterName = match.Groups[1].Value;

                    var realProperty = propertyName + "Real";
                    var imagProperty = propertyName + "Imag";

                    if (parameterName == "Power")
                    {
                        realProperty = "Active" + propertyName;
                        imagProperty = "Reactive" + propertyName;
                    }

                    typeof(T).GetProperty(realProperty)?.SetValue(target, complexValue.Real);
                    typeof(T).GetProperty(imagProperty)?.SetValue(target, complexValue.Imaginary);
                }
            };
            return action;

        }

        private Action<string, object> SetLineImpedanceProperties<T>(string propertyName) where T : LineConfiguration
        {
            var action = (string value, object target) =>
            {
                var targetObject = (T)target;
                string complexString = value;
                var impedance = complexString.ToComplexNumber();

                var real = impedance.Real;
                var imag = impedance.Imaginary;

                var prefix = propertyName.Substring(propertyName.Length - 2);


                typeof(T).GetProperty("Resistance" + prefix)?.SetValue(target, real);
                typeof(T).GetProperty("Reactance" + prefix)?.SetValue(target, imag);

            };
            return action;
        }

        private void SetImpedanceProperty(string value, object target)
        {
            if (target.GetType() == typeof(TransformerConfiguration))
            {
                var targetObject = (TransformerConfiguration)target;
                string complexString = value;
                var impedance = complexString.ToComplexNumber();

                targetObject.Resistance = impedance.Real;
                targetObject.Reactance = impedance.Imaginary;
            }
        }

        private void SetShuntImpedanceProperty(string value, object target)
        {
            if (target.GetType() == typeof(TransformerConfiguration))
            {
                var targetObject = (TransformerConfiguration)target;
                string complexString = value;
                var impedance = complexString.ToComplexNumber();

                targetObject.ShuntResistance = impedance.Real;
                targetObject.ShuntReactance = impedance.Imaginary;
            }
        }

        private void SetNodeProperty(PropertyInfo property, object value, object target)
        {
            var nodes = (IEnumerable<Node>)Results["Node"];
            var node = nodes.FirstOrDefault(c => c.Name == (string)value);
            if (node != null)
                property.SetValue(target, node, null);
        }

        private Action<PropertyInfo, object, object> SetConfigurationProperty<T>() where T : DistributionElement
        {
            var action = (PropertyInfo property, object value, object target) =>
            {
                var configurations = (IEnumerable<T>)Results[target.GetType().Name + "Configuration"];
                //Console.WriteLine("DEBUG: " + target.GetType().Name + "Configuration");
                var configuration = configurations.FirstOrDefault(c => c.Name == (string)value);
                if (configuration != null)
                    property.SetValue(target, configuration, null);

            };
            return action;
        }

        private void SetConductorProperty(PropertyInfo property, object value, object target)
        {
            if (target.GetType() == typeof(OverheadLineConfiguration))
            {
                var ovConductors = (IEnumerable<OverheadLineConductor>)Results["OverheadLineConductor"];
                var ovConductor = ovConductors.FirstOrDefault(c => c.Name == (string)value);
                if (ovConductor != null)
                    property.SetValue(target, ovConductor, null);

            }
            else if (target.GetType() == typeof(UndergroundLineConfiguration))
            {
                var undConductors = (IEnumerable<UndergroundLineConductor>)Results["UndergroundLineConductor"];
                var undConductor = undConductors.FirstOrDefault(c => c.Name == (string)value);
                if (undConductor != null)
                    property.SetValue(target, undConductor, null);
            }
        }
    }
}
