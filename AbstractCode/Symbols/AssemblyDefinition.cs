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
using AbstractCode.Symbols.Resolution;

namespace AbstractCode.Symbols
{
    /// <summary>
    /// Represents an assembly in a compilation.
    /// </summary>
    public abstract class AssemblyDefinition : INamedDefinition, IScopeProvider, INamespaceDefinitionProvider,
        ITypeDefinitionProvider
    {
        /// <summary>
        /// Represents the resolution scope of an assembly in a compilation.
        /// </summary>
        public class AssemblyDefinitionScope : AbstractScope<AssemblyDefinition>
        {
            public AssemblyDefinitionScope(AssemblyDefinition definition)
                : base(definition)
            {
            }

            public override ResolveResult ResolveIdentifier(string identifier)
            {
                var candidates = (from definition in Container.GetNamespaceDefinitions()
                    where definition.FullName == identifier
                    select definition).ToArray();

                if (candidates.Length > 0)
                    return new NamespaceResolveResult(candidates);

                return base.ResolveIdentifier(identifier);
            }
        }

        private AssemblyDefinitionScope _scope;

        /// <summary>
        /// Gets the compilation the assembly is referenced in.
        /// </summary>
        public Compilation Compilation
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Gets the full name of the assembly.
        /// </summary>
        public virtual string FullName
        {
            get { return string.Format("{0}, v{1}", Name, Version); }
        }

        /// <summary>
        /// Gets the version of the assembly.
        /// </summary>
        public abstract Version Version
        {
            get;
        }

        /// <summary>
        /// Gets the path of the assembly file.
        /// </summary>
        public abstract string FilePath
        {
            get;
        }

        public IScope GetDeclaringScope()
        {
            return Compilation?.GetScope();
        }

        public IScope GetScope()
        {
            return _scope ?? (_scope = new AssemblyDefinitionScope(this));
        }

        /// <summary>
        /// Yields a collection of namespaces this assembly defines.
        /// </summary>
        /// <returns>A collection of namespaces.</returns>
        public abstract IEnumerable<NamespaceDefinition> GetNamespaceDefinitions();

        /// <summary>
        /// Yields a collection of top-level types (excluding nested types) this assembly defines.
        /// </summary>
        /// <returns>A collection of types.</returns>
        public IEnumerable<TypeDefinition> GetTypeDefinitions()
        {
            foreach (var @namespace in GetNamespaceDefinitions())
                foreach (var type in @namespace.GetTypeDefinitions())
                    yield return type;
        }
    }
}