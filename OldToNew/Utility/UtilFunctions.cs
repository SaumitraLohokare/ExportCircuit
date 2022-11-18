using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using FileConverter.Utility;
using FileConverter.Extensions;

namespace FileConverter.Utility
{
    public class UtilFunctions
    {
        public static string ConvertColumnToOld(string name)
        {
            var patternImpedence = @"Impedance\d+";
            if (Regex.Match(name, patternImpedence).Success)
            {
                return name.Replace("Impedance", "z");
            }

            var patternCapacitance = @"Capacitance\d+";
            if (Regex.Match(name, patternCapacitance).Success)
            {
                return name.Replace("Capacitance", "c");
            }

            foreach (var item in Maps.PropertyNewName)
            {
                if (item.Value == name)
                {
                    return item.Key;
                }
            }

            return name.PascalToSnake();
        }

        public static bool CompareNewCellName(string propertyName, string cellName)
        {
            // first check if it is z<num>
            var patternImpedence = @"z\d+";
            if (Regex.Match(cellName, patternImpedence).Success)
            {
                return ZToImpedence(cellName) == propertyName;
            }

            // check if it is c<num>
            var patternCapacitance = @"c\d+";
            if (Regex.Match(cellName, patternCapacitance).Success)
            {
                return CToCapacitance(cellName) == propertyName;
            }

            // check if it is in dictionary
            if (Maps.PropertyNewName.ContainsKey(cellName))
            {
                return Maps.PropertyNewName[cellName] == propertyName;
            }

            // convert sc -> pc
            return cellName.SnakeToPascal() == propertyName;
        }

        /// <summary>
        /// Converts new sheet name to old sheet name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ConvertSheetName(string name)
        {
            if (Maps.SheetMap.ContainsKey(name))
            {
                return Maps.SheetMap[name];
            }

            return name.PascalToSnake();
        }

        private static string ZToImpedence(string s)
        {
            return s.Replace("z", "Impedance");
        }

        // function to convert c<num> to Capacitance<num>
        private static string CToCapacitance(string s)
        {
            return s.Replace("c", "Capacitance");
        }
    }
}
