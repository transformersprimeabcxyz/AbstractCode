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
using AsmResolver.Net.Metadata;
using AsmResolver.Net.Signatures;

namespace AbstractCode.Assembly
{
    public class AssemblyParameterDefinition : Symbols.ParameterDefinition
    {
        private readonly AssemblyMethodDefinition _method;
        private readonly ParameterSignature _signature;
        private readonly ParameterDefinition _definition;
        private Symbols.TypeDefinition _parameterType;
        

        internal AssemblyParameterDefinition(AssemblyMethodDefinition method, ParameterSignature signature, ParameterDefinition definition)
        {
            _method = method;
            _signature = signature;
            _definition = definition;
        }

        public override int Index
        {
            get { return -1; } // TODO
        }

        public override bool IsByVal
        {
            get { return !IsByRef; }
        }

        public override bool IsByRef
        {
            get { return _definition.Attributes.HasFlag(ParameterAttributes.In | ParameterAttributes.Out); }
        }

        public override bool IsOut
        {
            get { return _definition.Attributes.HasFlag(ParameterAttributes.Out); }
        }

        public override string Name
        {
            get { return _definition.Name; }
        }

        public override Symbols.TypeDefinition VariableType
        {
            get
            {
                if (_parameterType == null)
                    _parameterType = ((NetAssembly)_method.Assembly).ResolveType(_signature.ParameterType).Member as Symbols.TypeDefinition;
                return _parameterType;
            }
        }

        public override Symbols.IScope GetDeclaringScope()
        {
            return _method.GetScope();
        }
    }
}
