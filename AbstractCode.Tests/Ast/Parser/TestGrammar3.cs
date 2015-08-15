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
    public class TestGrammar3 : Grammar
    {
        public TestGrammar3()
        {
            var S = new GrammarDefinition("S");
            var E = new GrammarDefinition("E");
            var T = new GrammarDefinition("T");

            S.Rule = E;
            E.Rule = T 
                | E + ToElement(CSharpAstTokenCode.PLUS) + T;
            T.Rule = ToElement(CSharpAstTokenCode.LITERAL)
                | ToElement(CSharpAstTokenCode.OPEN_PARENS) + E + ToElement(CSharpAstTokenCode.CLOSE_PARENS);

            RootDefinitions.Add(DefaultRoot = S);
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