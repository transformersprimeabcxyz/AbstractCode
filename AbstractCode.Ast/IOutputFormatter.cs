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
    public interface IOutputFormatter 
    {
        void StartNode(AstNode node);
        void EndNode();
        void Indent();
        void Unindent();
        void OpenBrace(BraceStyle style);
        void CloseBrace(BraceStyle style);
        void WriteLine();
        void WriteSpace();
        void WriteKeyword(string keyword);
        void WriteIdentifier(string identifier);
        void WriteToken(string token);
    }

    public static class OutputFormatterExtensions
    {
        public static void WriteKeywordLine(this IOutputFormatter formatter, string line)
        {
            formatter.WriteKeyword(line);
            formatter.WriteLine();
        }

        public static void WriteIdentifierLine(this IOutputFormatter formatter, string line)
        {
            formatter.WriteIdentifier(line);
            formatter.WriteLine();
        }

        public static void WriteTokenLine(this IOutputFormatter formatter, string line)
        {
            formatter.WriteToken(line);
            formatter.WriteLine();
        }
    }
}
