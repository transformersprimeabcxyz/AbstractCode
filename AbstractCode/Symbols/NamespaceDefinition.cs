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
    /// Provides methods for representing an object that holds namespace definitions.
    /// </summary>
    public interface INamespaceDefinitionProvider
    {
        /// <summary>
        /// Yields a collection of namespaces that are defined by this object.
        /// </summary>
        /// <returns>A collection of namespaces.</returns>
        IEnumerable<NamespaceDefinition> GetNamespaceDefinitions();
    }

    /// <summary>
    /// Represents a namespace in an assembly.
    /// </summary>
    public abstract class NamespaceDefinition : INamespaceDefinitionProvider, ITypeDefinitionProvider, INamedDefinition,
        IScopeProvider
    {
        /// <summary>
        /// Represents the resolution scope of a namespace.
        /// </summary>
        public class NamespaceDefinitionScope : AbstractScope<NamespaceDefinition>
        {
            public NamespaceDefinitionScope(NamespaceDefinition container)
                : base(container)
            {
            }

            public override ResolveResult ResolveIdentifier(string identifier)
            {
                var definitions = (from type in Container.GetTypeDefinitions()
                    where type.Name == identifier
                    select type).ToArray();

                if (definitions.Length > 1)
                    return new AmbiguousMemberResolveResult(definitions);
                if (definitions.Length == 1)
                    return new MemberResolveResult(definitions[0]);

                if (definitions.Length == 0)
                {
                    var nestedNamespaces = (from @namespace in Container.GetNamespaceDefinitions()
                        where @namespace.Name == identifier
                        select @namespace).ToArray();

                    if (nestedNamespaces.Length >= 1)
                        return new NamespaceResolveResult(nestedNamespaces);
                }

                return base.ResolveIdentifier(identifier);
            }
        }

        private NamespaceDefinitionScope _scope;
        
        /// <summary>
        /// Gets the assembly that defines the namespace.
        /// </summary>
        public abstract AssemblyDefinition Assembly
        {
            get;
        }

        /// <summary>
        /// Gets the namespace that defines the namespace.
        /// </summary>
        public abstract NamespaceDefinition Parent
        {
            get;
        }

        /// <summary>
        /// Gets the name of the namespace.
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Gets the full name of the namespace, including the parent namespaces.
        /// </summary>
        public string FullName
        {
            get { return Parent != null ? string.Format("{0}.{1}", Parent.FullName, Name) : Name; }
        }

        public abstract IEnumerable<NamespaceDefinition> GetNamespaceDefinitions();

        public abstract IEnumerable<TypeDefinition> GetTypeDefinitions();

        public IScope GetScope()
        {
            return _scope ?? (_scope = new NamespaceDefinitionScope(this));
        }

        public virtual IScope GetDeclaringScope()
        {
            return Parent != null 
                ? Parent.GetScope()
                : Assembly?.GetScope();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return FullName;
        }
    }

    /// <summary>
    /// Represents a collection of namespaces that share the same name.
    /// </summary>
    public sealed class MergedNamespaceDefinition : NamespaceDefinition
    {
        private readonly NamespaceDefinition[] _definitions;

        public MergedNamespaceDefinition(IEnumerable<NamespaceDefinition> namespaces)
            : this(namespaces.ToArray())
        {
        }

        public MergedNamespaceDefinition(NamespaceDefinition[] namespaces)
        {
            var name = namespaces[0].FullName;

            if (namespaces.Any(@namespace => @namespace.FullName != name))
                throw new ArgumentException("Namespaces must be all of the same name.", "namespaces");

            _definitions = namespaces;
        }

        public override AssemblyDefinition Assembly
        {
            get { return null; }
        }

        public override NamespaceDefinition Parent
        {
            get { return null; }
        }

        public override string Name
        {
            get { return _definitions[0].Name; }
        }

        public override IEnumerable<NamespaceDefinition> GetNamespaceDefinitions()
        {
            foreach (var @namespace in _definitions)
            {
                foreach (var nestedNamespace in @namespace.GetNamespaceDefinitions())
                    yield return nestedNamespace;
            }
        }

        public override IEnumerable<TypeDefinition> GetTypeDefinitions()
        {
            foreach (var @namespace in _definitions)
            {
                foreach (var type in @namespace.GetTypeDefinitions())
                    yield return type;
            }
        }
    }
}