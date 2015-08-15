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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AbstractCode.Ast;
using AbstractCode.Ast.CSharp;
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Parser;
using AbstractCode.Ast.Statements;
using AbstractCode.Ast.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractCode.Tests.Ast.Parser.CSharp
{
    public static class CSharpAstValidator
    {
        private static readonly AutomatonSourceParser Parser;
        private static readonly CSharpGrammar Grammar = CSharpLanguage.Instance.Grammar;

        static CSharpAstValidator()
        {
            Parser = new AutomatonSourceParser(Grammar);
        }
        
        public static void AssertType(string code, TypeReference expected)
        {
            AssertExpression("new " + code + "()", new CreateObjectExpression(expected));
        }

        public static void AssertExpression(string code, Expression expected)
        {
            AssertStatement("tmp = " + code + ";", new ExpressionStatement(new AssignmentExpression(
                new IdentifierExpression("tmp"),
                AssignmentOperator.Assign,
                expected)));
        }

        public static void AssertStatement(string code, Statement expected)
        {
            var parserTree = Parser.Parse(new AstTokenStream(new CSharpLexer(new StringReader(code))), Grammar.StatementRule);
            var statement = parserTree.CreateAstNode();

            if (!statement.Match(expected))
            {
                Assert.Fail("Expected {0}, actual parsed code {1}", GenerateCode(expected), GenerateCode(statement));
            }
        }

        public static void AssertCompilationUnit(string code, CompilationUnit expected)
        {
            var parserTree = Parser.Parse(new AstTokenStream(new CSharpLexer(new StringReader(code))));
            var statement = parserTree.CreateAstNode();

            if (!statement.Match(expected))
            {
                Assert.Fail("Expected {0}, actual parsed code {1}", GenerateCode(expected), GenerateCode(statement));
            }
        }

        private static string GenerateCode(IVisitable node)
        {
            using (var writer = new StringWriter())
            {
                var astWriter = CSharpLanguage.Instance.CreateWriter(new TextOutputFormatter(new StringTextOutput(writer, "   ")));
                node.AcceptVisitor(astWriter);
                return writer.ToString();
            }
        }
    }
}
