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
using AbstractCode.Ast.Statements;

namespace AbstractCode.Ast.VisualBasic.Statements
{
    public class ExitStatement : Statement, IVisualBasicVisitable
    {
        public static readonly AstNodeTitle<AstToken> ExitKeywordTitle = new AstNodeTitle<AstToken>("ExitKeyword");
        public static readonly AstNodeTitle<AstToken> VariantKeywordTitle = new AstNodeTitle<AstToken>("VariantKeyword");

        internal static readonly Dictionary<string, ExitStatementVariant> VariantMapping =
            new Dictionary<string, ExitStatementVariant>(StringComparer.OrdinalIgnoreCase)
            {
                { "Do", ExitStatementVariant.Do },
                { "For", ExitStatementVariant.For },
                { "Function", ExitStatementVariant.Function },
                { "Property", ExitStatementVariant.Property },
                { "Select", ExitStatementVariant.Select },
                { "Sub", ExitStatementVariant.Sub },
                { "Try", ExitStatementVariant.Try },
                { "While", ExitStatementVariant.While },
            };

        public static ExitStatementVariant VariantFromString(string variantString)
        {
            ExitStatementVariant variant;
            if (!VariantMapping.TryGetValue(variantString, out variant))
                throw new ArgumentException("Variant is not recognized as a valid Visual Basic exit variant.");
            return variant;
        }

        public static string VariantToString(ExitStatementVariant variant)
        {
            var pair = VariantMapping.FirstOrDefault(x => x.Value == variant);
            if (string.IsNullOrEmpty(pair.Key))
                throw new ArgumentException("Variant is not supported in Visual Basic.");
            return pair.Key;
        }

        public ExitStatement()
        {
        }

        public ExitStatement(ExitStatementVariant variant)
        {
            Variant = variant;
        }

        public AstToken ExitKeyword
        {
            get { return GetChildByTitle(ExitKeywordTitle); }
            set { SetChildByTitle(ExitKeywordTitle, value); }
        }

        public AstToken VariantElement
        {
            get { return GetChildByTitle(VariantKeywordTitle); }
            set { SetChildByTitle(VariantKeywordTitle, value); }
        }

        public ExitStatementVariant Variant
        {
            get;
            set;
        }

        public override bool Match(AstNode other)
        {
            var statement = other as ExitStatement;
            return statement != null
                   && Variant == statement.Variant;
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
            visitor.VisitExitStatement(this);
        }

        public TResult AcceptVisitor<TResult>(IVisualBasicAstVisitor<TResult> visitor)
        {
            return visitor.VisitExitStatement(this);
        }

        public TResult AcceptVisitor<TData, TResult>(IVisualBasicAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitExitStatement(this, data);
        }
    }

    public enum ExitStatementVariant
    {
        Do,
        For,
        Function,
        Property,
        Select,
        Sub,
        Try,
        While,
    }
}