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
    public class SwitchCaseLabel : AstNode
    {
        public SwitchCaseLabel()
        {
        }

        public SwitchCaseLabel(Expression condition)
        {
            Condition = condition;
        }

        public AstToken CaseKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public Expression Condition
        {
            get { return GetChildByTitle(AstNodeTitles.Condition); }
            set { SetChildByTitle(AstNodeTitles.Condition, value); }
        }

        public virtual bool IsDefaultCase
        {
            get { return Condition == null; }
        }

        public override bool Match(AstNode other)
        {
            var label = other as SwitchCaseLabel;
            return label != null
                   && Condition.MatchOrNull(label.Condition);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitSwitchCaseLabel(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitSwitchCaseLabel(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitSwitchCaseLabel(this, data);
        }

        
    }
}
