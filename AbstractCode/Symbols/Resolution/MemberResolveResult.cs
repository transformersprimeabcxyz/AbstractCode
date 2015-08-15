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
using System.Collections.Generic;

namespace AbstractCode.Symbols.Resolution
{
    /// <summary>
    /// Represents a resolution of a member.
    /// </summary>
    public class MemberResolveResult : ResolveResult
    {
        public MemberResolveResult(MemberDefinition member)
            : base(member == null ? null : (member is TypeDefinition ? (TypeDefinition)member : member.MemberType))
        {
            Member = member;
        }

        /// <summary>
        /// Gets the member that is resolved.
        /// </summary>
        public MemberDefinition Member
        {
            get;
            protected set;
        }

        /// <summary>
        /// Yields a collection of candidate members that are resolved.
        /// </summary>
        /// <returns>A collection of <see cref="MemberDefinition"/>.</returns>
        public virtual IEnumerable<MemberDefinition> GetMembers()
        {
            if (Member != null)
                yield return Member;
        }
    }
}