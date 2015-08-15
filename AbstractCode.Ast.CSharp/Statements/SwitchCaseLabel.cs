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
using SwitchCaseLabelBase = AbstractCode.Ast.Statements.SwitchCaseLabel;

namespace AbstractCode.Ast.CSharp.Statements
{
    public class SwitchCaseLabel : SwitchCaseLabelBase
    {
        public SwitchCaseLabel()
        {

        }

        public SwitchCaseLabel(Expression condition)
        {
            Condition = condition;
        }

        public AstToken Colon
        {
            get { return GetChildByTitle(AstNodeTitles.Colon); }
            set { SetChildByTitle(AstNodeTitles.Colon, value); }
        }

        public override bool IsDefaultCase
        {
            get { return CaseKeyword.Value == "default"; }
        }
    }
}
