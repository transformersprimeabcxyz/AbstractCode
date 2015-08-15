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
using System.Collections.ObjectModel;

namespace AbstractCode.Symbols.Resolution
{
    /// <summary>
    /// Represents a resolved namespace.
    /// </summary>
    public class NamespaceResolveResult : ResolveResult
    {
        public NamespaceResolveResult(NamespaceDefinition resolvedDefinition)
            : this(new[] { resolvedDefinition })
        {
        }

        public NamespaceResolveResult(IList<NamespaceDefinition> resolvedDefinitions)
            : base(new MergedNamespaceDefinition(resolvedDefinitions))
        {
            ResolvedDefinitions = new ReadOnlyCollection<NamespaceDefinition>(resolvedDefinitions);
        }

        /// <summary>
        /// Gets a collection of namespaces that are resolved.
        /// </summary>
        public IList<NamespaceDefinition> ResolvedDefinitions
        {
            get;
            private set;
        }
    }
}