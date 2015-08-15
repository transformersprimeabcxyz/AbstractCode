// This file is part of AbstractCode.
// 
// AbstractCode is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// AbstractCode is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with AbstractCode.  If not, see <http://www.gnu.org/licenses/>.
// 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AbstractCode.Ast.VisualBasic
{
    public class VisualBasicNumberFormatter : NumberFormatter
    {
        private static readonly Dictionary<string, Type> NumberSuffixes = new Dictionary<string, Type>()
        {
            { "S", typeof (short) },
            { "US", typeof (ushort) },
            { "I", typeof (int) },
            { "UI", typeof (uint) },
            { "L", typeof (long) },
            { "UL", typeof (ulong) },
            { "F", typeof (float) },
            { "R", typeof (double) },
            { "D", typeof (decimal) },
        };

        private static readonly NumberFormatInfo NumberFormat;

        static VisualBasicNumberFormatter()
        {
            var culture = CultureInfo.InstalledUICulture;
            NumberFormat = (NumberFormatInfo)culture.NumberFormat.Clone();
            NumberFormat.NumberDecimalSeparator = ".";
        }

        public override string FormatNumber(object number)
        {
            var result = Convert.ChangeType(number, typeof (string), NumberFormat);
            var suffix = string.Empty;

            if (!(number is int) && !(number is double))
                suffix = NumberSuffixes.FirstOrDefault(x => x.Value == number.GetType()).Key;

            return result + suffix;
        }

        public override object EvaluateFormattedNumber(string formattedNumber)
        {
            int index = -1;
            bool hasDecimalSpecifier = false;

            for (int i = formattedNumber.Length - 1; i >= 0; i--)
            {
                if (formattedNumber[i] == '.')
                {
                    if (hasDecimalSpecifier)
                        throw new FormatException("Too many decimal specifiers in number.");
                    hasDecimalSpecifier = true;
                }
                else if (char.IsLetter(formattedNumber[i]))
                {
                    index = i;
                }
            }

            bool hasSuffix = index != -1;

            Type numberType;
            string number = hasSuffix ? formattedNumber.Remove(index) : formattedNumber;
            string suffix = hasSuffix ? formattedNumber.Substring(index) : string.Empty;

            if (!NumberSuffixes.TryGetValue(suffix.ToUpperInvariant(), out numberType))
            {
                if (suffix.Length > 0)
                    throw new FormatException("Unrecognized number suffix.");

                // implicit number type
                numberType = hasDecimalSpecifier ? typeof (double) : typeof (int);
            }

            return Convert.ChangeType(number, numberType, NumberFormat);
        }
    }
}