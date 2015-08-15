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
    public class MethodDeclaration : AbstractMethodDeclaration, ITypeParameterProvider, IDefinitionProvider
    {
        private sealed class MethodDeclarationWrapper : MethodDefinition
        {
            private readonly MethodDeclaration _declaration;
            private TypeDefinition _memberType;

            public MethodDeclarationWrapper(MethodDeclaration declaration)
            {
                _declaration = declaration;
            }

            public override AssemblyDefinition Assembly
            {
                get { return null; } // TODO
            }

            public override IEnumerable<ParameterDefinition> GetParameters()
            {
                foreach (var declaration in _declaration.Parameters)
                    yield return declaration.GetDefinition();
            }

            public override string Name
            {
                get { return _declaration.Name; }
            }
            
            public override IScope GetDeclaringScope()
            {
                return _declaration.GetDeclaringScope();
            }

            public override TypeDefinition DeclaringType
            {
                get
                {
                    var typeDeclaration = _declaration.GetMemberContainer() as TypeDeclaration;
                    return typeDeclaration?.GetDefinition();
                }
            }

            public override TypeDefinition MemberType
            {
                get
                {
                    if (_memberType != null)
                        return _memberType;

                    var symbolsReference = _declaration.ReturnType.GetSymbolsReference();
                    var result = symbolsReference.Resolve(GetDeclaringScope());
                    return _memberType = result.ScopeProvider as TypeDefinition;
                }
            }
        }

        private MethodDefinition _definition;

        public MethodDeclaration()
        {
            TypeParameters = new AstNodeCollection<TypeParameterDeclaration>(this, AstNodeTitles.TypeParameter);
        }
        
        public MethodDeclaration(string name, TypeReference returnType)
        {
            Identifier = new Identifier(name);
            ReturnType = returnType;
            TypeParameters = new AstNodeCollection<TypeParameterDeclaration>(this, AstNodeTitles.TypeParameter);
        }

        public TypeReference ReturnType
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
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

        public override bool Match(AstNode other)
        {
            var declaration = other as MethodDeclaration;
            return declaration != null
                   && MatchModifiersAndAttributes(declaration)
                   && Identifier.MatchOrNull(declaration.Identifier)
                   && ReturnType.MatchOrNull(declaration.ReturnType)
                   && TypeParameters.Match(declaration.TypeParameters)
                   && Parameters.Match(declaration.Parameters)
                   && Body.MatchOrNull(declaration.Body);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitMethodDeclaration(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitMethodDeclaration(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitMethodDeclaration(this, data);
        }

        public override MethodDefinition GetDefinition()
        {
            return _definition ?? (_definition = new MethodDeclarationWrapper(this));
        }

        IEnumerable<INamedDefinition> IDefinitionProvider.GetDefinitions()
        {
            yield return GetDefinition();
        }
    }
}
