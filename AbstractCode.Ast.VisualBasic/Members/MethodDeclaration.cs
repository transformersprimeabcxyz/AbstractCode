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
using AbstractCode.Ast.Types;
using MethodDeclarationBase = AbstractCode.Ast.Members.MethodDeclaration;

namespace AbstractCode.Ast.VisualBasic.Members
{
    public class MethodDeclaration : MethodDeclarationBase, IVBMethodDeclaration
    {
        public static readonly AstNodeTitle<AstToken> VariantElementTitle = new AstNodeTitle<AstToken>("VariantElement");
        public static readonly AstNodeTitle<AstToken> AsKeywordTitle = new AstNodeTitle<AstToken>("AsKeywordRole");

        public static readonly AstNodeTitle<HandlesClause> HandlesClauseTitle =
            new AstNodeTitle<HandlesClause>("HandlesClauseRole");

        public static readonly AstNodeTitle<ImplementsClause> ImplementsClauseTitle =
            new AstNodeTitle<ImplementsClause>("ImplementsClauseRole");

        internal static readonly Dictionary<string, MethodDeclarationVariant> VariantMapping =
            new Dictionary<string, MethodDeclarationVariant>(StringComparer.OrdinalIgnoreCase)
            {
                { "Function", MethodDeclarationVariant.Function },
                { "Sub", MethodDeclarationVariant.Sub },
            };

        public static MethodDeclarationVariant VariantFromString(string variantString)
        {
            MethodDeclarationVariant variant;
            if (!VariantMapping.TryGetValue(variantString, out variant))
                throw new ArgumentException("Variant is not recognized as a valid Visual Basic method variant.");
            return variant;
        }

        public static string VariantToString(MethodDeclarationVariant variant)
        {
            var pair = VariantMapping.FirstOrDefault(x => x.Value == variant);
            if (string.IsNullOrEmpty(pair.Key))
                throw new ArgumentException("Variant is not supported in Visual Basic.");
            return pair.Key;
        }

        public MethodDeclaration()
        {
        }

        public MethodDeclaration(string name, TypeReference returnType)
            : base(name, returnType)
        {
        }

        public MethodDeclarationVariant Variant
        {
            get;
            set;
        }

        public AstToken VariantElement
        {
            get { return GetChildByTitle(VariantElementTitle); }
            set { SetChildByTitle(VariantElementTitle, value); }
        }

        bool IVBMethodDeclaration.CanHoldHandlesClause
        {
            get { return true; }
        }

        public HandlesClause HandlesClause
        {
            get { return GetChildByTitle(HandlesClauseTitle); }
            set { SetChildByTitle(HandlesClauseTitle, value); }
        }

        bool IVBMethodDeclaration.CanHoldImplementsClause
        {
            get { return true; }
        }

        public ImplementsClause ImplementsClause
        {
            get { return GetChildByTitle(ImplementsClauseTitle); }
            set { SetChildByTitle(ImplementsClauseTitle, value); }
        }

        public AstToken AsKeyword
        {
            get { return GetChildByTitle(AsKeywordTitle); }
            set { SetChildByTitle(AsKeywordTitle, value); }
        }
    }

    public enum MethodDeclarationVariant
    {
        Function,
        Sub,
    }
}