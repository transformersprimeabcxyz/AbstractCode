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

namespace AbstractCode.Ast
{
    public class SyntaxError 
    {
        protected SyntaxError()
        {
        }

        public SyntaxError(TextLocation location, string message, MessageSeverity severity)
        {
            Location = location;
            Message = message;
            Severity = severity;
        }

        public int ErrorCode
        {
            get;
            protected set;
        }

        public TextLocation Location
        {
            get;
            protected set;
        }

        public virtual string Message
        {
            get;
            protected set;
        }

        public virtual MessageSeverity Severity
        {
            get;
            protected set;
        }
    }

    public enum MessageSeverity
    {
        Error,
        Warning,
        Message,
    }
}
