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

namespace AbstractCode.Ast.Members
{
    public class CustomAttributeSection : AstNode
    {
        public CustomAttributeSection()
        {
            Attributes = new AstNodeCollection<CustomAttribute>(this, AstNodeTitles.CustomAttribute);
        }

        public CustomAttributeSection(params CustomAttribute[] attributes) 
            :this((IEnumerable<CustomAttribute>)attributes)
        {

        }

        public CustomAttributeSection(IEnumerable<CustomAttribute> attributes)
            : this()
        {
            Attributes.AddRange(attributes);
        }

        public AstToken LeftBracket
        {
            get { return GetChildByTitle(AstNodeTitles.LeftBracket); }
            set { SetChildByTitle(AstNodeTitles.LeftBracket, value); }
        }

        public AstNodeCollection<CustomAttribute> Attributes
        {
            get;
        }

        public AstToken RightBracket
        {
            get { return GetChildByTitle(AstNodeTitles.LeftBracket); }
            set { SetChildByTitle(AstNodeTitles.LeftBracket, value); }
        }

        public override bool Match(AstNode other)
        {
            var section = other as CustomAttributeSection;
            return section != null && Attributes.Match(section.Attributes);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitCustomAttributeSection(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitCustomAttributeSection(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitCustomAttributeSection(this, data);
        }
    }
}
