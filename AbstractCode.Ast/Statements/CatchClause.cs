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
using AbstractCode.Ast.Types;

namespace AbstractCode.Ast.Statements
{
    public class CatchClause : AstNode
    {       
        public CatchClause()
        {
        }

        public CatchClause(TypeReference exceptionType)
            : this(exceptionType, null)
        {

        }

        public CatchClause(TypeReference exceptionType, Identifier identifier)
        {
            ExceptionType = exceptionType;
            ExceptionIdentifier = identifier;
        }

        public AstToken CatchKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }
        
        public TypeReference ExceptionType
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }
        
        public Identifier ExceptionIdentifier
        {
            get { return GetChildByTitle(AstNodeTitles.Identifier); }
            set { SetChildByTitle(AstNodeTitles.Identifier, value); }
        }

        public BlockStatement Body
        {
            get { return GetChildByTitle(AstNodeTitles.Body); }
            set { SetChildByTitle(AstNodeTitles.Body, value); }
        }

        public override string ToString()
        {
            return string.Format("{0} (Type = {1})", GetType().Name, ExceptionType);
        }

        public override bool Match(AstNode other)
        {
            var clause = other as CatchClause;
            return clause != null
                   && ExceptionType.MatchOrNull(clause.ExceptionType)
                   && ExceptionIdentifier.MatchOrNull(clause.ExceptionIdentifier)
                   && Body.MatchOrNull(clause.Body);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitCatchClause(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitCatchClause(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitCatchClause(this, data);
        }
    }
}
