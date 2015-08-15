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
using AbstractCode.Ast.Expressions;

namespace AbstractCode.Ast.Statements
{
    public class LockStatement : Statement
    {
        public static readonly AstNodeTitle<Expression> LockObjectTitle = new AstNodeTitle<Expression>("LockObject");

        public LockStatement()
        {
        }

        public LockStatement(Expression lockObject)
        {
            LockObject = lockObject;
        }

        public LockStatement(Expression lockObject, Statement body)
            : this(lockObject)
        {
            Body = body;
        }

        public AstToken LockKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public Expression LockObject
        {
            get { return GetChildByTitle(LockObjectTitle); }
            set { SetChildByTitle(LockObjectTitle, value); }
        }

        public Statement Body
        {
            get { return GetChildByTitle(AstNodeTitles.BodyStatement); }
            set { SetChildByTitle(AstNodeTitles.BodyStatement, value); }
        }

        public override bool Match(AstNode other)
        {
            var statement = other as LockStatement;
            return statement != null
                   && LockObject.MatchOrNull(statement.LockObject)
                   && Body.Match(statement.Body);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitLockStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitLockStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitLockStatement(this, data);
        }
    }
}
