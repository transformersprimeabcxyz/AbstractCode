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
    public class SwitchStatement : Statement
    {
        public static readonly AstNodeTitle<SwitchSection> SwitchSectionTitle = new AstNodeTitle<SwitchSection>("SwitchSection");

        public SwitchStatement()
        {
            Sections = new AstNodeCollection<SwitchSection>(this, SwitchSectionTitle);
        }

        public SwitchStatement(Expression condition, params SwitchSection[] sections)
            : this()
        {
            Condition = condition;
            Sections.AddRange(sections);
        }

        public AstToken SwitchKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public Expression Condition
        {
            get { return GetChildByTitle(AstNodeTitles.Condition); }
            set { SetChildByTitle(AstNodeTitles.Condition, value); }
        }

        public AstNodeCollection<SwitchSection> Sections
        {
            get;
        }

        public override bool Match(AstNode other)
        {
            var statement = other as SwitchStatement;
            return statement != null
                   && Condition.MatchOrNull(statement.Condition)
                   && Sections.Match(statement.Sections);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitSwitchStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitSwitchStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitSwitchStatement(this, data);
        }
    }
}
