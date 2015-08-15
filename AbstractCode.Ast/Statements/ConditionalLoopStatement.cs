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
    public abstract class ConditionalLoopStatement : Statement
    {
        protected ConditionalLoopStatement()
        {

        }

        protected ConditionalLoopStatement(Expression condition)
        {
            Condition = condition;
        }

        public Expression Condition
        {
            get { return GetChildByTitle(AstNodeTitles.Condition); }
            set { SetChildByTitle(AstNodeTitles.Condition, value); }
        }

        public Statement Body
        {
            get { return GetChildByTitle(AstNodeTitles.BodyStatement); }
            set { SetChildByTitle(AstNodeTitles.BodyStatement, value); }
        }
    }
}
