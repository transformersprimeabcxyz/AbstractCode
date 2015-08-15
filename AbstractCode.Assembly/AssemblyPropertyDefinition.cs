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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsmResolver.Net.Metadata;

namespace AbstractCode.Assembly
{
    public class AssemblyPropertyDefinition : Symbols.PropertyDefinition
    {
        private readonly AssemblyTypeDefinition _declaringType;
        private readonly PropertyDefinition _definition;
        private Symbols.TypeDefinition _propertyType;

        internal AssemblyPropertyDefinition(AssemblyTypeDefinition declaringType, PropertyDefinition definition)
        {
            _declaringType = declaringType;
            _definition = definition;
        }

        public override bool CanRead
        {
            get { return _definition.GetMethod != null; }
        }

        public override bool CanWrite
        {
            get { return _definition.SetMethod != null; }
        }

        public override string Name
        {
            get { return _definition.Name; }
        }

        public override Symbols.TypeDefinition DeclaringType
        {
            get { return _declaringType; }
        }

        public override Symbols.TypeDefinition MemberType
        {
            get
            {
                if (_propertyType == null)
                    _propertyType = ((NetAssembly)this.Assembly).ResolveType(_definition.Signature.PropertyType).Member as Symbols.TypeDefinition;
                return _propertyType;
            }
        }

        public override Symbols.IScope GetDeclaringScope()
        {
            return _declaringType.GetScope();
        }
    }
}
