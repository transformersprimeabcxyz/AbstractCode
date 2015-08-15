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

namespace AbstractCode.Ast.Expressions
{
    public class ArrayInitializer : Expression
    {
        public ArrayInitializer()
        {
            Elements = new AstNodeCollection<Expression>(this, AstNodeTitles.Element);
        }

        public ArrayInitializer(params Expression[] elements)
            : this()
        {
            Elements.AddRange(elements);
        }

        public AstNode OpeningBrace
        {
            get { return GetChildByTitle(AstNodeTitles.StartScope); }
            set { SetChildByTitle(AstNodeTitles.StartScope, value); }
        }

        public AstNodeCollection<Expression> Elements
        {
            get;
            private set;
        }

        public AstNode ClosingBrace
        {
            get { return GetChildByTitle(AstNodeTitles.EndScope); }
            set { SetChildByTitle(AstNodeTitles.EndScope, value); }
        }

        public override bool Match(AstNode other)
        {
            var initializer = other as ArrayInitializer;
            return initializer != null
                   && Elements.Match(initializer.Elements);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitArrayInitializer(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitArrayInitializer(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitArrayInitializer(this, data);
        }
    }
}
