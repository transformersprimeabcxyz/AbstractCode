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

using AbstractCode.Ast.Members;

namespace AbstractCode.Ast.Expressions
{
    public abstract class LinqClause : AstNode
    {

    }

    public class LinqFromClause : LinqClause
    {
        public static readonly AstNodeTitle<AstToken> FromKeywordTitle = new AstNodeTitle<AstToken>("FromKeyword");
        public static readonly AstNodeTitle<AstToken> InKeywordTitle = new AstNodeTitle<AstToken>("InKeyword");

        public LinqFromClause()
        {

        }

        public LinqFromClause(Identifier variableName, Expression dataSource)
        {
            VariableName = variableName;
            DataSource = dataSource;
        }

        public AstToken FromKeyword
        {
            get { return GetChildByTitle(FromKeywordTitle); }
            set { SetChildByTitle(FromKeywordTitle, value); }
        }

        public Identifier VariableName
        {
            get { return GetChildByTitle(AstNodeTitles.Identifier); }
            set { SetChildByTitle(AstNodeTitles.Identifier, value); }
        }

        public AstToken InKeyword
        {
            get { return GetChildByTitle(InKeywordTitle); }
            set { SetChildByTitle(InKeywordTitle, value); }
        }

        public Expression DataSource
        {
            get { return GetChildByTitle(AstNodeTitles.TargetExpression); }
            set { SetChildByTitle(AstNodeTitles.TargetExpression, value); }
        }

        public override bool Match(AstNode other)
        {
            var clause = other as LinqFromClause;
            return clause != null
                   && VariableName.MatchOrNull(clause.VariableName)
                   && DataSource.MatchOrNull(clause.DataSource);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitLinqFromClause(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitLinqFromClause(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitLinqFromClause(this, data);
        }
    }

    // public class LinqJoinClause : LinqClause
    // {
    // 
    // }
    // 

    public class LinqLetClause : LinqClause
    {
        public LinqLetClause()
        {
        }

        public LinqLetClause(VariableDeclarator variable)
        {
            Variable = variable;
        }

        public AstToken LetKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public VariableDeclarator Variable
        {
            get { return GetChildByTitle(AstNodeTitles.Declarator); }
            set { SetChildByTitle(AstNodeTitles.Declarator, value); }
        }

        public override bool Match(AstNode other)
        {
            var clause = other as LinqLetClause;
            return clause != null 
                && Variable.MatchOrNull(clause.Variable);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitLinqLetClause(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitLinqLetClause(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitLinqLetClause(this, data);
        }
    }
    
    public class LinqGroupByClause : LinqClause
    {
        public static readonly AstNodeTitle<AstToken> GroupKeywordTitle = new AstNodeTitle<AstToken>("GroupKeyword");
        public static readonly AstNodeTitle<AstToken> ByKeywordTitle = new AstNodeTitle<AstToken>("ByKeyword");
        public static readonly AstNodeTitle<Expression> KeyExpressionTitle = new AstNodeTitle<Expression>("KeyExpression");

        public LinqGroupByClause()
        {
        }

        public LinqGroupByClause(Expression expression, Expression key)
        {
            Expression = expression;
            KeyExpression = key;
        }

        public AstToken GroupKeyword
        {
            get { return GetChildByTitle(GroupKeywordTitle); }
            set { SetChildByTitle(GroupKeywordTitle, value); }
        }

        public Expression Expression
        {
            get { return GetChildByTitle(AstNodeTitles.Expression); }
            set { SetChildByTitle(AstNodeTitles.Expression, value); }
        }

        public AstToken ByKeyword
        {
            get { return GetChildByTitle(ByKeywordTitle); }
            set { SetChildByTitle(ByKeywordTitle, value); }
        }

        public Expression KeyExpression
        {
            get { return GetChildByTitle(KeyExpressionTitle); }
            set { SetChildByTitle(KeyExpressionTitle, value); }
        }

        public override bool Match(AstNode other)
        {
            var clause = other as LinqGroupByClause;
            return clause != null
                   && Expression.MatchOrNull(clause.Expression)
                   && KeyExpression.MatchOrNull(clause.KeyExpression);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitLinqGroupByClause(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
           return visitor.VisitLinqGroupByClause(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitLinqGroupByClause(this, data);
        }
    }
    
    public class LinqOrderByClause : LinqClause
    {
        public static readonly AstNodeTitle<LinqOrdering> OrderningTitle =
            new AstNodeTitle<LinqOrdering>("Ordening");

        public LinqOrderByClause()
        {
            Ordernings = new AstNodeCollection<LinqOrdering>(this, OrderningTitle);
        }

        public LinqOrderByClause(params LinqOrdering[] orderings)
            : this()
        {
            Ordernings.AddRange(orderings);
        }

        public AstToken OrderByKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public AstNodeCollection<LinqOrdering> Ordernings
        {
            get;
        }

        public override bool Match(AstNode other)
        {
            var clause = other as LinqOrderByClause;
            return clause != null
                   && Ordernings.Match(clause.Ordernings);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitLinqOrderByClause(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitLinqOrderByClause(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitLinqOrderByClause(this, data);
        }
    }

    public class LinqOrdering : AstNode
    {
        public LinqOrdering()
        {
        }

        public LinqOrdering(Expression expression)
        {
            Expression = expression;
        }

        public LinqOrdering(Expression expression, LinqOrderingDirection direction)
        {
            Expression = expression;
            Direction = direction;
        }

        public Expression Expression
        {
            get { return GetChildByTitle(AstNodeTitles.Expression); }
            set { SetChildByTitle(AstNodeTitles.Expression, value); }
        }

        public LinqOrderingDirection Direction
        {
            get;
            set;
        }

        public AstToken DirectionKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public override bool Match(AstNode other)
        {
            var ordering = other as LinqOrdering;
            return ordering != null && Direction == ordering.Direction;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitLinqOrdering(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitLinqOrdering(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitLinqOrdering(this, data);
        }
    }

    public enum LinqOrderingDirection
    {
        None,
        Ascending,
        Descending,
    }

    public class LinqWhereClause : LinqClause
    {
        public LinqWhereClause()
        {

        }

        public LinqWhereClause(Expression condition)
        {
            Condition = condition;
        }

        public AstToken WhereKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public Expression Condition
        {
            get { return GetChildByTitle(AstNodeTitles.Condition); }
            set { SetChildByTitle(AstNodeTitles.Condition, value); }
        }

        public override bool Match(AstNode other)
        {
            var clause = other as LinqWhereClause;
            return clause != null
                   && Condition.MatchOrNull(clause.Condition);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitLinqWhereClause(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitLinqWhereClause(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitLinqWhereClause(this, data);
        }
    }

    public class LinqSelectClause : LinqClause
    {
        public LinqSelectClause()
        {

        }

        public LinqSelectClause(Expression target)
        {
            Target = target;
        }

        public AstToken SelectKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public Expression Target
        {
            get { return GetChildByTitle(AstNodeTitles.TargetExpression); }
            set { SetChildByTitle(AstNodeTitles.TargetExpression, value); }
        }

        public override bool Match(AstNode other)
        {
            var clause = other as LinqSelectClause;
            return clause != null && Target.MatchOrNull(clause.Target);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitLinqSelectClause(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitLinqSelectClause(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitLinqSelectClause(this, data);
        }
    }

}
