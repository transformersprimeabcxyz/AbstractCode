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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractCode.Tests.Ast.Parser.CSharp.Expressions
{
    [TestClass]
    public class PrimitiveExpressionTests
    {
        [TestMethod]
        public void True()
        {
            CSharpAstValidator.AssertExpression("true", new PrimitiveExpression(true));
        }

        [TestMethod]
        public void False()
        {
            CSharpAstValidator.AssertExpression("false", new PrimitiveExpression(false));
        }

        [TestMethod]
        public void Null()
        {
            CSharpAstValidator.AssertExpression("null", new PrimitiveExpression(null));
        }

        [TestMethod]
        public void Integer()
        {
            CSharpAstValidator.AssertExpression("1337", new PrimitiveExpression(1337));
        }

        [TestMethod]
        public void IntegerWithSuffix()
        {
            CSharpAstValidator.AssertExpression("1337u", new PrimitiveExpression(1337u));
        }

        [TestMethod]
        public void FloatingPoint()
        {
            CSharpAstValidator.AssertExpression("1337.1337", new PrimitiveExpression(1337.1337));
        }

        [TestMethod]
        public void String()
        {
            CSharpAstValidator.AssertExpression("\"Lorem ipsum dolor sit amet.\"", new PrimitiveExpression("Lorem ipsum dolor sit amet."));
        }

        [TestMethod]
        public void StringWithSpecialChars()
        {
            CSharpAstValidator.AssertExpression("\"This string has \\\"quotes\\\" and\\r\\nnew lines.\"", new PrimitiveExpression("This string has \"quotes\" and\r\nnew lines."));
        }
    }
}
