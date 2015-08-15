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
using System.IO;

namespace AbstractCode
{
    public class StringTextOutput : ITextOutput
    {
        private bool _indentationRequired = true;

        public StringTextOutput(TextWriter writer, string indentationString)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            if (indentationString == null)
                throw new ArgumentNullException("indentationString");
            Writer = writer;
            IndentationString = indentationString;
        }

        public TextWriter Writer
        {
            get;
        }

        public int Indentation
        {
            get;
            private set;
        }

        public string IndentationString
        {
            get;
            set;
        }

        protected virtual void WriteIndentationIfNeeded()
        {
            if (_indentationRequired)
            {
                for (int i = 0; i < Indentation; i++)
                {
                    Writer.Write(IndentationString);
                    Location = new TextLocation(Location.Line, Location.Column + IndentationString.Length);
                }
                _indentationRequired = false;
            }
        }

        public TextLocation Location
        {
            get;
            private set;
        } = TextLocation.Empty;

        public void Indent()
        {
            Indentation++;
        }

        public void Unindent()
        {
            Indentation--;
        }

        public void Write(char @char)
        {
            WriteIndentationIfNeeded();
            Writer.Write(@char);
            Location = new TextLocation(Location.Line, Location.Column + 1);
        }

        public void Write(string text)
        {
            WriteIndentationIfNeeded();
            Writer.Write(text);
            Location = new TextLocation(Location.Line, Location.Column + text.Length);
        }

        public void WriteLine()
        {
            Writer.WriteLine();
            _indentationRequired = true;
            Location = new TextLocation(Location.Line + 1, 0);
        }
    }
}