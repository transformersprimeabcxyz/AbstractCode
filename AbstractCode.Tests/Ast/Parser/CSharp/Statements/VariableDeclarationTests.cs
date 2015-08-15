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
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Members;
using AbstractCode.Ast.Statements;
using AbstractCode.Ast.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractCode.Tests.Ast.Parser.CSharp.Statements
{
    [TestClass]
    public class VariableDeclarationTests
    {
        [TestMethod]
        public void Local()
        {
            CSharpAstValidator.AssertStatement("int x;",
                new VariableDeclarationStatement(
                    new PrimitiveTypeReference(PrimitiveType.Int32),
                    "x"));
        }

        [TestMethod]
        public void LocalWithValue()
        {
            CSharpAstValidator.AssertStatement("int x = 3;",
                new VariableDeclarationStatement(
                    new PrimitiveTypeReference(PrimitiveType.Int32),
                    "x",
                    new PrimitiveExpression(3)));
        }

        [TestMethod]
        public void LocalMultipleDeclarators()
        {
            CSharpAstValidator.AssertStatement("int x, y = 3, z = 4;",
                new VariableDeclarationStatement(
                    new PrimitiveTypeReference(PrimitiveType.Int32),
                    new VariableDeclarator("x"),
                    new VariableDeclarator("y", new PrimitiveExpression(3)),
                    new VariableDeclarator("z", new PrimitiveExpression(4))));
        }

        [TestMethod]
        public void LocalDeclaratorWithArrayInitializer()
        {
            CSharpAstValidator.AssertStatement("var x = { 1, 2, 3, 4 };",
                new VariableDeclarationStatement(
                    new SimpleTypeReference("var"),
                    "x",
                    new ArrayInitializer(
                        new PrimitiveExpression(1),
                        new PrimitiveExpression(2),
                        new PrimitiveExpression(3),
                        new PrimitiveExpression(4))));
        }
    }
}
