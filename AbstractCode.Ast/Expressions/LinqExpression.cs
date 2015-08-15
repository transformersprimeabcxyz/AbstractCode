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

using System.Collections.Generic;

namespace AbstractCode.Ast.Expressions
{
    public class LinqExpression : Expression
    {
        public LinqExpression() 
        {
            Clauses = new AstNodeCollection<LinqClause>(this, AstNodeTitles.LinqClause);
        }

        public LinqExpression(params LinqClause[] clauses)
            :this((IEnumerable<LinqClause>)clauses)
        {

        }

        public LinqExpression(IEnumerable<LinqClause> clauses)
            : this()
        {
            Clauses.AddRange(clauses);
        }

        public AstNodeCollection<LinqClause> Clauses
        {
            get;
        }

        public override bool Match(AstNode other)
        {
            var expression = other as LinqExpression;
            return expression != null && Clauses.Match(expression.Clauses);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitLinqExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitLinqExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitLinqExpression(this, data);
        }
    }
}
