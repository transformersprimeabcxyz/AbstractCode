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
    /// Represents a language specific number formatter.
    /// </summary>
    public abstract class NumberFormatter
    {
        /// <summary>
        /// Formats a number to a string.
        /// </summary>
        /// <param name="number">The number to format.</param>
        /// <returns>The formatted number.</returns>
        public abstract string FormatNumber(object number);

        /// <summary>
        /// Evaulates the formatted text to an actual number.
        /// </summary>
        /// <param name="formattedNumber">The formatted text to evaulate.</param>
        /// <returns>The number that was represented by the formatted text.</returns>
        public abstract object EvaluateFormattedNumber(string formattedNumber);
    }
}