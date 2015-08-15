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
    /// Represents a collection of ambiguous members that were found during a resolution process.
    /// </summary>
    public class AmbiguousMemberResolveResult : MemberResolveResult
    {
        public AmbiguousMemberResolveResult(IEnumerable<MemberDefinition> candidates)
            : base(null) // TODO
        {
            Candidates = candidates.ToList().AsReadOnly();
            Member = Candidates[0];
        }

        /// <summary>
        /// Gets a collection of the ambiguous candidates that were found.
        /// </summary>
        public IList<MemberDefinition> Candidates
        {
            get;
        }

        public override bool IsError
        {
            get { return true; }
        }
        
        public override IEnumerable<MemberDefinition> GetMembers()
        {
            return Candidates;
        }
    }
}