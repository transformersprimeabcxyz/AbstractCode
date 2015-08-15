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
    public class TestGrammar1 : Grammar
    {
        public TestGrammar1()
        {
            // This grammar is the implementation of the grammar mentioned in 
            // http://web.stanford.edu/class/archive/cs/cs143/cs143.1128/lectures/06/Slides06.pdf

            var S = new GrammarDefinition("S");
            var E = new GrammarDefinition("E");
            var L = new GrammarDefinition("L");
            var R = new GrammarDefinition("R");

            S.Rule = E;
            E.Rule = L + ToElement(CSharpAstTokenCode.EQUALS) + R
                     | R;
            L.Rule = ToElement(CSharpAstTokenCode.IDENTIFIER)
                     | ToElement(CSharpAstTokenCode.STAR) + R;
            R.Rule = L;

            RootDefinitions.Add(DefaultRoot = E);
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