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
using ConditionalExpressionBase = AbstractCode.Ast.Expressions.ConditionalExpression;

namespace AbstractCode.Ast.CSharp.Expressions
{
    public class ConditionalExpression : ConditionalExpressionBase
    {
        public ConditionalExpression()
        {
        }

        public ConditionalExpression(Expression condition, Expression trueExpression, Expression falseExpression)
            : base(condition, trueExpression, falseExpression)
        {

        }

        public AstToken OperatorToken
        {
            get { return GetChildByTitle(AstNodeTitles.Operator); }
            set { SetChildByTitle(AstNodeTitles.Operator, value); }
        }

        public AstToken ColonToken
        {
            get { return GetChildByTitle(AstNodeTitles.Colon); }
            set { SetChildByTitle(AstNodeTitles.Colon, value); }
        }
    }
}
