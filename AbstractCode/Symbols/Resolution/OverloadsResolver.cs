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

namespace AbstractCode.Symbols.Resolution
{
    public static class OverloadsResolver
    {
        public static ResolveResult Resolve(IEnumerable<MemberDefinition> members, TypeDefinition[] argumentTypes)
        {
            var matches = (from candidate in members
                let method = (MethodDefinition)candidate
                where method.MatchesSignature(argumentTypes)
                select method).ToArray();

            // no matching elements
            if (matches.Length == 0)
                return ErrorResolveResult.Instance;

            // one matching element
            if (matches.Length == 1)
                return new MemberResolveResult(matches[0]);

            // multiple elements
            var bestMatch = matches[0];
            for (int i = 1; i < matches.Length; i++)
            {
            }

            return null;
        }

        private static int CompareSignature(MethodDefinition method1, MethodDefinition method2,
            TypeDefinition[] argumentTypes)
        {
            var parameters1 = method1.GetParameters().ToArray();
            var parameters2 = method1.GetParameters().ToArray();

            for (int i = 0; i < parameters1.Length; i++)
            {
                var index1 = parameters1[i].VariableType.GetHierarchyIndex();
                var index2 = parameters2[i].VariableType.GetHierarchyIndex();
            }

            return 0;
        }
    }
}