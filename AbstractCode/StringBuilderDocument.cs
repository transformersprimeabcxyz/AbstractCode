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
using System.Text;

namespace AbstractCode
{
    public class StringBuilderDocument : IDocument
    {
        private ReadOnlyDocument _cachedSnapshot;

        public StringBuilderDocument(StringBuilder builder)
        {
            Builder = builder;
        }

        public StringBuilder Builder
        {
            get;
        }

        public event TextChangeEventHandler TextChanging;
        public event TextChangeEventHandler TextChanged;

        public IDocument CreateDocumentSnapshot()
        {
            return _cachedSnapshot ?? (_cachedSnapshot = new ReadOnlyDocument(this));
        }

        public int LocationToOffset(TextLocation location)
        {
            return CreateDocumentSnapshot().LocationToOffset(location);
        }

        public TextLocation OffsetToLocation(int offset)
        {
            return CreateDocumentSnapshot().OffsetToLocation(offset);
        }

        public int LineToOffset(int line)
        {
            return CreateDocumentSnapshot().LineToOffset(line);
        }

        public int OffsetToLine(int offset)
        {
            return CreateDocumentSnapshot().OffsetToLine(offset);
        }

        public string GetRangeText(TextRange range)
        {
            return CreateDocumentSnapshot().GetRangeText(range);
        }

        public int GetRangeTextLength(TextRange range)
        {
            return CreateDocumentSnapshot().GetRangeTextLength(range);
        }

        public void BeginUpdate()
        {
        }

        public void EndUpdate()
        {
        }

        public void Remove(int offset, int length)
        {
            Replace(offset, length, string.Empty);
        }

        public void Insert(int offset, string text)
        {
            Replace(offset, 0, text);
        }

        public void Replace(int offset, int length, string text)
        {
            if (offset < 0 || offset > TextLength)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0 || offset + length > TextLength)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var eventArgs = new TextChangeEventArgs(offset, GetText(offset, length), text);
            OnTextChanging(eventArgs);

            _cachedSnapshot = null;
            Builder.Remove(offset, length);
            Builder.Insert(offset, text);

            OnTextChanged(eventArgs);
        }

        public int TextLength
        {
            get { return CreateDocumentSnapshot().TextLength; }
        }

        public string Text
        {
            get { return CreateDocumentSnapshot().Text; }
        }

        public ITextSource CreateSnapshot()
        {
            return new StringTextSource(Text);
        }

        public ITextSource CreateSnapshot(int offset, int length)
        {
            return new StringTextSource(Text.Substring(offset, length));
        }
        
        public char GetChar(int offset)
        {
            return Builder[offset];
        }

        public string GetText(int offset, int length)
        {
            return Builder.ToString(offset, length);
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

        protected virtual void OnTextChanging(TextChangeEventArgs e)
        {
            TextChanging?.Invoke(this, e);
        }

        protected virtual void OnTextChanged(TextChangeEventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }
    }
}