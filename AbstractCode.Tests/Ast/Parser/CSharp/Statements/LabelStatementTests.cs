using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractCode.Ast.Statements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractCode.Tests.Ast.Parser.CSharp.Statements
{
    [TestClass]
    public class LabelStatementTests
    {
        [TestMethod]
        public void DefineLabel()
        {
            CSharpAstValidator.AssertStatement("MyLabel:", 
                new LabelStatement("MyLabel"));
        }

        [TestMethod]
        public void GotoLabel()
        {
            CSharpAstValidator.AssertStatement("goto MyLabel;",
                new GotoStatement("MyLabel"));
        }
    }
}
