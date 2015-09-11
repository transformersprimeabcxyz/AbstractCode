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

using System.Linq;
using AbstractCode.Ast.CSharp.Expressions;
using AbstractCode.Ast.CSharp.Statements;
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Members;
using AbstractCode.Ast.Parser;
using AbstractCode.Ast.Statements;
using AbstractCode.Ast.Types;
using CatchClause = AbstractCode.Ast.CSharp.Statements.CatchClause;
using ConditionalExpression = AbstractCode.Ast.CSharp.Expressions.ConditionalExpression;
using DoLoopStatement = AbstractCode.Ast.CSharp.Statements.DoLoopStatement;
using ExplicitCastExpression = AbstractCode.Ast.CSharp.Expressions.ExplicitCastExpression;
using IfElseStatement = AbstractCode.Ast.CSharp.Statements.IfElseStatement;
using LockStatement = AbstractCode.Ast.CSharp.Statements.LockStatement;
using PointerTypeReference = AbstractCode.Ast.CSharp.Types.PointerTypeReference;
using PrimitiveTypeReference = AbstractCode.Ast.Types.PrimitiveTypeReference;
using SwitchCaseLabel = AbstractCode.Ast.CSharp.Statements.SwitchCaseLabel;
using SwitchStatement = AbstractCode.Ast.CSharp.Statements.SwitchStatement;
using TypeCheckExpression = AbstractCode.Ast.CSharp.Expressions.TypeCheckExpression;
using UsingStatement = AbstractCode.Ast.CSharp.Statements.UsingStatement;
using WhileLoopStatement = AbstractCode.Ast.CSharp.Statements.WhileLoopStatement;
using YieldStatement = AbstractCode.Ast.CSharp.Statements.YieldStatement;
using static AbstractCode.Ast.CSharp.CSharpAstTokenCode;

namespace AbstractCode.Ast.CSharp
{
    public class CSharpGrammar : Grammar
    {
        public CSharpGrammar()
        {
            // Please let me know if there is a better way of tidying this :s

            #region Definitions to use later

            var statementList = new GrammarDefinition("StatementList");
            var statementListOptional = new GrammarDefinition("StatementListOptional",
                rule: null
                      | statementList);

            var variableDeclarator = new GrammarDefinition("VariableDeclarator");
            var variableDeclaratorList = new GrammarDefinition("VariableDeclaratorList");
            variableDeclaratorList.Rule = variableDeclarator
                                          | variableDeclaratorList
                                          + ToElement(COMMA)
                                          + variableDeclarator;
            var variableDeclaration = new GrammarDefinition("VariableDeclaration");
            var variableInitializer = new GrammarDefinition("VariableInitializer");
            var arrayInitializer = new GrammarDefinition("ArrayInitializer");
            var arrayInitializerOptional = new GrammarDefinition("ArrayInitializerOptional",
                rule: null | arrayInitializer);
            var identifierInsideBody = new GrammarDefinition("IdentifierInsideBody",
                rule: ToElement(IDENTIFIER),
                createNode: node => new Identifier(node.Children[0].Token.Value, node.Range));
            var identifierInsideBodyOptional = new GrammarDefinition("IdentifierInsideBodyOptional",
                rule: null | identifierInsideBody);

            variableDeclarator.Rule = identifierInsideBody
                                      | identifierInsideBody
                                      + ToElement(EQUALS)
                                      + variableInitializer;
            variableDeclarator.CreateNode = node =>
            {
                var result = new VariableDeclarator(node.Children[0].CreateAstNode<Identifier>());
                if (node.Children.Count > 1)
                {
                    result.OperatorToken = node.Children[1].Token;
                    result.Value = node.Children[2].CreateAstNode<Expression>();
                }
                return result;
            };

            var typeReference = new GrammarDefinition("TypeReference");

            var identifierExpression = new GrammarDefinition("IdentifierExpression",
                rule: identifierInsideBody,
                createNode: node => new IdentifierExpression(node.Children[0].CreateAstNode<Identifier>()));

            var usingDirectiveListOptional = new GrammarDefinition("UsingDirectiveListOptional");

            #endregion

            #region Type References

            var namespaceOrTypeExpression = new GrammarDefinition("NamespaceOrTypeExpression");

            namespaceOrTypeExpression.Rule = identifierExpression |
                                             namespaceOrTypeExpression
                                             + ToElement(DOT)
                                             + ToElement(IDENTIFIER);

            namespaceOrTypeExpression.CreateNode = node =>
            {
                if (node.Children.Count == 1)
                    return ((IConvertibleToType) node.Children[0].CreateAstNode()).ToTypeReference();
                var result = new MemberTypeReference();
                result.Target = node.Children[0].CreateAstNode<TypeReference>();
                result.AddChild(AstNodeTitles.Accessor, node.Children[1].Token);
                result.Identifier = new Identifier(node.Children[2].Token.Value, node.Children[2].Range);
                return result;
            };

            CreateAstNodeDelegate createPrimitiveTypeExpression = node =>
            {
                if (node.Children[0].Token == null)
                    return node.Children[0].CreateAstNode();
                return new PrimitiveTypeReference
                {
                    Identifier = new Identifier(node.Children[0].Token.Value, node.Range),
                    PrimitiveType = CSharpLanguage.PrimitiveTypeFromString(node.Children[0].Token.Value)
                };
            };

            var integralType = new GrammarDefinition("IntegralType",
                rule: ToElement(SBYTE)
                      | ToElement(BYTE)
                      | ToElement(SHORT)
                      | ToElement(USHORT)
                      | ToElement(INT)
                      | ToElement(UINT)
                      | ToElement(LONG)
                      | ToElement(ULONG)
                      | ToElement(CHAR),
                createNode: createPrimitiveTypeExpression);

            var primitiveType = new GrammarDefinition("PrimitiveTypeExpression",
                rule: ToElement(OBJECT)
                      | ToElement(STRING)
                      | ToElement(BOOL)
                      | ToElement(DECIMAL)
                      | ToElement(FLOAT)
                      | ToElement(DOUBLE)
                      | ToElement(VOID)
                      | integralType,
                createNode: createPrimitiveTypeExpression);

            var dimensionSeparators = new GrammarDefinition("DimensionSeparators");
            dimensionSeparators.Rule = ToElement(COMMA)
                                       | dimensionSeparators + ToElement(COMMA);

            var rankSpecifier = new GrammarDefinition("RankSpecifier",
                rule: ToElement(OPEN_BRACKET) + ToElement(CLOSE_BRACKET)
                      | ToElement(OPEN_BRACKET) + dimensionSeparators
                      + ToElement(CLOSE_BRACKET),
                createNode: node =>
                {
                    var result = new ArrayTypeRankSpecifier();
                    result.LeftBracket = node.Children[0].Token;
                    if (node.Children.Count == 3)
                    {
                        foreach (var dimensionSeparator in node.Children[1].GetAllNodesFromListDefinition()
                            .Select(x => x.Token))
                        {
                            result.Dimensions++;
                            result.AddChild(AstNodeTitles.ElementSeparator, dimensionSeparator);
                        }
                    }
                    result.RightBracket = node.Children[node.Children.Count - 1].Token;
                    return result;
                });

            var arrayType = new GrammarDefinition("ArrayType",
                rule: typeReference
                      + rankSpecifier,
                createNode: node => new ArrayTypeReference()
                {
                    BaseType = node.Children[0].CreateAstNode<TypeReference>(),
                    RankSpecifier = node.Children[1].CreateAstNode<ArrayTypeRankSpecifier>()
                });

            var pointerType = new GrammarDefinition("PointerType",
                rule: typeReference
                      + ToElement(STAR),
                createNode: node => new PointerTypeReference()
                {
                    BaseType = node.Children[0].CreateAstNode<TypeReference>(),
                    PointerToken = node.Children[1].Token
                });

            var typeExpression = new GrammarDefinition("TypeExpression",
                rule: namespaceOrTypeExpression
                      | primitiveType);

            typeReference.Rule = typeExpression
                                 | arrayType
                                 | pointerType
                ;

            #endregion

            #region Expressions

            CreateAstNodeDelegate createBinaryOperatorExpression = node =>
            {
                if (node.Children.Count == 1)
                    return node.Children[0].CreateAstNode();

                var result = new BinaryOperatorExpression();
                result.Left = node.Children[0].CreateAstNode<Expression>();
                var operatorToken = node.Children[1].Token ?? node.Children[1].Children[0].Token;
                result.Operator = CSharpLanguage.BinaryOperatorFromString(operatorToken.Value);
                result.OperatorToken = operatorToken;
                result.Right = node.Children[2].CreateAstNode<Expression>();
                return result;
            };

            var expression = new GrammarDefinition("Expression");
            var expressionOptional = new GrammarDefinition("ExpressionOptional",
                rule: null
                      | expression);

            var primaryExpression = new GrammarDefinition("PrimaryExpression");

            var primitiveExpression = new GrammarDefinition("PrimitiveExpression",
                rule: ToElement(LITERAL)
                      | ToElement(TRUE)
                      | ToElement(FALSE)
                      | ToElement(NULL),
                createNode: node =>
                {
                    object interpretedValue;
                    node.Children[0].Token.UserData.TryGetValue("InterpretedValue", out interpretedValue);
                    var result = new PrimitiveExpression(interpretedValue, node.Children[0].Token.Value, node.Range);
                    return result;
                });

            var parenthesizedExpression = new GrammarDefinition("ParenthesizedExpression",
                rule: ToElement(OPEN_PARENS)
                      + expression
                      + ToElement(CLOSE_PARENS),
                createNode: node => new ParenthesizedExpression
                {
                    LeftParenthese = node.Children[0].Token,
                    Expression = node.Children[1].CreateAstNode<Expression>(),
                    RightParenthese = node.Children[2].Token,
                });

            var memberAccessorOperator = new GrammarDefinition("MemberAccessorOperator",
                rule: ToElement(DOT)
                      | ToElement(OP_PTR)
                      | ToElement(INTERR_OPERATOR));

            var memberReferenceExpression = new GrammarDefinition("MemberReferenceExpression",
                rule: primaryExpression + memberAccessorOperator + identifierInsideBody,
                createNode: node => new MemberReferenceExpression
                {
                    Target =
                        (Expression)
                            ((IConvertibleToExpression) node.Children[0].CreateAstNode()).ToExpression().Remove(),
                    Accessor = CSharpLanguage.AccessorFromString(node.Children[1].Children[0].Token.Value),
                    AccessorToken = node.Children[1].Children[0].Token,
                    Identifier = node.Children[2].CreateAstNode<Identifier>()
                });

            var argument = new GrammarDefinition("Argument",
                rule: expression
                      | ToElement(REF) + expression
                      | ToElement(OUT) + expression,
                createNode: node =>
                {
                    if (node.Children.Count > 1)
                    {
                        return new DirectionExpression()
                        {
                            DirectionToken = node.Children[0].Token,
                            Direction = CSharpLanguage.DirectionFromString(node.Children[0].Token.Value),
                            Expression = node.Children[1].CreateAstNode<Expression>()
                        };
                    }
                    return node.Children[0].CreateAstNode<Expression>();
                });

            var argumentList = new GrammarDefinition("ArgumentList");
            argumentList.Rule = argument
                                | argumentList + ToElement(COMMA) + argument;
            var argumentListOptional = new GrammarDefinition("ArgumentListOptional",
                rule: null | argumentList);

            var invocationExpression = new GrammarDefinition("InvocationExpression",
                rule: primaryExpression
                      + ToElement(OPEN_PARENS)
                      + argumentListOptional
                      + ToElement(CLOSE_PARENS),
                createNode: node =>
                {
                    var result = new InvocationExpression()
                    {
                        Target = node.Children[0].CreateAstNode<Expression>(),
                        LeftParenthese = node.Children[1].Token,
                    };

                    if (node.Children[2].HasChildren)
                    {
                        foreach (var subNode in node.Children[2].Children[0].GetAllNodesFromListDefinition())
                        {
                            if (subNode.Token != null)
                                result.AddChild(AstNodeTitles.ElementSeparator, subNode.Token);
                            else
                                result.Arguments.Add(subNode.CreateAstNode<Expression>());
                        }
                    }

                    result.RightParenthese = node.Children[3].Token;
                    return result;
                });

            var indexerExpression = new GrammarDefinition("IndexerExpression",
                rule: primaryExpression
                      + ToElement(OPEN_BRACKET)
                      + argumentList
                      + ToElement(CLOSE_BRACKET),
                createNode: node =>
                {
                    var result = new IndexerExpression()
                    {
                        Target = node.Children[0].CreateAstNode<Expression>(),
                        LeftBracket = node.Children[1].Token,
                    };

                    foreach (var subNode in node.Children[2].GetAllNodesFromListDefinition())
                    {
                        if (subNode.Token != null)
                            result.AddChild(AstNodeTitles.ElementSeparator, subNode.Token);
                        else
                            result.Indices.Add(subNode.CreateAstNode<Expression>());
                    }

                    result.RightBracket = node.Children[3].Token;
                    return result;
                });

            var createObjectExpression = new GrammarDefinition("CreateObjectExpression",
                rule: ToElement(NEW)
                      + typeReference
                      + ToElement(OPEN_PARENS)
                      + argumentListOptional
                      + ToElement(CLOSE_PARENS)
                      + arrayInitializerOptional
                      | ToElement(NEW)
                      + namespaceOrTypeExpression
                      + arrayInitializer,
                createNode: node =>
                {
                    var result = new CreateObjectExpression();
                    result.NewKeyword = node.Children[0].Token;
                    result.Type = node.Children[1].CreateAstNode<TypeReference>();

                    if (node.Children.Count == 6)
                    {
                        result.LeftParenthese = node.Children[2].Token;

                        if (node.Children[3].HasChildren)
                        {
                            foreach (var subNode in node.Children[3].Children[0].GetAllNodesFromListDefinition())
                            {
                                if (subNode.Token != null)
                                    result.AddChild(AstNodeTitles.ElementSeparator, subNode.Token);
                                else
                                    result.Arguments.Add(subNode.CreateAstNode<Expression>());
                            }
                        }

                        result.RightParenthese = node.Children[4].Token;
                    }

                    var initializerNode = node.Children[node.Children.Count - 1];
                    if (initializerNode.HasChildren)
                        result.Initializer = initializerNode.CreateAstNode<ArrayInitializer>();
                    return result;
                });

            var createArrayExpression = new GrammarDefinition("CreateArrayExpression",
                rule: ToElement(NEW)
                      + rankSpecifier
                      + arrayInitializer
                      | ToElement(NEW)
                      + typeReference
                      + rankSpecifier
                      + arrayInitializer
                      | ToElement(NEW)
                      + typeReference
                      + ToElement(OPEN_BRACKET)
                      + argumentList
                      + ToElement(CLOSE_BRACKET)
                      + arrayInitializerOptional
                ,
                createNode: node =>
                {
                    var result = new CreateArrayExpression();
                    result.NewKeyword = node.Children[0].Token;

                    switch (node.Children.Count)
                    {
                        case 3:
                            var rankSpecifierNode = node.Children[1].CreateAstNode<ArrayTypeRankSpecifier>();
                            result.LeftBracket = (AstToken) rankSpecifierNode.LeftBracket.Remove();
                            result.RightBracket = (AstToken) rankSpecifierNode.RightBracket.Remove();
                            break;
                        case 6:
                            result.Type = node.Children[1].CreateAstNode<TypeReference>();
                            result.LeftBracket = node.Children[2].Token;
                            if (node.Children[3].HasChildren)
                            {
                                foreach (var subNode in node.Children[3].Children[0].GetAllNodesFromListDefinition())
                                {
                                    if (subNode.Token != null)
                                        result.AddChild(AstNodeTitles.ElementSeparator, subNode.Token);
                                    else
                                        result.Arguments.Add(subNode.CreateAstNode<Expression>());
                                }
                            }
                            result.RightBracket = node.Children[4].Token;
                            break;
                    }
                    var initializerNode = node.Children[node.Children.Count - 1];
                    if (initializerNode.HasChildren)
                        result.Initializer = initializerNode.CreateAstNode<ArrayInitializer>();
                    return result;
                });

            var primitiveTypeExpression = new GrammarDefinition("PrimitiveTypeExpression",
                rule: primitiveType,
                createNode: node => ((IConvertibleToExpression) node.Children[0].CreateAstNode()).ToExpression());

            var typeNameExpression = new GrammarDefinition("TypeNameExpression",
                rule: identifierExpression
                      | memberReferenceExpression
                      | primitiveTypeExpression,
                createNode: node =>
                {
                    var result = node.Children[0].CreateAstNode();
                    var type = ((IConvertibleToType) result).ToTypeReference();
                    if (CSharpAstToken.KeywordMapping.ContainsKey(type.Name))
                    {
                        return new PrimitiveTypeReference()
                        {
                            PrimitiveType = CSharpLanguage.PrimitiveTypeFromString(type.Name),
                            Identifier = (Identifier) type.Identifier.Remove()
                        };
                    }
                    return result;
                });

            var thisExpression = new GrammarDefinition("ThisExpression",
                rule: ToElement(THIS),
                createNode: node => new ThisReferenceExpression()
                {
                    ThisKeywordToken = node.Children[0].Token,
                });

            var baseExpression = new GrammarDefinition("BaseExpression",
                rule: ToElement(BASE),
                createNode: node => new BaseReferenceExpression()
                {
                    BaseKeywordToken = node.Children[0].Token,
                });

            var typeofExpression = new GrammarDefinition("TypeOfExpression",
                rule: ToElement(TYPEOF) + ToElement(OPEN_PARENS) + typeReference
                      + ToElement(CLOSE_PARENS),
                createNode: node => new GetTypeExpression()
                {
                    GetTypeKeywordToken = node.Children[0].Token,
                    LeftParenthese = node.Children[1].Token,
                    TargetType = node.Children[2].CreateAstNode<TypeReference>(),
                    RightParenthese = node.Children[3].Token,
                });
            
            var defaultExpression = new GrammarDefinition("DefaultExpression",
                rule: ToElement(DEFAULT)
                      + ToElement(OPEN_PARENS)
                      + typeReference
                      + ToElement(CLOSE_PARENS),
                createNode: node => new DefaultExpression()
                {
                    KeywordToken = node.Children[0].Token,
                    LeftParenthese = node.Children[1].Token,
                    TargetType = node.Children[2].CreateAstNode<TypeReference>(),
                    RightParenthese = node.Children[3].Token,
                });

            var sizeofExpression = new GrammarDefinition("SizeOfExpression",
                rule: ToElement(SIZEOF) + ToElement(OPEN_PARENS) + typeReference
                      + ToElement(CLOSE_PARENS),
                createNode: node => new SizeOfExpression()
                {
                    KeywordToken = node.Children[0].Token,
                    LeftParenthese = node.Children[1].Token,
                    TargetType = node.Children[2].CreateAstNode<TypeReference>(),
                    RightParenthese = node.Children[3].Token,
                });

            var checkedExpression = new GrammarDefinition("CheckedExpression",
                rule:
                    ToElement(CHECKED) + ToElement(OPEN_PARENS) + expression +
                    ToElement(CLOSE_PARENS),
                createNode: node => new CheckedExpression()
                {
                    KeywordToken = node.Children[0].Token,
                    LeftParenthese = node.Children[1].Token,
                    TargetExpression = node.Children[2].CreateAstNode<Expression>(),
                    RightParenthese = node.Children[3].Token,
                });

            var uncheckedExpression = new GrammarDefinition("UncheckedExpression",
                rule:
                    ToElement(UNCHECKED) + ToElement(OPEN_PARENS) + expression +
                    ToElement(CLOSE_PARENS),
                createNode: node => new UncheckedExpression()
                {
                    KeywordToken = node.Children[0].Token,
                    LeftParenthese = node.Children[1].Token,
                    TargetExpression = node.Children[2].CreateAstNode<Expression>(),
                    RightParenthese = node.Children[3].Token,
                });

            var stackAllocExpression = new GrammarDefinition("StackAllocExpression",
                rule:
                    ToElement(STACKALLOC) 
                    + typeReference
                    + ToElement(OPEN_BRACKET) 
                    + expression 
                    + ToElement(CLOSE_BRACKET),
                createNode: node => new StackAllocExpression()
                {
                    KeywordToken = node.Children[0].Token,
                    Type = node.Children[1].CreateAstNode<TypeReference>(),
                    LeftBracket = node.Children[2].Token,
                    Counter = node.Children[3].CreateAstNode<Expression>(),
                    RightBracket = node.Children[4].Token,
                });

            primaryExpression.Rule =
                typeNameExpression
                | primitiveExpression
                | parenthesizedExpression
                | invocationExpression
                | indexerExpression
                | thisExpression
                | baseExpression
                | createObjectExpression
                | createArrayExpression
                | typeofExpression
                // | defaultExpression
                | sizeofExpression
                | checkedExpression
                | uncheckedExpression
                | stackAllocExpression
                ;

            var preFixUnaryOperator = new GrammarDefinition("PreFixUnaryOperator",
                rule: ToElement(PLUS)
                      | ToElement(MINUS)
                      | ToElement(STAR)
                      | ToElement(BANG)
                      | ToElement(OP_INC)
                      | ToElement(OP_DEC)
                      | ToElement(BITWISE_AND)
                      | ToElement(TILDE)
                      | ToElement(AWAIT));
            var postFixUnaryOperator = new GrammarDefinition("PostFixUnaryOperator",
                rule: ToElement(OP_INC)
                      | ToElement(OP_DEC));


            var castExpression = new GrammarDefinition("CastExpression");
            
            var unaryOperatorExpression = new GrammarDefinition("UnaryOperatorExpression",
                rule: primaryExpression
                      | castExpression
                      | (preFixUnaryOperator + primaryExpression)
                      | (primaryExpression + postFixUnaryOperator),
                createNode: node =>
                {
                    if (node.Children.Count == 1)
                        return node.Children[0].CreateAstNode();

                    var result = new UnaryOperatorExpression();
                    var isPrefix = node.Children[0].GrammarElement == preFixUnaryOperator;
                    if (isPrefix)
                    {
                        result.Operator =
                            CSharpLanguage.UnaryOperatorFromString(node.Children[0].Children[0].Token.Value);
                        result.OperatorToken = node.Children[0].Children[0].Token;
                    }

                    result.Expression = node.Children[isPrefix ? 1 : 0].CreateAstNode<Expression>();
                    if (!isPrefix)
                    {
                        result.Operator =
                            CSharpLanguage.UnaryOperatorFromString(node.Children[1].Children[0].Token.Value, false);
                        result.OperatorToken = node.Children[1].Children[0].Token;
                    }
                    return result;
                });

            castExpression.Rule = ToElement(OPEN_PARENS)
                                  + typeNameExpression
                                  + ToElement(CLOSE_PARENS)
                                  + unaryOperatorExpression;
            castExpression.CreateNode = node => new ExplicitCastExpression
            {
                LeftParenthese = node.Children[0].Token,
                TargetType = ((IConvertibleToType)node.Children[1].CreateAstNode()).ToTypeReference(),
                RightParenthese = node.Children[2].Token,
                TargetExpression = node.Children[3].CreateAstNode<Expression>()
            };

            var multiplicativeOperator = new GrammarDefinition("MultiplicativeOperator",
                rule: ToElement(STAR)
                      | ToElement(DIV)
                      | ToElement(PERCENT));

            var multiplicativeExpression = new GrammarDefinition("MultiplicativeExpression");
            multiplicativeExpression.Rule = unaryOperatorExpression
                                            | multiplicativeExpression
                                            + multiplicativeOperator
                                            + unaryOperatorExpression;
            multiplicativeExpression.CreateNode = createBinaryOperatorExpression;

            var additiveOperator = new GrammarDefinition("AdditiveOperator",
                rule: ToElement(PLUS)
                      | ToElement(MINUS));
            var additiveExpression = new GrammarDefinition("AdditiveExpression");
            additiveExpression.Rule = multiplicativeExpression
                                      | additiveExpression
                                      + additiveOperator
                                      + multiplicativeExpression;
            additiveExpression.CreateNode = createBinaryOperatorExpression;

            var shiftOperator = new GrammarDefinition("ShiftOperator",
                rule: ToElement(OP_SHIFT_LEFT)
                      | ToElement(OP_SHIFT_RIGHT));
            var shiftExpression = new GrammarDefinition("ShiftExpression");
            shiftExpression.Rule = additiveExpression
                                   | shiftExpression
                                   + shiftOperator
                                   + additiveExpression;
            shiftExpression.CreateNode = createBinaryOperatorExpression;

            var relationalOperator = new GrammarDefinition("RelationalOperator",
                rule: ToElement(OP_GT)
                      | ToElement(OP_GE)
                      | ToElement(OP_LT)
                      | ToElement(OP_LE)
                      | ToElement(IS)
                      | ToElement(AS));
            var relationalExpression = new GrammarDefinition("RelationalExpression");
            relationalExpression.Rule = shiftExpression
                                        | relationalExpression
                                        + relationalOperator
                                        + shiftExpression;
            relationalExpression.CreateNode = node =>
            {
                if (node.Children.Count == 1)
                    return node.Children[0].CreateAstNode();
                var operatorToken = node.Children[1].Children[0].Token;
                switch (operatorToken.GetTokenCode())
                {
                    case (int)IS:
                        return new TypeCheckExpression()
                        {
                            TargetExpression = node.Children[0].CreateAstNode<Expression>(),
                            IsKeyword = operatorToken,
                            TargetType = ((IConvertibleToType)node.Children[2].CreateAstNode()).ToTypeReference()
                        };
                    case (int)AS:
                        return new SafeCastExpression()
                        {
                            TargetExpression = node.Children[0].CreateAstNode<Expression>(),
                            CastKeyword = operatorToken,
                            TargetType = ((IConvertibleToType)node.Children[2].CreateAstNode()).ToTypeReference()
                        };
                    default:
                        return createBinaryOperatorExpression(node);
                }
            };

            var equalityOperator = new GrammarDefinition("equalityOperator",
                rule: ToElement(OP_EQUALS)
                      | ToElement(OP_NOTEQUALS));
            var equalityExpression = new GrammarDefinition("EqualityExpression");
            equalityExpression.Rule = relationalExpression
                                      | equalityExpression
                                      + equalityOperator
                                      + relationalExpression;
            equalityExpression.CreateNode = createBinaryOperatorExpression;

            var logicalAndExpression = new GrammarDefinition("LogicalAndExpression");
            logicalAndExpression.Rule = equalityExpression
                                      | logicalAndExpression
                                      + ToElement(BITWISE_AND)
                                      + equalityExpression;
            logicalAndExpression.CreateNode = createBinaryOperatorExpression;

            var logicalXorExpression = new GrammarDefinition("LogicalOrExpression");
            logicalXorExpression.Rule = logicalAndExpression
                                      | logicalXorExpression
                                      + ToElement(CARRET)
                                      + logicalAndExpression;
            logicalXorExpression.CreateNode = createBinaryOperatorExpression;

            var logicalOrExpression = new GrammarDefinition("LogicalOrExpression");
            logicalOrExpression.Rule = logicalXorExpression
                                      | logicalOrExpression
                                      + ToElement(BITWISE_OR)
                                      + logicalXorExpression;
            logicalOrExpression.CreateNode = createBinaryOperatorExpression;

            var conditionalAndExpression = new GrammarDefinition("ConditionalAndExpression");
            conditionalAndExpression.Rule = logicalOrExpression
                                      | conditionalAndExpression
                                      + ToElement(OP_AND)
                                      + logicalOrExpression;
            conditionalAndExpression.CreateNode = createBinaryOperatorExpression;

            var conditionalOrExpression = new GrammarDefinition("ConditionalOrExpression");
            conditionalOrExpression.Rule = conditionalAndExpression
                                      | conditionalOrExpression
                                      + ToElement(OP_OR)
                                      + conditionalAndExpression;
            conditionalOrExpression.CreateNode = createBinaryOperatorExpression;

            var nullCoalescingExpression = new GrammarDefinition("NullCoalescingExpression");
            nullCoalescingExpression.Rule = conditionalOrExpression
                                      | nullCoalescingExpression
                                      + ToElement(OP_COALESCING)
                                      + conditionalOrExpression;
            nullCoalescingExpression.CreateNode = createBinaryOperatorExpression;

            var conditionalExpression = new GrammarDefinition("ConditionalExpression",
                rule: nullCoalescingExpression
                      | nullCoalescingExpression
                      + ToElement(INTERR)
                      + expression + ToElement(COLON) + expression,
                createNode: node => node.Children.Count == 1
                    ? node.Children[0].CreateAstNode()
                    : new ConditionalExpression
                    {
                        Condition = node.Children[0].CreateAstNode<Expression>(),
                        OperatorToken = node.Children[1].Token,
                        TrueExpression = node.Children[2].CreateAstNode<Expression>(),
                        ColonToken = node.Children[3].Token,
                        FalseExpression = node.Children[4].CreateAstNode<Expression>()
                    });

            var assignmentOperator = new GrammarDefinition("AssignmentOperator",
                rule: ToElement(EQUALS)
                      | ToElement(OP_ADD_ASSIGN)
                      | ToElement(OP_SUB_ASSIGN)
                      | ToElement(OP_MULT_ASSIGN)
                      | ToElement(OP_DIV_ASSIGN)
                      | ToElement(OP_AND_ASSIGN)
                      | ToElement(OP_OR_ASSIGN)
                      | ToElement(OP_XOR_ASSIGN)
                      | ToElement(OP_SHIFT_LEFT_ASSIGN)
                      | ToElement(OP_SHIFT_RIGHT_ASSIGN));
            var assignmentExpression = new GrammarDefinition("AssignmentExpression",
                rule: unaryOperatorExpression
                      + assignmentOperator
                      + expression,
                createNode: node => new AssignmentExpression
                {
                    Target = node.Children[0].CreateAstNode<Expression>(),
                    Operator = CSharpLanguage.AssignmentOperatorFromString(node.Children[1].Children[0].Token.Value),
                    OperatorToken = node.Children[1].Children[0].Token,
                    Value = node.Children[2].CreateAstNode<Expression>(),
                });

            var fromClause = new GrammarDefinition("FromClause",
                rule: ToElement(FROM) + identifierInsideBody + ToElement(IN) + expression,
                createNode: node => new LinqFromClause
                {
                    FromKeyword = node.Children[0].Token,
                    VariableName = node.Children[1].CreateAstNode<Identifier>(),
                    InKeyword = node.Children[2].Token,
                    DataSource = node.Children[3].CreateAstNode<Expression>()
                });

            var letClause = new GrammarDefinition("LetClause",
                rule: ToElement(LET) + variableDeclarator,
                createNode: node => new LinqLetClause()
                {
                    LetKeyword = node.Children[0].Token,
                    Variable = node.Children[1].CreateAstNode<VariableDeclarator>()
                });

            var whereClause = new GrammarDefinition("WhereClause",
                rule: ToElement(WHERE) + expression,
                createNode: node => new LinqWhereClause()
                {
                    WhereKeyword = node.Children[0].Token,
                    Condition = node.Children[1].CreateAstNode<Expression>()
                });

            var orderingDirection = new GrammarDefinition("OrderingDirection",
                rule: null | ToElement(ASCENDING) | ToElement(DESCENDING));

            var ordering = new GrammarDefinition("Ordering",
                rule: expression + orderingDirection,
                createNode: node =>
                {
                    var result = new LinqOrdering();
                    result.Expression = node.Children[0].CreateAstNode<Expression>();

                    if (node.Children[1].HasChildren)
                    {
                        var directionNode = node.Children[1].Children[0];
                        result.DirectionKeyword = directionNode.Token;
                        result.Direction = directionNode.Token != null
                            ? CSharpLanguage.OrderningDirectionFromString(directionNode.Token.Value)
                            : LinqOrderingDirection.None;
                    }

                    return result;
                });

            var orderings = new GrammarDefinition("Orderings");
            orderings.Rule = ordering | orderings + ToElement(COMMA) + ordering;

            var orderByClause = new GrammarDefinition("OrderByClause",
                rule: ToElement(ORDERBY) + orderings,
                createNode: node =>
                {
                    var result = new LinqOrderByClause();
                    result.OrderByKeyword = node.Children[0].Token;
                    foreach (var subNode in node.Children[1].GetAllNodesFromListDefinition())
                    {
                        if (subNode.Token != null)
                            result.AddChild(AstNodeTitles.ElementSeparator, subNode.Token);
                        else
                            result.Ordernings.Add(subNode.CreateAstNode<LinqOrdering>());
                    }
                    return result;
                });

            var groupByClause = new GrammarDefinition("GroupByClause",
                rule: ToElement(GROUP) + expression + ToElement(BY) + expression,
                createNode: node => new LinqGroupByClause()
                {
                    GroupKeyword = node.Children[0].Token,
                    Expression = node.Children[1].CreateAstNode<Expression>(),
                    ByKeyword = node.Children[2].Token,
                    KeyExpression = node.Children[3].CreateAstNode<Expression>()
                });

            var selectClause = new GrammarDefinition("SelectClause",
                rule: ToElement(SELECT) + expression,
                createNode: node => new LinqSelectClause()
                {
                    SelectKeyword = node.Children[0].Token,
                    Target = node.Children[1].CreateAstNode<Expression>()
                });

            var queryBodyClause = new GrammarDefinition("QueryBodyClause",
                rule:
                fromClause
                | letClause
                | groupByClause
                | whereClause
                | orderByClause
                 );

            var queryBodyClauses = new GrammarDefinition("QueryBodyClauses");
            queryBodyClauses.Rule = queryBodyClause | queryBodyClauses + queryBodyClause;

            var queryBodyClausesOptional = new GrammarDefinition("QueryBodyClausesOptional",
                rule: null | queryBodyClauses);

            var linqExpression = new GrammarDefinition("LinqExpression",
                rule: fromClause + queryBodyClausesOptional + selectClause,
                createNode: node =>
                {
                    var result = new LinqExpression();
                    result.Clauses.Add(node.Children[0].CreateAstNode<LinqFromClause>());

                    if (node.Children[1].HasChildren)
                    {
                        result.Clauses.AddRange(node.Children[1].Children[0].GetAllNodesFromListDefinition()
                            .Select(x => x.CreateAstNode<LinqClause>()));
                    }

                    result.Clauses.Add(node.Children[2].CreateAstNode<LinqSelectClause>());
                    return result;
                });

            expression.Rule = conditionalExpression
                              | linqExpression
                              | assignmentExpression;

            #endregion

            #region Statements
            var statement = new GrammarDefinition("Statement");
            var embeddedStatement = new GrammarDefinition("EmbeddedStatement");

            var emptyStatement = new GrammarDefinition("EmptyStatement",
                rule: ToElement(SEMICOLON),
                createNode: node => new EmptyStatement());

            var labelStatement = new GrammarDefinition("LabelStatement",
                rule: identifierInsideBody + ToElement(COLON),
                createNode: node => new LabelStatement(node.Children[0].CreateAstNode<Identifier>())
                {
                    Colon = node.Children[1].Token
                });

            var expressionStatement = new GrammarDefinition("ExpressionStatement",
                rule: expression + ToElement(SEMICOLON),
                createNode: node =>
                {
                    var result = new ExpressionStatement(node.Children[0].CreateAstNode<Expression>());
                    result.AddChild(AstNodeTitles.Semicolon, node.Children[1].Token);
                    return result;
                });

            var blockStatement = new GrammarDefinition("BlockStatement",
                rule: ToElement(OPEN_BRACE)
                      + statementListOptional
                      + ToElement(CLOSE_BRACE),
                createNode: node =>
                {
                    var result = new BlockStatement();
                    result.StartScope = node.Children[0].Token;
                    if (node.Children[1].HasChildren)
                    {
                        result.Statements.AddRange(node.Children[1].Children[0].GetAllNodesFromListDefinition()
                            .Select(x => x.CreateAstNode<Statement>()));
                    }
                    result.EndScope = node.Children[2].Token;
                    return result;
                });

            var variableDeclarationStatement = new GrammarDefinition("VariableDeclarationStatement",
                rule: variableDeclaration
                      + ToElement(SEMICOLON),
                createNode: node =>
                {
                    var result = node.Children[0].CreateAstNode<VariableDeclarationStatement>();
                    result.AddChild(AstNodeTitles.Semicolon, node.Children[1].Token);
                    return result;
                });

            var ifElseStatement = new GrammarDefinition("IfElseStatement",
                rule: ToElement(IF) + ToElement(OPEN_PARENS) + expression + ToElement(CLOSE_PARENS) + embeddedStatement
                | ToElement(IF) + ToElement(OPEN_PARENS) + expression + ToElement(CLOSE_PARENS) + embeddedStatement + ToElement(ELSE) + embeddedStatement
                ,
                createNode: node =>
                {
                    var result = new IfElseStatement();
                    result.IfKeyword = node.Children[0].Token;
                    result.LeftParenthese = node.Children[1].Token;
                    result.Condition = node.Children[2].CreateAstNode<Expression>();
                    result.RightParenthese = node.Children[3].Token;
                    result.TrueBlock = node.Children[4].CreateAstNode<Statement>();

                    if (node.Children.Count > 5)
                    {
                        result.ElseKeyword = node.Children[5].Token;
                        result.FalseBlock = node.Children[6].CreateAstNode<Statement>();
                    }
                    return result;
                });

            var switchLabel = new GrammarDefinition("SwitchLabel",
                rule: ToElement(CASE) + expression + ToElement(COLON)
                      | ToElement(DEFAULT) + ToElement(COLON),
                createNode: node =>
                {
                    var result = new SwitchCaseLabel();
                    result.CaseKeyword = node.Children[0].Token;
                    if (node.Children.Count > 2)
                        result.Condition = node.Children[1].CreateAstNode<Expression>();
                    result.Colon = node.Children[node.Children.Count - 1].Token;
                    return result;
                });

            var switchLabels = new GrammarDefinition("SwitchLabels");
            switchLabels.Rule = switchLabel | switchLabels + switchLabel;

            var switchSection = new GrammarDefinition("SwitchSection",
                rule: switchLabels + statementList,
                createNode: node =>
                {
                    var result = new SwitchSection();
                    result.Labels.AddRange(node.Children[0].GetAllNodesFromListDefinition()
                        .Select(x => x.CreateAstNode<SwitchCaseLabel>()));

                    result.Statements.AddRange(node.Children[1].GetAllNodesFromListDefinition()
                        .Select(x => x.CreateAstNode<Statement>()));

                    return result;
                });

            var switchSections = new GrammarDefinition("SwitchSections");
            switchSections.Rule = switchSection
                                  | switchSections + switchSection;

            var switchBlock = new GrammarDefinition("SwitchBlock",
                rule: ToElement(OPEN_BRACE)
                      + switchSections
                      + ToElement(CLOSE_BRACE));

            var switchStatement = new GrammarDefinition("SwitchStatement",
                rule: ToElement(SWITCH)
                      + ToElement(OPEN_PARENS)
                      + expression
                      + ToElement(CLOSE_PARENS)
                      + switchBlock,
                createNode: node =>
                {
                    var result = new SwitchStatement();
                    result.SwitchKeyword = node.Children[0].Token;
                    result.LeftParenthese = node.Children[1].Token;
                    result.Condition = node.Children[2].CreateAstNode<Expression>();
                    result.RightParenthese = node.Children[3].Token;
                    var switchBlockNode = node.Children[4];
                    result.StartScope = switchBlockNode.Children[0].Token;
                    result.Sections.AddRange(switchBlockNode.Children[1].GetAllNodesFromListDefinition()
                        .Select(x => x.CreateAstNode<SwitchSection>()));
                    result.EndScope = switchBlockNode.Children[2].Token;
                    return result;
                });

            var selectionStatement = new GrammarDefinition("SelectionStatement",
                rule: ifElseStatement
                      | switchStatement);

            var whileLoopStatement = new GrammarDefinition("WhileLoopStatement",
                rule: ToElement(WHILE)
                      + parenthesizedExpression
                      + embeddedStatement,
                createNode: node => new WhileLoopStatement
                {
                    WhileKeyword = node.Children[0].Token,
                    LeftParenthese = node.Children[1].Children[0].Token,
                    Condition = node.Children[1].Children[1].CreateAstNode<Expression>(),
                    RightParenthese = node.Children[1].Children[2].Token,
                    Body = node.Children[2].CreateAstNode<Statement>()
                });

            var doLoopStatement = new GrammarDefinition("DoLoopStatement",
                rule: ToElement(DO)
                      + embeddedStatement
                      + ToElement(WHILE)
                      + parenthesizedExpression
                      + ToElement(SEMICOLON),
                createNode: node => new DoLoopStatement
                {
                    DoKeyword = node.Children[0].Token,
                    Body = node.Children[1].CreateAstNode<Statement>(),
                    WhileKeyword = node.Children[2].Token,
                    LeftParenthese = node.Children[3].Children[0].Token,
                    Condition = node.Children[3].Children[1].CreateAstNode<Expression>(),
                    RightParenthese = node.Children[3].Children[2].Token,
                    Semicolon = node.Children[4].Token
                });

            var forLoopInitializer = new GrammarDefinition("ForLoopInitializer",
                rule: variableDeclaration
                     | null 
                     // TODO: statement-expression-list
                     );

            var forLoopCondition = new GrammarDefinition("ForLoopCondition",
                rule: expression 
                | null);

            var forLoopStatement = new GrammarDefinition("ForLoopStatement",
                rule: ToElement(FOR)
                + ToElement(OPEN_PARENS)
                + forLoopInitializer
                + ToElement(SEMICOLON)
                + expressionOptional
                + ToElement(SEMICOLON)
                + expressionOptional // TODO: statement-expression-list
                + ToElement(CLOSE_PARENS)
                + embeddedStatement,
                createNode: node =>
                {
                    var result = new ForLoopStatement();
                    result.ForKeyword = node.Children[0].Token;
                    result.LeftParenthese = node.Children[1].Token;

                    if (node.Children[2].HasChildren)
                    {
                        if (node.Children[2].Children[0].GrammarDefinition == variableDeclaration)
                        {
                            result.Initializers.Add(node.Children[2].CreateAstNode<VariableDeclarationStatement>());
                        }
                        else
                        {
                            result.Initializers.AddRange(node.Children[2].GetAllNodesFromListDefinition()
                                .Select(x => new ExpressionStatement(x.CreateAstNode<Expression>())));
                        }
                    }

                    result.AddChild(AstNodeTitles.Semicolon, node.Children[3].Token);

                    if (node.Children[4].HasChildren)
                        result.Condition = node.Children[4].CreateAstNode<Expression>();

                    result.AddChild(AstNodeTitles.Semicolon, node.Children[5].Token);

                    if (node.Children[6].HasChildren)
                    {
                        result.Iterators.AddRange(node.Children[6].Children[0].GetAllNodesFromListDefinition()
                            .Select(x => new ExpressionStatement(x.CreateAstNode<Expression>())));
                    }

                    result.RightParenthese = node.Children[7].Token;
                    result.Body = node.Children[8].CreateAstNode<Statement>();

                    return result;
                });

            var foreachLoopStatement = new GrammarDefinition("ForEachLoopStatement",
                rule: ToElement(FOREACH)
                      + ToElement(OPEN_PARENS)
                      + typeReference
                      + identifierInsideBody
                      + ToElement(IN)
                      + expression
                      + ToElement(CLOSE_PARENS)
                      + embeddedStatement,
                createNode: node => new ForeachLoopStatement
                {
                    ForeachKeyword = node.Children[0].Token,
                    LeftParenthese = node.Children[1].Token,
                    Type = node.Children[2].CreateAstNode<TypeReference>(),
                    Identifier = node.Children[3].CreateAstNode<Identifier>(),
                    InKeyword = node.Children[4].Token,
                    Target = node.Children[5].CreateAstNode<Expression>(),
                    RightParenthese = node.Children[6].Token,
                    Body = node.Children[7].CreateAstNode<Statement>()
                });

            var loopStatement = new GrammarDefinition("LoopStatement",
                rule: whileLoopStatement
                      | doLoopStatement
                      | forLoopStatement
                      | foreachLoopStatement);

            var lockStatement = new GrammarDefinition("LockStatement",
                rule: ToElement(LOCK)
                      + ToElement(OPEN_PARENS)
                      + expression
                      + ToElement(CLOSE_PARENS)
                      + statement,
                createNode: node => new LockStatement
                {
                    LockKeyword = node.Children[0].Token,
                    LeftParenthese = node.Children[1].Token,
                    LockObject = node.Children[2].CreateAstNode<Expression>(),
                    RightParenthese = node.Children[3].Token,
                    Body = node.Children[4].CreateAstNode<Statement>()
                });

            var resourceAcquisition = new GrammarDefinition("ResourceAcquisition",
                rule: variableDeclaration | expression);

            var usingStatement = new GrammarDefinition("UsingStatement",
                rule: ToElement(USING)
                      + ToElement(OPEN_PARENS)
                      + resourceAcquisition
                      + ToElement(CLOSE_PARENS)
                      + statement,
                createNode: node => new UsingStatement()
                {
                    UsingKeyword = node.Children[0].Token,
                    LeftParenthese = node.Children[1].Token,
                    DisposableObject = node.Children[2].CreateAstNode(),
                    RightParenthese = node.Children[3].Token,
                    Body = node.Children[4].CreateAstNode<Statement>()
                });

            var breakStatement = new GrammarDefinition("BreakStatement",
                rule: ToElement(BREAK)
                      + ToElement(SEMICOLON),
                createNode: node => new BreakStatement()
                {
                    Keyword = node.Children[0].Token,
                    Semicolon = node.Children[1].Token
                });

            var continueStatement = new GrammarDefinition("ContinueStatement",
                rule: ToElement(CONTINUE)
                      + ToElement(SEMICOLON),
                createNode: node => new BreakStatement()
                {
                    Keyword = node.Children[0].Token,
                    Semicolon = node.Children[1].Token
                });

            var returnStatement = new GrammarDefinition("ReturnStatement",
                rule: ToElement(RETURN)
                      + expressionOptional
                      + ToElement(SEMICOLON),
                createNode: node =>
                {
                    var result = new ReturnStatement();
                    result.ReturnKeyword = node.Children[0].Token;
                    if (node.Children[1].HasChildren)
                        result.Value = node.Children[1].CreateAstNode<Expression>();
                    result.AddChild(AstNodeTitles.Semicolon, node.Children[2].Token);
                    return result;
                });

            var throwStatement = new GrammarDefinition("ThrowStatement",
                rule: ToElement(THROW)
                      + expressionOptional
                      + ToElement(SEMICOLON),
                createNode: node =>
                {
                    var result = new ThrowStatement();
                    result.ThrowKeyword = node.Children[0].Token;
                    if (node.Children[1].HasChildren)
                        result.Expression = node.Children[1].CreateAstNode<Expression>();
                    result.AddChild(AstNodeTitles.Semicolon, node.Children[2].Token);
                    return result;
                });

            var gotoStatement = new GrammarDefinition("GotoStatement",
                rule: ToElement(GOTO)
                      + identifierInsideBody
                      + ToElement(SEMICOLON),
                // TODO: goto case and goto default statements.
                createNode: node =>
                {
                    var result = new GotoStatement();
                    result.GotoKeyword = node.Children[0].Token;
                    result.LabelIdentifier = node.Children[1].CreateAstNode<Identifier>();
                    result.AddChild(AstNodeTitles.Semicolon, node.Children[2].Token);
                    return result;
                });

            var jumpStatement = new GrammarDefinition("JumpStatement",
                rule: breakStatement
                      | continueStatement
                      | gotoStatement
                      | returnStatement
                      | throwStatement);

            var yieldStatement = new GrammarDefinition("YieldStatement",
                rule: ToElement(YIELD)
                      + ToElement(RETURN)
                      + expression
                      + ToElement(SEMICOLON),
                createNode: node => new YieldStatement()
                {
                    YieldKeyword = node.Children[0].Token,
                    ReturnKeyword = node.Children[1].Token,
                    Value = node.Children[2].CreateAstNode<Expression>()
                });

            var yieldBreakStatement = new GrammarDefinition("YieldBreakStatement",
                rule: ToElement(YIELD)
                      + ToElement(BREAK)
                      + ToElement(SEMICOLON),
                createNode: node => new YieldBreakStatement()
                {
                    Keyword = node.Children[0].Token,
                    BreakKeyword = node.Children[1].Token
                });

            var specificCatchClause = new GrammarDefinition("SpecificCatchClause",
                rule: ToElement(CATCH) + ToElement(OPEN_PARENS)
                    + namespaceOrTypeExpression + identifierInsideBodyOptional + ToElement(CLOSE_PARENS)
                    + blockStatement,
                createNode: node =>
                {
                    var result = new CatchClause();
                    result.CatchKeyword = node.Children[0].Token;
                    result.LeftParenthese = node.Children[1].Token;
                    result.ExceptionType = node.Children[2].CreateAstNode<TypeReference>();

                    if (node.Children[3].HasChildren)
                        result.ExceptionIdentifier = node.Children[3].CreateAstNode<Identifier>();

                    result.RightParenthese = node.Children[4].Token;
                    result.Body = node.Children[5].CreateAstNode<BlockStatement>();
                    return result;
                });

            var generalCatchClause = new GrammarDefinition("GeneralCatchClause",
                rule: ToElement(CATCH) + blockStatement,
                createNode: node => new CatchClause
                {
                    CatchKeyword = node.Children[0].Token,
                    Body = node.Children[1].CreateAstNode<BlockStatement>()
                });

            var catchClause = new GrammarDefinition("CatchClause",
                rule: specificCatchClause | generalCatchClause);

            var catchClauses = new GrammarDefinition("CatchClauses");
            catchClauses.Rule = catchClause | catchClauses + catchClause;

            var finallyClause = new GrammarDefinition("FinallyClause",
                rule: ToElement(FINALLY) + blockStatement);

            var tryCatchStatement = new GrammarDefinition("TryCatchStatement",
                rule: ToElement(TRY) + blockStatement + catchClauses
                      | ToElement(TRY) + blockStatement + finallyClause
                      | ToElement(TRY) + blockStatement + catchClauses + finallyClause,
                createNode: node =>
                {
                    var result = new TryCatchStatement();
                    result.TryKeyword = node.Children[0].Token;
                    result.TryBlock = node.Children[1].CreateAstNode<BlockStatement>();

                    ParserNode finallyClauseNode = null;
                    if (node.Children[2].GrammarElement == finallyClause)
                    {
                        finallyClauseNode = node.Children[2];
                    }
                    else
                    {
                        result.CatchClauses.AddRange(node.Children[2].GetAllNodesFromListDefinition()
                            .Select(x => x.CreateAstNode<CatchClause>()));
                    }

                    if (node.Children.Count == 4)
                        finallyClauseNode = node.Children[3];

                    if (finallyClauseNode != null)
                    {
                        result.FinallyKeyword = finallyClauseNode.Children[0].Token;
                        result.FinallyBlock = finallyClauseNode.Children[1].CreateAstNode<BlockStatement>();
                    }

                    return result;
                });

            var unsafeStatement = new GrammarDefinition("UnsafeStatement",
                rule: ToElement(UNSAFE) + blockStatement,
                createNode: node => new UnsafeStatement()
                {
                    Keyword = node.Children[0].Token,
                    Body = node.Children[1].CreateAstNode<BlockStatement>()
                });

            var fixedStatement = new GrammarDefinition("FixedStatement",
                rule: ToElement(FIXED) + ToElement(OPEN_PARENS)
                + variableDeclaration + ToElement(CLOSE_PARENS) + embeddedStatement,
                createNode: node =>
                {
                    var result = new FixedStatement();
                    result.Keyword = node.Children[0].Token;
                    result.LeftParenthese = node.Children[1].Token;

                    result.VariableDeclaration = node.Children[2].CreateAstNode<VariableDeclarationStatement>();

                    result.RightParenthese = node.Children[3].Token;
                    result.Body = node.Children[4].CreateAstNode<Statement>();

                    return result;
                });

            embeddedStatement.Rule = emptyStatement
                                     | expressionStatement
                                     | blockStatement
                                     | selectionStatement
                                     | loopStatement
                                     | jumpStatement
                                     | lockStatement
                                     | usingStatement
                                     | yieldStatement
                                     | yieldBreakStatement
                                     | tryCatchStatement
                                     | unsafeStatement
                                     | fixedStatement
                ;


            statement.Rule = variableDeclarationStatement
                             | labelStatement
                             | embeddedStatement
                ;
            #endregion

            #region Members

            var modifier = new GrammarDefinition("Modifier",
                rule: ToElement(PRIVATE)
                      | ToElement(PROTECTED)
                      | ToElement(INTERNAL)
                      | ToElement(PUBLIC)
                      | ToElement(STATIC)
                      | ToElement(ABSTRACT)
                      | ToElement(OVERRIDE)
                      | ToElement(VIRTUAL)
                      | ToElement(SEALED)
                      | ToElement(UNSAFE)
                      | ToElement(ASYNC)
                      | ToElement(EXTERN),
                createNode: node => new ModifierElement(node.Children[0].Token.Value, node.Range)
                {
                    Modifier = CSharpLanguage.ModifierFromString(node.Children[0].Token.Value)
                });

            var modifierList = new GrammarDefinition("MethodModifierList");
            modifierList.Rule = modifier | modifierList + modifier;
            var modifierListOptional = new GrammarDefinition("MethodModifierListOptional",
                rule: null | modifierList);

            var fieldDeclaration = new GrammarDefinition("FieldDeclaration",
                rule: modifierListOptional
                      + typeReference
                      + variableDeclaratorList
                      + ToElement(SEMICOLON),
                createNode: node =>
                {
                    var result = new FieldDeclaration();

                    if (node.Children[0].HasChildren)
                    {
                        result.ModifierElements.AddRange(node.Children[0].Children[0].GetAllNodesFromListDefinition()
                            .Select(x => x.CreateAstNode<ModifierElement>()));
                    }

                    result.FieldType = node.Children[1].CreateAstNode<TypeReference>();

                    foreach (var subNode in node.Children[2].GetAllNodesFromListDefinition())
                    {
                        if (subNode.Token != null)
                            result.AddChild(AstNodeTitles.ElementSeparator, subNode.Token);
                        else
                            result.Declarators.Add(subNode.CreateAstNode<VariableDeclarator>());
                    }

                    result.AddChild(AstNodeTitles.Semicolon, node.Children[3].Token);
                    return result;
                });

            var parameterModifier = new GrammarDefinition("ParameterModifier",
                rule: null
                      | ToElement(THIS)
                      | ToElement(REF)
                      | ToElement(OUT)
                      | ToElement(PARAMS));
            var parameterDeclaration = new GrammarDefinition("ParameterDeclaration",
                rule: parameterModifier
                      + typeReference
                      + variableDeclarator,
                createNode: node => new ParameterDeclaration
                {
                    ParameterModifierToken = node.Children[0].HasChildren ? node.Children[0].Children[0].Token : null,
                    ParameterType = node.Children[1].CreateAstNode<TypeReference>(),
                    Declarator = node.Children[2].CreateAstNode<VariableDeclarator>()
                });

            var parameterDeclarationList = new GrammarDefinition("ParameterDeclarationList");
            parameterDeclarationList.Rule = parameterDeclaration | parameterDeclarationList + ToElement(COMMA) + parameterDeclaration;

            var methodDeclaration = new GrammarDefinition("MethodDeclaration",
                rule: modifierListOptional
                      + typeReference
                      + ToElement(IDENTIFIER)
                      + ToElement(OPEN_PARENS)
                      + parameterDeclarationList
                      + ToElement(CLOSE_PARENS)
                      + blockStatement,
                createNode: node =>
                {
                    var result = new MethodDeclaration();

                    if (node.Children[0].HasChildren)
                    {
                        result.ModifierElements.AddRange(node.Children[0].Children[0].GetAllNodesFromListDefinition()
                            .Select(x => x.CreateAstNode<ModifierElement>()));
                    }

                    result.ReturnType = node.Children[1].CreateAstNode<TypeReference>();
                    result.Identifier = new Identifier(node.Children[2].Token.Value, node.Children[2].Range);
                    result.LeftParenthese = node.Children[3].Token;

                    foreach (var subNode in node.Children[4].GetAllNodesFromListDefinition())
                    {
                        if (subNode.Token != null)
                            result.AddChild(AstNodeTitles.ElementSeparator, subNode.Token);
                        else
                            result.Parameters.Add(subNode.CreateAstNode<ParameterDeclaration>());
                    }

                    result.RightParenthese = node.Children[5].Token;
                    result.Body = node.Children[6].CreateAstNode<BlockStatement>();
                    return result;
                });

            var eventDeclaration = new GrammarDefinition("EventDeclaration",
                rule: modifierListOptional
                      + ToElement(EVENT)
                      + typeReference
                      + variableDeclaratorList
                      + ToElement(SEMICOLON),
                createNode: node =>
                {
                    var result = new EventDeclaration();

                    if (node.Children[0].HasChildren)
                    {
                        result.ModifierElements.AddRange(node.Children[0].Children[0].GetAllNodesFromListDefinition()
                            .Select(x => x.CreateAstNode<ModifierElement>()));
                    }

                    result.EventKeyword = node.Children[1].Token;
                    result.EventType = node.Children[2].CreateAstNode<TypeReference>();

                    foreach (var subNode in node.Children[3].GetAllNodesFromListDefinition())
                    {
                        if (subNode.Token != null)
                            result.AddChild(AstNodeTitles.ElementSeparator, subNode.Token);
                        else
                            result.Declarators.Add(subNode.CreateAstNode<VariableDeclarator>());
                    }

                    result.AddChild(AstNodeTitles.Semicolon, node.Children[4].Token);
                    return result;
                });

            var accessorKeyword = new GrammarDefinition("AccessorKeyword",
                rule: ToElement(GET)
                | ToElement(SET));
            var accessorBody = new GrammarDefinition("AccessorBody",
                rule: ToElement(SEMICOLON)
                      | blockStatement);

            var accessorDeclaration = new GrammarDefinition("AccessorDeclaration",
                rule: modifierListOptional
                      + accessorKeyword
                      + accessorBody,
                createNode: node =>
                {
                    var result = new AccessorDeclaration();

                    if (node.Children[0].HasChildren)
                    {
                        result.ModifierElements.AddRange(node.Children[0].Children[0].GetAllNodesFromListDefinition()
                            .Select(x => x.CreateAstNode<ModifierElement>()));
                    }

                    result.AccessorKeyword = node.Children[1].Children[0].Token;

                    var bodyNode = node.Children[2].Children[0];
                    if (bodyNode.Token == null)
                        result.Body = bodyNode.CreateAstNode<BlockStatement>();
                    else
                        result.AddChild(AstNodeTitles.Semicolon, bodyNode.Token);

                    return result;
                });

            var accessorDeclarationList = new GrammarDefinition("AccessorDeclarationList");
            accessorDeclarationList.Rule = accessorDeclaration | accessorDeclaration + accessorDeclaration;

            var propertyDeclaration = new GrammarDefinition("PropertyDeclaration",
                rule: modifierListOptional
                      + typeReference
                      + ToElement(IDENTIFIER)
                      + ToElement(OPEN_BRACE)
                      + accessorDeclarationList
                      + ToElement(CLOSE_BRACE),
                createNode: node =>
                {
                    var result = new PropertyDeclaration();

                    if (node.Children[0].HasChildren)
                    {
                        result.ModifierElements.AddRange(node.Children[0].Children[0].GetAllNodesFromListDefinition()
                            .Select(x => x.CreateAstNode<ModifierElement>()));
                    }

                    result.PropertyType = node.Children[1].CreateAstNode<TypeReference>();
                    result.Identifier = new Identifier(node.Children[2].Token.Value, node.Children[2].Range);
                    result.StartScope = node.Children[3].Token;

                    foreach (var accessor in node.Children[4].Children)
                    {
                        var declaration = accessor.CreateAstNode<AccessorDeclaration>();
                        // TODO: detect duplicate accessor declarations.
                        switch (declaration.AccessorKeyword.Value)
                        {
                            case "get":
                                result.Getter = declaration;
                                break;
                            case "set":
                                result.Setter = declaration;
                                break;
                        }
                    }

                    result.EndScope = node.Children[5].Token;
                    return result;
                });

            var memberDeclaration = new GrammarDefinition("MemberDeclaration");
            var memberDeclarationList = new GrammarDefinition("MemberDeclarationList");
            memberDeclarationList.Rule = memberDeclaration | memberDeclarationList + memberDeclaration;
            var memberDeclarationListOptional = new GrammarDefinition("MemberDeclarationListOptional");
            memberDeclarationListOptional.Rule = null | memberDeclarationList;

            var typeVariantKeyword = new GrammarDefinition("TypeVariantKeyword",
                rule: ToElement(CLASS)
                      | ToElement(STRUCT)
                      | ToElement(INTERFACE)
                      | ToElement(ENUM));
            var typeDeclaration = new GrammarDefinition("TypeDeclaration",
                rule: modifierListOptional
                      + typeVariantKeyword
                      + ToElement(IDENTIFIER)
                      + ToElement(OPEN_BRACE)
                      + memberDeclarationListOptional
                      + ToElement(CLOSE_BRACE),
                createNode: node =>
                {
                    var result = new TypeDeclaration();

                    if (node.Children[0].HasChildren)
                    {
                        result.ModifierElements.AddRange(node.Children[0].Children[0].GetAllNodesFromListDefinition()
                            .Select(x => x.CreateAstNode<ModifierElement>()));
                    }

                    result.TypeVariant = CSharpLanguage.TypeVariantFromString(node.Children[1].Children[0].Token.Value);
                    result.TypeVariantToken = node.Children[1].Children[0].Token;
                    result.Identifier = new Identifier(node.Children[2].Token.Value, node.Children[2].Range);
                    result.StartScope = node.Children[3].Token;

                    if (node.Children[4].HasChildren)
                    {
                        result.Members.AddRange(node.Children[4].Children[0].GetAllNodesFromListDefinition()
                            .Select(x => x.CreateAstNode<MemberDeclaration>()));
                    }

                    result.EndScope = node.Children[5].Token;
                    return result;
                });

            memberDeclaration.Rule = methodDeclaration
                                     | propertyDeclaration
                                     | eventDeclaration
                                     | fieldDeclaration
                                     | typeDeclaration;

            var typeOrNamespaceDeclarationList = new GrammarDefinition("TypeOrNamespaceDeclarationList");
            var typeOrNamespaceDeclarationListOptional = new GrammarDefinition("TypeOrNamespaceDeclarationListOptional",
                rule: null | typeOrNamespaceDeclarationList);

            var namespaceDeclaration = new GrammarDefinition("NamespaceDeclaration",
                rule: ToElement(NAMESPACE)
                      + ToElement(IDENTIFIER)
                      + ToElement(OPEN_BRACE)
                      + usingDirectiveListOptional
                      + typeOrNamespaceDeclarationListOptional
                      + ToElement(CLOSE_BRACE),
                createNode: node =>
                {
                    var result = new NamespaceDeclaration();
                    result.Keyword = node.Children[0].Token;
                    result.Identifier = new Identifier(node.Children[1].Token.Value, node.Children[1].Range);
                    result.StartScope = node.Children[2].Token;

                    if (node.Children[3].HasChildren)
                    {
                        result.UsingDirectives.AddRange(node.Children[3].Children[0].GetAllNodesFromListDefinition()
                            .Select(x => x.CreateAstNode<UsingDirective>()));
                    }

                    if (node.Children[4].HasChildren)
                    {
                        foreach (var subNode in node.Children[4].Children[0].GetAllNodesFromListDefinition())
                        {
                            var declarationNode = subNode.Children[0];
                            if (declarationNode.GrammarElement == typeDeclaration)
                                result.Types.Add(declarationNode.CreateAstNode<TypeDeclaration>());
                            else
                                result.Namespaces.Add(declarationNode.CreateAstNode<NamespaceDeclaration>());
                        }
                    }

                    result.EndScope = node.Children[5].Token;

                    return result;
                });

            var typeOrNamespaceDeclaration = new GrammarDefinition("TypeOrNamespaceDeclaration",
                rule: namespaceDeclaration
                      | typeDeclaration);
            typeOrNamespaceDeclarationList.Rule = typeOrNamespaceDeclaration
                                                  | typeOrNamespaceDeclarationList
                                                  + typeOrNamespaceDeclaration;

            #endregion

            #region Initialize definitions

            var variableInitializerList = new GrammarDefinition("VariableInitializerList");
            variableInitializerList.Rule = variableInitializer
                                           | variableInitializerList
                                           + ToElement(COMMA)
                                           + variableInitializer;
            var variableInitializerListOptional = new GrammarDefinition("VariableInitializerListOptional",
                rule: null | variableInitializerList);

            arrayInitializer.Rule = ToElement(OPEN_BRACE)
                                    + variableInitializerListOptional
                                    + ToElement(CLOSE_BRACE)
                                    | ToElement(OPEN_BRACE)
                                    + variableInitializerList
                                    + ToElement(COMMA)
                                    + ToElement(CLOSE_BRACE);

            arrayInitializer.CreateNode = node =>
            {
                var result = new ArrayInitializer();
                result.OpeningBrace = node.Children[0].Token;

                ParserNode initializersNode = null;
                if (node.Children.Count == 4)
                {
                    initializersNode = node.Children[1];
                }
                else
                {
                    if (node.Children[1].HasChildren)
                        initializersNode = node.Children[1].Children[0];
                }

                if (initializersNode != null)
                {
                    foreach (var element in initializersNode.GetAllNodesFromListDefinition())
                    {
                        if (element.Token != null)
                            result.AddChild(AstNodeTitles.ElementSeparator, element.Token);
                        else
                            result.Elements.Add(element.CreateAstNode<Expression>());
                    }
                }

                if (node.Children.Count == 4)
                    result.AddChild(AstNodeTitles.ElementSeparator, node.Children[2].Token);
                result.ClosingBrace = node.Children[node.Children.Count - 1].Token;
                return result;
            };

            variableInitializer.Rule = expression
                                       | arrayInitializer
                ;

            // Types are recognized as expressions to prevent a conflict in the grammar.
            // TODO: also support array and pointer types.
            variableDeclaration.Rule = typeNameExpression
                + variableDeclaratorList;

            variableDeclaration.CreateNode = node =>
            {
                var result = new VariableDeclarationStatement();
                result.VariableType = ((IConvertibleToType)node.Children[0].CreateAstNode()).ToTypeReference();
                
                foreach (var subNode in node.Children[1].GetAllNodesFromListDefinition())
                {
                    if (subNode.Token != null)
                        result.AddChild(AstNodeTitles.ElementSeparator, subNode.Token);
                    else
                        result.Declarators.Add(subNode.CreateAstNode<VariableDeclarator>());
                }

                return result;
            };

            statementList.Rule = statement | statementList + statement;

            #endregion

            #region Root compilation unit

            var usingNamespaceDirective = new GrammarDefinition("UsingNamespaceDirective",
                rule: ToElement(USING)
                      + namespaceOrTypeExpression
                      + ToElement(SEMICOLON),
                createNode: node =>
                {
                    var result = new UsingNamespaceDirective();

                    result.UsingKeyword = node.Children[0].Token;
                    result.NamespaceIdentifier = node.Children[1].CreateAstNode<TypeReference>().ToIdentifier();
                    result.AddChild(AstNodeTitles.Semicolon, node.Children[2].Token);

                    return result;
                });

            var usingAliasDirective = new GrammarDefinition("UsingAliasDirective",
                rule: ToElement(USING)
                      + ToElement(IDENTIFIER)
                      + ToElement(EQUALS)
                      + typeReference
                      + ToElement(SEMICOLON),
                createNode: node =>
                {
                    var result = new UsingAliasDirective
                    {
                        UsingKeyword = node.Children[0].Token,
                        AliasIdentifier = new Identifier(node.Children[1].Token.Value, node.Children[1].Range),
                        OperatorToken = node.Children[2].Token,
                        TypeImport = node.Children[3].CreateAstNode<TypeReference>()
                    };
                    result.AddChild(AstNodeTitles.Semicolon, node.Children[4].Token);
                    return result;
                });

            var usingDirective = new GrammarDefinition("UsingNamespaceDirective",
                rule: usingNamespaceDirective | usingAliasDirective);

            var usingDirectiveList = new GrammarDefinition("UsingDirectiveList");
            usingDirectiveList.Rule = usingDirective | usingDirectiveList + usingDirective;
            usingDirectiveListOptional.Rule = null | usingDirectiveList;

            var compilationUnit = new GrammarDefinition("CompilationUnit",
                rule: usingDirectiveListOptional
                + typeOrNamespaceDeclarationListOptional,
                createNode: node =>
                {
                    var result = new CompilationUnit();

                    if (node.Children[0].HasChildren)
                    {
                        result.UsingDirectives.AddRange(node.Children[0].Children[0].GetAllNodesFromListDefinition()
                            .Select(x => x.CreateAstNode<UsingDirective>()));
                    }

                    if (node.Children[1].HasChildren)
                    {
                        foreach (var subNode in node.Children[1].Children[0].GetAllNodesFromListDefinition())
                        {
                            var declaration = subNode.Children[0].CreateAstNode();
                            var typeDecl = declaration as TypeDeclaration;
                            if (typeDecl == null)
                                result.Namespaces.Add((NamespaceDeclaration)declaration);
                            else
                                result.Types.Add(typeDecl);
                        }
                    }

                    return result;
                });
            
            #endregion

            RootDefinitions.Add(DefaultRoot = compilationUnit);
            RootDefinitions.Add(StatementRule = statement);
        }

        public GrammarDefinition StatementRule
        {
            get;
        }

        public TokenGrammarElement ToElement(CSharpAstTokenCode code)
        {
            TokenGrammarElement definition;
            if (!TokenMapping.TryGetValue((int)code, out definition))
            {
                TokenMapping.Add((int)code, definition = new TokenGrammarElement(code.ToString()));
            }
            return definition;
        }
    }
}