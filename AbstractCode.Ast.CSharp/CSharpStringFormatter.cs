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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractCode.Ast.CSharp
{
    public class CSharpStringFormatter : StringFormatter
    {
        public override string FormatString(string content)
        {
            var builder = new StringBuilder();

            builder.Append("\"");

            for (int i = 0; i < content.Length; i++)
                builder.Append(FormatCharInternal(content[i]));
           
            builder.Append("\"");

            return builder.ToString();
        }

        public override string FormatChar(char character)
        {
            return string.Format("'{0}'", FormatCharInternal(character));
        }

        private static string FormatCharInternal(char character)
        {
            switch (character)
            {
                case '\0': return "\\0";
                case '\a': return "\\a";
                case '\b': return "\\b";
                case '\n': return "\\n";
                case '\r': return "\\r";
                case '\t': return "\\t";
                case '\v': return "\\v";
                case '\\': return "\\\\";
                default:
                    return character.ToString();
            }
        }

        public override string EvaluateFormattedString(string stringCode)
        {
            var builder = new StringBuilder();
            
            if (stringCode[0] != '\"')
                throw new ArgumentException("String code does not start with a '\"'");

            if (stringCode[stringCode.Length - 1] != '\"')
                throw new ArgumentException("String code does not end with a '\"'");

            for (int i = 1; i < stringCode.Length - 1; i++)
            {
                builder.Append(EvaluateCharAt(stringCode, ref i));
            }

            return builder.ToString();
        }

        public override char EvaluateFormattedChar(string charCode)
        {
            if (charCode[0] != '\"')
                throw new ArgumentException("Char code does not start with a '\"'");

            if (charCode[charCode.Length - 1] != '\"')
                throw new ArgumentException("Char code does not end with a '\"'");

            int position = 1;
            char character = EvaluateCharAt(charCode, ref position);

            if (position != charCode.Length - 1)
                throw new ArgumentException("Char code's length is too long.");

            return character;
        }

        private static char EvaluateCharAt(string stringCode, ref int position)
        {
            if (stringCode[position] == '\\')
            {
                switch (stringCode[++position])
                {
                    case '0': return '\0';
                    case 'a': return '\a';
                    case 'b': return '\b';
                    case 'n': return '\n';
                    case 'r': return '\r';
                    case 't': return '\t';
                    case 'v': return '\v';
                }
            }

            return stringCode[position];
        }
    }
}
