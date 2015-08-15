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

namespace AbstractCode
{
    /// <summary>
    /// Provides methods for representing a text source.
    /// </summary>
    public interface ITextSource
    {
        /// <summary>
        /// Gets the length of the text in the text source.
        /// </summary>
        int TextLength
        {
            get;
        }

        /// <summary>
        /// Gets the text of the text source.
        /// </summary>
        string Text
        {
            get;
        }

        /// <summary>
        /// Creates a snapshot of the text source.
        /// </summary>
        /// <returns>The snapshot of the text source.</returns>
        ITextSource CreateSnapshot();

        /// <summary>
        /// Creates a trimmed snapshot of the text source.
        /// </summary>
        /// <param name="offset">The starting offset of the text to create a snapshot from.</param>
        /// <param name="length">The length of the text to create a snapshot from.</param>
        /// <returns></returns>
        ITextSource CreateSnapshot(int offset, int length);

        /// <summary>
        /// Gets the character at a given offset.
        /// </summary>
        /// <param name="offset">The offset of the character to get.</param>
        /// <returns>The character at the given offset.</returns>
        char GetChar(int offset);

        /// <summary>
        /// Gets the text at a given offset and length.
        /// </summary>
        /// <param name="offset">The offset of the text to get.</param>
        /// <param name="length">The length of the text to get.</param>
        /// <returns>The text at the given offset and length.</returns>
        string GetText(int offset, int length);

        /// <summary>
        /// Gets the offset of a specific character.
        /// </summary>
        /// <param name="character">The character to search.</param>
        /// <param name="offset">The starting offset of the text to search in.</param>
        /// <param name="length">The length of the text to search in.</param>
        /// <returns></returns>
        int IndexOf(char character, int offset, int length);

        /// <summary>
        /// Gets the offset of the character that appears first in the text source.
        /// </summary>
        /// <param name="characters">The characters to search.</param>
        /// <param name="offset">The starting offset of the text to search in.</param>
        /// <param name="length">The length of the text to search in.</param>
        /// <returns></returns>
        int IndexOfAny(char[] characters, int offset, int length);

        /// <summary>
        /// Creates a text reader for this text source.
        /// </summary>
        /// <returns></returns>
        TextReader CreateReader();

        /// <summary>
        /// Writes the text of this text source to the given text writer.
        /// </summary>
        /// <param name="writer">The text writer to write to.</param>
        void WriteTo(TextWriter writer);
    }
}