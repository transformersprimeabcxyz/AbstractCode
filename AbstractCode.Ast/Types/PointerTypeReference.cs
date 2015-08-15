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
    public class PointerTypeReference : ComplexTypeReference
    {
        public PointerTypeReference()
        {

        }

        public PointerTypeReference(TypeReference baseType)
            : base(baseType)
        {

        }

        public override string Name
        {
            get { return base.Name + "*"; }
        }

        public override bool Match(AstNode other)
        {
            var reference = other as PointerTypeReference;
            return reference != null
                   && BaseType.MatchOrNull(reference.BaseType);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitPointerTypeReference(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitPointerTypeReference(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitPointerTypeReference(this, data);
        }
    }
}
