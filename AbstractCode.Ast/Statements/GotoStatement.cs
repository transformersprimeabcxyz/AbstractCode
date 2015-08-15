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

using AbstractCode.Ast.Members;

namespace AbstractCode.Ast.Statements
{
    public class GotoStatement : Statement
    {
        public GotoStatement()
        {

        }

        public GotoStatement(string label)
        {
            Label = label;
        }

        public AstToken GotoKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public Identifier LabelIdentifier
        {
            get { return GetChildByTitle(AstNodeTitles.Identifier); }
            set { SetChildByTitle(AstNodeTitles.Identifier, value); }
        }

        public string Label
        {
            get { return LabelIdentifier.Name; }
            set { LabelIdentifier = new Identifier(value); }
        }

        public override bool Match(AstNode other)
        {
            var statement = other as GotoStatement;
            return statement != null && LabelIdentifier.MatchOrNull(statement.LabelIdentifier);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitGotoStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitGotoStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitGotoStatement(this, data);
        }
        
    }
}
