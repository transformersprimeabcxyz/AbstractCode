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
using AbstractCode.Symbols;
using TypeReference = AbstractCode.Ast.Types.TypeReference;

namespace AbstractCode.Ast.Members
{
    public class DelegateDeclaration : MemberDeclaration, IParameterProvider, ITypeParameterProvider, IDefinitionProvider
    {
        private static readonly AstNodeTitle<AstToken> DelegateKeywordTitle = new AstNodeTitle<AstToken>("DelegateKeyword");

        public DelegateDeclaration()
        {
            Parameters = new AstNodeCollection<ParameterDeclaration>(this, AstNodeTitles.Parameter);
            TypeParameters = new AstNodeCollection<TypeParameterDeclaration>(this, AstNodeTitles.TypeParameter);
        }

        public TypeReference DelegateType
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }

        public AstToken DelegateKeyword
        {
            get { return GetChildByTitle(DelegateKeywordTitle); }
            set { SetChildByTitle(DelegateKeywordTitle, value); }
        }

        public Identifier Identifier
        {
            get { return GetChildByTitle(AstNodeTitles.Identifier); }
            set { SetChildByTitle(AstNodeTitles.Identifier, value); }
        }

        public AstToken LeftChevron
        {
            get { return GetChildByTitle(AstNodeTitles.LeftChevron); }
            set { SetChildByTitle(AstNodeTitles.LeftChevron, value); }
        }

        public AstNodeCollection<TypeParameterDeclaration> TypeParameters
        {
            get;
        }

        public AstToken RightChevron
        {
            get { return GetChildByTitle(AstNodeTitles.RightChevron); }
            set { SetChildByTitle(AstNodeTitles.RightChevron, value); }
        }
        
        public AstToken LeftParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.LeftParenthese); }
            set { SetChildByTitle(AstNodeTitles.LeftParenthese, value); }
        }
        
        public AstNodeCollection<ParameterDeclaration> Parameters
        {
            get;
        }

        public AstToken RightParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.RightParenthese); }
            set { SetChildByTitle(AstNodeTitles.RightParenthese, value); }
        }

        public override bool Match(AstNode other)
        {
            var declaration = other as DelegateDeclaration;
            return declaration != null
                   && MatchModifiersAndAttributes(declaration)
                   && DelegateType.MatchOrNull(declaration.DelegateType)
                   && Identifier.MatchOrNull(declaration.Identifier)
                   && TypeParameters.Match(declaration.TypeParameters)
                   && Parameters.Match(declaration.Parameters);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitDelegateDeclaration(this);
        }
        
        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitDelegateDeclaration(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitDelegateDeclaration(this, data);
        }

        public TypeDefinition GetDefinition()
        {
            return null; // TODO
        }

        IEnumerable<INamedDefinition> IDefinitionProvider.GetDefinitions()
        {
            yield return GetDefinition();
        }
    }
}
