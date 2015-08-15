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
using AbstractCode.Symbols;
using TypeReference = AbstractCode.Ast.Types.TypeReference;

namespace AbstractCode.Ast.Members
{
    public class FieldDeclaration : MemberDeclaration, IVariableDeclaratorProvider, IDefinitionProvider
    {
        private sealed class FieldDeclarationWrapper : FieldDefinition
        {
            private readonly FieldDeclaration _declaration;
            private readonly VariableDeclarator _declarator;
            private TypeDefinition _memberType;

            public FieldDeclarationWrapper(FieldDeclaration declaration, VariableDeclarator declarator)
            {
                _declaration = declaration;
                _declarator = declarator;
            }

            public override bool IsConstant
            {
                get { return _declaration.Modifiers.HasFlag(Modifier.Const); }
            }

            public override object DefaultValue
            {
                get { return _declarator.Value; }
            }

            public override string Name
            {
                get { return _declarator.Identifier.Name; }
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

                    var symbolsReference = _declaration.FieldType.GetSymbolsReference();
                    var result = symbolsReference.Resolve(GetDeclaringScope());
                    return _memberType = result.ScopeProvider as TypeDefinition;
                }
            }
        }

        public FieldDeclaration()
        {
            Declarators = new AstNodeCollection<VariableDeclarator>(this, AstNodeTitles.Declarator);
        }

        public FieldDeclaration(string name, TypeReference fieldType)
            : this()
        {
            FieldType = fieldType;
            Declarators.Add(new VariableDeclarator(name));
        }

        public TypeReference FieldType
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }
        
        public AstNodeCollection<VariableDeclarator> Declarators
        {
            get;
        }

        public override bool Match(AstNode other)
        {
            var declaration = other as FieldDeclaration;
            return declaration != null
                   && MatchModifiersAndAttributes(declaration)
                   && FieldType.MatchOrNull(declaration.FieldType)
                   && Declarators.Match(declaration.Declarators);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitFieldDeclaration(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitFieldDeclaration(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitFieldDeclaration(this, data);
        }

        public FieldDefinition GetDefinition()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FieldDefinition> GetDefinitions()
        {
            foreach (var initializer in Declarators)
                yield return new FieldDeclarationWrapper(this, initializer);
        }

        IEnumerable<INamedDefinition> IDefinitionProvider.GetDefinitions()
        {
            return GetDefinitions();
        }
    }
}
