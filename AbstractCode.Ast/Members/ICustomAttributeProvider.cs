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

namespace AbstractCode.Ast.Members
{
    public interface ICustomAttributeProvider : IAstNode
    {
        AstNodeCollection<CustomAttributeSection> CustomAttributeSections { get; }
    }

    public static class CustomAttributeProviderExtensions
    {
        public static IEnumerable<CustomAttribute> GetAllCustomAttributes(this ICustomAttributeProvider provider)
        {
            foreach (var section in provider.CustomAttributeSections)
            {
                foreach (var attribute in section.Attributes)
                    yield return attribute;
            }
        }
    }
}
