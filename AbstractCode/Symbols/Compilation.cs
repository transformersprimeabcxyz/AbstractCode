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
using System.Linq;
using AbstractCode.Collections.Generic;
using AbstractCode.Symbols.Resolution;

namespace AbstractCode.Symbols
{
    /// <summary>
    /// Represents a compilation of an assembly, using referenced assemblies.
    /// </summary>
    public class Compilation : IScopeProvider
    {
        /// <summary>
        /// Represents the resolution scope of a compilation.
        /// </summary>
        public class CompilationScope : IScope
        {
            private readonly Compilation _compilation;

            public CompilationScope(Compilation compilation)
            {
                _compilation = compilation;
            }
            
            public IScopeProvider GetDeclaringProvider()
            {
                return null;
            }

            public ResolveResult ResolveIdentifier(string identifier)
            {
                var candidates = (from assembly in _compilation.Assemblies
                    from @namespace in assembly.GetNamespaceDefinitions()
                    where @namespace.FullName == identifier
                    select @namespace).ToArray();

                if (candidates.Length == 0)
                    return new UnknownIdentifierResolveResult(identifier);
                return new NamespaceResolveResult(candidates);
            }
        }

        private CompilationScope _scope;

        public Compilation(AssemblyDefinition mainAssembly)
        {
            MainAssembly = mainAssembly;
            mainAssembly.Compilation = this;
            Assemblies = new EventBasedCollection<AssemblyDefinition>()
            {
                mainAssembly,
            };

            Assemblies.InsertingItem += Assemblies_InsertingItem;
            Assemblies.InsertedItem += Assemblies_InsertedItem;
            Assemblies.RemovedItem += Assemblies_RemovedItem;
        }

        /// <summary>
        /// Gets the main assembly to be compiled.
        /// </summary>
        public AssemblyDefinition MainAssembly
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the referenced assemblies to use for the compilation process.
        /// </summary>
        public EventBasedCollection<AssemblyDefinition> Assemblies
        {
            get;
        }

        public virtual IScope GetScope()
        {
            if (_scope == null)
                _scope = new CompilationScope(this);
            return _scope;
        }

        public IScope GetDeclaringScope()
        {
            return null;
        }

        private static void Assemblies_InsertingItem(object sender, CollectionChangingEventArgs<AssemblyDefinition> e)
        {
            if (e.TargetObject.Compilation != null)
            {
                e.Cancel = true;
                throw new InvalidOperationException("Assembly reference is already added to another compilation.");
            }
        }

        private static void Assemblies_RemovedItem(object sender, CollectionChangedEventArgs<AssemblyDefinition> e)
        {
            e.TargetObject.Compilation = null;
        }

        private void Assemblies_InsertedItem(object sender, CollectionChangedEventArgs<AssemblyDefinition> e)
        {
            e.TargetObject.Compilation = this;
        }
    }
}