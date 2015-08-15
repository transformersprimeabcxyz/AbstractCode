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

using AbstractCode.Ast.Expressions;

namespace AbstractCode.Ast.Statements
{
    public class IfElseStatement : Statement
    {
        public static readonly AstNodeTitle<AstToken> IfKeywordTitle = new AstNodeTitle<AstToken>("IfKeyword");
        public static readonly AstNodeTitle<AstToken> ElseKeywordTitle = new AstNodeTitle<AstToken>("ElseKeyword");

        public static readonly AstNodeTitle<Statement> TrueBlockTitle = new AstNodeTitle<Statement>("TrueBlock");
        public static readonly AstNodeTitle<Statement> FalseBlockTitle = new AstNodeTitle<Statement>("FalseBlock");

        public IfElseStatement()
        {

        }

        public IfElseStatement(Expression condition, Statement trueBlock)
        {
            Condition = condition;
            TrueBlock = trueBlock;
        }

        public IfElseStatement(Expression condition, Statement trueBlock, Statement falseBlock)
        {
            Condition = condition;
            TrueBlock = trueBlock;
            FalseBlock = falseBlock;
        }

        public AstToken IfKeyword
        {
            get { return GetChildByTitle(IfKeywordTitle); }
            set { SetChildByTitle(IfKeywordTitle, value); }
        }

        public Expression Condition
        {
            get { return GetChildByTitle(AstNodeTitles.Condition); }
            set { SetChildByTitle(AstNodeTitles.Condition, value); }
        }

        public Statement TrueBlock
        {
            get { return GetChildByTitle(TrueBlockTitle); }
            set { SetChildByTitle(TrueBlockTitle, value); }
        }

        public AstToken ElseKeyword
        {
            get { return GetChildByTitle(ElseKeywordTitle); }
            set { SetChildByTitle(ElseKeywordTitle, value); }
        }

        public Statement FalseBlock
        {
            get { return GetChildByTitle(FalseBlockTitle); }
            set { SetChildByTitle(FalseBlockTitle, value); }
        }

        public override string ToString()
        {
            return string.Format("{0} (Condition = {1})", GetType().Name, Condition);
        }

        public override bool Match(AstNode other)
        {
            var statement = other as IfElseStatement;
            return statement != null
                   && Condition.MatchOrNull(statement.Condition)
                   && TrueBlock.MatchOrNull(statement.TrueBlock)
                   && FalseBlock.MatchOrNull(statement.FalseBlock);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitIfElseStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitIfElseStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitIfElseStatement(this, data);
        }

        
    }
}
