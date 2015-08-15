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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractCode.Tests.Ast.Parser.CSharp.Expressions
{
    [TestClass]
    public class LinqExpressionTests
    {
        [TestMethod]
        public void Select()
        {
            CSharpAstValidator.AssertExpression("from x in y select x.a",
                new LinqExpression(
                    new LinqFromClause(
                        new Identifier("x"),
                        new IdentifierExpression("y")),
                    new LinqSelectClause(
                        new MemberReferenceExpression(
                            new IdentifierExpression("x"),
                            "a"))));
        }

        [TestMethod]
        public void Where()
        {
            CSharpAstValidator.AssertExpression("from x in y where x.a select x.b",
                new LinqExpression(
                    new LinqFromClause(
                        new Identifier("x"),
                        new IdentifierExpression("y")),
                    new LinqWhereClause(
                        new MemberReferenceExpression(
                            new IdentifierExpression("x"),
                            "a")),
                    new LinqSelectClause(
                        new MemberReferenceExpression(
                            new IdentifierExpression("x"),
                            "b"))));
        }

        [TestMethod]
        public void GroupBy()
        {
            CSharpAstValidator.AssertExpression("from x in y where x.a group a by b select x.b",
                new LinqExpression(
                    new LinqFromClause(
                        new Identifier("x"),
                        new IdentifierExpression("y")),
                    new LinqWhereClause(
                        new MemberReferenceExpression(
                            new IdentifierExpression("x"),
                            "a")),
                    new LinqGroupByClause(
                        new IdentifierExpression("a"),
                        new IdentifierExpression("b")),
                    new LinqSelectClause(
                        new MemberReferenceExpression(
                            new IdentifierExpression("x"),
                            "b"))));
        }

        [TestMethod]
        public void OrderBy()
        {
            CSharpAstValidator.AssertExpression("from x in y where x.a orderby x.a select x.b",
                new LinqExpression(
                    new LinqFromClause(
                        new Identifier("x"),
                        new IdentifierExpression("y")),
                    new LinqWhereClause(
                        new MemberReferenceExpression(
                            new IdentifierExpression("x"),
                            "a")),
                    new LinqOrderByClause(
                        new LinqOrdering(
                            new MemberReferenceExpression(
                                new IdentifierExpression("x"),
                                "a"))),
                    new LinqSelectClause(
                        new MemberReferenceExpression(
                            new IdentifierExpression("x"),
                            "b"))));
        }

        [TestMethod]
        public void OrderByAscendingExplicit()
        {
            CSharpAstValidator.AssertExpression("from x in y where x.a orderby x.a ascending select x.b",
                new LinqExpression(
                    new LinqFromClause(
                        new Identifier("x"),
                        new IdentifierExpression("y")),
                    new LinqWhereClause(
                        new MemberReferenceExpression(
                            new IdentifierExpression("x"),
                            "a")),
                    new LinqOrderByClause(
                        new LinqOrdering(
                            new MemberReferenceExpression(
                                new IdentifierExpression("x"),
                                "a"),
                            LinqOrderingDirection.Ascending)),
                    new LinqSelectClause(
                        new MemberReferenceExpression(
                            new IdentifierExpression("x"),
                            "b"))));
        }

        [TestMethod]
        public void Let()
        {
            CSharpAstValidator.AssertExpression("from x in y let z = x.a select z",
                new LinqExpression(
                    new LinqFromClause(
                        new Identifier("x"),
                        new IdentifierExpression("y")),
                    new LinqLetClause(
                        new VariableDeclarator("z",
                            new MemberReferenceExpression(
                                new IdentifierExpression("x"),
                                "a"))),
                    new LinqSelectClause(
                        new IdentifierExpression("z"))));
        }
    }
}
