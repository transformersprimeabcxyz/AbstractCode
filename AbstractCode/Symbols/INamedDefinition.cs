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
namespace AbstractCode.Symbols
{
    /// <summary>
    /// Provides properties for giving a definition in the type system a name.
    /// </summary>
    public interface INamedDefinition : IScopeMember
    {
        /// <summary>
        /// Gets the name of this member.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets the full name of this member.
        /// </summary>
        string FullName
        {
            get;
        }
    }
}