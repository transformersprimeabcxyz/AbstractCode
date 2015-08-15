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

namespace AbstractCode.Ast
{
    public class TextOutputFormatter : IOutputFormatter
    {
        private bool _startOfLine = true;

        public TextOutputFormatter(ITextOutput output)
        {
            if (output == null)
                throw new ArgumentNullException("output");
            Output = output;
        }

        public ITextOutput Output
        {
            get;
        }

        public virtual void Indent()
        {
            Output.Indent();
        }

        public virtual void Unindent()
        {
            Output.Unindent();
        }

        public virtual void StartNode(AstNode node)
        {
        }

        public virtual void EndNode()
        {
        }

        public virtual void WriteLine()
        {
            Output.WriteLine();
            _startOfLine = true;
        }

        public virtual void WriteSpace()
        {
            Output.Write(' ');
        }

        public virtual void OpenBrace(BraceStyle style)
        {
            if (!_startOfLine)
            {
                switch (style)
                {
                    case BraceStyle.EndOfLineNoSpacing:
                        break;

                    case BraceStyle.EndOfLine:
                        WriteSpace();
                        break;

                    case BraceStyle.NextLine:
                        WriteLine();
                        break;

                    case BraceStyle.NextLineIndented:
                        WriteLine();
                        Indent();
                        break;
                }
            }

            Output.Write('{');
            Indent();
            WriteLine();
        }

        public virtual void CloseBrace(BraceStyle style)
        {
            Unindent();

            if (!_startOfLine)
            {
                switch (style)
                {
                    case BraceStyle.EndOfLine:
                        WriteSpace();
                        break;

                    case BraceStyle.NextLine:
                        WriteLine();
                        break;
                }
            }

            Output.Write('}');
            _startOfLine = false;
        }

        public virtual void WriteKeyword(string keyword)
        {
            Output.Write(keyword);
            _startOfLine = false;
        }

        public virtual void WriteIdentifier(string identifier)
        {
            Output.Write(identifier);
            _startOfLine = false;
        }

        public virtual void WriteToken(string token)
        {
            Output.Write(token);
            _startOfLine = false;
        }

    }
}
