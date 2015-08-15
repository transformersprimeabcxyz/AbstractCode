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

namespace AbstractCode.Ast.Members
{
    public class TypeParameterDeclaration : AstNode
    {
        public static readonly AstNodeTitle<AstToken> VarianceElementTitle = new AstNodeTitle<AstToken>("VarianceToken");

        public TypeParameterDeclaration()
        {

        }

        public TypeParameterDeclaration(Identifier identifier)
            : this(identifier, TypeParameterVariance.Invariant)
        {
        }

        public TypeParameterDeclaration(Identifier identifier, TypeParameterVariance variance)
        {
            Identifier = identifier;
            Variance = variance;
        }

        public TypeParameterDeclaration(string identifier)
            : this(new Identifier(identifier))
        {
        }

        public TypeParameterDeclaration(string identifier, TypeParameterVariance variance)
            : this(new Identifier(identifier), variance)
        {
        }

        public Identifier Identifier
        {
            get { return GetChildByTitle(AstNodeTitles.Identifier); }
            set { SetChildByTitle(AstNodeTitles.Identifier, value); }
        }

        public AstToken VarianceToken
        {
            get { return GetChildByTitle(VarianceElementTitle); }
            set { SetChildByTitle(VarianceElementTitle, value); }
        }

        public virtual TypeParameterVariance Variance
        {
            get;
            set;
        }

        public override bool Match(AstNode other)
        {
            var declaration = other as TypeParameterDeclaration;
            return declaration != null
                   && Identifier.MatchOrNull(declaration.Identifier)
                   && Variance == declaration.Variance;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitTypeParameterDeclaration(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitTypeParameterDeclaration(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitTypeParameterDeclaration(this, data);
        }
    }

    public enum TypeParameterVariance
    {
        Invariant,
        Covariant,
        Contravariant,
    }
}
