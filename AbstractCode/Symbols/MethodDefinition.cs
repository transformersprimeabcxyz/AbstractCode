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
using System.Collections.Generic;
using System.Linq;
using AbstractCode.Symbols.Resolution;

namespace AbstractCode.Symbols
{
    /// <summary>
    /// Represents a method in a type.
    /// </summary>
    public abstract class MethodDefinition : MemberDefinition, IScopeProvider
    {
        /// <summary>
        /// Represents the resolution scope of a method.
        /// </summary>
        public class MethodDefinitionScope : AbstractScope<MethodDefinition>
        {
            public MethodDefinitionScope(MethodDefinition definition)
                : base(definition)
            {
            }

            public override ResolveResult ResolveIdentifier(string identifier)
            {
                var parameters = (from definition in Container.GetParameters()
                    where definition.Name == identifier
                    select definition).ToArray();

                if (parameters.Length > 1)
                    return new AmbiguousMemberResolveResult(null); //TODO

                if (parameters.Length == 1)
                    return new LocalResolveResult(parameters[0]);

                return base.ResolveIdentifier(identifier);
            }
        }

        private MethodDefinitionScope _scope;
        
        public override string FullName
        {
            get
            {
                return string.Format("{0}({1})", base.FullName, string.Join(", ",
                    GetParameters()
                        .Select(
                            x =>
                                string.Format("{0} {1}", x.VariableType != null ? x.VariableType.FullName : "<unknown>",
                                    x.Name))));
            }
        }

        /// <summary>
        /// Yields a collection of parameters that are defined in the method.
        /// </summary>
        /// <returns>A collection of parameters.</returns>
        public abstract IEnumerable<ParameterDefinition> GetParameters();

        public IScope GetScope()
        {
            return _scope ?? (_scope = new MethodDefinitionScope(this));
        }

        /// <summary>
        /// Determines whether the method parameter types matches with the given argument types.
        /// </summary>
        /// <param name="argumentTypes">The types of the arguments to check.</param>
        /// <returns><c>True</c> if the method signature matches, <c>False</c> otherwise.</returns>
        public bool MatchesSignature(params TypeDefinition[] argumentTypes)
        {
            int current = 0;
            foreach (var parameter in GetParameters())
            {
                if (current >= argumentTypes.Length)
                    return false;

                if (!argumentTypes[current].IsBasedOn(parameter.VariableType))
                    return false;
                current++;
            }

            return current == argumentTypes.Length;
        }
    }
}