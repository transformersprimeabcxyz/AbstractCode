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
using AbstractCode.Ast.Statements;
using AbstractCode.Symbols;

namespace AbstractCode.Ast.Members
{
    public abstract class AbstractMethodDeclaration : MemberDeclaration, IScopeProvider, IParameterProvider, IDefinitionProvider
    {
        protected AbstractMethodDeclaration()
        {
            Parameters = new AstNodeCollection<ParameterDeclaration>(this, AstNodeTitles.Parameter);
        }

        public Identifier Identifier
        {
            get { return GetChildByTitle(AstNodeTitles.Identifier); }
            set { SetChildByTitle(AstNodeTitles.Identifier, value); }
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

        public BlockStatement Body
        {
            get { return GetChildByTitle(AstNodeTitles.Body); }
            set { SetChildByTitle(AstNodeTitles.Body, value); }
        }

        public bool HasBody
        {
            get { return Body != null; }
        }
        
        public string Name
        {
            get { return Identifier.Name; }
        }

        public string FullName
        {
            get
            {
                var type = Parent as TypeDeclaration;
                string name = type == null ? Name : string.Format("{0}::{1}", type.FullName, Name);
                return string.Format("{0}({1})", name, string.Join(", ", Parameters.Select(x => x.FullName)));
            }
        }

        public abstract MethodDefinition GetDefinition();
        
        IEnumerable<INamedDefinition> IDefinitionProvider.GetDefinitions()
        {
            yield return GetDefinition();
        }
        
        IScope IScopeProvider.GetScope()
        {
            return GetDefinition().GetScope();
        }
        
        IScope IScopeMember.GetDeclaringScope()
        {
            throw new NotImplementedException();
        }
    }
}
