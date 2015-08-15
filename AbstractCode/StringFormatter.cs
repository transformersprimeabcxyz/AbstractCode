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
namespace AbstractCode
{
    /// <summary>
    /// Represents a language specific string formatter.
    /// </summary>
    public abstract class StringFormatter
    {
        /// <summary>
        /// Formats the given string to a syntax friendly text.
        /// </summary>
        /// <param name="content">The string to format.</param>
        /// <returns>The formatted text.</returns>
        public abstract string FormatString(string content);

        /// <summary>
        /// Formats or escapes the given character to a syntax friendly character which can appear in a string.
        /// </summary>
        /// <param name="character">The character to format.</param>
        /// <returns>The formatted character.</returns>
        public abstract string FormatChar(char character);

        /// <summary>
        /// Evaluates the formatted string (as they appear in source code) to an actual string.
        /// </summary>
        /// <param name="stringCode">The string code to evaluate.</param>
        /// <returns>The string that was represented by the string code.</returns>
        public abstract string EvaluateFormattedString(string stringCode);

        /// <summary>
        /// Evaluates the formatted character (as they appear in source code) to an actual character.
        /// </summary>
        /// <param name="charCode">The character code to evaluate.</param>
        /// <returns>The character that was represented by the character code.</returns>
        public abstract char EvaluateFormattedChar(string charCode);
    }
}