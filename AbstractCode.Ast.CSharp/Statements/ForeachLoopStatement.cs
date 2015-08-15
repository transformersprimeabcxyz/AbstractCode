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
using System.Text;
using System.Threading.Tasks;
using AbstractCode.Ast;
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Members;
using AbstractCode.Ast.Statements;
using AbstractCode.Ast.Types;

namespace AbstractCode.Ast.CSharp.Statements
{
    public class ForeachLoopStatement : Statement, ICSharpVisitable
    {

        public static readonly AstNodeTitle<AstToken> ForeachKeywordTitle = new AstNodeTitle<AstToken>("ForeachKeyword");
        public static readonly AstNodeTitle<AstToken> InKeywordTitle = new AstNodeTitle<AstToken>("InKeyword");

        public ForeachLoopStatement()
        {
        }

        public ForeachLoopStatement(TypeReference type, Identifier identifier, Expression target)
        {
            Type = type;
            Identifier = identifier;
            Target = target;
        }

        public AstToken ForeachKeyword
        {
            get { return GetChildByTitle(ForeachKeywordTitle); }
            set { SetChildByTitle(ForeachKeywordTitle, value); }
        }

        public AstToken LeftParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.LeftParenthese); }
            set { SetChildByTitle(AstNodeTitles.LeftParenthese, value); }
        }

        public TypeReference Type
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }

        public Identifier Identifier
        {
            get { return GetChildByTitle(AstNodeTitles.Identifier); }
            set { SetChildByTitle(AstNodeTitles.Identifier, value); }
        }

        public AstToken InKeyword
        {
            get { return GetChildByTitle(InKeywordTitle); }
            set { SetChildByTitle(InKeywordTitle, value); }
        }

        public Expression Target
        {
            get { return GetChildByTitle(AstNodeTitles.TargetExpression); }
            set { SetChildByTitle(AstNodeTitles.TargetExpression, value); }
        }

        public AstToken RightParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.RightParenthese); }
            set { SetChildByTitle(AstNodeTitles.RightParenthese, value); }
        }

        public Statement Body
        {
            get { return GetChildByTitle(AstNodeTitles.BodyStatement); }
            set { SetChildByTitle(AstNodeTitles.BodyStatement, value); }
        }

        public override bool Match(AstNode other)
        {
            var statement = other as ForeachLoopStatement;
            return statement != null
                   && Identifier.MatchOrNull(statement.Identifier)
                   && Target.MatchOrNull(statement.Target)
                   && Body.MatchOrNull(statement.Body);
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
            visitor.VisitForeachLoopStatement(this);
        }

        public TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor)
        {
            return visitor.VisitForeachLoopStatement(this);
        }

        public TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitForeachLoopStatement(this, data);
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
