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
using System.IO;

namespace AbstractCode
{
    public class StringTextSource : ITextSource
    {
        public StringTextSource(string text)
        {
            Text = text;
        }

        public int TextLength
        {
            get { return Text.Length; }
        }

        public string Text
        {
            get;
        }

        public ITextSource CreateSnapshot()
        {
            return this;
        }

        public ITextSource CreateSnapshot(int offset, int length)
        {
            return new StringTextSource(Text.Substring(offset, length));
        }
        
        public char GetChar(int offset)
        {
            return Text[offset];
        }

        public string GetText(int offset, int length)
        {
            return Text.Substring(offset, length);
        }

        public int IndexOf(char character, int offset, int length)
        {
            return Text.IndexOf(character, offset, length);
        }

        public int IndexOfAny(char[] characters, int offset, int length)
        {
            return Text.IndexOfAny(characters, offset, length);
        }

        public TextReader CreateReader()
        {
            return new StringReader(Text);
        }

        public void WriteTo(TextWriter writer)
        {
            writer.Write(Text);
        }
    }
}