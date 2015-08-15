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

using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Members;
using AbstractCode.Ast.Statements;
using AbstractCode.Ast.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractCode.Tests.Ast.Parser.CSharp.Statements
{
    [TestClass]
    public class ExceptionHandlerTests
    {
        [TestMethod]
        public void Rethrow()
        {
            CSharpAstValidator.AssertStatement("throw;",
                new ThrowStatement());
        }

        [TestMethod]
        public void ThrowExisting()
        {
            CSharpAstValidator.AssertStatement("throw x;",
                new ThrowStatement(
                    new IdentifierExpression("x")));
        }

        [TestMethod]
        public void ThrowNew()
        {
            CSharpAstValidator.AssertStatement("throw new x();",
                new ThrowStatement(
                    new CreateObjectExpression(
                        new SimpleTypeReference("x"))));
        }

        [TestMethod]
        public void CatchAll()
        {
            CSharpAstValidator.AssertStatement("try { } catch { }",
                new TryCatchStatement()
                {
                    TryBlock = new BlockStatement(),
                    CatchClauses =
                    {
                        new CatchClause()
                        {
                            Body = new BlockStatement()
                        }
                    }
                });
        }

        [TestMethod]
        public void CatchType()
        {
            CSharpAstValidator.AssertStatement("try { } catch (Exception) { }",
                new TryCatchStatement()
                {
                    TryBlock = new BlockStatement(),
                    CatchClauses =
                    {
                        new CatchClause(
                            new SimpleTypeReference("Exception"))
                        {
                            Body = new BlockStatement()
                        }
                    }
                });
        }

        [TestMethod]
        public void CatchVariable()
        {
            CSharpAstValidator.AssertStatement("try { } catch (Exception ex) { }",
                new TryCatchStatement()
                {
                    TryBlock = new BlockStatement(),
                    CatchClauses =
                    {
                        new CatchClause(
                            new SimpleTypeReference("Exception"),
                            new Identifier("ex"))
                        {
                            Body = new BlockStatement()
                        }
                    }
                });
        }

        [TestMethod]
        public void CatchMultiple()
        {
            CSharpAstValidator.AssertStatement("try { } catch (ArgumentException) { } catch (Exception ex) { }",
                new TryCatchStatement()
                {
                    TryBlock = new BlockStatement(),
                    CatchClauses =
                    {
                        new CatchClause(
                            new SimpleTypeReference("ArgumentException"))
                        {
                            Body = new BlockStatement()
                        },
                        new CatchClause(
                            new SimpleTypeReference("Exception"),
                            new Identifier("ex"))
                        {
                            Body = new BlockStatement()
                        }
                    }
                });
        }

        [TestMethod]
        public void Finally()
        {
            CSharpAstValidator.AssertStatement("try { } finally { }",
                new TryCatchStatement()
                {
                    TryBlock = new BlockStatement(),
                    FinallyBlock = new BlockStatement()
                });
        }

        [TestMethod]
        public void CatchFinally()
        {
            CSharpAstValidator.AssertStatement("try { } catch (Exception ex) { } finally { }",
                new TryCatchStatement()
                {
                    TryBlock = new BlockStatement(),
                    CatchClauses =
                    {
                        new CatchClause(
                            new SimpleTypeReference("Exception"),
                            new Identifier("ex"))
                        {
                            Body = new BlockStatement()
                        }
                    },
                    FinallyBlock = new BlockStatement()
                });
        }
    }
}
