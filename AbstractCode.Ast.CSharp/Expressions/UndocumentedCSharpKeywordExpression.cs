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

namespace AbstractCode.Ast.CSharp.Expressions
{
    public enum UndocumentedCSharpKeyword
    {
        ArgList,
        MakeRef,
        RefType,
        RefValue,
    }

    public class UndocumentedCSharpKeywordExpression : CSharpSpecificKeywordExpression
    {
        internal static readonly Dictionary<string, UndocumentedCSharpKeyword> KeywordMapping = new Dictionary<string, UndocumentedCSharpKeyword>()
        {
            {"__arglist", UndocumentedCSharpKeyword.ArgList},
            {"__makeref", UndocumentedCSharpKeyword.MakeRef},
            {"__reftype", UndocumentedCSharpKeyword.RefType},
            {"__refvalue", UndocumentedCSharpKeyword.RefValue},
        };

        public static UndocumentedCSharpKeyword KeywordFromString(string keywordString)
        {
            UndocumentedCSharpKeyword keyword;
            if (!KeywordMapping.TryGetValue(keywordString, out keyword))
                throw new ArgumentException("Keyword is not recognized as a valid undocumented C# keyword.");
            return keyword;
        }

        public static string KeywordToString(UndocumentedCSharpKeyword keyword)
        {
            var pair = KeywordMapping.FirstOrDefault(x => x.Value == keyword);
            if (string.IsNullOrEmpty(pair.Key))
                throw new ArgumentException("Keyword is not supported in C#.");
            return pair.Key;
        }

        public UndocumentedCSharpKeywordExpression()
        {
            Arguments = new AstNodeCollection<Expression>(this, AstNodeTitles.Argument);
        }

        public UndocumentedCSharpKeywordExpression(UndocumentedCSharpKeyword keyword)
            : this()
        {
            Keyword = keyword;
        }

        public UndocumentedCSharpKeywordExpression(UndocumentedCSharpKeyword keyword, params Expression[] arguments)
            : this()
        {
            Keyword = keyword;
            Arguments.AddRange(arguments);
        }

        public UndocumentedCSharpKeyword Keyword
        {
            get;
            set;
        }

        public AstToken LeftParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.LeftParenthese); }
            set { SetChildByTitle(AstNodeTitles.LeftParenthese, value); }
        }

        public AstNodeCollection<Expression> Arguments
        {
            get;
        }

        public AstToken RightParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.RightParenthese); }
            set { SetChildByTitle(AstNodeTitles.RightParenthese, value); }
        }

        public override void AcceptVisitor(ICSharpAstVisitor visitor)
        {
            visitor.VisitUndocumentedCSharpKeywordExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor)
        {
            return visitor.VisitUndocumentedCSharpKeywordExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitUndocumentedCSharpKeywordExpression(this, data);
        }

        public override bool Match(AstNode other)
        {
            var expression = other as UndocumentedCSharpKeywordExpression;
            return expression != null
                   && Keyword == expression.Keyword
                   && Arguments.Match(expression.Arguments);
        }
    }
}