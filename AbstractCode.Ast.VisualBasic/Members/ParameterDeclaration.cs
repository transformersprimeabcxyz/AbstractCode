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
using ParameterDeclarationBase = AbstractCode.Ast.Members.ParameterDeclaration;

namespace AbstractCode.Ast.VisualBasic.Members
{
    public class ParameterDeclaration : ParameterDeclarationBase
    {
        public static readonly AstNodeTitle<AstToken> VariantElementTitle = new AstNodeTitle<AstToken>("VariantElement");
        public static readonly AstNodeTitle<AstToken> AsKeywordTitle = new AstNodeTitle<AstToken>("AsKeywordRole");

        internal static readonly Dictionary<string, ParameterModifier> ModifierMapping =
            new Dictionary<string, ParameterModifier>(StringComparer.OrdinalIgnoreCase)
            {
                { "ByVal", ParameterModifier.None },
                { "", ParameterModifier.None },
                { "ByRef", ParameterModifier.Ref },
                { "ParamArray", ParameterModifier.Params },
            };

        public static ParameterModifier ModifierFromString(string variantString)
        {
            ParameterModifier variant;
            if (!ModifierMapping.TryGetValue(variantString, out variant))
                throw new ArgumentException("Modifier is not recognized as a valid Visual Basic parameter modifier.");
            return variant;
        }

        public static string ModifierToString(ParameterModifier variant)
        {
            var pair = ModifierMapping.FirstOrDefault(x => x.Value == variant);
            if (string.IsNullOrEmpty(pair.Key))
                throw new ArgumentException("Modifier is not supported in Visual Basic.");
            return pair.Key;
        }

        public override ParameterModifier ParameterModifier
        {
            get;
            set;
        }
    }
}