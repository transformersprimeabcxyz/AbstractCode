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
using System.Text;
using System.Threading.Tasks;
using AbstractCode.Ast;
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Types;

namespace AbstractCode.Ast.CSharp.Expressions
{
    public abstract class CSharpSpecificKeywordExpression : Expression, ICSharpVisitable
    {
        public AstToken KeywordToken
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
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

        public abstract void AcceptVisitor(ICSharpAstVisitor visitor);

        public abstract TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor);

        public abstract TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data);
    }

    public class CheckedExpression : CSharpSpecificKeywordExpression
    {
        public CheckedExpression()
        {
        }

        public CheckedExpression(Expression targetExpression)
        {
            TargetExpression = targetExpression;
        }

        public AstToken LeftParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.LeftParenthese); }
            set { SetChildByTitle(AstNodeTitles.LeftParenthese, value); }
        }

        public Expression TargetExpression
        {
            get { return GetChildByTitle(AstNodeTitles.TargetExpression); }
            set { SetChildByTitle(AstNodeTitles.TargetExpression, value); }
        }

        public AstToken RightParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.RightParenthese); }
            set { SetChildByTitle(AstNodeTitles.RightParenthese, value); }
        }

        public override void AcceptVisitor(ICSharpAstVisitor visitor)
        {
            visitor.VisitCheckedExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor)
        {
            return visitor.VisitCheckedExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitCheckedExpression(this, data);
        }

        public override bool Match(AstNode other)
        {
            var expression = other as CheckedExpression;
            return expression != null
                   && TargetExpression.MatchOrNull(expression.TargetExpression);
        }
    }

    public class UncheckedExpression : CSharpSpecificKeywordExpression
    {
        public UncheckedExpression()
        {
        }

        public UncheckedExpression(Expression targetExpression)
        {
            TargetExpression = targetExpression;
        }

        public AstToken LeftParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.LeftParenthese); }
            set { SetChildByTitle(AstNodeTitles.LeftParenthese, value); }
        }

        public Expression TargetExpression
        {
            get { return GetChildByTitle(AstNodeTitles.TargetExpression); }
            set { SetChildByTitle(AstNodeTitles.TargetExpression, value); }
        }

        public AstToken RightParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.RightParenthese); }
            set { SetChildByTitle(AstNodeTitles.RightParenthese, value); }
        }

        public override bool Match(AstNode other)
        {
            var expression = other as UncheckedExpression;
            return expression != null
                   && TargetExpression.MatchOrNull(expression.TargetExpression);
        }

        public override void AcceptVisitor(ICSharpAstVisitor visitor)
        {
            visitor.VisitUncheckedExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor)
        {
            return visitor.VisitUncheckedExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitUncheckedExpression(this, data);
        }
    }

    public class SizeOfExpression : CSharpSpecificKeywordExpression
    {
        public SizeOfExpression()
        {
        }

        public SizeOfExpression(TypeReference targetType)
        {
            TargetType = targetType;
        }

        public AstToken LeftParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.LeftParenthese); }
            set { SetChildByTitle(AstNodeTitles.LeftParenthese, value); }
        }

        public TypeReference TargetType
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }

        public AstToken RightParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.RightParenthese); }
            set { SetChildByTitle(AstNodeTitles.RightParenthese, value); }
        }

        public override bool Match(AstNode other)
        {
            var expression = other as SizeOfExpression;
            return expression != null
                   && TargetType.MatchOrNull(expression.TargetType);
        }

        public override void AcceptVisitor(ICSharpAstVisitor visitor)
        {
            visitor.VisitSizeOfExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor)
        {
            return visitor.VisitSizeOfExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitSizeOfExpression(this, data);
        }
    }

    public class DefaultExpression : CSharpSpecificKeywordExpression
    {
        public DefaultExpression()
        {
        }

        public DefaultExpression(TypeReference targetType)
        {
            TargetType = targetType;
        }

        public AstToken LeftParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.LeftParenthese); }
            set { SetChildByTitle(AstNodeTitles.LeftParenthese, value); }
        }

        public TypeReference TargetType
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }

        public AstToken RightParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.RightParenthese); }
            set { SetChildByTitle(AstNodeTitles.RightParenthese, value); }
        }

        public override bool Match(AstNode other)
        {
            var expression = other as DefaultExpression;
            return expression != null
                   && TargetType.MatchOrNull(expression.TargetType);
        }

        public override void AcceptVisitor(ICSharpAstVisitor visitor)
        {
            visitor.VisitDefaultExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor)
        {
            return visitor.VisitDefaultExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitDefaultExpression(this, data);
        }
    }

    public class StackAllocExpression : CSharpSpecificKeywordExpression
    {
        private static readonly AstNodeTitle<Expression> _counterTitle = new AstNodeTitle<Expression>("Counter");

        public StackAllocExpression()
        {
        }

        public StackAllocExpression(TypeReference type, Expression counter)
        {
            Type = type;
            Counter = counter;
        }

        public TypeReference Type
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }

        public AstToken LeftBracket
        {
            get { return GetChildByTitle(AstNodeTitles.LeftBracket); }
            set { SetChildByTitle(AstNodeTitles.LeftBracket, value); }
        }

        public Expression Counter
        {
            get { return GetChildByTitle(_counterTitle); }
            set { SetChildByTitle(_counterTitle, value); }
        }

        public AstToken RightBracket
        {
            get { return GetChildByTitle(AstNodeTitles.RightBracket); }
            set { SetChildByTitle(AstNodeTitles.RightBracket, value); }
        }

        public override bool Match(AstNode other)
        {
            var expression = other as StackAllocExpression;
            return expression != null
                   && Type.MatchOrNull(expression.Type)
                   && Counter.MatchOrNull(expression.Counter);
        }

        public override void AcceptVisitor(ICSharpAstVisitor visitor)
        {
            visitor.VisitStackAllocExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor)
        {
            return visitor.VisitStackAllocExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitStackAllocExpression(this, data);
        }
    }
}
