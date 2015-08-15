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

namespace AbstractCode.Ast.VisualBasic.Statements
{
    public class EndScopeBlockClause : AstNode, IVisualBasicVisitable
    {
        public static readonly AstNodeTitle<AstToken> EndKeywordTitle = new AstNodeTitle<AstToken>("EndKeyword");
        public static readonly AstNodeTitle<AstToken> VariantKeywordTitle = new AstNodeTitle<AstToken>("VariantKeyword");

        internal static readonly Dictionary<string, EndScopeBlockVariant> VariantMapping =
            new Dictionary<string, EndScopeBlockVariant>(StringComparer.OrdinalIgnoreCase)
            {
                { "Namespace", EndScopeBlockVariant.Namespace },
                { "Class", EndScopeBlockVariant.Class },
                { "Structure", EndScopeBlockVariant.Structure },
                { "Interface", EndScopeBlockVariant.Interface },
                { "Enum", EndScopeBlockVariant.Enum },
                { "Function", EndScopeBlockVariant.Function },
                { "Sub", EndScopeBlockVariant.Sub },
                { "Property", EndScopeBlockVariant.Property },
                { "Get", EndScopeBlockVariant.Get },
                { "Set", EndScopeBlockVariant.Set },
                { "AddHandler", EndScopeBlockVariant.AddHandler },
                { "RemoveHandler", EndScopeBlockVariant.RemoveHandler },
                { "If", EndScopeBlockVariant.If },
                { "Using", EndScopeBlockVariant.Using },
                { "While", EndScopeBlockVariant.While },
            };

        public static EndScopeBlockVariant VariantFromString(string variantString)
        {
            EndScopeBlockVariant variant;
            if (!VariantMapping.TryGetValue(variantString, out variant))
                throw new ArgumentException("Variant is not recognized as a valid Visual Basic end scope block variant.");
            return variant;
        }

        public static string VariantToString(EndScopeBlockVariant variant)
        {
            var pair = VariantMapping.FirstOrDefault(x => x.Value == variant);
            if (string.IsNullOrEmpty(pair.Key))
                throw new ArgumentException("Variant is not supported in Visual Basic.");
            return pair.Key;
        }

        public EndScopeBlockClause()
        {
        }

        public AstToken EndKeyword
        {
            get { return GetChildByTitle(EndKeywordTitle); }
            set { SetChildByTitle(EndKeywordTitle, value); }
        }

        public AstToken VariantElement
        {
            get { return GetChildByTitle(VariantKeywordTitle); }
            set { SetChildByTitle(VariantKeywordTitle, value); }
        }

        public EndScopeBlockVariant Variant
        {
            get;
            set;
        }

        public override bool Match(AstNode other)
        {
            var clause = other as EndScopeBlockClause;
            return clause != null
                   && Variant == clause.Variant;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            this.AcceptVisualBasicVisitor(visitor);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return this.AcceptVisualBasicVisitor(visitor);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return this.AcceptVisualBasicVisitor(visitor, data);
        }

        public void AcceptVisitor(IVisualBasicAstVisitor visitor)
        {
            visitor.VisitEndScopeBlockClause(this);
        }

        public TResult AcceptVisitor<TResult>(IVisualBasicAstVisitor<TResult> visitor)
        {
            return visitor.VisitEndScopeBlockClause(this);
        }

        public TResult AcceptVisitor<TData, TResult>(IVisualBasicAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitEndScopeBlockClause(this, data);
        }
    }

    public enum EndScopeBlockVariant
    {
        Namespace,
        Class,
        Structure,
        Interface,
        Enum,
        Function,
        Sub,
        Property,
        Get,
        Set,
        AddHandler,
        RemoveHandler,
        If,
        Using,
        While,
    }
}