using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractCode.Ast.CSharp.Statements;
using AbstractCode.Ast.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YieldStatement = AbstractCode.Ast.Statements.YieldStatement;

namespace AbstractCode.Tests.Ast.Parser.CSharp.Statements
{
    [TestClass]
    public class YieldStatementTests
    {
        [TestMethod]
        public void YieldReturn()
        {
            CSharpAstValidator.AssertStatement("yield return x;",
                new YieldStatement(
                    new IdentifierExpression("x")));
        }

        [TestMethod]
        public void YieldBreak()
        {
            CSharpAstValidator.AssertStatement("yield break;",
                new YieldBreakStatement());
        }
    }
}
