using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractCode.Ast.CSharp.Expressions;
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractCode.Tests.Ast.Parser.CSharp.Expressions
{
    [TestClass]
    public class KeywordExpressionsTests
    {
        [TestMethod]
        public void TypeOf()
        {
            CSharpAstValidator.AssertExpression("typeof(int)",
                new GetTypeExpression(new PrimitiveTypeReference(PrimitiveType.Int32)));
        }

        [TestMethod]
        public void Checked()
        {
            CSharpAstValidator.AssertExpression("checked(1 + 2)",
                new CheckedExpression(
                    new BinaryOperatorExpression(
                        new PrimitiveExpression(1),
                        BinaryOperator.Add,
                        new PrimitiveExpression(2))));
        }

        [TestMethod]
        public void Unchecked()
        {
            CSharpAstValidator.AssertExpression("unchecked(1 + 2)",
                new UncheckedExpression(
                    new BinaryOperatorExpression(
                        new PrimitiveExpression(1),
                        BinaryOperator.Add,
                        new PrimitiveExpression(2))));
        }

        [TestMethod]
        public void SizeOf()
        {
            CSharpAstValidator.AssertExpression("sizeof(int)",
                new SizeOfExpression(
                    new PrimitiveTypeReference(PrimitiveType.Int32)));
        }

        [TestMethod]
        public void Default()
        {
            CSharpAstValidator.AssertExpression("default(int)",
                new DefaultExpression(
                    new PrimitiveTypeReference(PrimitiveType.Int32)));
        }

        [TestMethod]
        public void StackAlloc()
        {
            CSharpAstValidator.AssertExpression("stackalloc int[x]",
                new StackAllocExpression(
                    new PrimitiveTypeReference(PrimitiveType.Int32), 
                    new IdentifierExpression("x")));
        }
    }
}
