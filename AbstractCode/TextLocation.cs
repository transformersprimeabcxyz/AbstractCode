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
    /// Represents a location in a text document.
    /// </summary>
    public struct TextLocation
    {
        public static readonly TextLocation Empty = new TextLocation(0, 0);

        public TextLocation(int line, int column)
        {
            Line = line;
            Column = column;
        }

        /// <summary>
        /// Gets the line number of the location.
        /// </summary>
        public int Line
        {
            get;
        }

        /// <summary>
        /// Gets the column number of the location.
        /// </summary>
        public int Column
        {
            get;
        }

        /// <summary>
        /// Translates the text location by a given amount of lines and columns.
        /// </summary>
        /// <param name="deltaLine">The line translation to use.</param>
        /// <param name="deltaColumn">The column translation to use.</param>
        /// <returns>The translated text location.</returns>
        public TextLocation Offset(int deltaLine, int deltaColumn)
        {
            return new TextLocation(Line + deltaLine, Column + deltaColumn);
        }

        public override int GetHashCode()
        {
            return Line ^ Column;
        }

        public override bool Equals(object obj)
        {
            if (obj is TextLocation)
                return (TextLocation)obj == this;
            return false;
        }

        public static bool operator ==(TextLocation a, TextLocation b)
        {
            return a.Line == b.Line && a.Column == b.Column;
        }

        public static bool operator !=(TextLocation a, TextLocation b)
        {
            return !(a == b);
        }

        public static bool operator >(TextLocation a, TextLocation b)
        {
            return a.Line > b.Line || (a.Line == b.Line && a.Column > b.Column);
        }

        public static bool operator >=(TextLocation a, TextLocation b)
        {
            return !(a < b);
        }

        public static bool operator <(TextLocation a, TextLocation b)
        {
            return a.Line < b.Line || (a.Line == b.Line && a.Column < b.Column);
        }

        public static bool operator <=(TextLocation a, TextLocation b)
        {
            return !(a > b);
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}}}", Line, Column);
        }
    }
}