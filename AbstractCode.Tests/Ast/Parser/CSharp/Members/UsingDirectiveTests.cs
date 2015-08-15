using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractCode.Ast;
using AbstractCode.Ast.Members;
using AbstractCode.Ast.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractCode.Tests.Ast.Parser.CSharp.Members
{
    [TestClass]
    public class UsingDirectiveTests
    {
        [TestMethod]
        public void UsingNamespace()
        {
            CSharpAstValidator.AssertCompilationUnit(@"
using System;
using System.IO;",
            new CompilationUnit()
            {
                UsingDirectives =
                {
                    new UsingNamespaceDirective("System"),
                    new UsingNamespaceDirective("System.IO"),
                }
            });
        }

        [TestMethod]
        public void UsingAlias()
        {
            CSharpAstValidator.AssertCompilationUnit(@"
using x = System.String;",
            new CompilationUnit()
            {
                UsingDirectives =
                {
                    new UsingAliasDirective(
                        "x",
                        new MemberTypeReference(
                            new SimpleTypeReference("System"),
                            new Identifier("String")))
                }
            });
        }

        [TestMethod]
        public void UsingMixed()
        {
            CSharpAstValidator.AssertCompilationUnit(@"
using System;
using System.IO;
using x = System.String;",
            new CompilationUnit()
            {
                UsingDirectives =
                {
                    new UsingNamespaceDirective("System"),
                    new UsingNamespaceDirective("System.IO"),
                    new UsingAliasDirective(
                        "x",
                        new MemberTypeReference(
                            new SimpleTypeReference("System"),
                            new Identifier("String")))
                }
            });
        }
    }
}
