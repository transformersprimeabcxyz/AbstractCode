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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractCode.Tests.Ast.Parser.CSharp.Expressions
{
    [TestClass]
    public class AssignmentExpressionTests
    {
        [TestMethod]
        public void Simple()
        {
            CSharpAstValidator.AssertExpression("x = 3",
                new AssignmentExpression(
                    new IdentifierExpression("x"),
                    AssignmentOperator.Assign,
                    new PrimitiveExpression(3)));
        }

        [TestMethod]
        public void Add()
        {
            CSharpAstValidator.AssertExpression("x += 3",
                new AssignmentExpression(
                    new IdentifierExpression("x"),
                    AssignmentOperator.Add,
                    new PrimitiveExpression(3)));
        }

        [TestMethod]
        public void Nested()
        {
            CSharpAstValidator.AssertExpression("x = y -= 3",
                new AssignmentExpression(
                    new IdentifierExpression("x"),
                    AssignmentOperator.Assign,
                    new AssignmentExpression(
                        new IdentifierExpression("y"),
                        AssignmentOperator.Subtract,
                        new PrimitiveExpression(3))));
        }

    }
}
