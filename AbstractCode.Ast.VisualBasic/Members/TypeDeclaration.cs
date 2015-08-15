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
using AbstractCode.Ast.Members;
using TypeDeclarationBase = AbstractCode.Ast.Members.TypeDeclaration;

namespace AbstractCode.Ast.VisualBasic.Members
{
    public class TypeDeclaration : TypeDeclarationBase
    {
        public static readonly AstNodeTitle<AstToken> ImplementsKeywordTitle =
            new AstNodeTitle<AstToken>("ImplementsKeyword");

        internal static readonly Dictionary<string, TypeVariant> TypeVariantMapping =
            new Dictionary<string, TypeVariant>(StringComparer.OrdinalIgnoreCase)
            {
                { "Class", TypeVariant.Class },
                { "Structure", TypeVariant.ValueType },
                { "Enum", TypeVariant.Enum },
                { "Interface", TypeVariant.Interface },
            };

        public static TypeVariant TypeVariantFromString(string variantString)
        {
            TypeVariant variant;
            if (!TypeVariantMapping.TryGetValue(variantString, out variant))
                throw new ArgumentException("Type variant is not recognized as a valid Visual Basic type variant.");
            return variant;
        }

        public static string TypeVariantToString(TypeVariant variant)
        {
            var pair = TypeVariantMapping.FirstOrDefault(x => x.Value == variant);
            if (string.IsNullOrEmpty(pair.Key))
                throw new ArgumentException("Type variant is not supported in Visual Basic.");
            return pair.Key;
        }

        public override TypeVariant TypeVariant
        {
            get;
            set;
        }

        public AstToken ImplementsKeyword
        {
            get { return GetChildByTitle(ImplementsKeywordTitle); }
            set { SetChildByTitle(ImplementsKeywordTitle, value); }
        }
    }
}