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

using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Members;
using AbstractCode.Ast.Statements;
using AbstractCode.Ast.Types;

namespace AbstractCode.Ast
{
    public abstract class AstNodeTitle
    {
        protected internal AstNodeTitle(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            protected set;
        }
    }

    public class AstNodeTitle<TNode> : AstNodeTitle where TNode : AstNode
    {
        public AstNodeTitle(string name)
            : base(name)
        {
        }
    }

    public static class AstNodeTitles
    {
        public static readonly AstNodeTitle<AstNode>                  Root                    = AstNode.RootTitle;
        public static readonly AstNodeTitle<AstNode>                  UnprocessedNode         = new AstNodeTitle<AstNode>("UnprocessedNode");
        public static readonly AstNodeTitle<Expression>               Expression              = new AstNodeTitle<Expression>("Expression");
        public static readonly AstNodeTitle<Expression>               TargetExpression        = new AstNodeTitle<Expression>("Target");
        public static readonly AstNodeTitle<Expression>               ValueExpression         = new AstNodeTitle<Expression>("Value");
        public static readonly AstNodeTitle<Statement>                Statement               = new AstNodeTitle<Statement>("Statement");
        public static readonly AstNodeTitle<AstToken>                 Operator                = new AstNodeTitle<AstToken>("Operator");
        public static readonly AstNodeTitle<AstToken>                 Accessor                = new AstNodeTitle<AstToken>("Accessor");
        public static readonly AstNodeTitle<Identifier>               Identifier              = new AstNodeTitle<Identifier>("Identifier");
        public static readonly AstNodeTitle<Expression>               Condition               = new AstNodeTitle<Expression>("Condition");
        public static readonly AstNodeTitle<TypeReference>            Type                    = new AstNodeTitle<TypeReference>("Type");
        public static readonly AstNodeTitle<AstToken>                 Keyword                 = new AstNodeTitle<AstToken>("Keyword");
        public static readonly AstNodeTitle<AstToken>                 Semicolon               = new AstNodeTitle<AstToken>("EndStatement");
        public static readonly AstNodeTitle<AstToken>                 Colon                   = new AstNodeTitle<AstToken>("RightBracket");
        public static readonly AstNodeTitle<AstToken>                 LeftParenthese          = new AstNodeTitle<AstToken>("StartElement");
        public static readonly AstNodeTitle<AstToken>                 RightParenthese         = new AstNodeTitle<AstToken>("EndElement");
        public static readonly AstNodeTitle<AstNode>                  StartScope              = new AstNodeTitle<AstNode>("StartScope");
        public static readonly AstNodeTitle<AstNode>                  EndScope                = new AstNodeTitle<AstNode>("EndScope");
        public static readonly AstNodeTitle<Expression>               Element                 = new AstNodeTitle<Expression>("Element");
        public static readonly AstNodeTitle<AstToken>                 ElementSeparator        = new AstNodeTitle<AstToken>("ElementSeparator");
        public static readonly AstNodeTitle<Expression>               Argument                = new AstNodeTitle<Expression>("Argument");
        public static readonly AstNodeTitle<BlockStatement>           Body                    = new AstNodeTitle<BlockStatement>("Body");
        public static readonly AstNodeTitle<Statement>                BodyStatement           = new AstNodeTitle<Statement>("Body");
        public static readonly AstNodeTitle<VariableDeclarator>       Declarator              = new AstNodeTitle<VariableDeclarator>("Declarator");
        public static readonly AstNodeTitle<ModifierElement>          Modifier                = new AstNodeTitle<ModifierElement>("Modifier");
        public static readonly AstNodeTitle<ParameterDeclaration>     Parameter               = new AstNodeTitle<ParameterDeclaration>("Parameter");
        public static readonly AstNodeTitle<MemberDeclaration>        MemberDeclaration       = new AstNodeTitle<MemberDeclaration>("MemberDeclaration");
        public static readonly AstNodeTitle<UsingDirective>           UsingDirective          = new AstNodeTitle<UsingDirective>("UsingDirective");
        public static readonly AstNodeTitle<Expression>               Index                   = new AstNodeTitle<Expression>("Index");
        public static readonly AstNodeTitle<AstToken>                 LeftBracket             = new AstNodeTitle<AstToken>("LeftBracket");
        public static readonly AstNodeTitle<AstToken>                 RightBracket            = new AstNodeTitle<AstToken>("RightBracket");
        public static readonly AstNodeTitle<Comment>                  Comment                 = new AstNodeTitle<Comment>("Comment");
        public static readonly AstNodeTitle<ArrayInitializer>         ArrayInitializer        = new AstNodeTitle<ArrayInitializer>("ArrayInitializer");
        public static readonly AstNodeTitle<AstToken>                 LeftChevron             = new AstNodeTitle<AstToken>("LeftChevron");
        public static readonly AstNodeTitle<AstToken>                 RightChevron            = new AstNodeTitle<AstToken>("RightChevron");
        public static readonly AstNodeTitle<TypeParameterDeclaration> TypeParameter           = new AstNodeTitle<TypeParameterDeclaration>("TypeParameter");
        public static readonly AstNodeTitle<TypeReference>            TypeArgument            = new AstNodeTitle<TypeReference>("TypeArgument");
        public static readonly AstNodeTitle<CustomAttribute>          CustomAttribute         = new AstNodeTitle<CustomAttribute>("CustomAttribute");
        public static readonly AstNodeTitle<CustomAttributeSection>   CustomAttributeSection  = new AstNodeTitle<CustomAttributeSection>("CustomAttributeSection");
        public static readonly AstNodeTitle<LinqClause>               LinqClause              = new AstNodeTitle<LinqClause>("LinqClause");
        public static readonly AstNodeTitle<NamespaceDeclaration>     NamespaceDeclaration    = new AstNodeTitle<NamespaceDeclaration>("NamespaceDeclaration");
        public static readonly AstNodeTitle<TypeDeclaration>          TypeDeclaration         = new AstNodeTitle<TypeDeclaration>("TypeDeclaration");
        public static readonly AstNodeTitle<ArrayTypeRankSpecifier>   RankSpecifier           = new AstNodeTitle<ArrayTypeRankSpecifier>("RankSpecifier");
    }
}
