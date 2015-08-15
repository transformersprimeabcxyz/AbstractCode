using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Statements;
using AbstractCode.Ast.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractCode.Tests.Ast.Parser.CSharp.Statements
{
    [TestClass]
    public class UsingStatementTests
    {
        [TestMethod]
        public void NewObject()
        {
            CSharpAstValidator.AssertStatement("using(var x = new y()) { }",
                new UsingStatement(
                    new VariableDeclarationStatement(
                        new SimpleTypeReference("var"),
                        "x",
                        new CreateObjectExpression(
                            new SimpleTypeReference("y"))),
                    new BlockStatement()));
        }

        [TestMethod]
        public void ExistingObject()
        {
            CSharpAstValidator.AssertStatement("using(x) { }",
                new UsingStatement(
                    new IdentifierExpression("x"),
                    new BlockStatement()));
        }
    }
}
