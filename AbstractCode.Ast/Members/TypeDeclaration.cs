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
using System.Text;
using System.Threading.Tasks;
using AbstractCode.Ast.Types;
using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;
using TypeReference = AbstractCode.Ast.Types.TypeReference;

namespace AbstractCode.Ast.Members
{
    public class TypeDeclaration : MemberDeclaration, IScopeProvider, ITypeParameterProvider
    {
        private sealed class TypeDeclarationWrapper : TypeDefinition
        {
            private readonly TypeDeclaration _declaration;

            public TypeDeclarationWrapper(TypeDeclaration declaration)
            {
                _declaration = declaration;
            }

            public override Symbols.AssemblyDefinition Assembly
            {
                get { return null; } // TODO
            }

            public override IEnumerable<MemberDefinition> GetMembers()
            {
                foreach (var member in _declaration.Members)
                {
                    var definitionProvider = member as IDefinitionProvider;
                    if (definitionProvider != null)
                    {
                        foreach (var definition in definitionProvider.GetDefinitions().Cast<MemberDefinition>())
                            yield return definition;
                    }
                }

                // return from member in _declaration.Members
                //        where member is IDefinitionProvider
                //        from definition in ((IDefinitionProvider)member).GetDefinitions()
                //        select definition;
            }

            private IEnumerable<TMemberDefinition> GetMembers<TMemberDefinition>() where TMemberDefinition : MemberDefinition
            {
                return from member in GetMembers()
                       where member is TMemberDefinition
                       select member as TMemberDefinition;
            }

            public override IEnumerable<TypeDefinition> GetNestedTypes()
            {
                return GetMembers<TypeDefinition>();
            }

            public override IEnumerable<FieldDefinition> GetFields()
            {
                return GetMembers<FieldDefinition>();
            }

            public override IEnumerable<EventDefinition> GetEvents()
            {
                return GetMembers<EventDefinition>();
            }
             
            public override IEnumerable<PropertyDefinition> GetProperties()
            {
                return GetMembers<PropertyDefinition>();
            }

            public override IEnumerable<MethodDefinition> GetMethods()
            {
                return GetMembers<MethodDefinition>();
            }

            public override string Name
            {
                get { return _declaration.Name; }
            }

            public override NamespaceDefinition Namespace
            {
                get
                {
                    var namespaceDeclaration = _declaration.Parent as NamespaceDeclaration;
                    return namespaceDeclaration?.GetDefinition();
                }
            }
            
            public override IScope GetDeclaringScope()
            {
                return _declaration.GetDeclaringScope();
            }
            
            public override TypeDefinition DeclaringType
            {
                get
                {
                    var declaringType = _declaration.Parent as TypeDeclaration;
                    return declaringType != null ? declaringType.GetDefinition() : null;
                }
            }

            public override TypeDefinition MemberType
            {
                get { return null; }
            }
        }

        public static readonly AstNodeTitle<AstToken> TypeKeywordTitle = new AstNodeTitle<AstToken>("TypeKeyword");
        public static readonly AstNodeTitle<AstToken> BaseTypePrecedorTitle = new AstNodeTitle<AstToken>("BaseTypePrecedor");
        public static readonly AstNodeTitle<TypeReference> BaseTypeTitle = new AstNodeTitle<TypeReference>("BaseType");
        private TypeDefinition _definition;

        public TypeDeclaration()
        {
            Members = new AstNodeCollection<MemberDeclaration>(this, AstNodeTitles.MemberDeclaration);
            BaseTypes = new AstNodeCollection<TypeReference>(this, BaseTypeTitle);
            TypeParameters = new AstNodeCollection<TypeParameterDeclaration>(this, AstNodeTitles.TypeParameter);
        }

        public TypeDeclaration(string name, TypeVariant typeVariant)
            : this(new Identifier(name), typeVariant)
        {
        }

        public TypeDeclaration(Identifier identifier, TypeVariant typeVariant)
            : this()
        {
            Identifier = identifier;
            TypeVariant = typeVariant;
        }

        public AstToken TypeVariantToken
        {
            get { return GetChildByTitle(TypeKeywordTitle); }
            set { SetChildByTitle(TypeKeywordTitle, value); }
        }

        public virtual TypeVariant TypeVariant
        {
            get;
            set;
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

        public AstToken BaseTypePrecedor
        {
            get { return GetChildByTitle(BaseTypePrecedorTitle); }
            set { SetChildByTitle(BaseTypePrecedorTitle, value); }
        }

        public AstNodeCollection<TypeReference> BaseTypes
        {
            get;
            private set;
        }

        public AstNode StartScope
        {
            get { return GetChildByTitle(AstNodeTitles.StartScope); }
            set { SetChildByTitle(AstNodeTitles.StartScope, value); }
        }

        public AstNodeCollection<MemberDeclaration> Members
        {
            get;
        }

        public AstNode EndScope
        {
            get { return GetChildByTitle(AstNodeTitles.EndScope); }
            set { SetChildByTitle(AstNodeTitles.EndScope, value); }
        }

        public string Name
        {
            get { return Identifier.Name; }
        }

        public string FullName
        {
            get
            {
                string parentName = null;
                var namespaceNode = Parent as NamespaceDeclaration;
                if (namespaceNode != null)
                    parentName = namespaceNode.Identifier.Name;
                else
                {
                    var typeDeclaration = Parent as TypeDeclaration;
                    if (typeDeclaration != null)
                        parentName = typeDeclaration.FullName;
                }

                return parentName == null 
                    ? Identifier.Name 
                    : string.Format("{0}.{1}", parentName, Identifier.Name);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} (Name = {1}, TypeVariant = {2})", GetType().Name, Identifier.Name, TypeVariant);
        }

        public override bool Match(AstNode other)
        {
            var declaration = other as TypeDeclaration;
            return declaration != null
                   && MatchModifiersAndAttributes(declaration)
                   && TypeVariant == declaration.TypeVariant
                   && Identifier.MatchOrNull(declaration.Identifier)
                   && BaseTypes.Match(declaration.BaseTypes)
                   && Members.Match(declaration.Members);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitTypeDeclaration(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitTypeDeclaration(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitTypeDeclaration(this, data);
        }

        public IEnumerable<TMember> GetMembers<TMember>() where TMember : MemberDeclaration
        {
            return from member in Members
                   where member is TMember
                   select member as TMember;
        }

        public TypeDefinition GetDefinition()
        {
            return _definition ?? (_definition = new TypeDeclarationWrapper(this));
        }

        public IScope GetScope()
        {
            return GetDefinition().GetScope();
        }
    }

    public enum TypeVariant
    {
        Class,
        ValueType,
        Enum,
        Interface,
        Module // VB only.
    }
}
