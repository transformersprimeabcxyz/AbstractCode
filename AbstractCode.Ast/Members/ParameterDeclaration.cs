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

using AbstractCode.Symbols;
using TypeReference = AbstractCode.Ast.Types.TypeReference;

namespace AbstractCode.Ast.Members
{
    public class ParameterDeclaration : MemberDeclaration
    {
        private sealed class ParameterDeclarationWrapper : ParameterDefinition
        {
            private readonly ParameterDeclaration _declaration;
            private TypeDefinition _parameterType;

            public ParameterDeclarationWrapper(ParameterDeclaration declaration)
            {
                _declaration = declaration;
            }

            // TODO:

            public override int Index
            {
                get { return -1; }
            }

            public override bool IsByVal
            {
                get { return !IsByRef; }
            }

            public override bool IsByRef
            {
                get { return _declaration.ParameterModifier.HasFlag(ParameterModifier.Ref); }
            }

            public override bool IsOut
            {
                get { return _declaration.ParameterModifier.HasFlag(ParameterModifier.Out); }
            }

            public override string Name
            {
                get { return _declaration.Declarator.Identifier.Name; }
            }

            public override IScope GetDeclaringScope()
            {
                return _declaration.GetDeclaringScope();
            }

            public override TypeDefinition VariableType
            {
                get
                {
                    if (_parameterType != null)
                        return _parameterType;

                    var resolveResult = _declaration.ParameterType.Resolve(GetDeclaringScope());
                    _parameterType = resolveResult.ScopeProvider as TypeDefinition;
                    return _parameterType;
                }
            }
        }

        public static readonly AstNodeTitle<AstToken> ParameterModifierElementTitle = new AstNodeTitle<AstToken>("ParameterModifierToken");
        private ParameterDefinition _definition;

        public ParameterDeclaration()
        {

        }

        public ParameterDeclaration(string name, TypeReference parameterType)
        {
            Declarator = new VariableDeclarator(name);
            ParameterType = parameterType;
        }

        public TypeReference ParameterType
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }

        public VariableDeclarator Declarator
        {
            get { return GetChildByTitle(AstNodeTitles.Declarator); }
            set { SetChildByTitle(AstNodeTitles.Declarator, value); }
        }

        public AstToken ParameterModifierToken
        {
            get { return GetChildByTitle(ParameterModifierElementTitle); }
            set { SetChildByTitle(ParameterModifierElementTitle, value); }
        }

        public virtual ParameterModifier ParameterModifier
        {
            get;
            set;
        }

        public override bool Match(AstNode other)
        {
            var declaration = other as ParameterDeclaration;
            return declaration != null
                   && MatchModifiersAndAttributes(declaration)
                   && ParameterModifier == declaration.ParameterModifier
                   && Declarator.MatchOrNull(declaration.Declarator);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitParameterDeclaration(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitParameterDeclaration(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitParameterDeclaration(this, data);
        }

        public string Name
        {
            get { return Declarator.Identifier.Name; }
        }

        public string FullName
        {
            get { return string.Format("{0} {1}", ParameterType.FullName, Name); }
        }

        public ParameterDefinition GetDefinition()
        {
            return _definition ?? (_definition = new ParameterDeclarationWrapper(this));
        }

        public override string ToString()
        {
            return $"{base.ToString()}, ParameterType: {ParameterType}, ParameterModifier: {ParameterModifier}, Name: {Name}";
        }
    }

    public enum ParameterModifier
    {
        None,
        Ref,
        In,
        Out,
        Params,
        This,
    }
}
