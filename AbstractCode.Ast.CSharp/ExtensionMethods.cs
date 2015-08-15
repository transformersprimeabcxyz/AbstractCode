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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractCode.Ast;
using AbstractCode.Ast.CSharp;

namespace AbstractCode.Ast.CSharp
{
    internal static class ExtensionMethods
    {

        public static bool IsBasedOn(this Type type, Type baseType)
        {
            while (type != null)
            {
                if (type == baseType)
                    return true;
                type = type.BaseType;
            }
            return false;
        }

        public static void AcceptCSharpVisitor(this AstNode node, IAstVisitor visitor)
        {
            var csharpVisitor = visitor as ICSharpAstVisitor;
            var csNode = node as ICSharpVisitable;

            if (csharpVisitor != null && csNode != null)
                csNode.AcceptVisitor(csharpVisitor);
        }

        public static TResult AcceptCSharpVisitor<TResult>(this AstNode node, IAstVisitor<TResult> visitor)
        {
            var csharpVisitor = visitor as ICSharpAstVisitor<TResult>;
            var csNode = node as ICSharpVisitable;

            if (csharpVisitor != null && csNode != null)
                return csNode.AcceptVisitor(csharpVisitor);
            return default(TResult);
        }

        public static TResult AcceptCSharpVisitor<TData, TResult>(this AstNode node, IAstVisitor<TData, TResult> visitor, TData data)
        {
            var csharpVisitor = visitor as ICSharpAstVisitor<TData, TResult>;
            var csNode = node as ICSharpVisitable;

            if (csharpVisitor != null && csNode != null)
                return csNode.AcceptVisitor(csharpVisitor, data);
            return default(TResult);
        }
    }
}
