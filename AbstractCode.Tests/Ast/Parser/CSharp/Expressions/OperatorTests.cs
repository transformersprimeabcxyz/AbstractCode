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
using AbstractCode.Ast.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractCode.Tests.Ast.Parser.CSharp.Expressions
{
    [TestClass]
    public class OperatorTests
    {
        [TestMethod]
        public void ArithmeticPrecedence1()
        {
            CSharpAstValidator.AssertExpression("x + y * z",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.Add,
                    new BinaryOperatorExpression(
                        new IdentifierExpression("y"),
                        BinaryOperator.Multiply,
                        new IdentifierExpression("z"))));
        }

        [TestMethod]
        public void ArithmeticPrecedence2()
        {
            CSharpAstValidator.AssertExpression("x * y + z",
                new BinaryOperatorExpression(
                    new BinaryOperatorExpression(
                        new IdentifierExpression("x"),
                        BinaryOperator.Multiply,
                        new IdentifierExpression("y")),
                    BinaryOperator.Add,
                    new IdentifierExpression("z")));
        }

        [TestMethod]
        public void ArithmeticPrecedenceOverride()
        {
            CSharpAstValidator.AssertExpression("x * (y + z)",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.Multiply,
                    new ParenthesizedExpression(
                        new BinaryOperatorExpression(
                            new IdentifierExpression("y"),
                            BinaryOperator.Add,
                            new IdentifierExpression("z")))));
        }

        [TestMethod]
        public void EqualityPrecedence()
        {
            CSharpAstValidator.AssertExpression("x == y + 3",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.Equals,
                    new BinaryOperatorExpression(
                        new IdentifierExpression("y"),
                        BinaryOperator.Add,
                        new PrimitiveExpression(3))));
        }

        [TestMethod]
        public void AddNegative()
        {
            CSharpAstValidator.AssertExpression("x + -y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.Add,
                    new UnaryOperatorExpression(
                        UnaryOperator.Negative,
                        new IdentifierExpression("y"))));
        }

        [TestMethod]
        public void Dereference()
        {
            CSharpAstValidator.AssertExpression("x + *y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.Add,
                    new UnaryOperatorExpression(
                        UnaryOperator.Dereference,
                        new IdentifierExpression("y"))));
        }

        [TestMethod]
        public void BitwiseNot()
        {
            CSharpAstValidator.AssertExpression("~x",
                new UnaryOperatorExpression(UnaryOperator.BitwiseNot, new IdentifierExpression("x")));
        }

        [TestMethod]
        public void PreIncrement()
        {
            CSharpAstValidator.AssertExpression("x + ++y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.Add,
                    new UnaryOperatorExpression(
                        UnaryOperator.PreIncrement,
                        new IdentifierExpression("y"))));
        }

        [TestMethod]
        public void PostIncrement()
        {
            CSharpAstValidator.AssertExpression("x + y++",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.Add,
                    new UnaryOperatorExpression(
                        UnaryOperator.PostIncrement,
                        new IdentifierExpression("y"))));
        }

        [TestMethod]
        public void PreDecrement()
        {
            CSharpAstValidator.AssertExpression("x + --y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.Add,
                    new UnaryOperatorExpression(
                        UnaryOperator.PreDecrement,
                        new IdentifierExpression("y"))));
        }

        [TestMethod]
        public void PostDecrement()
        {
            CSharpAstValidator.AssertExpression("x + y--",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.Add,
                    new UnaryOperatorExpression(
                        UnaryOperator.PostDecrement,
                        new IdentifierExpression("y"))));
        }

        [TestMethod]
        public void Add()
        {
            CSharpAstValidator.AssertExpression("x + y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.Add,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void Subtract()
        {
            CSharpAstValidator.AssertExpression("x - y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.Subtract,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void Multiply()
        {
            CSharpAstValidator.AssertExpression("x * y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.Multiply,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void Divide()
        {
            CSharpAstValidator.AssertExpression("x / y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.Divide,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void Modulus()
        {
            CSharpAstValidator.AssertExpression("x % y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.Modulus,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void BitwiseAnd()
        {
            CSharpAstValidator.AssertExpression("x & y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.BitwiseAnd,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void BitwiseOr()
        {
            CSharpAstValidator.AssertExpression("x | y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.BitwiseOr,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void BitwiseXor()
        {
            CSharpAstValidator.AssertExpression("x ^ y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.BitwiseXor,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void LogicalAnd()
        {
            CSharpAstValidator.AssertExpression("x && y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.LogicalAnd,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void LogicalOr()
        {
            CSharpAstValidator.AssertExpression("x || y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.LogicalOr,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void ShiftLeft()
        {
            CSharpAstValidator.AssertExpression("x << y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.ShiftLeft,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void ShiftRight()
        {
            CSharpAstValidator.AssertExpression("x >> y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.ShiftRight,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void LessThan()
        {
            CSharpAstValidator.AssertExpression("x < y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.LessThan,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void LessThanOrEquals()
        {
            CSharpAstValidator.AssertExpression("x <= y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.LessThanOrEquals,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void GreaterThan()
        {
            CSharpAstValidator.AssertExpression("x > y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.GreaterThan,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void GreaterThanOrEquals()
        {
            CSharpAstValidator.AssertExpression("x >= y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.GreaterThanOrEquals,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void Equals()
        {
            CSharpAstValidator.AssertExpression("x == y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.Equals,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void NotEquals()
        {
            CSharpAstValidator.AssertExpression("x != y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.NotEquals,
                    new IdentifierExpression("y")));
        }

        [TestMethod]
        public void NullCoalescing()
        {
            CSharpAstValidator.AssertExpression("x ?? y",
                new BinaryOperatorExpression(
                    new IdentifierExpression("x"),
                    BinaryOperator.NullCoalescing,
                    new IdentifierExpression("y")));
        }
    }
}
