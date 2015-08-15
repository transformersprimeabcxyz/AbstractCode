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

using AbstractCode.Ast.Statements;

namespace AbstractCode.Ast.Members
{
    public class AccessorDeclaration : MemberDeclaration
    {
        public AstToken AccessorKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public BlockStatement Body
        {
            get { return GetChildByTitle(AstNodeTitles.Body); }
            set { SetChildByTitle(AstNodeTitles.Body, value); }
        }

        public bool HasBody
        {
            get { return Body != null; }
        }

        public override bool Match(AstNode other)
        {
            var declaration = other as AccessorDeclaration;
            return declaration != null
                   && MatchModifiersAndAttributes(declaration)
                   && Body.Match(declaration.Body);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitAccessorDeclaration(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitAccessorDeclaration(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitAccessorDeclaration(this, data);
        }
    }
}
