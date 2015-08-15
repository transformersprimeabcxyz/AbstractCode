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

namespace AbstractCode.Ast.Parser
{
    public abstract class Grammar
    {
        public static readonly TokenGrammarElement Eof = new TokenGrammarElement("EOF");
        public static readonly TokenGrammarElement Epsilon = new TokenGrammarElement("EPSILON");
        public static readonly TokenGrammarElement Error = new TokenGrammarElement("ERROR");

        protected readonly Dictionary<int, TokenGrammarElement> TokenMapping =
            new Dictionary<int, TokenGrammarElement>();

        protected Grammar()
        {
            RootDefinitions = new List<GrammarDefinition>();
            TokenMapping.Add(0, Eof);
        }
        
        public GrammarDefinition DefaultRoot
        {
            get;
            protected set;
        }

        public IList<GrammarDefinition> RootDefinitions
        {
            get;
        }

        public IEnumerable<TokenGrammarElement> TokenElements
        {
            get { return TokenMapping.Values; }
        }

        public TokenGrammarElement ToElement(int code)
        {
            TokenGrammarElement definition;
            if (!TokenMapping.TryGetValue(code, out definition))
            {
                TokenMapping.Add(code, definition = new TokenGrammarElement(code.ToString()));
            }
            return definition;
        }
    }
}
