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
using System.Linq;

namespace AbstractCode.Symbols.Resolution
{
    /// <summary>
    /// Represents a method that is being invoked and resolved using its signature.
    /// </summary>
    public class InvocationResolveResult : MemberResolveResult
    {
        public InvocationResolveResult(MemberDefinition member, params ResolveResult[] arguments)
            : base(member)
        {
            Arguments = arguments.ToList().AsReadOnly();
        }

        public InvocationResolveResult(MemberDefinition member, IEnumerable<ResolveResult> arguments)
            : this(member, arguments.ToArray())
        {
        }

        /// <summary>
        /// Gets a collection of results representing the arguments of the invocation.
        /// </summary>
        public IList<ResolveResult> Arguments
        {
            get;
            private set;
        }
    }
}