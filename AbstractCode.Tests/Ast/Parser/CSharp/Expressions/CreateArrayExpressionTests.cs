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
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractCode.Tests.Ast.Parser.CSharp.Expressions
{
    [TestClass]
    public class CreateArrayExpressionTests
    {
        [TestMethod]
        public void FixedSize()
        {
            CSharpAstValidator.AssertExpression("new int[2]",
                new CreateArrayExpression(
                    new PrimitiveTypeReference(PrimitiveType.Int32),
                    new PrimitiveExpression(2)));
        }

        [TestMethod]
        public void ImplicitInitializer()
        {
            CSharpAstValidator.AssertExpression("new[] { 1, 2, 3, 4 }",
                new CreateArrayExpression
                {
                    Initializer = new ArrayInitializer(
                        new PrimitiveExpression(1),
                        new PrimitiveExpression(2),
                        new PrimitiveExpression(3),
                        new PrimitiveExpression(4))
                });
        }

        [TestMethod]
        public void Initializer()
        {
            CSharpAstValidator.AssertExpression("new int[] { 1, 2, 3, 4 }",
                new CreateArrayExpression(new PrimitiveTypeReference(PrimitiveType.Int32))
                {
                    Initializer = new ArrayInitializer(
                        new PrimitiveExpression(1),
                        new PrimitiveExpression(2),
                        new PrimitiveExpression(3),
                        new PrimitiveExpression(4))
                });
        }

        [TestMethod]
        public void InitializerWithTrailingComma()
        {
            CSharpAstValidator.AssertExpression("new int[] { 1, 2, 3, 4, }",
                new CreateArrayExpression(new PrimitiveTypeReference(PrimitiveType.Int32))
                {
                    Initializer = new ArrayInitializer(
                        new PrimitiveExpression(1),
                        new PrimitiveExpression(2),
                        new PrimitiveExpression(3),
                        new PrimitiveExpression(4))
                });
        }

        [TestMethod]
        public void FixedSizeAndInitializer()
        {
            CSharpAstValidator.AssertExpression("new int[4] { 1, 2, 3, 4, }",
                new CreateArrayExpression(
                    new PrimitiveTypeReference(PrimitiveType.Int32),
                    new PrimitiveExpression(4))
                {
                    Initializer = new ArrayInitializer(
                        new PrimitiveExpression(1),
                        new PrimitiveExpression(2),
                        new PrimitiveExpression(3),
                        new PrimitiveExpression(4))
                });
        }


    }
}