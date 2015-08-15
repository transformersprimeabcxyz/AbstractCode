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
    public class PropertyDeclaration : MemberDeclaration, IDefinitionProvider
    {
        private sealed class PropertyDeclarationWrapper : PropertyDefinition
        {
            private readonly PropertyDeclaration _declaration;
            private TypeDefinition _memberType;

            public PropertyDeclarationWrapper(PropertyDeclaration declaration)
            {
                _declaration = declaration;
            }
            
            public override bool CanRead
            {
                get { return _declaration.Getter != null; }
            }

            public override bool CanWrite
            {
                get { return _declaration.Setter != null; }
            }

            public override string Name
            {
                get { return _declaration.Name; }
            }

            public override TypeDefinition DeclaringType
            {
                get
                {
                    var declaringType = _declaration.Parent as TypeDeclaration;
                    return declaringType?.GetDefinition();
                }
            }

            public override TypeDefinition MemberType
            {
                get
                {
                    if (_memberType != null)
                        return _memberType;

                    var symbolsReference = _declaration.PropertyType.GetSymbolsReference();
                    var result = symbolsReference.Resolve(GetDeclaringScope());
                    return _memberType = result.ScopeProvider as TypeDefinition;
                }
            }

            public override IScope GetDeclaringScope()
            {
                return DeclaringType?.GetScope();
            }
        }

        public static readonly AstNodeTitle<AccessorDeclaration> GetterTitle = new AstNodeTitle<AccessorDeclaration>("Getter");
        public static readonly AstNodeTitle<AccessorDeclaration> SetterTitle = new AstNodeTitle<AccessorDeclaration>("Setter");
        private PropertyDeclarationWrapper _definition;

        public PropertyDeclaration()
        {
            Parameters = new AstNodeCollection<ParameterDeclaration>(this, AstNodeTitles.Parameter);
        }
        
        public TypeReference PropertyType
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }

        public Identifier Identifier
        {
            get { return GetChildByTitle(AstNodeTitles.Identifier); }
            set { SetChildByTitle(AstNodeTitles.Identifier, value); }
        }

        public AstToken LeftBracket
        {
            get { return GetChildByTitle(AstNodeTitles.LeftBracket); }
            set { SetChildByTitle(AstNodeTitles.LeftBracket, value); }
        }

        public AstNodeCollection<ParameterDeclaration> Parameters
        {
            get;
            private set;
        }

        public AstToken RightBracket
        {
            get { return GetChildByTitle(AstNodeTitles.RightBracket); }
            set { SetChildByTitle(AstNodeTitles.RightBracket, value); }
        }

        public AstNode StartScope
        {
            get { return GetChildByTitle(AstNodeTitles.StartScope); }
            set { SetChildByTitle(AstNodeTitles.StartScope, value); }
        }

        public AccessorDeclaration Getter
        {
            get { return GetChildByTitle(GetterTitle); }
            set { SetChildByTitle(GetterTitle, value); }
        }

        public AccessorDeclaration Setter
        {
            get { return GetChildByTitle(SetterTitle); }
            set { SetChildByTitle(SetterTitle, value); }
        }

        public AstNode EndScope
        {
            get { return GetChildByTitle(AstNodeTitles.EndScope); }
            set { SetChildByTitle(AstNodeTitles.EndScope, value); }
        }

        public override bool Match(AstNode other)
        {
            var declaration = other as PropertyDeclaration;
            return declaration != null
                   && MatchModifiersAndAttributes(declaration)
                   && PropertyType.MatchOrNull(declaration.PropertyType)
                   && Identifier.MatchOrNull(declaration.Identifier)
                   && Getter.MatchOrNull(declaration.Getter)
                   && Setter.MatchOrNull(declaration.Setter);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitPropertyDeclaration(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitPropertyDeclaration(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitPropertyDeclaration(this, data);
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
                if (type == null)
                    return Name;

                return string.Format("{0} {1}::{2}",
                    PropertyType.FullName,
                    type.FullName,
                    Name);
            }
        }

        private PropertyDefinition GetDefinition()
        {
            return _definition ?? (_definition = new PropertyDeclarationWrapper(this));
        }

        IEnumerable<INamedDefinition> IDefinitionProvider.GetDefinitions()
        {
            yield return GetDefinition();
        }
    }
}
