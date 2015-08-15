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
using System.Linq;
using System.Text;

namespace AbstractCode.Ast.CSharp
{
    public class CSharpAstToken : AstToken
    {
        public static readonly Dictionary<string, CSharpAstTokenCode> KeywordMapping = new Dictionary<string, CSharpAstTokenCode>()
        {
            ["__arglist"] = CSharpAstTokenCode.ARGLIST,
            ["abstract"] = CSharpAstTokenCode.ABSTRACT,
            ["as"] = CSharpAstTokenCode.AS,
            ["add"] = CSharpAstTokenCode.ADD,
            ["base"] = CSharpAstTokenCode.BASE,
            ["bool"] = CSharpAstTokenCode.BOOL,
            ["break"] = CSharpAstTokenCode.BREAK,
            ["byte"] = CSharpAstTokenCode.BYTE,
            ["case"] = CSharpAstTokenCode.CASE,
            ["catch"] = CSharpAstTokenCode.CATCH,
            ["char"] = CSharpAstTokenCode.CHAR,
            ["checked"] = CSharpAstTokenCode.CHECKED,
            ["class"] = CSharpAstTokenCode.CLASS,
            ["const"] = CSharpAstTokenCode.CONST,
            ["continue"] = CSharpAstTokenCode.CONTINUE,
            ["decimal"] = CSharpAstTokenCode.DECIMAL,
            ["default"] = CSharpAstTokenCode.DEFAULT,
            ["delegate"] = CSharpAstTokenCode.DELEGATE,
            ["do"] = CSharpAstTokenCode.DO,
            ["double"] = CSharpAstTokenCode.DOUBLE,
            ["else"] = CSharpAstTokenCode.ELSE,
            ["enum"] = CSharpAstTokenCode.ENUM,
            ["event"] = CSharpAstTokenCode.EVENT,
            ["explicit"] = CSharpAstTokenCode.EXPLICIT,
            ["extern"] = CSharpAstTokenCode.EXTERN,
            ["false"] = CSharpAstTokenCode.FALSE,
            ["finally"] = CSharpAstTokenCode.FINALLY,
            ["fixed"] = CSharpAstTokenCode.FIXED,
            ["float"] = CSharpAstTokenCode.FLOAT,
            ["for"] = CSharpAstTokenCode.FOR,
            ["foreach"] = CSharpAstTokenCode.FOREACH,
            ["get"] = CSharpAstTokenCode.GET,
            ["goto"] = CSharpAstTokenCode.GOTO,
            ["if"] = CSharpAstTokenCode.IF,
            ["implicit"] = CSharpAstTokenCode.IMPLICIT,
            ["in"] = CSharpAstTokenCode.IN,
            ["int"] = CSharpAstTokenCode.INT,
            ["interface"] = CSharpAstTokenCode.INTERFACE,
            ["internal"] = CSharpAstTokenCode.INTERNAL,
            ["is"] = CSharpAstTokenCode.IS,
            ["lock"] = CSharpAstTokenCode.LOCK,
            ["long"] = CSharpAstTokenCode.LONG,
            ["namespace"] = CSharpAstTokenCode.NAMESPACE,
            ["new"] = CSharpAstTokenCode.NEW,
            ["null"] = CSharpAstTokenCode.NULL,
            ["object"] = CSharpAstTokenCode.OBJECT,
            ["operator"] = CSharpAstTokenCode.OPERATOR,
            ["out"] = CSharpAstTokenCode.OUT,
            ["override"] = CSharpAstTokenCode.OVERRIDE,
            ["params"] = CSharpAstTokenCode.PARAMS,
            ["private"] = CSharpAstTokenCode.PRIVATE,
            ["protected"] = CSharpAstTokenCode.PROTECTED,
            ["public"] = CSharpAstTokenCode.PUBLIC,
            ["readonly"] = CSharpAstTokenCode.READONLY,
            ["ref"] = CSharpAstTokenCode.REF,
            ["return"] = CSharpAstTokenCode.RETURN,
            ["remove"] = CSharpAstTokenCode.REMOVE,
            ["sbyte"] = CSharpAstTokenCode.SBYTE,
            ["sealed"] = CSharpAstTokenCode.SEALED,
            ["set"] = CSharpAstTokenCode.SET,
            ["short"] = CSharpAstTokenCode.SHORT,
            ["sizeof"] = CSharpAstTokenCode.SIZEOF,
            ["stackalloc"] = CSharpAstTokenCode.STACKALLOC,
            ["static"] = CSharpAstTokenCode.STATIC,
            ["string"] = CSharpAstTokenCode.STRING,
            ["struct"] = CSharpAstTokenCode.STRUCT,
            ["switch"] = CSharpAstTokenCode.SWITCH,
            ["this"] = CSharpAstTokenCode.THIS,
            ["throw"] = CSharpAstTokenCode.THROW,
            ["true"] = CSharpAstTokenCode.TRUE,
            ["try"] = CSharpAstTokenCode.TRY,
            ["typeof"] = CSharpAstTokenCode.TYPEOF,
            ["uint"] = CSharpAstTokenCode.UINT,
            ["ulong"] = CSharpAstTokenCode.ULONG,
            ["unchecked"] = CSharpAstTokenCode.UNCHECKED,
            ["unsafe"] = CSharpAstTokenCode.UNSAFE,
            ["ushort"] = CSharpAstTokenCode.USHORT,
            ["using"] = CSharpAstTokenCode.USING,
            ["virtual"] = CSharpAstTokenCode.VIRTUAL,
            ["void"] = CSharpAstTokenCode.VOID,
            ["volatile"] = CSharpAstTokenCode.VOLATILE,
            ["where"] = CSharpAstTokenCode.WHERE,
            ["while"] = CSharpAstTokenCode.WHILE,
            ["arglist"] = CSharpAstTokenCode.ARGLIST,
            ["partial"] = CSharpAstTokenCode.PARTIAL,
            ["from"] = CSharpAstTokenCode.FROM,
            ["join"] = CSharpAstTokenCode.JOIN,
            ["on"] = CSharpAstTokenCode.ON,
            ["equals"] = CSharpAstTokenCode.EQUALS,
            ["select"] = CSharpAstTokenCode.SELECT,
            ["group"] = CSharpAstTokenCode.GROUP,
            ["by"] = CSharpAstTokenCode.BY,
            ["let"] = CSharpAstTokenCode.LET,
            ["orderby"] = CSharpAstTokenCode.ORDERBY,
            ["ascending"] = CSharpAstTokenCode.ASCENDING,
            ["descending"] = CSharpAstTokenCode.DESCENDING,
            ["into"] = CSharpAstTokenCode.INTO,
            ["__refvalue"] = CSharpAstTokenCode.REFVALUE,
            ["__reftype"] = CSharpAstTokenCode.REFTYPE,
            ["__makeref"] = CSharpAstTokenCode.MAKEREF,
            ["async"] = CSharpAstTokenCode.ASYNC,
            ["await"] = CSharpAstTokenCode.AWAIT,
            ["when"] = CSharpAstTokenCode.WHEN,
            ["yield"] = CSharpAstTokenCode.YIELD
        };

        public CSharpAstToken(CSharpAstTokenCode code, string value)
            : this(code, value, TextRange.Empty)
        {
        }

        public CSharpAstToken(CSharpAstTokenCode code, string value, TextRange range) 
            : base(value, range)
        {
            Code = code;
        }

        public CSharpAstTokenCode Code
        {
            get;
            set;
        }

        public override int GetTokenCode()
        {
            return (int)Code;
        }

        public override bool Match(AstNode other)
        {
            var token = other as CSharpAstToken;
            return token != null && Code == token.Code;
        }
    }
}
