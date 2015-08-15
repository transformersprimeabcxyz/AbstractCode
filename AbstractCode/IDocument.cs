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

namespace AbstractCode
{
    /// <summary>
    /// Provides methods for representing a text document.
    /// </summary>
    public interface IDocument : ITextSource
    {
        /// <summary>
        /// Occurs when (a part of) the text of the document is about to change.
        /// </summary>
        event TextChangeEventHandler TextChanging;

        /// <summary>
        /// Occurs when (a part of) the text of the document has changed.
        /// </summary>
        event TextChangeEventHandler TextChanged;

        /// <summary>
        /// Creates a read-only snapshot of the text document.
        /// </summary>
        /// <returns>A read-only document representing the snapshot.</returns>
        IDocument CreateDocumentSnapshot();

        /// <summary>
        /// Converts the specified text location's line and column number in the document to a character offset of the total document string.
        /// </summary>
        /// <param name="location">The text location to convert.</param>
        /// <returns>The character offset of the text location.</returns>
        int LocationToOffset(TextLocation location);

        /// <summary>
        /// Converts the specified character offset to the corresponding text location in the text document.
        /// </summary>
        /// <param name="offset">The character offset to convert.</param>
        /// <returns>The text location of the character offset.</returns>
        TextLocation OffsetToLocation(int offset);
        
        /// <summary>
        /// Converts the specified line number to a character offset of the total document string.
        /// </summary>
        /// <param name="line">The line number to convert.</param>
        /// <returns>The character offset of the line number.</returns>
        int LineToOffset(int line);

        /// <summary>
        /// Converts the specified character offset to the line number of the text document.
        /// </summary>
        /// <param name="offset">The offset to convert.</param>
        /// <returns>The line number.</returns>
        int OffsetToLine(int offset);

        /// <summary>
        /// Gets the text located at the specified range in the text document.
        /// </summary>
        /// <param name="range">The range of the text to get.</param>
        /// <returns>The text at the given range.</returns>
        string GetRangeText(TextRange range);

        /// <summary>
        /// Gets the length of the text located at the specified range in the text document.
        /// </summary>
        /// <param name="range">The range of the text to get.</param>
        /// <returns>The length of the text at the given range.</returns>
        int GetRangeTextLength(TextRange range);

        /// <summary>
        /// Prepares the document for editing.
        /// </summary>
        void BeginUpdate();

        /// <summary>
        /// Finalises the editing process.
        /// </summary>
        void EndUpdate();

        /// <summary>
        /// Removes a given amount of characters from the document at the given offset.
        /// </summary>
        /// <param name="offset">The offset of the text to remove.</param>
        /// <param name="length">The length of the text to remove.</param>
        void Remove(int offset, int length);

        /// <summary>
        /// Inserts a string into the document at the given offset.
        /// </summary>
        /// <param name="offset">The offset of the string to insert.</param>
        /// <param name="text">The string to insert.</param>
        void Insert(int offset, string text);

        /// <summary>
        /// Replaces a given amount of characters from the document at the given offset with a new string.
        /// </summary>
        /// <param name="offset">The offset of the text to replace.</param>
        /// <param name="length">The length of the text to replace.</param>
        /// <param name="text">The text to use as a replacement.</param>
        void Replace(int offset, int length, string text);
    }

    public delegate void TextChangeEventHandler(object sender, TextChangeEventArgs e);

    public class TextChangeEventArgs : EventArgs
    {
        public TextChangeEventArgs(int offset, string removedText, string insertedText)
        {
            Offset = offset;
            RemovedText = removedText;
            InsertedText = insertedText;
        }

        public int Offset
        {
            get;
        }

        public string RemovedText
        {
            get;
        }

        public string InsertedText
        {
            get;
        }

        public int CalculateNewOffset(int oldOffset)
        {
            if (oldOffset >= Offset && oldOffset <= Offset - RemovedText.Length)
            {
                return oldOffset;
            }

            int delta = InsertedText.Length - RemovedText.Length;
            return oldOffset - delta;
        }
    }
}