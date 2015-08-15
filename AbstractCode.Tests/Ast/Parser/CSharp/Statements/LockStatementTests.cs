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
    public class LockStatementTests
    {
        [TestMethod]
        public void Simple()
        {
            CSharpAstValidator.AssertStatement("lock(this) { }", 
                new LockStatement(
                    new ThisReferenceExpression(), 
                    new BlockStatement()));
        }
    }
}
