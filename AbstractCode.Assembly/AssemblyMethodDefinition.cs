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
    public class AssemblyMethodDefinition : Symbols.MethodDefinition
    {
        private readonly MethodDefinition _definition;
        private readonly AssemblyTypeDefinition _declaringType;
        private Symbols.TypeDefinition _methodType;
        private Symbols.ParameterDefinition[] _parameters;
 
        internal AssemblyMethodDefinition(AssemblyTypeDefinition declaringType, MethodDefinition definition)
        {
            _declaringType = declaringType;
            _definition = definition;
        }

        public override IEnumerable<Symbols.ParameterDefinition> GetParameters()
        {
            if (_parameters==null)
            {
                _parameters = new Symbols.ParameterDefinition[_definition.Parameters.Count];
                for (int i = 0; i < _definition.Parameters.Count; i++)
                    _parameters[i] = new AssemblyParameterDefinition(this, _definition.Signature.Parameters[i], _definition.Parameters[i]);
            }
            return _parameters;
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
                if (_methodType == null)
                    _methodType = ((NetAssembly)this.Assembly).ResolveType(_definition.Signature.ReturnType).Member as Symbols.TypeDefinition;
                return _methodType;
            }
        }

        public override Symbols.IScope GetDeclaringScope()
        {
            return _declaringType.GetScope();
        }
    }
}
