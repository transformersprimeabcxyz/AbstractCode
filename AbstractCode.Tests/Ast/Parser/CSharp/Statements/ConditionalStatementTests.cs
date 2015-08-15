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


using AbstractCode.Ast.CSharp.Statements;
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Members;
using AbstractCode.Ast.Statements;
using AbstractCode.Ast.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoLoopStatement = AbstractCode.Ast.Statements.DoLoopStatement;
using IfElseStatement = AbstractCode.Ast.Statements.IfElseStatement;
using SwitchCaseLabel = AbstractCode.Ast.Statements.SwitchCaseLabel;
using SwitchStatement = AbstractCode.Ast.Statements.SwitchStatement;
using WhileLoopStatement = AbstractCode.Ast.Statements.WhileLoopStatement;

namespace AbstractCode.Tests.Ast.Parser.CSharp.Statements
{
    [TestClass]
    public class ConditionalStatementTests
    {
        [TestMethod]
        public void If()
        {
            CSharpAstValidator.AssertStatement("if (true) { }",
                new IfElseStatement(new PrimitiveExpression(true),
                new BlockStatement()));
        }

        [TestMethod]
        public void IfElse()
        {
            CSharpAstValidator.AssertStatement("if (true) { } else { }",
                new IfElseStatement(new PrimitiveExpression(true),
                new BlockStatement(),
                new BlockStatement()));
        }

        [TestMethod]
        public void IfElseIf()
        {
            CSharpAstValidator.AssertStatement("if (true) { } else if (x) { }",
                new IfElseStatement(new PrimitiveExpression(true),
                    new BlockStatement(),
                    new IfElseStatement(new IdentifierExpression("x"),
                        new BlockStatement())));
        }

        [TestMethod]
        public void Switch()
        {
            CSharpAstValidator.AssertStatement(@"switch (a) 
{
    case 1:
    case 2:
        break;
    case 3:
        break;
    default:
        break;
}",
                new SwitchStatement(
                    new IdentifierExpression("a"),
                    new SwitchSection(
                        new SwitchCaseLabel(new PrimitiveExpression(1)),
                        new SwitchCaseLabel(new PrimitiveExpression(2)))
                    {
                        Statements =
                        {
                            new BreakStatement()
                        }
                    },
                    new SwitchSection(
                        new SwitchCaseLabel(new PrimitiveExpression(3)))
                    {
                        Statements =
                        {
                            new BreakStatement()
                        }
                    },
                    new SwitchSection(
                        new SwitchCaseLabel())
                    {
                        Statements =
                        {
                            new BreakStatement()
                        }
                    }
                    ));
        }

        [TestMethod]
        public void Do()
        {
            CSharpAstValidator.AssertStatement("do { } while (true);",
                new DoLoopStatement(new BlockStatement(), new PrimitiveExpression(true)));
        }

        [TestMethod]
        public void While()
        {
            CSharpAstValidator.AssertStatement("while (true) { }",
                new WhileLoopStatement(new PrimitiveExpression(true), new BlockStatement()));
        }

        [TestMethod]
        public void For()
        {
            CSharpAstValidator.AssertStatement("for (int x = 0; x < 10; x++) { }",
                new ForLoopStatement(
                    new VariableDeclarationStatement(
                        new PrimitiveTypeReference(PrimitiveType.Int32),
                        "x",
                        new PrimitiveExpression(0)),
                    new BinaryOperatorExpression(
                        new IdentifierExpression("x"),
                        BinaryOperator.LessThan,
                        new PrimitiveExpression(10)),
                    new ExpressionStatement(
                        new UnaryOperatorExpression(
                            UnaryOperator.PostIncrement,
                            new IdentifierExpression("x"))))
                {
                    Body = new BlockStatement()
                });
        }

        [TestMethod]
        public void ForEmpty()
        {
            CSharpAstValidator.AssertStatement("for (;;) { }",
                new ForLoopStatement(
                    null,
                    null,
                    null)
                {
                    Body = new BlockStatement()
                });
        }

        [TestMethod]
        public void Foreach()
        {
            CSharpAstValidator.AssertStatement("foreach (var x in y) { }",
                new ForeachLoopStatement(new SimpleTypeReference("var"), 
                    new Identifier("x"),
                    new IdentifierExpression("y"))
                {
                    Body = new BlockStatement()
                });
        }
    }
}
