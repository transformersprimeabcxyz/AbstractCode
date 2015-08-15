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
using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;
using AsmResolver;
using AsmResolver.Net;
using AsmResolver.Net.Metadata;
using AssemblyDefinition = AbstractCode.Symbols.AssemblyDefinition;
using TypeDefinition = AsmResolver.Net.Metadata.TypeDefinition;
using TypeReference = AsmResolver.Net.Metadata.TypeReference;

namespace AbstractCode.Assembly
{
    /// <summary>
    /// Represents an already compiled assembly that was written in a .NET language.
    /// </summary>
    public class NetAssembly : AssemblyDefinition
    {
        private readonly MetadataTable<TypeDefinition> _typeTable;
        private List<AssemblyNamespaceDefinition> _namespaceDefinitions;

        public NetAssembly(string filePath)
        {
            FilePath = filePath;
            WindowsAssembly = WindowsAssembly.FromFile(filePath);

           var tableStream = WindowsAssembly.NetDirectory?.MetadataHeader?.GetStream<TableStream>();

            if (tableStream == null)
                throw new BadImageFormatException("File does not appear to be a .NET executable.");

            var assemblyDefTable = tableStream.GetTable<AsmResolver.Net.Metadata.AssemblyDefinition>();
            _typeTable = tableStream.GetTable<TypeDefinition>();
            Definition = assemblyDefTable[0];
        }


        public WindowsAssembly WindowsAssembly
        {
            get;
        }

        public AsmResolver.Net.Metadata.AssemblyDefinition Definition
        {
            get;
        }

        public override string Name
        {
            get { return Definition.Name; }
        }

        public override Version Version
        {
            get { return Definition.Version; }
        }

        public override string FilePath
        {
            get;
        }

        public override IEnumerable<NamespaceDefinition> GetNamespaceDefinitions()
        {
            if (_namespaceDefinitions == null)
            {
                _namespaceDefinitions = new List<AssemblyNamespaceDefinition>();
                foreach (var definition in _typeTable)
                {
                    if (definition.DeclaringType == null)
                    {
                        var namespaceDefinition = GetNamespaceDefinition(definition.Namespace);
                        var typeDefinition = new AssemblyTypeDefinition(namespaceDefinition, definition);
                        namespaceDefinition.AddTypeDefinition(typeDefinition);
                    }
                }
            }
            return _namespaceDefinitions.AsReadOnly();
        }

        private AssemblyNamespaceDefinition GetNamespaceDefinition(string @namespace)
        {
            var existingDefinition = _namespaceDefinitions.FirstOrDefault(x => x.FullName == @namespace);
            if (existingDefinition != null)
                return existingDefinition;

            int dotIndex = @namespace.LastIndexOf('.');

            if (dotIndex == -1)
            {
                existingDefinition = new AssemblyNamespaceDefinition(this, null, @namespace);
                _namespaceDefinitions.Add(existingDefinition);
                return existingDefinition;
            }

            var parentDefinition = GetNamespaceDefinition(@namespace.Remove(dotIndex));

            existingDefinition = new AssemblyNamespaceDefinition(this, parentDefinition, @namespace.Substring(dotIndex + 1, @namespace.Length - dotIndex - 1));
            parentDefinition.AddNamespaceDefinition(existingDefinition);
            _namespaceDefinitions.Add(existingDefinition);

            return existingDefinition;
        }

        public virtual MemberResolveResult ResolveType(ITypeDescriptor reference)
        {
            var typeRefScope = reference.ResolutionScope;

            if (typeRefScope is ModuleDefinition)
            {
                var @namespace = GetNamespaceDefinitions().FirstOrDefault(x => x.FullName == reference.Namespace);
                if (@namespace != null)
                {
                    var result = @namespace.GetScope().ResolveIdentifier(reference.Name) as MemberResolveResult;
                    if (result != null)
                        return result;
                }
            }
            
            if (typeRefScope is AssemblyReference)
            {
                var namespaceResult = Compilation.GetScope().ResolveIdentifier(reference.Namespace) as NamespaceResolveResult;
                if (namespaceResult != null)
                {
                    return (MemberResolveResult)namespaceResult.ScopeProvider.GetScope().ResolveIdentifier(reference.Name);
                }
            }

            if (typeRefScope is TypeReference)
            {
                var parentTypeResolve = ResolveType(typeRefScope as TypeReference);
                if (parentTypeResolve != null)
                    return (MemberResolveResult)parentTypeResolve.ScopeProvider.GetScope().ResolveIdentifier(reference.Name);
            }

            return new UnknownIdentifierResolveResult(reference.FullName);
        }
    }
}
