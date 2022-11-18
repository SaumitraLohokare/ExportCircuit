using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Numerics;

namespace FileConverter.Extensions
{
    public static class StringExtensions
    {
        public static Complex ToComplexNumber(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return new Complex(0, 0);

            //var regexPattern = @"^([+-]?(?:\d+(?:\.\d+)?|\.\d+|[Ii]nf)(?![ij.\d]))?(?:([+-]?(?:\d+(?:\.\d+)?|\.\d+|[Ii]nf)?)[ij])?$";
            var regexPattern = @"^([+-]?(?:\d+(?:\.\d+)?|\.\d+|[Ii]nfinity)(?![ij.\d]))?(?:([+-]?(?:\d+(?:\.\d+)?|\.\d+|[Ii]nfinity)?)[ij])?$";

            var regex = new Regex(regexPattern);
            var matches = regex.Match(str);

            if (matches.Success)
            {
                var match = matches.Groups[0].Value;
                var realString = matches.Groups[1].Value;
                var imagString = matches.Groups[2].Value;

                if (imagString == "-")
                    imagString = "-1";
                else if (imagString == "+")
                    imagString = "1";

                var imaginaryToken = match[match.Length - 1];
                if ((imaginaryToken == 'j' || imaginaryToken == 'i') && string.IsNullOrEmpty(imagString))
                {
                    imagString = "1";
                }

                Double.TryParse(realString, NumberStyles.Number, CultureInfo.InvariantCulture, out var real);
                Double.TryParse(imagString, NumberStyles.Number, CultureInfo.InvariantCulture, out var imag);

                return new Complex(real, imag);

            }

            return new Complex(0, 0);

        }

        public static string PascalToSnake(this string s)
        {
            string res = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsUpper(s[i]))
                {
                    if (i == 0)
                    {
                        res += char.ToLower(s[i]);
                    }
                    else
                    {
                        res += '_';
                        res += char.ToLower(s[i]);
                    }
                }
                else
                {
                    res += s[i];
                }
            }
            return res;
        }

        public static string SnakeToPascal(this string s)
        {
            var words = s.Split('_');
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
            }
            return String.Join("", words);
        }
    }
}
