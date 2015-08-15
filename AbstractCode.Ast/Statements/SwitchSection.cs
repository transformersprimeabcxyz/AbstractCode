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

namespace AbstractCode.Ast.Statements
{
    public class SwitchSection : AstNode 
    {
        public static readonly AstNodeTitle<SwitchCaseLabel> SwitchCaseLabelTitle = new AstNodeTitle<SwitchCaseLabel>("SwitchCaseLabel");

        public SwitchSection()
        {
            Labels = new AstNodeCollection<SwitchCaseLabel>(this, SwitchCaseLabelTitle);
            Statements = new AstNodeCollection<Statement>(this, AstNodeTitles.Statement);
        }

        public SwitchSection(params SwitchCaseLabel[] labels)
            : this()
        {
            Labels.AddRange(labels);
        }

        public AstNodeCollection<SwitchCaseLabel> Labels
        {
            get;
        }

        public AstNodeCollection<Statement> Statements
        {
            get;
        }

        public override bool Match(AstNode other)
        {
            var section = other as SwitchSection;
            return section != null
                   && Labels.Match(section.Labels)
                   && Statements.Match(section.Statements);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitSwitchSection(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitSwitchSection(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitSwitchSection(this, data);
        }
    }
}
