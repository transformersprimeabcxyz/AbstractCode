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

namespace AbstractCode.Ast.Types
{
    public class ArrayTypeReference : ComplexTypeReference
    {
        public ArrayTypeReference()
        {
        }

        public ArrayTypeReference(TypeReference baseType)
            : base(baseType)
        {
            RankSpecifier = new ArrayTypeRankSpecifier(1);
        }

        public override string Name
        {
            get { return base.Name + RankSpecifier; }
        }

        public override string FullName
        {
            get { return base.FullName + RankSpecifier; }
        }

        public ArrayTypeRankSpecifier RankSpecifier
        {
            get { return GetChildByTitle(AstNodeTitles.RankSpecifier); }
            set { SetChildByTitle(AstNodeTitles.RankSpecifier, value); }
        }

        public override string ToString()
        {
            return string.Format("{0} (FullName = {1}, Dimensions = {2})", GetType().Name, FullName, RankSpecifier.Dimensions);
        }

        public override bool Match(AstNode other)
        {
            var reference = other as ArrayTypeReference;
            return reference != null
                   && BaseType.MatchOrNull(reference.BaseType)
                   && RankSpecifier.MatchOrNull(reference.RankSpecifier);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitArrayTypeReference(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitArrayTypeReference(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitArrayTypeReference(this, data);
        }
    }
}
