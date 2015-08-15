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
using AbstractCode.Ast.Members;
using ConstructorDeclarationBase = AbstractCode.Ast.Members.ConstructorDeclaration;

namespace AbstractCode.Ast.CSharp.Members
{
    public class ConstructorDeclaration : ConstructorDeclarationBase
    {
        public static readonly AstNodeTitle<ConstructorInitializer> InitializerTitle = new AstNodeTitle<ConstructorInitializer>("Initializer");
        
        public ConstructorDeclaration()
        {
        }

        public ConstructorDeclaration(IEnumerable<ParameterDeclaration> parameters)
            : base(parameters)
        {
        }

        public ConstructorDeclaration(IEnumerable<ParameterDeclaration> parameters, ConstructorInitializer initializer)
            : base(parameters)
        {
            Initializer = initializer;
        }

        public AstToken Colon
        {
            get { return GetChildByTitle(AstNodeTitles.Colon); }
            set { SetChildByTitle(AstNodeTitles.Colon, value); }
        }

        public ConstructorInitializer Initializer
        {
            get { return GetChildByTitle(InitializerTitle); }
            set { SetChildByTitle(InitializerTitle, value); }
        }
    }
}
