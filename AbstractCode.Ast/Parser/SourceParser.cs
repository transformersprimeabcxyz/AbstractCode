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
using System;

namespace AbstractCode.Ast.Parser
{
    public abstract class SourceParser
    {
        public event SourceParserLogEventHandler LogMessageReceived;
        
        public bool EnableLogging
        {
            get;
            set;
        }

        public ParserNode Parse(Lexer lexer)
        {
            return Parse(new AstTokenStream(lexer));
        }

        public abstract ParserNode Parse(AstTokenStream stream);
        
        internal void SendLogMessage(ParserContext parserContext, MessageSeverity severity, string message)
        {
            if (EnableLogging)
                LogMessageReceived?.Invoke(this, new SourceParserLogEventArgs(parserContext, severity, message));
        }
    }

    public delegate void SourceParserLogEventHandler(object sender, SourceParserLogEventArgs args);

    public class SourceParserLogEventArgs : EventArgs
    {
        public SourceParserLogEventArgs(ParserContext parserContext, MessageSeverity messageSeverity, string message)
        {
            ParserContext = parserContext;
            MessageSeverity = messageSeverity;
            Message = message;
        }

        public ParserContext ParserContext
        {
            get;
        }

        public MessageSeverity MessageSeverity
        {
            get;
        }

        public string Message
        {
            get;
        }
    }
}