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
namespace AbstractCode.Symbols.Resolution
{
    /// <summary>
    /// Represents a failed identifier resolution.
    /// </summary>
    public class UnknownIdentifierResolveResult : MemberResolveResult
    {
        public UnknownIdentifierResolveResult(string identifier)
            : base(null)
        {
            Identifier = identifier;
        }

        /// <summary>
        /// Gets a value indicating whether the resolution failed.
        /// </summary>
        public override bool IsError
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the identifier of the object that was tried to be resolved.
        /// </summary>
        public string Identifier
        {
            get;
            protected set;
        }
    }
}