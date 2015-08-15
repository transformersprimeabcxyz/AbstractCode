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
using AbstractCode.Ast.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractCode.Tests.Ast.Parser.CSharp.Expressions
{
    [TestClass]
    public class InvocationExpressionTests
    {
        [TestMethod]
        public void NoParameters()
        {
            CSharpAstValidator.AssertExpression("method()",
                new InvocationExpression(new IdentifierExpression("method")));
        }

        [TestMethod]
        public void SingleParameter()
        {
            CSharpAstValidator.AssertExpression("method(1337)",
                new InvocationExpression(new IdentifierExpression("method"),
                    new PrimitiveExpression(1337)));
        }

        [TestMethod]
        public void MultipleParameters()
        {
            CSharpAstValidator.AssertExpression("method(1337, \"Lorem ipsum\", Math.PI)",
                new InvocationExpression(new IdentifierExpression("method"),
                    new PrimitiveExpression(1337),
                    new PrimitiveExpression("Lorem ipsum"),
                    new MemberReferenceExpression(
                        new IdentifierExpression("Math"),
                        "PI")));
        }

        [TestMethod]
        public void Nested()
        {
            CSharpAstValidator.AssertExpression("method(1337, method2(), method3(a(), b()))",
                new InvocationExpression(new IdentifierExpression("method"),
                    new PrimitiveExpression(1337),
                    new InvocationExpression(new IdentifierExpression("method2")),
                    new InvocationExpression(new IdentifierExpression("method3"),
                        new InvocationExpression(new IdentifierExpression("a")),
                        new InvocationExpression(new IdentifierExpression("b")))));
        }

        [TestMethod]
        public void DirectionParameters()
        {
            CSharpAstValidator.AssertExpression("method(ref x, out y, z)",
                new InvocationExpression(new IdentifierExpression("method"),
                    new DirectionExpression(FieldDirection.Ref, new IdentifierExpression("x")),
                    new DirectionExpression(FieldDirection.Out, new IdentifierExpression("y")),
                    new IdentifierExpression("z")));
        }

        [TestMethod]
        public void PrimitiveTypeMethods()
        {
            CSharpAstValidator.AssertExpression("int.Parse(\"1\")",
                new InvocationExpression(
                    new MemberReferenceExpression(
                        new IdentifierExpression("int"),
                        "Parse"),
                    new PrimitiveExpression("1")));
        }
    }
}
