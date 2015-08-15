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
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AbstractCode.Ast.Parser
{
    public abstract class Lexer
    {
        private readonly TextReader _reader;
        private readonly Stack<StringBuilder> _buffers = new Stack<StringBuilder>();

        protected Lexer(TextReader reader)
        {
            _reader = reader;
            Location = new TextLocation(1, 1);
        }

        public int Offset
        {
            get;
            private set;
        }

        public TextLocation Location
        {
            get;
            private set;
        }

        protected void StartReadBuffer()
        {
            _buffers.Push(new StringBuilder());
        }

        protected StringBuilder EndReadBuffer()
        {
            return _buffers.Pop();
        }

        protected bool Peek(out char character)
        {
            character = '\0';
            var next = _reader.Peek();
            if (next == -1)
                return false;
            character = (char)next;
            return true;
        }

        protected char Read()
        {
            char character;
            if (!Peek(out character))
                throw new EndOfStreamException();
            _reader.Read();

            foreach (var buffer in _buffers)
                buffer.Append(character);

            if (character == '\n')
            {
                OnLineTerminated();
                Location = new TextLocation(Location.Line + 1, 1);
            }
            else
            {
                Location = new TextLocation(Location.Line, Location.Column + 1);
            }

            Offset++;

            return character;
        }

        protected string ReadCharacters(Predicate<char> condition, out TextRange range)
        {
            var start = Location;
            
            StartReadBuffer();
            while (true)
            {
                char character;
                if (!Peek(out character))
                    break;

                if (condition(character))
                    Read();
                else
                    break;
            }
            var end = Location;
            range = new TextRange(start, end);
            return EndReadBuffer().ToString();
        }

        protected virtual bool MoveToNextToken()
        {
            while (true)
            {
                char character;
                if (!Peek(out character))
                    return false;

                if (!char.IsWhiteSpace(character))
                    return true;

                Read();
            }
        }

        public abstract AstToken ReadNextToken();

        protected virtual void OnLineTerminated()
        {

        }
    }
}