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
using AbstractCode.Ast.CSharp;
using AbstractCode.Ast.Parser;

namespace AbstractCode.Tests.Ast.Parser
{
    public class TestGrammar2 : Grammar
    {
        public TestGrammar2()
        {
            var statement = new GrammarDefinition("Statement");
            var expression = new GrammarDefinition("Expression");
            var identifierExpression = new GrammarDefinition("IdentifierExpression");
            var primitiveExpression = new GrammarDefinition("PrimitiveExpression");
            var parenthesizedExpression = new GrammarDefinition("ParenthesizedExpression");
            var binaryOperatorExpression = new GrammarDefinition("BinaryOperatorExpression");
            var binaryOperator = new GrammarDefinition("BinaryOperator");
            var invocationExpression = new GrammarDefinition("InvocationExpression");
            var argumentList = new GrammarDefinition("ArgumentList");

            expression.Rule = identifierExpression
                              | primitiveExpression
                              | binaryOperatorExpression
                              | parenthesizedExpression
                              | invocationExpression;

            identifierExpression.Rule = ToElement(CSharpAstTokenCode.IDENTIFIER);
            primitiveExpression.Rule = ToElement(CSharpAstTokenCode.LITERAL);

            argumentList.Rule = expression
                                | argumentList + ToElement(CSharpAstTokenCode.COMMA) + expression;

            invocationExpression.Rule = expression 
                                        + ToElement(CSharpAstTokenCode.OPEN_PARENS)
                                        + argumentList 
                                        + ToElement(CSharpAstTokenCode.CLOSE_PARENS);

            binaryOperatorExpression.Rule = expression + binaryOperator + expression;
            binaryOperator.Rule = ToElement(CSharpAstTokenCode.PLUS) | ToElement(CSharpAstTokenCode.MINUS);

            parenthesizedExpression.Rule = ToElement(CSharpAstTokenCode.OPEN_PARENS)
                                        + expression
                                        + ToElement(CSharpAstTokenCode.CLOSE_PARENS);

            statement.Rule = expression + ToElement(CSharpAstTokenCode.SEMICOLON);

            RootDefinitions.Add(DefaultRoot = statement);
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