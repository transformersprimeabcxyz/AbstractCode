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
    public class EventDeclaration : MemberDeclaration, IVariableDeclaratorProvider, IDefinitionProvider
    {
        public static readonly AstNodeTitle<AstToken> EventKeywordTitle = new AstNodeTitle<AstToken>("EventKeyword");
        public static readonly AstNodeTitle<AccessorDeclaration> AddAccessorTitle = new AstNodeTitle<AccessorDeclaration>("AddAccessor");
        public static readonly AstNodeTitle<AccessorDeclaration> RemoveAccessorTitle = new AstNodeTitle<AccessorDeclaration>("RemoveAccessor");
        
        public EventDeclaration()
        {
            Declarators = new AstNodeCollection<VariableDeclarator>(this, AstNodeTitles.Declarator);
        }

        public TypeReference EventType
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }

        public AstToken EventKeyword
        {
            get { return GetChildByTitle(EventKeywordTitle); }
            set { SetChildByTitle(EventKeywordTitle, value); }
        }

        public AstNodeCollection<VariableDeclarator> Declarators
        {
            get;
        }

        public AstNode StartScope
        {
            get { return GetChildByTitle(AstNodeTitles.StartScope); }
            set { SetChildByTitle(AstNodeTitles.StartScope, value); }
        }

        public AccessorDeclaration AddAccessor
        {
            get { return GetChildByTitle(AddAccessorTitle); }
            set { SetChildByTitle(AddAccessorTitle, value); }
        }

        public AccessorDeclaration RemoveAccessor
        {
            get { return GetChildByTitle(RemoveAccessorTitle); }
            set { SetChildByTitle(RemoveAccessorTitle, value); }
        }

        public AstNode EndScope
        {
            get { return GetChildByTitle(AstNodeTitles.EndScope); }
            set { SetChildByTitle(AstNodeTitles.EndScope, value); }
        }

        public override bool Match(AstNode other)
        {
            var declaration = other as EventDeclaration;
            return declaration != null
                   && MatchModifiersAndAttributes(declaration)
                   && Declarators.Match(declaration.Declarators)
                   && AddAccessor.MatchOrNull(declaration.AddAccessor)
                   && RemoveAccessor.MatchOrNull(declaration.RemoveAccessor);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitEventDeclaration(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitEventDeclaration(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitEventDeclaration(this, data);
        }

        public IEnumerable<EventDefinition> GetDefinitions()
        {
            yield break; // TODO
        }
        
        IEnumerable<INamedDefinition> IDefinitionProvider.GetDefinitions()
        {
            return GetDefinitions();
        }
        
    }
}
