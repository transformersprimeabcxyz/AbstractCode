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
    public class UsingStatement : Statement 
    {
        public static readonly AstNodeTitle<AstNode> DisposableObjectTitle = new AstNodeTitle<AstNode>("DisposableObject");

        public UsingStatement()
        {
        }

        public UsingStatement(AstNode disposableObject)
            : this()
        {
            DisposableObject = disposableObject;
        }

        public UsingStatement(AstNode disposableObject, Statement body)
        {
            DisposableObject = disposableObject;
            Body = body;
        }

        public AstToken UsingKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public AstNode DisposableObject
        {
            get { return GetChildByTitle(DisposableObjectTitle); }
            set { SetChildByTitle(DisposableObjectTitle, value); }
        }

        public Statement Body
        {
            get { return GetChildByTitle(AstNodeTitles.BodyStatement); }
            set { SetChildByTitle(AstNodeTitles.BodyStatement, value); }
        }

        public override bool Match(AstNode other)
        {
            var statement = other as UsingStatement;
            return statement != null
                   && DisposableObject.MatchOrNull(statement.DisposableObject)
                   && Body.MatchOrNull(statement.Body);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitUsingStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitUsingStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitUsingStatement(this, data);
        }
    }
}
