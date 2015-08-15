using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Statements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractCode.Tests.Ast.Parser.CSharp.Statements
{
    [TestClass]
    public class ReturnStatementTests
    {
        [TestMethod]
        public void Return()
        {
            CSharpAstValidator.AssertStatement("return;", 
                new ReturnStatement());
        }

        [TestMethod]
        public void ReturnValue()
        {
            CSharpAstValidator.AssertStatement("return x;", 
                new ReturnStatement(
                    new IdentifierExpression("x")));
        }
    }
}
