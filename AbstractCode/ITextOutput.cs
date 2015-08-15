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
    /// Provides methods for representing a text output.
    /// </summary>
    public interface ITextOutput
    {
        /// <summary>
        /// Gets the current location of the text output.
        /// </summary>
        TextLocation Location
        {
            get;
        }

        /// <summary>
        /// Increments the current indentation length.
        /// </summary>
        void Indent();

        /// <summary>
        /// Decrements the current indentation length.
        /// </summary>
        void Unindent();

        /// <summary>
        /// Writes a character to the text output.
        /// </summary>
        /// <param name="char">The character to write.</param>
        void Write(char @char);

        /// <summary>
        /// Writes a string to the text output.
        /// </summary>
        /// <param name="text">The string to write.</param>
        void Write(string text);

        /// <summary>
        /// Terminates the current line of text and starts a new one.
        /// </summary>
        void WriteLine();
    }

    /// <summary>
    /// Provides extension methods for a <see cref="ITextOutput"/> instance.
    /// </summary>
    public static class TextOutputExtensions
    {
        /// <summary>
        /// Writes a formatted string to the text output.
        /// </summary>
        /// <param name="output">The text output to write to.</param>
        /// <param name="format">The string format to use.</param>
        /// <param name="arguments">The formatting arguments to use.</param>
        public static void Write(this ITextOutput output, string format, params object[] arguments)
        {
            output.Write(string.Format(format, arguments));
        }

        /// <summary>
        /// Writes a character to the text output and terminates the current line.
        /// </summary>
        /// <param name="output">The text output to write to.</param>
        /// <param name="char">The character to write.</param>
        public static void WriteLine(this ITextOutput output, char @char)
        {
            output.Write(@char);
            output.WriteLine();
        }


        /// <summary>
        /// Writes a string to the text output and terminates the current line.
        /// </summary>
        /// <param name="output">The text output to write to.</param>
        /// <param name="text">The text to write.</param>
        public static void WriteLine(this ITextOutput output, string text)
        {
            output.Write(text);
            output.WriteLine();
        }

        /// <summary>
        /// Writes a formatted string to the text output and terminates the current line.
        /// </summary>
        /// <param name="output">The text output to write to.</param>
        /// <param name="format">The string format to use.</param>
        /// <param name="arguments">The formatting arguments to use.</param>
        public static void WriteLine(this ITextOutput output, string format, params object[] arguments)
        {
            output.WriteLine(string.Format(format, arguments));
        }
    }
}