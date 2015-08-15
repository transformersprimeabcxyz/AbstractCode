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

namespace AbstractCode.Ast
{
    public class Comment : AstNode
    {
        public Comment(string contents)
            : this(contents, CommentType.SingleLine)
        {
        }

        public Comment(string contents, CommentType commentType)
        {
            Contents = contents;
            CommentType = commentType;
        }

        public Comment(string contents, CommentType commentType, TextRange range)
        {
            Contents = contents;
            CommentType = commentType;
            Range = range;
        }

        public CommentType CommentType
        {
            get;
            set;
        }

        public string Contents
        {
            get;
            set;
        }

        public override bool Match(AstNode other)
        {
            var comment = other as Comment;
            return comment != null
                && comment.CommentType == this.CommentType
                && comment.Contents == this.Contents;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitComment(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitComment(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitComment(this, data);
        }
        
    }

    public enum CommentType
    {
        SingleLine,
        Block,
        Documentation,
    }
}
