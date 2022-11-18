/// <summary>
/// Assuming that all -add&lt;DER&gt; require a -scalefactor after it, we can omit '-scalefactor' and simply write -addevchargingstations 0.2 <br/>
/// <br />
/// Also assuming that if -addallder is given, we ignore other -add&lt;DER&gt; <br />
/// </summary>

public static class Flag
{
    public static readonly string IN = "-in";
    public static readonly string OUT = "-out";
    public static readonly string ADD_EV_CHARGING_STATION = "-addevchargingstation";
    public static readonly string ADD_GENERATOR = "-addgenerator";
    public static readonly string ADD_HEAT_PUMP = "-addheatpump";
    public static readonly string ADD_SOLAR = "-addsolar";
    public static readonly string ADD_STORAGE = "-addstorage";
    public static readonly string ADD_ALL_DER = "-addallder";

    public static Dictionary<string, List<string>> ParseFlags(string[] args)
    {
        var flags = new Dictionary<string, List<string>>();
        var currentlyParsing = "";
        try
        {
            for (int i = 0; i < args.Length; i++)
            {

                if (args[i] == IN)
                {
                    currentlyParsing = args[i];
                }
                else if (args[i] == OUT)
                {
                    currentlyParsing = args[i];
                }
                else if (args[i] == ADD_EV_CHARGING_STATION)
                {
                    currentlyParsing = args[i];
                }
                else if (args[i] == ADD_SOLAR)
                {
                    currentlyParsing = args[i];
                }
                else if (args[i] == ADD_STORAGE)
                {
                    currentlyParsing = args[i];
                }
                else if (args[i] == ADD_GENERATOR)
                {
                    currentlyParsing = args[i];
                }
                else if (args[i] == ADD_HEAT_PUMP)
                {
                    currentlyParsing = args[i];
                }
                else if (args[i] == ADD_ALL_DER)
                {
                    currentlyParsing = args[i];
                }
                else
                {
                    if (args[i].StartsWith("-"))
                    {
                        Console.WriteLine("WARNING: Flag not supported: " + args[i]);
                        continue;
                    }

                    if (flags.ContainsKey(currentlyParsing))
                    {
                        flags[currentlyParsing].Add(args[i]);
                    } else
                    {
                        flags.Add(currentlyParsing, new List<string>());
                        flags[currentlyParsing].Add(args[i]);
                    }
                }
            }
        } catch (Exception)
        {
            throw new NotImplementedException("Need to implement printing usage of the program.");
        }

        return flags;
    }

    
}
