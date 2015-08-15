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

using AbstractCode.Ast.Types;

namespace AbstractCode.Ast.Members
{
    
    public class UsingAliasDirective : UsingDirective
    {
        public UsingAliasDirective()
        {
        }

        public UsingAliasDirective(string alias, TypeReference type)
            : this(new Identifier(alias), type)
        {
        }

        public UsingAliasDirective(Identifier alias, TypeReference type)
        {
            AliasIdentifier = alias;
            TypeImport = type;
        }
        
        public Identifier AliasIdentifier
        {
            get { return GetChildByTitle(AstNodeTitles.Identifier); }
            set { SetChildByTitle(AstNodeTitles.Identifier, value); }
        }

        public AstToken OperatorToken
        {
            get { return GetChildByTitle(AstNodeTitles.Operator); }
            set { SetChildByTitle(AstNodeTitles.Operator, value); }
        }

        public TypeReference TypeImport
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }

        public override bool Match(AstNode other)
        {
            var directive = other as UsingAliasDirective;
            return directive != null
                   && AliasIdentifier.MatchOrNull(directive.AliasIdentifier)
                   && TypeImport.MatchOrNull(directive.TypeImport);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitUsingAliasDirective(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitUsingAliasDirective(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitUsingAliasDirective(this, data);
        }
    }
}