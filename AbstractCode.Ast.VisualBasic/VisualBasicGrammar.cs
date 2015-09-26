using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Members;
using AbstractCode.Ast.Parser;
using AbstractCode.Ast.Types;
using static AbstractCode.Ast.VisualBasic.VisualBasicAstTokenCode;

namespace AbstractCode.Ast.VisualBasic
{
    public class VisualBasicGrammar : Grammar
    {
        public VisualBasicGrammar()
        {

            #region Expressions
            
            CreateAstNodeDelegate createBinaryOperatorExpression = node =>
            {
                if (node.Children.Count == 1)
                    return node.Children[0].CreateAstNode();

                var result = new BinaryOperatorExpression();
                result.Left = node.Children[0].CreateAstNode<Expression>();
                var operatorToken = node.Children[1].Token ?? node.Children[1].Children[0].Token;
                result.Operator = VisualBasicLanguage.BinaryOperatorFromString(operatorToken.Value);
                result.OperatorToken = operatorToken;
                result.Right = node.Children[2].CreateAstNode<Expression>();
                return result;
            };

            var expression = new GrammarDefinition("Expression");

            var simpleExpression = new GrammarDefinition("SimpleExpression");

            var identifierExpression = new GrammarDefinition("IdentifierExpression",
                rule: ToElement(IDENTIFIER),
                createNode: node => new IdentifierExpression(new Identifier(
                    node.Children[0].Token.Value,
                    node.Range)));

            var literalExpression = new GrammarDefinition("LiteralExpression",
                rule: ToElement(LITERAL),
                createNode:  node => new PrimitiveExpression(
                    node.Children[0].Token.UserData["InterpretedValue"],
                    node.Children[0].Token.Value, 
                    node.Range));

            var parenthesizedExpression = new GrammarDefinition("ParenthesizedExpression",
                rule: ToElement(OPEN_PARENS) + expression + ToElement(CLOSE_PARENS),
                createNode: node => new ParenthesizedExpression()
                {
                    LeftParenthese = node.Children[0].Token,
                    Expression = node.Children[1].CreateAstNode<Expression>(),
                    RightParenthese = node.Children[2].Token,
                });

            var myBaseExpression = new GrammarDefinition("MyBaseExpression",
                rule: ToElement(MYBASE),
                createNode: node => new BaseReferenceExpression()
                {
                    BaseKeywordToken = node.Children[0].Token
                });

            var meExpression = new GrammarDefinition("MeExpression",
                rule: ToElement(ME),
                createNode: node => new ThisReferenceExpression()
                {
                    ThisKeywordToken = node.Children[0].Token
                });

            var memberReferenceExpression = new GrammarDefinition("MemberReferenceExpression",
                rule: simpleExpression + ToElement(DOT) + ToElement(IDENTIFIER),
                createNode: node => new MemberReferenceExpression
                {
                   Target = node.Children[0].CreateAstNode<Expression>(),
                   AccessorToken = node.Children[1].Token,
                   Identifier = ((IConvertibleToIdentifier)node.Children[2].Token).ToIdentifier()
                });

            simpleExpression.Rule = literalExpression
                | identifierExpression
                | meExpression
                | myBaseExpression
                | parenthesizedExpression
                | memberReferenceExpression
                ;

            var preFixUnaryOperator = new GrammarDefinition("PreFixUnaryOperator",
                rule: ToElement(PLUS)
                      | ToElement(MINUS)
                      | ToElement(NOT));
            
            var unaryOperatorExpression = new GrammarDefinition("UnaryOperatorExpression",
                rule: simpleExpression
                      | (preFixUnaryOperator + simpleExpression),
                createNode: node =>
                {
                    if (node.Children.Count == 1)
                        return node.Children[0].CreateAstNode();

                    var result = new UnaryOperatorExpression();
                    var isPrefix = node.Children[0].GrammarElement == preFixUnaryOperator;
                    if (isPrefix)
                    {
                        result.Operator =
                            VisualBasicLanguage.UnaryOperatorFromString(node.Children[0].Children[0].Token.Value);
                        result.OperatorToken = node.Children[0].Children[0].Token;
                    }

                    result.Expression = node.Children[isPrefix ? 1 : 0].CreateAstNode<Expression>();
                    if (!isPrefix)
                    {
                        //result.Operator =
                        //    CSharpLanguage.UnaryOperatorFromString(node.Children[1].Children[0].Token.Value, false);
                        result.OperatorToken = node.Children[1].Children[0].Token;
                    }
                    return result;
                });

            var multiplicativeOperator = new GrammarDefinition("MultiplicativeOperator",
                rule: ToElement(STAR)
                      | ToElement(DIV)
                      | ToElement(MOD));

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
                      | ToElement(ISNOT));
            var relationalExpression = new GrammarDefinition("RelationalExpression");
            relationalExpression.Rule = shiftExpression
                                        | relationalExpression
                                        + relationalOperator
                                        + shiftExpression;
            relationalExpression.CreateNode = createBinaryOperatorExpression;

            var equalityOperator = new GrammarDefinition("equalityOperator",
                rule: ToElement(EQUALS)
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
                                      + ToElement(BITWISE_XOR)
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
                                      + ToElement(OP_ANDALSO)
                                      + logicalOrExpression;
            conditionalAndExpression.CreateNode = createBinaryOperatorExpression;

            var conditionalOrExpression = new GrammarDefinition("ConditionalOrExpression");
            conditionalOrExpression.Rule = conditionalAndExpression
                                      | conditionalOrExpression
                                      + ToElement(OP_ORELSE)
                                      + conditionalAndExpression;
            conditionalOrExpression.CreateNode = createBinaryOperatorExpression;
            
            expression.Rule = conditionalOrExpression;

            #endregion

            #region Compilation units

            var compilationUnit = new GrammarDefinition("CompilationUnit",
                rule: expression,
                createNode: node =>
                {
                    var unit = new CompilationUnit();
                    unit.AddChild(AstNodeTitles.Expression, node.Children[0].CreateAstNode<Expression>());
                    return unit;
                });

            #endregion


            RootDefinitions.Add(DefaultRoot = compilationUnit);
        }

        public TokenGrammarElement ToElement(VisualBasicAstTokenCode code)
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
