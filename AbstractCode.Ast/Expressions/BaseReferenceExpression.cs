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
using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;

namespace AbstractCode.Ast.Expressions
{
    public class BaseReferenceExpression : Expression
    {
        public AstToken BaseKeywordToken
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public override bool Match(AstNode other)
        {
            return other is BaseReferenceExpression;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitBaseReferenceExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitBaseReferenceExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitBaseReferenceExpression(this, data);
        }

        public override ResolveResult Resolve(IScope scope)
        {
            var declaration = GetNodeContainer<TypeDeclaration>();
            if (declaration == null)
                return new UnknownIdentifierResolveResult(BaseKeywordToken.Value);
            var definition = declaration.GetDefinition();
            return new SelfReferenceResolveResult(definition.MemberType);
        }

    }
}
