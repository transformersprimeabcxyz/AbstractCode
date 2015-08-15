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
using AbstractCode.Ast.Expressions;

namespace AbstractCode.Ast.CSharp.Members
{
    public class ConstructorInitializer : AstNode, ICSharpVisitable
    {
        internal static readonly Dictionary<string, ConstructorInitializerVariant> VariantMapping = new Dictionary<string, ConstructorInitializerVariant>()
        {
            {"this", ConstructorInitializerVariant.This},
            {"base", ConstructorInitializerVariant.Base},
        };

        public static ConstructorInitializerVariant VariantFromString(string operatorString)
        {
            ConstructorInitializerVariant variant;
            if (!VariantMapping.TryGetValue(operatorString, out variant))
                throw new ArgumentException("Variant is not recognized as a valid C# constructor initializer variant.");
            return variant;
        }

        public static string VariantToString(ConstructorInitializerVariant variant)
        {
            var pair = VariantMapping.FirstOrDefault(x => x.Value == variant);
            if (string.IsNullOrEmpty(pair.Key))
                throw new ArgumentException("Variant is not supported in C#.");
            return pair.Key;
        }

        public ConstructorInitializer()
        {
            Arguments = new AstNodeCollection<Expression>(this, AstNodeTitles.Argument);
        }

        public ConstructorInitializer(ConstructorInitializerVariant variant, IEnumerable<Expression> arguments)
            : this()
        {
            Variant = variant;
            Arguments.AddRange(arguments);
        }

        public ConstructorInitializerVariant Variant
        {
            get;
            set;
        }

        public AstToken VariantToken
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public AstNodeCollection<Expression> Arguments
        {
            get;
        }

        public override bool Match(AstNode other)
        {
            var initializer = other as ConstructorInitializer;
            return initializer != null
                   && Variant == initializer.Variant
                   && Arguments.Match(initializer.Arguments);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            this.AcceptCSharpVisitor(visitor);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return this.AcceptCSharpVisitor(visitor);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return this.AcceptCSharpVisitor(visitor, data);
        }

        public void AcceptVisitor(ICSharpAstVisitor visitor)
        {
            visitor.VisitConstructorInitializer(this);
        }

        public TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor)
        {
            return visitor.VisitConstructorInitializer(this);
        }

        public TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitConstructorInitializer(this, data);
        }
    }

    public enum ConstructorInitializerVariant
    {
        This,
        Base,
    }
}
