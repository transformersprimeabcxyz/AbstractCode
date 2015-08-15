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

using System.IO;
using System.Text;

namespace AbstractCode.Ast.VisualBasic
{
    public class VisualBasicStringFormatter : StringFormatter
    {
        public override string FormatString(string content)
        {
            var builder = new StringBuilder();

            bool isInsideQuotes = false;
            bool requiresAmperand = false;
            for (int i = 0; i < content.Length; i++)
            {
                if (requiresAmperand)
                    builder.Append(" & ");

                if (IsSpecialChar(content[i]))
                {
                    if (isInsideQuotes)
                    {
                        builder.Append("\" & ");
                        isInsideQuotes = false;
                    }
                    requiresAmperand = true;

                    builder.Append(CreateChrWCall(content[i]));
                }
                else
                {
                    if (!isInsideQuotes)
                    {
                        builder.Append('"');
                        isInsideQuotes = true;
                    }

                    requiresAmperand = false;
                    builder.Append(content[i]);
                }
            }

            if (isInsideQuotes)
                builder.Append('"');

            return builder.ToString();
        }

        public override string FormatChar(char character)
        {
            if (IsSpecialChar(character))
                return CreateChrWCall(character);
            else
                return string.Format("\"{0}\"c", character);
        }

        private static bool IsSpecialChar(char character)
        {
            return (char.IsWhiteSpace(character) && character != ' ') || character == '"';
        }

        private static string CreateChrWCall(char character)
        {
            return string.Format("ChrW({0})", (ushort)character);
        }

        public override string EvaluateFormattedString(string stringCode)
        {
            var builder = new StringBuilder();
            using (var reader = new StringReader(stringCode))
            {
                bool isInsideQuotes = false;
                while (reader.Peek() != -1)
                    builder.Append(EvaluateNext(reader, ref isInsideQuotes));
            }
            return builder.ToString();
        }

        public override char EvaluateFormattedChar(string charCode)
        {
            using (var reader = new StringReader(charCode))
            {
                bool isInsideQuotes = false;
                return EvaluateNext(reader, ref isInsideQuotes);
            }
        }

        private static char EvaluateNext(StringReader reader, ref bool isInsideQuotes)
        {
            return (char)reader.Read();
        }
    }
}