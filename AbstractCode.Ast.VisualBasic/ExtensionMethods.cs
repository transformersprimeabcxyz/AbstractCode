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
using System.Linq;

namespace AbstractCode.Ast.VisualBasic
{
    internal static class ExtensionMethods
    {
        public static bool Contains(this IEnumerable<string> collection, string value, bool caseSensitive)
        {
            return collection.FirstOrDefault(x => string.Compare(x, value, !caseSensitive) == 0) != null;
        }

        public static bool IsAtSameLineAs(this AstNode a, AstNode b)
        {
            if (b == null)
                return false;

            return a.Range.End.Line == b.Range.Start.Line;
        }


        public static void AcceptVisualBasicVisitor(this AstNode node, IAstVisitor visitor)
        {
            var csharpVisitor = visitor as IVisualBasicAstVisitor;
            var csNode = node as IVisualBasicVisitable;

            if (csharpVisitor != null && csNode != null)
                csNode.AcceptVisitor(csharpVisitor);
        }

        public static TResult AcceptVisualBasicVisitor<TResult>(this AstNode node, IAstVisitor<TResult> visitor)
        {
            var csharpVisitor = visitor as IVisualBasicAstVisitor<TResult>;
            var csNode = node as IVisualBasicVisitable;

            if (csharpVisitor != null && csNode != null)
                return csNode.AcceptVisitor(csharpVisitor);
            return default(TResult);
        }

        public static TResult AcceptVisualBasicVisitor<TData, TResult>(this AstNode node,
            IAstVisitor<TData, TResult> visitor, TData data)
        {
            var csharpVisitor = visitor as IVisualBasicAstVisitor<TData, TResult>;
            var csNode = node as IVisualBasicVisitable;

            if (csharpVisitor != null && csNode != null)
                return csNode.AcceptVisitor(csharpVisitor, data);
            return default(TResult);
        }
    }
}