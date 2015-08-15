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
    public class AssemblyTypeDefinition : Symbols.TypeDefinition
    {
        private readonly TypeDefinition _definition;
        private readonly AssemblyTypeDefinition _parentType;
        private readonly AssemblyNamespaceDefinition _namespaceDefinition;

        private Symbols.TypeDefinition _baseType;
        private AssemblyTypeDefinition[] _nestedTypes;
        private AssemblyFieldDefinition[] _fields;
        private AssemblyEventDefinition[] _events;
        private AssemblyPropertyDefinition[] _properties;
        private AssemblyMethodDefinition[] _methods;

        internal AssemblyTypeDefinition(AssemblyTypeDefinition parentType, TypeDefinition definition)
        {
            _parentType = parentType;
            _definition = definition;
        }

        internal AssemblyTypeDefinition(AssemblyNamespaceDefinition namespaceDef, TypeDefinition definition)
        {
            _namespaceDefinition = namespaceDef;
            _definition = definition;
        }

        public override Symbols.AssemblyDefinition Assembly
        {
            get { return _namespaceDefinition.Assembly; }
        }

        public override Symbols.NamespaceDefinition Namespace
        {
            get { return _namespaceDefinition; }
        }

        public override IEnumerable<Symbols.MemberDefinition> GetMembers()
        {
            foreach (var type in GetNestedTypes())
                yield return type;

            foreach (var field in GetFields())
                yield return field;

            foreach (var @event in GetEvents())
                yield return @event;

            foreach (var property in GetProperties())
                yield return property;

            foreach (var method in GetMethods())
                yield return method;
        }

        public override IEnumerable<Symbols.TypeDefinition> GetNestedTypes()
        {
            if (_nestedTypes == null)
            {
                if (_definition.NestedClasses.Count == 0)
                    return _nestedTypes = new AssemblyTypeDefinition[0];

                _nestedTypes = new AssemblyTypeDefinition[_definition.NestedClasses.Count];
                for (int i = 0; i < _nestedTypes.Length; i++)
                    _nestedTypes[i] = new AssemblyTypeDefinition(this, _definition.NestedClasses[i].Class);
            }

            return _nestedTypes;
        }

        public override IEnumerable<Symbols.FieldDefinition> GetFields()
        {
            if (_fields == null)
            {
                if (_definition.Fields.Count == 0)
                    return _fields = new AssemblyFieldDefinition[0];

                _fields = new AssemblyFieldDefinition[_definition.Fields.Count];
                for (int i = 0; i < _fields.Length; i++)
                    _fields[i] = new AssemblyFieldDefinition(this, _definition.Fields[i]);
            }

            return _fields;
        }

        public override IEnumerable<Symbols.EventDefinition> GetEvents()
        {
            if (_events == null)
            {
                var eventMap = _definition.EventMap;
                if (eventMap == null || eventMap.Events.Count == 0)
                    return _events = new AssemblyEventDefinition[0];
                 
                var rawEvents = eventMap.Events;
                _events = new AssemblyEventDefinition[rawEvents.Count];
                for (int i = 0; i < _events.Length; i++)
                    _events[i] = new AssemblyEventDefinition(this, rawEvents[i]);
            }

            return _events;
        }

        public override IEnumerable<Symbols.PropertyDefinition> GetProperties()
        {
            if (_properties == null)
            {
                var propertyMap = _definition.PropertyMap;
                if (propertyMap == null || propertyMap.Properties.Count == 0)
                    return _properties = new AssemblyPropertyDefinition[0];

                var rawProperties = propertyMap.Properties;
                _properties = new AssemblyPropertyDefinition[rawProperties.Count];
                for (int i = 0; i < _properties.Length; i++)
                    _properties[i] = new AssemblyPropertyDefinition(this, rawProperties[i]);
            }

            return _properties;
        }

        public override IEnumerable<Symbols.MethodDefinition> GetMethods()
        {
            if (_methods == null)
            {
                if (_definition.Methods.Count == 0)
                    return _methods = new AssemblyMethodDefinition[0];

                _methods = new AssemblyMethodDefinition[_definition.Methods.Count];
                for (int i = 0; i < _methods.Length; i++)
                    _methods[i] = new AssemblyMethodDefinition(this, _definition.Methods[i]);
            }

            return _methods;
        }

        public override string Name
        {
            get { return _definition.Name; }
        }

        public override Symbols.TypeDefinition DeclaringType
        {
            get { return _parentType; }
        }

        public override Symbols.TypeDefinition MemberType
        {
            get
            {
                if (_baseType == null && _definition.BaseType != null)
                    _baseType = ((NetAssembly)Assembly).ResolveType(_definition.BaseType).Member as Symbols.TypeDefinition;
                return _baseType;
            }
        }

        public override Symbols.IScope GetDeclaringScope()
        {
            if (_parentType != null)
                return _parentType.GetScope();
            if (_namespaceDefinition != null)
                return _namespaceDefinition.GetScope();
            return null;
        }
    }
}
