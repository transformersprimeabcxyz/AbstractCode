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
using System.Text;
using System.Threading.Tasks;

namespace AbstractCode.Ast.CSharp
{
    public class CSharpNumberFormatter : NumberFormatter
    {
        private static readonly Dictionary<string, Type> _numberSuffixes = new Dictionary<string, Type>()
        {
            {"U", typeof(uint)},
            {"L", typeof(long)},
            {"UL", typeof(ulong)},
            {"LU", typeof(ulong)},
            {"F", typeof(float)},
            {"D", typeof(double)},
            {"M", typeof(decimal)},
        };

        private static readonly NumberFormatInfo _numberFormat;

        static CSharpNumberFormatter()
        {
            var culture = CultureInfo.InstalledUICulture;
            _numberFormat = (NumberFormatInfo)culture.NumberFormat.Clone();
            _numberFormat.NumberDecimalSeparator = ".";
        }

        public override string FormatNumber(object number)
        {
            var result = Convert.ChangeType(number, typeof(string), _numberFormat);
            var suffix = string.Empty;

            if (!(number is double))
                suffix = _numberSuffixes.FirstOrDefault(x => x.Value == number.GetType()).Key;

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
                    else
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

            if (!_numberSuffixes.TryGetValue(suffix.ToUpperInvariant(), out numberType))
            {
                if (suffix.Length > 0)
                    throw new FormatException("Unrecognized number suffix.");

                // implicit number type
                numberType = hasDecimalSpecifier ? typeof(double) : typeof(int);
            }

            return Convert.ChangeType(number, numberType, _numberFormat);
        }
    }
}
