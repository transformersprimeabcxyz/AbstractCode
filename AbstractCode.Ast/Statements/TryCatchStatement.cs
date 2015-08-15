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

namespace AbstractCode.Ast.Statements
{
    public class TryCatchStatement : Statement 
    {
        public static readonly AstNodeTitle<AstToken> TryKeywordTitle = new AstNodeTitle<AstToken>("TryKeyword");
        public static readonly AstNodeTitle<BlockStatement> TryBlockTitle = new AstNodeTitle<BlockStatement>("TryBlock");
        public static readonly AstNodeTitle<CatchClause> CatchClauseTitle = new AstNodeTitle<CatchClause>("CatchClause");
        public static readonly AstNodeTitle<AstToken> FinallyKeywordTitle = new AstNodeTitle<AstToken>("FinallyKeyword");
        public static readonly AstNodeTitle<BlockStatement> FinallyBlockTitle = new AstNodeTitle<BlockStatement>("FinallyBlock");

        public TryCatchStatement()
        {
            CatchClauses = new AstNodeCollection<CatchClause>(this, CatchClauseTitle);
        }

        public AstToken TryKeyword
        {
            get { return GetChildByTitle(TryKeywordTitle); }
            set { SetChildByTitle(TryKeywordTitle, value); }
        }

        public BlockStatement TryBlock
        {
            get { return GetChildByTitle(TryBlockTitle); }
            set { SetChildByTitle(TryBlockTitle, value); }
        }

        public AstNodeCollection<CatchClause> CatchClauses
        {
            get;
            private set;
        }
        
        public AstToken FinallyKeyword
        {
            get { return GetChildByTitle(FinallyKeywordTitle); }
            set { SetChildByTitle(FinallyKeywordTitle, value); }
        }

        public BlockStatement FinallyBlock
        {
            get { return GetChildByTitle(FinallyBlockTitle); }
            set { SetChildByTitle(FinallyBlockTitle, value); }
        }

        public override bool Match(AstNode other)
        {
            var statement = other as TryCatchStatement;
            return statement != null
                   && TryBlock.MatchOrNull(statement.TryBlock)
                   && CatchClauses.Match(statement.CatchClauses)
                   && FinallyBlock.MatchOrNull(statement.FinallyBlock);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitTryCatchStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitTryCatchStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitTryCatchStatement(this, data);
        }
    }
}
