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
    public class ParserContext
    {
        public ParserContext(SourceParser parser, Grammar grammar, AstTokenStream stream)
        {
            Parser = parser;
            Grammar = grammar;
            Stream = stream;
            ParserStack = new Stack<ParserNode>();
        }

        public ParserState CurrentState
        {
            get;
            set;
        }

        public ParserNode CurrentNode
        {
            get;
            set;
        }

        public Stack<ParserNode> ParserStack
        {
            get;
        } 

        public SourceParser Parser
        {
            get;
        }

        public Grammar Grammar
        {
            get;
            set;
        }

        public AstTokenStream Stream
        {
            get;
        }

        public ParserNode Root
        {
            get;
            set;
        }

        public IList<SyntaxError> SyntaxErrors
        {
            get;
            set;
        }
        
        public void SendLogMessage(MessageSeverity severity, string message)
        {
            Parser.SendLogMessage(this, severity, message);
        }

        public void SendLogMessage(MessageSeverity severity, string format, params object[] args)
        {
            Parser.SendLogMessage(this, severity, string.Format(format, args));
        }
    }
}