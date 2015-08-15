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
    /// Provides styles of brace placements in a source code file.
    /// </summary>
    public enum BraceStyle
    {
        /// <summary>
        /// Specifies that braces are placed at the same line of the statement.
        /// </summary>
        EndOfLine,

        /// <summary>
        /// Specifies that braces are placed at the same line of the statement, but no extra space is used.
        /// </summary>
        EndOfLineNoSpacing,

        /// <summary>
        /// Specifies that braces are placed at the next line of the statement.
        /// </summary>
        NextLine,

        /// <summary>
        /// Specifies that braces are placed at the next line of the statement, but the amount of indentation is increased.
        /// </summary>
        NextLineIndented
    }
}