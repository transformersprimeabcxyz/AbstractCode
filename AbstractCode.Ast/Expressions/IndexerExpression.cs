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

namespace AbstractCode.Ast.Expressions
{
    public class IndexerExpression : Expression
    {
        public IndexerExpression()
            : this(null)
        {
            
        }

        public IndexerExpression(Expression target)
            : this(target, null)
        {
        }

        public IndexerExpression(Expression target, params Expression[] indices)
        {
            Target = target;
            Indices = new AstNodeCollection<Expression>(this, AstNodeTitles.Index);
            if (indices != null)
                Indices.AddRange(indices);
        }


        public Expression Target
        {
            get { return GetChildByTitle(AstNodeTitles.TargetExpression); }
            set { SetChildByTitle(AstNodeTitles.TargetExpression, value); }
        }

        public AstToken LeftBracket
        {
            get { return GetChildByTitle(AstNodeTitles.LeftBracket); }
            set { SetChildByTitle(AstNodeTitles.LeftBracket, value); }
        }

        public AstNodeCollection<Expression> Indices
        {
            get;
        }
    
        public AstToken RightBracket
        {
            get { return GetChildByTitle(AstNodeTitles.RightBracket); }
            set { SetChildByTitle(AstNodeTitles.RightBracket, value); }
        }

        public override bool Match(AstNode other)
        {
            var expression = other as IndexerExpression;
            return expression != null
                   && Target.MatchOrNull(expression.Target)
                   && Indices.Match(expression.Indices);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitIndexerExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitIndexerExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitIndexerExpression(this, data);
        }
    }
}
