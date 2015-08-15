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
using AbstractCode.Symbols;

namespace AbstractCode.Assembly
{
    public class AssemblyNamespaceDefinition : NamespaceDefinition
    {
        private readonly NetAssembly _assembly;
        private readonly AssemblyNamespaceDefinition _parent;
        private readonly string _name;
        private readonly List<AssemblyTypeDefinition> _types = new List<AssemblyTypeDefinition>();
        private readonly List<AssemblyNamespaceDefinition> _namespaces = new List<AssemblyNamespaceDefinition>();
        
        internal AssemblyNamespaceDefinition(NetAssembly assembly, AssemblyNamespaceDefinition parent, string name)
        {
            _assembly = assembly;
            _parent = parent;
            _name = name;            
        }

        public override AssemblyDefinition Assembly
        {
            get { return _assembly; }
        }
        
        public override NamespaceDefinition Parent
        {
            get { return _parent; }
        }

        public override string Name
        {
            get { return _name; }
        }

        public override IEnumerable<NamespaceDefinition> GetNamespaceDefinitions()
        {
            return _namespaces.AsReadOnly();
        }

        public override IEnumerable<TypeDefinition> GetTypeDefinitions()
        {
            return _types.AsReadOnly();
        }

        internal void AddTypeDefinition(AssemblyTypeDefinition definition)
        {
            _types.Add(definition);
        }

        internal void AddNamespaceDefinition(AssemblyNamespaceDefinition definition)
        {
            _namespaces.Add(definition);
        }

    }
}
