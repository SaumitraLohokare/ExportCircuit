using ExcelTools;
using ProsumerGridExporter.ExcelExporter;
using Prosumergrid.Domain.Entities.BaseClasses;
using Prosumergrid.Domain.Entities.Circuit;

//var inFolder = @"D:\ProsumerGrid\OldToNew\OldToNew\FeederData\";
//var outFolder = @"D:\ProsumerGrid\OldToNew\OldToNew\FeederDataOut\";

public class Program
{
    Dictionary<string, List<string>> flags = new Dictionary<string, List<string>>();
    List<HashSet<string>> quartiles = new List<HashSet<string>>();

    public static void Main(string[] args)
    {
        var prog = new Program();
        prog.run(args);
    }

    public void run(string[] args)
    {
        flags = Flag.ParseFlags(args);

        // TODO: Polishing add check here to see if any values were provided
        var inFolder = GetFlagValue(Flag.IN).First();
        inFolder = inFolder.Replace("/", "\\");
        // check if it is a file or a folder
        var isInFolder = File.GetAttributes(inFolder).HasFlag(FileAttributes.Directory);

        // TODO: Polishing add check here to see if any values were provided
        var outFolder = GetFlagValue(Flag.OUT).First();
        outFolder = outFolder.Replace("/", "\\");
        var isOutFolder = File.GetAttributes(outFolder).HasFlag(FileAttributes.Directory);

        if (!isOutFolder)
        {
            Console.WriteLine($"{Flag.OUT} flag must be a folder.");
            return;
        }

        string[]? inFiles;
        if (isInFolder)
        {
            inFiles = Directory.GetFiles(inFolder!);
        } else
        {
            inFiles = new string[] { inFolder };
        }

        var derOptions = GetDEROptions();
        Console.WriteLine("\n> DER Options: ");
        string[] derNames = { "EVChargingStation", "Generator", "HeatPump", "Solar", "Storage" };
        for (int i = 0; i < derOptions.Length; i++)
        {
            Console.WriteLine(derNames[i] + " : " + (derOptions[i].scale == -1 ? "None" : "[" + derOptions[i] + "]"));
        }

        Console.WriteLine($"\n> Converting {inFiles.Length} files.\n");

        foreach (var file in inFiles)
        {
            ConvertFile(file, outFolder, derOptions);
        }
        Console.WriteLine("\nConverted ALL files successfully!");
    }

    void ConvertFile(string file, string outFolder, DEROptions[] derOptions)
    {
        var workingFileName = file.Split("\\").Last();
        Console.WriteLine("Working on: " + workingFileName);
        var reader = new Reader();
        var book = Reader.ReadWorkbook(file);

        if (book == null) throw new FileNotFoundException($"{file} cannot be found.");

        var objects = reader.ReadData(book, Guid.NewGuid());
        book.Close();

        FillQuartiles(objects["Load"]);

        for (int i = 0; i < derOptions.Length; i++)
        {
            if (derOptions[i].scale == -1) continue;

            // Quartile calculation here


            // calculate total_feeder_active_power here
            var total_feeder_active_power = CalculateTotalFeederActivePower(objects, derOptions[i].filterClass, null);

            GenerateDER(i, total_feeder_active_power, derOptions[i], ref objects);
        }

        int objects_count = 0;
        foreach (var obj in objects)
        {
            objects_count += obj.Value.Count();
        }

        // now give this as input to ExcelExporter...
        Console.WriteLine("\tObjects for " + workingFileName + " extracted. (" + objects_count + " objects)");
        var newFileName = file.Split('\\').Last();
        newFileName = newFileName.Split('.').First();
        var outputFileName = newFileName.Split('.').First() + ".xlsx";
        newFileName = outFolder + outputFileName;
        Console.WriteLine("\tWriting new format to " + outputFileName);

        Exporter.Export(objects, newFileName);
        Console.WriteLine("Done!\n");
    }

    void FillQuartiles(IEnumerable<Element> loads)
    {
        List<double> ranges = new List<double>();
        foreach (var e in loads)
        {
            var load = (Load)e;
            var threePhaseActivePower = load.ActivePowerA + load.ActivePowerB + load.ActivePowerC;
            ranges.Add(threePhaseActivePower);
        }

        // 
    }

    double CalculateTotalFeederActivePower(Dictionary<string, IEnumerable<Element>> objects, LoadClass? classOption, int? quartile)
    {
        var total_feeder_active_power = 0d;
        foreach (var load in objects["Load"])
        {
            var e = (Load)load;
            if (classOption != null && e.LoadClass != classOption) continue;
            if (quartile != null) /* Check if load is in quartile. */;

            total_feeder_active_power += e.ConstantActivePowerA + e.ConstantActivePowerB + e.ConstantActivePowerC;
        }
        return total_feeder_active_power;
    }

    void GenerateDER(int derNo, double total_feeder_active_power, DEROptions dEROptions, ref Dictionary<string, IEnumerable<Element>> objects)
    {
        if (!objects.TryGetValue("Load", out var loads)) return;

        foreach (Load load in loads)
        {
            if (load.LoadClass != dEROptions.filterClass) continue;

            var der = MakeDERElement(derNo);

            der.Name = der.GetType().Name.ToLower() + "_" + load.Name;
            
            der.ParentNode = load.ParentNode;
            der.Phases = load.Phases;
            der.Feeder = load.Feeder;

            var constantPowerSum = load.ConstantPowerA + load.ConstantPowerB + load.ConstantPowerC;
            der.MaxActivePower = dEROptions.scale * constantPowerSum.Real / total_feeder_active_power;
            der.MaxReactivePower = dEROptions.scale * constantPowerSum.Imaginary / total_feeder_active_power;

            // EVChargingStation specific
            if (derNo == 0)
            {
                ((EVChargingStation)der).MaxEnergy = der.MaxActivePower * 6;
                ((EVChargingStation)der).VariableOperatingCost = (decimal)0.001;
                ((EVChargingStation)der).ChargeEfficiency = 0.92;
            }

            // Generator specific
            if (derNo == 1)
            {
                ((Generator)der).UnitType = "natural_gas";
                ((Generator)der).VariableOperatingCost = (decimal)0.19;
            }

            // HeatPump specific
            if (derNo == 2)
            {
                ((HeatPump)der).VariableOperatingCost = (decimal)0.19;
            }

            // Solar specific
            if (derNo == 3)
            {
                ((Solar)der).UnitType = "solar_pv";
                ((Solar)der).VariableOperatingCost = (decimal)0.001;
            }

            // Storage specific
            if (derNo == 4)
            {
                ((Storage)der).MaxEnergy = der.MaxActivePower * 3;
                ((Storage)der).UnitType = "li_ion";
                ((Storage)der).VariableOperatingCost = (decimal)0.001;
                ((Storage)der).ChargeEfficiency = 0.92;
            }

            // All other properties????

            // add to objects
            if (derNo == 0)
            {
                if (objects.TryGetValue("EVChargingStation", out var evChargingStations))
                {
                    objects["EVChargingStation"] = evChargingStations.Append(der);
                }
            } 
            else if (derNo == 1)
            {
                if (objects.TryGetValue("Generator", out var generators))
                {
                    objects["Generator"] = generators.Append(der);
                }
            }
            else if (derNo == 2)
            {
                if (objects.TryGetValue("HeatPump", out var heatPumps))
                {
                    objects["HeatPump"] = heatPumps.Append(der);
                }
            }
            else if (derNo == 3)
            {
                if (objects.TryGetValue("Solar", out var solars))
                {
                    objects["Solar"] = solars.Append(der);
                }
            }
            else if (derNo == 4)
            {
                if (objects.TryGetValue("Storage", out var storages))
                {
                    objects["Storage"] = storages.Append(der);
                }
            }
        }
    }

    DERElement MakeDERElement(int derNo)
    {
        if (derNo == 0) return new EVChargingStation();
        if (derNo == 1) return new Generator();
        if (derNo == 2) return new HeatPump();
        if (derNo == 3) return new Solar();
        if (derNo == 4) return new Storage();

        throw new NotImplementedException($"No DER for derNo {derNo}");
    }

    List<string> GetFlagValue(string flag)
    {
        var result = flags.TryGetValue(flag, out var value);
        if (!result || value == null)
        {
            throw new ArgumentNullException($"Please provide {Flag.IN} flag");
        }
        return value;
    }


    /// <summary>
    /// <p>
    /// 5 doubles are returned. <br/>
    /// If value is -1, we do not generate that DER
    ///
    /// 0 -> EVChargingStation scale<br/>
    /// 1 -> Generator scale<br/>
    /// 2 -> HeatPump scale<br/>
    /// 3 -> Solar scale<br/>
    /// 4 -> Storage scale<br/>
    /// </p>
    /// </summary>
    DEROptions[] GetDEROptions()
    {
        // set all der options to -1 (meaning don't generate)
        DEROptions[] derOptions = new DEROptions[5];


        if (flags.TryGetValue(Flag.ADD_ALL_DER, out var args))
        {
            // TODO: Polish make sure atleast 1, 2, or 3 arguments are passed
            var scaleValue = double.Parse(args[0]);
            LoadClass? loadClass = null;
            int? quartileFilter = null;
            if (args.Count == 2)
            {
                if (!Enum.TryParse(typeof(LoadClass), args[1], out var classFilter))
                    throw new ArgumentException($"Invalid load class {args[1]}");
                else
                    loadClass = (LoadClass?)classFilter;
            }
            else if (args.Count == 3)
            {
                if (!Enum.TryParse(typeof(LoadClass), args[1], out var classFilter))
                    throw new ArgumentException($"Invalid load class {args[1]}");
                else
                    loadClass = (LoadClass?)classFilter;

                if (!int.TryParse(args[2], out var quartile) || (quartile < 1 || quartile > 3))
                    throw new ArgumentException($"Invalid quartile {quartile}");
                else
                    quartileFilter = quartile;
            }
            for (int i = 0; i < derOptions.Length; i++) derOptions[i] = new DEROptions(scaleValue, loadClass, quartileFilter);

            return derOptions;
        }

        if (flags.TryGetValue(Flag.ADD_EV_CHARGING_STATION, out var evChargingStation))
        {
            LoadClass? loadClass = null;
            int? quartileFilter = null;
            if (evChargingStation.Count == 2)
            {
                if (!Enum.TryParse(typeof(LoadClass), evChargingStation[1], out var classFilter))
                    throw new ArgumentException($"Invalid load class {evChargingStation[1]}");
                else
                    loadClass = (LoadClass?)classFilter;
            }
            else if (evChargingStation.Count == 3)
            {
                if (!Enum.TryParse(typeof(LoadClass), evChargingStation[1], out var classFilter))
                    throw new ArgumentException($"Invalid load class {evChargingStation[1]}");
                else
                    loadClass = (LoadClass?)classFilter;

                if (!int.TryParse(evChargingStation[2], out var quartile) || (quartile < 0 || quartile > 3))
                    throw new ArgumentException($"Invalid quartile {quartile}");
                else
                    quartileFilter = quartile;
            }
            derOptions[0] = new DEROptions(double.Parse(evChargingStation[0]), loadClass, quartileFilter);
        }
        if (flags.TryGetValue(Flag.ADD_GENERATOR, out var generator))
        {
            LoadClass? loadClass = null;
            int? quartileFilter = null;
            if (generator.Count == 2)
            {
                if (!Enum.TryParse(typeof(LoadClass), generator[1], out var classFilter))
                    throw new ArgumentException($"Invalid load class {generator[1]}");
                else
                    loadClass = (LoadClass?)classFilter;
            }
            else if (generator.Count == 3)
            {
                if (!Enum.TryParse(typeof(LoadClass), generator[1], out var classFilter))
                    throw new ArgumentException($"Invalid load class {generator[1]}");
                else
                    loadClass = (LoadClass?)classFilter;

                if (!int.TryParse(generator[2], out var quartile) || (quartile < 0 || quartile > 3))
                    throw new ArgumentException($"Invalid quartile {quartile}");
                else
                    quartileFilter = quartile;
            }
            derOptions[1] = new DEROptions(double.Parse(generator[0]), loadClass, quartileFilter);
        }
        if (flags.TryGetValue(Flag.ADD_HEAT_PUMP, out var heatPump))
        {
            LoadClass? loadClass = null;
            int? quartileFilter = null;
            if (heatPump.Count == 2)
            {
                if (!Enum.TryParse(typeof(LoadClass), heatPump[1], out var classFilter))
                    throw new ArgumentException($"Invalid load class {heatPump[1]}");
                else
                    loadClass = (LoadClass?)classFilter;
            }
            else if (heatPump.Count == 3)
            {
                if (!Enum.TryParse(typeof(LoadClass), heatPump[1], out var classFilter))
                    throw new ArgumentException($"Invalid load class {heatPump[1]}");
                else
                    loadClass = (LoadClass?)classFilter;

                if (!int.TryParse(heatPump[2], out var quartile) || (quartile < 0 || quartile > 3))
                    throw new ArgumentException($"Invalid quartile {quartile}");
                else
                    quartileFilter = quartile;
            }
            derOptions[2] = new DEROptions(double.Parse(heatPump[0]), loadClass, quartileFilter);
        }
        if (flags.TryGetValue(Flag.ADD_SOLAR, out var solar))
        {
            LoadClass? loadClass = null;
            int? quartileFilter = null;
            if (solar.Count == 2)
            {
                if (!Enum.TryParse(typeof(LoadClass), solar[1], out var classFilter))
                    throw new ArgumentException($"Invalid load class {solar[1]}");
                else
                    loadClass = (LoadClass?)classFilter;
            }
            else if (solar.Count == 3)
            {
                if (!Enum.TryParse(typeof(LoadClass), solar[1], out var classFilter))
                    throw new ArgumentException($"Invalid load class {solar[1]}");
                else
                    loadClass = (LoadClass?)classFilter;

                if (!int.TryParse(solar[2], out var quartile) || (quartile < 0 || quartile > 3))
                    throw new ArgumentException($"Invalid quartile {quartile}");
                else
                    quartileFilter = quartile;
            }
            derOptions[3] = new DEROptions(double.Parse(solar[0]), loadClass, quartileFilter);
        }
        if (flags.TryGetValue(Flag.ADD_STORAGE, out var storage))
        {
            LoadClass? loadClass = null;
            int? quartileFilter = null;
            if (storage.Count == 2)
            {
                if (!Enum.TryParse(typeof(LoadClass), storage[1], out var classFilter))
                    throw new ArgumentException($"Invalid load class {storage[1]}");
                else
                    loadClass = (LoadClass?)classFilter;
            }
            else if (storage.Count == 3)
            {
                if (!Enum.TryParse(typeof(LoadClass), storage[1], out var classFilter))
                    throw new ArgumentException($"Invalid load class {storage[1]}");
                else
                    loadClass = (LoadClass?)classFilter;

                if (!int.TryParse(storage[2], out var quartile) || (quartile < 0 || quartile > 3))
                    throw new ArgumentException($"Invalid quartile {quartile}");
                else
                    quartileFilter = quartile;
            }
            derOptions[4] = new DEROptions(double.Parse(storage[0]), loadClass, quartileFilter);
        }

        for (int i = 0; i < derOptions.Length; i++)
        {
            if (derOptions[i] == null)
                derOptions[i] = new DEROptions(-1, null, null);
        }
        return derOptions;
    }

    record DEROptions(double scale, LoadClass? filterClass, int? filterQuartile)
    {
        public sealed override string ToString()
        {
            return $"scale: {scale} | loadClassFilter: {(filterClass == null ? "None" : filterClass)} | quartileFilter: {(filterQuartile == null ? "None" : filterQuartile)}";
        }
    }
}