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
    public class TestGrammar4 : Grammar
    {
        public TestGrammar4()
        {
            var expression = new GrammarDefinition("Expression");
            var primaryExpression = new GrammarDefinition("PrimaryExpression");

            //var identifierInsideBody = new GrammarDefinition("IdentifierInsideBody",
            //    rule: ToElement(CSharpAstTokenCode.IDENTIFIER) );

            var identifierExpression = //new GrammarDefinition("IdentifierExpression",
                ToElement(CSharpAstTokenCode.IDENTIFIER)
                //)
                ;

            var literalExpression = //new GrammarDefinition("LiteralExpression",
                ToElement(CSharpAstTokenCode.LITERAL)//)
                ;

            var memberReferenceExpression = new GrammarDefinition("MemberReferenceExpression",
                rule: primaryExpression
                      + ToElement(CSharpAstTokenCode.DOT)
                      + ToElement(CSharpAstTokenCode.IDENTIFIER));

            var typeNameExpression = new GrammarDefinition("TypeNameExpression",
                rule: identifierExpression
                      | memberReferenceExpression
                      //| primitiveTypeExpression
                      );

            primaryExpression.Rule = typeNameExpression
                                     | literalExpression;
            
            var multiplicativeExpression = new GrammarDefinition("MultiplicativeExpression");
            multiplicativeExpression.Rule = primaryExpression
                                        | multiplicativeExpression
                                        + ToElement(CSharpAstTokenCode.STAR)
                                        + primaryExpression;
            
            var additiveExpression = new GrammarDefinition("AdditiveExpression");
            additiveExpression.Rule = multiplicativeExpression
                                        | additiveExpression
                                        + ToElement(CSharpAstTokenCode.PLUS)
                                        + multiplicativeExpression;
            
            var assignmentExpression = new GrammarDefinition("AssignmentExpression");
            assignmentExpression.Rule = additiveExpression
                                        | additiveExpression
                                        + ToElement(CSharpAstTokenCode.EQUALS)
                                        + assignmentExpression;
            expression.Rule = primaryExpression;

            var statement = new GrammarDefinition("Statement");

            var expressionStatement = new GrammarDefinition("ExpressionStatement",
                rule: expression + ToElement(CSharpAstTokenCode.SEMICOLON));

            var labelStatement = new GrammarDefinition("LabelStatement",
                rule: identifierExpression + ToElement(CSharpAstTokenCode.COLON));

            statement.Rule = expressionStatement | labelStatement;

            var statementList = new GrammarDefinition("StatementList");
            statementList.Rule = statement | statementList + statement;

            var statementListOpt = new GrammarDefinition("StatementListOpt", rule: null|statementList);

            RootDefinitions.Add(DefaultRoot = statementListOpt);
            RootDefinitions.Add(expression);
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