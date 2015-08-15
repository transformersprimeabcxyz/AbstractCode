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
using AbstractCode.Ast.Statements;
using LockStatementBase = AbstractCode.Ast.Statements.LockStatement;

namespace AbstractCode.Ast.CSharp.Statements
{
    public class LockStatement : LockStatementBase
    {
        public LockStatement()
        {

        }

        public LockStatement(Expression lockObject)
            : base(lockObject)
        {

        }

        public LockStatement(Expression lockObject, Statement body)
            : base(lockObject, body)
        {
        }

        public AstToken LeftParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.LeftParenthese); }
            set { SetChildByTitle(AstNodeTitles.LeftParenthese, value); }
        }
        
        public AstToken RightParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.RightParenthese); }
            set { SetChildByTitle(AstNodeTitles.RightParenthese, value); }
        }
    }
}
