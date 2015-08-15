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
    public class AssemblyFieldDefinition : Symbols.FieldDefinition
    {
        private readonly AssemblyTypeDefinition _declaringType;
        private readonly FieldDefinition _definition;
        private Symbols.TypeDefinition _fieldType;

        internal AssemblyFieldDefinition(AssemblyTypeDefinition declaringType, FieldDefinition definition)
        {
            _declaringType = declaringType;
            _definition = definition;
        }

        public override bool IsConstant
        {
            get { return _definition.Attributes.HasFlag(FieldAttributes.Literal); }
        }

        public override object DefaultValue
        {
            get
            {
                if (_definition.Constant != null)
                    return _definition.Constant.Value;
                return null;
            }
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
                if (_fieldType == null)
                    _fieldType = ((NetAssembly)this.Assembly).ResolveType(_definition.Signature.FieldType).Member as Symbols.TypeDefinition;
                return _fieldType;
            }
        }

        public override Symbols.IScope GetDeclaringScope()
        {
            return _declaringType.GetScope();
        }
    }
}
