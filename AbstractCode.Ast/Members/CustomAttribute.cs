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
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Types;

namespace AbstractCode.Ast.Members
{
    public class CustomAttribute : AstNode
    {
        public CustomAttribute()
        {
            Arguments = new AstNodeCollection<Expression>(this, AstNodeTitles.Argument);
        }

        public CustomAttribute(TypeReference type, params Expression[] arguments)
            : this(type, (IEnumerable<Expression>)arguments)
        {

        }

        public CustomAttribute(TypeReference type, IEnumerable<Expression> arguments)
            : this()
        {
            Type = type;
            Arguments.AddRange(arguments);
        }

        public TypeReference Type
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }

        public AstToken LeftParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.LeftParenthese); }
            set { SetChildByTitle(AstNodeTitles.LeftParenthese, value); }
        }

        public AstNodeCollection<Expression> Arguments
        {
            get;
        }

        public AstToken RightParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.LeftParenthese); }
            set { SetChildByTitle(AstNodeTitles.LeftParenthese, value); }
        }

        public override bool Match(AstNode other)
        {
            var attribute = other as CustomAttribute;
            return attribute != null
                   && Type.MatchOrNull(attribute.Type)
                   && Arguments.Match(attribute.Arguments);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitCustomAttribute(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitCustomAttribute(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitCustomAttribute(this, data);
        }
    }
}
