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

namespace AbstractCode
{
    /// <summary>
    /// Represents a read-only text document.
    /// </summary>
    public sealed class ReadOnlyDocument : IDocument
    {
        private readonly ITextSource _snapshot;
        private readonly int[] _lineOffsets;

        public ReadOnlyDocument(string text)
            : this(new StringTextSource(text))
        {
        }

        public ReadOnlyDocument(ITextSource textSource)
        {
            _snapshot = textSource.CreateSnapshot();
            var offsets = new List<int>();

            int currentOffset = 0;
            offsets.Add(0);

            while (currentOffset != -1)
            {
                while (currentOffset < _snapshot.TextLength && _snapshot.GetChar(currentOffset) == '\n')
                    offsets.Add(++currentOffset);

                if (currentOffset >= _snapshot.TextLength)
                    break;

                currentOffset = _snapshot.IndexOf('\n', currentOffset + 1, _snapshot.TextLength - currentOffset - 1);
            }

            _lineOffsets = offsets.ToArray();
        }

        event TextChangeEventHandler IDocument.TextChanging
        {
            add { }
            remove { }
        }

        event TextChangeEventHandler IDocument.TextChanged
        {
            add { }
            remove { }
        }

        public IDocument CreateDocumentSnapshot()
        {
            return this;
        }

        public int LocationToOffset(TextLocation location)
        {
            if (location.Line == 0 || location.Column == 0)
                throw new ArgumentOutOfRangeException(nameof(location));

            int lineOffset = LineToOffset(location.Line);
            return lineOffset + location.Column - 1;
        }

        public TextLocation OffsetToLocation(int offset)
        {
            if (offset < 0 || offset > TextLength)
                throw new ArgumentOutOfRangeException(nameof(offset));

            int line = OffsetToLine(offset);
            int lineOffset = LineToOffset(line);

            return new TextLocation(line, offset - lineOffset + 1);
        }

        public int LineToOffset(int line)
        {
            return _lineOffsets[line - 1];
        }

        public int OffsetToLine(int offset)
        {
            for (int i = 1; i <= _lineOffsets.Length - 1; i++)
            {
                if (offset < _lineOffsets[i])
                    return i;
            }
            return _lineOffsets.Length;
        }

        public string GetRangeText(TextRange range)
        {
            int offset = LocationToOffset(range.Start);
            int length = GetRangeTextLength(range);
            return GetText(offset, length);
        }

        public int GetRangeTextLength(TextRange range)
        {
            int totalLength = 0;

            for (int i = range.Start.Line; i <= range.End.Line; i++)
            {
                int lineLength = GetLineTextLength(i);
                int delta = lineLength;

                if (i == range.Start.Line)
                    delta -= (range.Start.Column - 1);

                if (i == range.End.Line)
                    delta -= (lineLength - range.End.Column);

                totalLength += delta;
            }

            return totalLength - 1;
        }

        private int GetLineTextLength(int line)
        {
            if (line < 0 || line > _lineOffsets.Length)
                throw new ArgumentOutOfRangeException(nameof(line));

            return line < _lineOffsets.Length - 2
                ? _lineOffsets[line] - _lineOffsets[line - 1]
                : TextLength - (_lineOffsets[line - 1] + 1);
        }

        void IDocument.BeginUpdate()
        {
        }

        void IDocument.EndUpdate()
        {
        }

        void IDocument.Remove(int offset, int length)
        {
            throw new InvalidOperationException();
        }

        void IDocument.Insert(int offset, string text)
        {
            throw new InvalidOperationException();
        }

        void IDocument.Replace(int offset, int length, string text)
        {
            throw new InvalidOperationException();
        }

        public int TextLength
        {
            get { return _snapshot.TextLength; }
        }

        public string Text
        {
            get { return _snapshot.Text; }
        }

        public ITextSource CreateSnapshot()
        {
            return _snapshot;
        }

        public ITextSource CreateSnapshot(int offset, int length)
        {
            return _snapshot.CreateSnapshot(offset, length);
        }
        
        public char GetChar(int offset)
        {
            return _snapshot.GetChar(offset);
        }

        public string GetText(int offset, int length)
        {
            return _snapshot.GetText(offset, length);
        }

        public int IndexOf(char character, int offset, int length)
        {
            return _snapshot.IndexOf(character, offset, length);
        }

        public int IndexOfAny(char[] characters, int offset, int length)
        {
            return _snapshot.IndexOfAny(characters, offset, length);
        }

        public System.IO.TextReader CreateReader()
        {
            return _snapshot.CreateReader();
        }

        public void WriteTo(System.IO.TextWriter writer)
        {
            _snapshot.WriteTo(writer);
        }
    }
}