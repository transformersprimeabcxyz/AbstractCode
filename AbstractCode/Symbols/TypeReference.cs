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
using AbstractCode.Symbols.Resolution;

namespace AbstractCode.Symbols
{
    /// <summary>
    /// Represents a reference to a type definition.
    /// </summary>
    public abstract class TypeReference
    {
        /// <summary>
        /// Resolves the type by using a resolution scope.
        /// </summary>
        /// <param name="scope">The resolution scope to use for resolving the reference.</param>
        /// <returns>The result of the reoslution process.</returns>
        public abstract ResolveResult Resolve(IScope scope);

        /// <summary>
        /// Gets the absolute base type of this reference.
        /// </summary>
        /// <returns></returns>
        public virtual TypeReference GetElementType()
        {
            return this;
        }
    }

    /// <summary>
    /// Provides the base for complex type references that are being built using a base type and a specification (e.g. an array or pointer specifier).
    /// </summary>
    public abstract class ComplexTypeReference : TypeReference
    {
        protected ComplexTypeReference(TypeReference baseType)
        {
            if (baseType == null)
                throw new ArgumentNullException(nameof(baseType));
            BaseType = baseType;
        }

        /// <summary>
        /// The type the complex type is based on.
        /// </summary>
        public TypeReference BaseType
        {
            get;
        }

        public override TypeReference GetElementType()
        {
            return BaseType.GetElementType();
        }
    }
}