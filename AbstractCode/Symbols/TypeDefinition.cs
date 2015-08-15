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
    /// Provides methods for representing an object that holds type definitions.
    /// </summary>
    public interface ITypeDefinitionProvider
    {
        /// <summary>
        /// Yields a collection of types that are defined by this object.
        /// </summary>
        /// <returns>A collection of types.</returns>
        IEnumerable<TypeDefinition> GetTypeDefinitions();
    }

    /// <summary>
    /// Provides extension methods for any <see cref="ITypeDefinitionProvider"/> instance.
    /// </summary>
    public static class TypeDefinitionProviderExtensions
    {
        /// <summary>
        /// Yields a collection of types that are defined by this object, using a specific predicate as a filter.
        /// </summary>
        /// <param name="provider">The provider to get the types from.</param>
        /// <param name="predicate">The filter to apply.</param>
        /// <returns>A filtered collection of types.</returns>
        public static IEnumerable<TypeDefinition> GetTypeDefinitions(this ITypeDefinitionProvider provider,
            Predicate<TypeDefinition> predicate)
        {
            return from definition in provider.GetTypeDefinitions()
                where predicate(definition)
                select definition;
        }
    }

    /// <summary>
    /// Represents a type in a namespace or an assembly.
    /// </summary>
    public abstract class TypeDefinition : MemberDefinition, IScopeProvider, ITypeDefinitionProvider
    {
        /// <summary>
        /// Represents the resolution scope of a type.
        /// </summary>
        public class TypeDefinitionScope : AbstractScope<TypeDefinition>
        {
            public TypeDefinitionScope(TypeDefinition definition)
                : base(definition)
            {
            }

            public override ResolveResult ResolveIdentifier(string identifier)
            {
                var candidates = new List<MemberDefinition>();

                candidates.AddRange(from member in Container.GetMembers()
                    where member.Name == identifier
                    select member);

                if (Container.MemberType != null)
                {
                    var parentResult = Container.MemberType.GetScope().ResolveIdentifier(identifier);
                    var memberResult = parentResult as MemberResolveResult;
                    var ambiguousResult = parentResult as AmbiguousMemberResolveResult;
                    if (ambiguousResult != null)
                        candidates.AddRange(ambiguousResult.Candidates);
                    else if (memberResult?.Member != null)
                        candidates.Add(memberResult.Member);
                }

                if (candidates.Count > 1)
                    return new AmbiguousMemberResolveResult(candidates);

                if (candidates.Count == 1)
                    return new MemberResolveResult(candidates[0]);

                return base.ResolveIdentifier(identifier);
            }
        }

        private TypeDefinitionScope _scope;

        /// <summary>
        /// Gets the namespace that defines the type.
        /// </summary>
        public abstract NamespaceDefinition Namespace
        {
            get;
        }

        /// <summary>
        /// Gets the full name of the type.
        /// </summary>
        public override string FullName
        {
            get { return Namespace != null ? string.Format("{0}.{1}", Namespace.FullName, Name) : Name; }
        }

        /// <summary>
        /// Yields a collection of members that are defined in the type.
        /// </summary>
        /// <returns>A collection of fields, methods, properties, events, and/or type, or an empty collection if no member is defined.</returns>
        public abstract IEnumerable<MemberDefinition> GetMembers();

        /// <summary>
        /// Yields a collection of nested types that are defined in the type.
        /// </summary>
        /// <returns>A collection of types, or an empty collection if no nested type is defined.</returns>
        public abstract IEnumerable<TypeDefinition> GetNestedTypes();

        /// <summary>
        /// Yields a collection of fields that are defined in the type.
        /// </summary>
        /// <returns>A collection of fields, or an empty collection if no field is defined.</returns>
        public abstract IEnumerable<FieldDefinition> GetFields();

        /// <summary>
        /// Yields a collection of events that are defined in the type.
        /// </summary>
        /// <returns>A collection of events, or an empty collection if no event is defined.</returns>
        public abstract IEnumerable<EventDefinition> GetEvents();

        /// <summary>
        /// Yields a collection of properties that are defined in the type.
        /// </summary>
        /// <returns>A collection of properties, or an empty collection if no property is defined.</returns>
        public abstract IEnumerable<PropertyDefinition> GetProperties();

        /// <summary>
        /// Yields a collection of methods that are defined in the type.
        /// </summary>
        /// <returns>A collection of methods, or an empty collection if no method is defined.</returns>
        public abstract IEnumerable<MethodDefinition> GetMethods();

        /// <summary>
        /// Yields a collection of members with the specified name.
        /// </summary>
        /// <param name="name">The name of the members to search.</param>
        /// <returns>A collection of members sharing the same name, or an empty collection if no match was found.</returns>
        public virtual IEnumerable<MemberDefinition> GetMembersByName(string name)
        {
            return from member in GetMembers()
                where member.Name == name
                select member;
        }

        public IScope GetScope()
        {
            return _scope ?? (_scope = new TypeDefinitionScope(this));
        }

        IEnumerable<TypeDefinition> ITypeDefinitionProvider.GetTypeDefinitions()
        {
            return GetNestedTypes();
        }

        /// <summary>
        /// Determines whether the type is derived from the specified type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>True</c> when the type is derived from the specified type, <c>False</c> otherwise.</returns>
        public bool IsBasedOn(TypeDefinition type)
        {
            if (type == null)
                return false;

            var currentType = this;
            while (currentType != null)
            {
                if (currentType.FullName == type.FullName)
                    return true;
                currentType = currentType.MemberType;
            }
            return false;
        }

        /// <summary>
        /// Gets the hierarchy index of the hierarchy tree of the type.
        /// </summary>
        /// <returns></returns>
        public int GetHierarchyIndex()
        {
            int index = -1;
            var currentType = this;
            while (currentType != null)
            {
                index++;
                currentType = currentType.MemberType;
            }

            return index;
        }
    }
}