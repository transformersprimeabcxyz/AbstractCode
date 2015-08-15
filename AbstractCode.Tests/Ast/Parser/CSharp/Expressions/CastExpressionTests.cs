using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractCode.Tests.Ast.Parser.CSharp.Expressions
{
    [TestClass]
    public class CastExpressionTests
    {
        [TestMethod]
        public void Explicit()
        {
            CSharpAstValidator.AssertExpression("(int)obj",
                new ExplicitCastExpression(
                    new PrimitiveTypeReference(PrimitiveType.Int32), 
                    new IdentifierExpression("obj")));
        }

        [TestMethod]
        public void Safe()
        {
            CSharpAstValidator.AssertExpression("obj as int",
                new SafeCastExpression(
                    new PrimitiveTypeReference(PrimitiveType.Int32), 
                    new IdentifierExpression("obj")));
        }

        [TestMethod]
        public void TypeCheck()
        {
            CSharpAstValidator.AssertExpression("obj is int",
                new TypeCheckExpression(
                    new IdentifierExpression("obj"), 
                    new PrimitiveTypeReference(PrimitiveType.Int32)));
        }
    }
}
