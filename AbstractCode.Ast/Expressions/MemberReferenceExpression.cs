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
using System.Text;
using AbstractCode.Ast.Members;
using AbstractCode.Ast.Types;
using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;
using TypeReference = AbstractCode.Ast.Types.TypeReference;

namespace AbstractCode.Ast.Expressions
{
    public class MemberReferenceExpression : Expression, ITypeArgumentProvider, IConvertibleToType, IConvertibleToIdentifier
    {
        public MemberReferenceExpression()
        {
            TypeArguments = new AstNodeCollection<TypeReference>(this, AstNodeTitles.TypeArgument);
        }

        public MemberReferenceExpression(Expression target, string identifier)
            : this(target, MemberAccessor.Normal, new Identifier(identifier))
        {
        }

        public MemberReferenceExpression(Expression target, Identifier identifier)
            : this(target, MemberAccessor.Normal, identifier)
        {
        }

        public MemberReferenceExpression(Expression target, MemberAccessor accessor, Identifier identifier)
            : this()
        {
            Target = target;
            Accessor = accessor;
            Identifier = identifier;
        }

        public Expression Target
        {
            get { return GetChildByTitle(AstNodeTitles.TargetExpression); }
            set { SetChildByTitle(AstNodeTitles.TargetExpression, value); }
        }

        public virtual MemberAccessor Accessor
        {
            get;
            set;
        }

        public AstToken AccessorToken
        {
            get { return GetChildByTitle(AstNodeTitles.Accessor); }
            set { SetChildByTitle(AstNodeTitles.Accessor, value); }
        }

        public Identifier Identifier
        {
            get { return GetChildByTitle(AstNodeTitles.Identifier); }
            set { SetChildByTitle(AstNodeTitles.Identifier, value); }
        }

        public AstNodeCollection<TypeReference> TypeArguments
        {
            get;
        }

        public TypeReference ToTypeReference()
        {
            TypeReference newTarget;

            var convertibleTarget = Target as IConvertibleToType;
            if (convertibleTarget != null)
                newTarget = convertibleTarget.ToTypeReference();
            else
                throw new ArgumentException("Target is not convertible to a type reference.");

           return new MemberTypeReference((TypeReference)newTarget.Remove(), Identifier.CreateCopy());
        }

        public Identifier ToIdentifier()
        {
            var builder = new StringBuilder();
            Expression currentNode = this;

            while (true)
            {
                if (builder.Length > 0)
                    builder.Insert(0, AccessorToken.Value);

                if (currentNode is MemberReferenceExpression)
                {
                    var reference = currentNode as MemberReferenceExpression;
                    builder.Insert(0, reference.Identifier.Name);
                    currentNode = (currentNode as MemberReferenceExpression).Target;
                }
                else if (currentNode is IdentifierExpression)
                {
                    builder.Insert(0, (currentNode as IdentifierExpression).Identifier.Name);
                    break;
                }
                else
                    throw new ArgumentException();
            }

            var identifier = new Identifier(builder.ToString(), this.Range);
            ReplaceWith(identifier);
            return identifier;
        }

        public override ResolveResult Resolve(IScope scope)
        {
            if (scope != null)
            {
                var resolvableTarget = Target as IResolvable;
                var targetResult = resolvableTarget?.Resolve(scope);

                if (targetResult?.ScopeProvider != null)
                    return targetResult.ScopeProvider.GetScope().ResolveIdentifier(Identifier.Name);
            }

            return new UnknownIdentifierResolveResult(Identifier.Name);
        }

        public override string ToString()
        {
            return string.Format("{0} (NamespaceIdentifier = {1}, Accessor = {2})", GetType().Name, Identifier.Name, Accessor);
        }

        public override bool Match(AstNode other)
        {
            var expression = other as MemberReferenceExpression;
            return expression != null
                   && Target.MatchOrNull(expression.Target)
                   && Accessor == expression.Accessor
                   && Identifier.MatchOrNull(expression.Identifier);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitMemberReferenceExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitMemberReferenceExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitMemberReferenceExpression(this, data);
        }
    }

    public enum MemberAccessor
    {
        Normal,
        Static,
        Pointer,
        NullPropagation
    }
}
