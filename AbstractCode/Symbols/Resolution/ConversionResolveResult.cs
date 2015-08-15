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

namespace AbstractCode.Symbols.Resolution
{
    /// <summary>
    /// Represents a result of an implicit or explicit cast or other type conversion.
    /// </summary>
    public class ConversionResolveResult : ResolveResult
    {
        public ConversionResolveResult(MemberResolveResult typeResolveResult)
            : base(typeResolveResult.Member as IScopeProvider)
        {
            var type = typeResolveResult.Member as TypeDefinition;
            if (type == null)
                throw new ArgumentException("Member resolve result does not hold a type definition.");

            TypeResolveResult = typeResolveResult;
        }

        /// <summary>
        /// Gets the type resolve result of the converted value.
        /// </summary>
        public MemberResolveResult TypeResolveResult
        {
            get;
            private set;
        }
    }
}