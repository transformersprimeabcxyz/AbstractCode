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

// ReSharper disable InconsistentNaming
namespace AbstractCode.Ast.VisualBasic
{
    public enum VisualBasicAstTokenCode
    {
        EOF,
        ERROR,
        NEWLINE,

        LITERAL,
        OPEN_PARENS,
        CLOSE_PARENS,
        OPEN_BRACE,
        CLOSE_BRACE,
        COMMA,

        FIRST_KEYWORD,

        TRUE,
        FALSE,
        BITWISE_AND,
        BITWISE_OR,
        BITWISE_XOR,
        OP_ANDALSO,
        OP_ORELSE,
        MOD,
        NOT,
        IF,
        IFF,
        ME,
        MYBASE,

        LAST_KEYWORD,

        IDENTIFIER,

        STAR,
        DIV,
        PERCENT,
        PLUS,
        MINUS,
        OP_SHIFT_LEFT,
        OP_SHIFT_RIGHT,
        OP_GT,
        OP_GE,
        OP_LT,
        OP_LE,
        IS,
        ISNOT,
        EQUALS,
        OP_NOTEQUALS,
        OP_ADD_ASSIGN,
        OP_SUB_ASSIGN,
        OP_MULT_ASSIGN,
        OP_SHIFT_LEFT_ASSIGN,
        OP_SHIFT_RIGHT_ASSIGN,
        COLON,
        DOT,
    }
}