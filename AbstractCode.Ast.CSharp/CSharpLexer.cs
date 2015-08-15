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
using System.IO;
using System.Linq;
using System.Text;
using AbstractCode.Ast.Parser;

namespace AbstractCode.Ast.CSharp
{
    public class CSharpLexer : Lexer
    {
        private const string _extraWordIndicators = "_@";

        public CSharpLexer(TextReader reader) 
            : base(reader)
        {
        }

        public override AstToken ReadNextToken()
        {
            while (true)
            {
                if (!MoveToNextToken())
                    return new CSharpAstToken(CSharpAstTokenCode.EOF, string.Empty, new TextRange(Location, Location));

                char currentChar;
                Peek(out currentChar);

                CSharpAstToken token;

                if (currentChar == '/')
                {
                    var node = TokenizeForwardSlash();
                    token = node as CSharpAstToken;
                    if (token == null)
                        continue; // TODO: expose comment nodes.
                }
                else if (char.IsDigit(currentChar))
                {
                    token = ReadNumberToken();
                }
                else if (IsWordElement(currentChar))
                {
                    token = ReadWordToken();
                    CSharpAstTokenCode keywordCode;
                    if (CSharpAstToken.KeywordMapping.TryGetValue(token.Value, out keywordCode))
                    {
                        token.Code = keywordCode;

                        switch (keywordCode)
                        {
                            case CSharpAstTokenCode.TRUE:
                                token.UserData["InterpretedValue"] = true;
                                break;
                            case CSharpAstTokenCode.FALSE:
                                token.UserData["InterpretedValue"] = false;
                                break;
                        }
                    }
                }
                else if (currentChar == '\'' || currentChar == '"')
                {
                    token = ReadStringToken();
                }
                else
                {
                    var start = Location;
                    StartReadBuffer();
                    var code = TokenizeCurrentSymbol();
                    var end = Location;

                    token = new CSharpAstToken(code, EndReadBuffer().ToString(), new TextRange(start, end));
                }

                return token;
            }
        }

        private static bool IsWordElement(char character)
        {
            return char.IsLetterOrDigit(character) || _extraWordIndicators.Contains(character);
        }

        protected CSharpAstToken ReadWordToken()
        {
            TextRange range;
            string token = ReadCharacters(IsWordElement, out range);
            return new CSharpAstToken(CSharpAstTokenCode.IDENTIFIER, token, range);
        }

        protected CSharpAstToken ReadNumberToken()
        {
            TextRange range;
            bool hasSuffix = false;
            bool hasDecimalSpecifier = false;

            string token = ReadCharacters(x =>
            {
                if (char.IsLetter(x))
                {
                    hasSuffix = true;
                    return true;
                }

                if (char.IsDigit(x))
                    return !hasSuffix;

                if (x == '.')
                {
                    if (hasDecimalSpecifier)
                        throw new Exception("Too many decimal specifiers in number.");
                    hasDecimalSpecifier = true;
                    return true;
                }

                return false;

            }, out range);
            
            var astToken = new CSharpAstToken(CSharpAstTokenCode.LITERAL, token, range);
            astToken.UserData["InterpretedValue"] = CSharpLanguage.Instance.NumberFormatter.EvaluateFormattedNumber(token);
            return astToken;
        }

        private CSharpAstToken ReadStringToken()
        {
            var startLocation = Location;
            var startCharacter = Read();

            TextRange range;

            bool escaped = false;

            string stringToken = startCharacter + ReadCharacters(x =>
            {
                if (x == startCharacter)
                {
                    if (escaped)
                        escaped = false;
                    else
                        return false;
                }
                else if (x == '\\')
                {
                    escaped = !escaped;
                }
                else
                {
                    escaped = false;
                }

                return true;
            }, out range);

            char character;
            if (Peek(out character) && character == startCharacter)
                stringToken += Read();
            var endLocation = Location;

            var astToken = new CSharpAstToken(CSharpAstTokenCode.LITERAL, stringToken, new TextRange(startLocation, endLocation));
            astToken.UserData["InterpretedValue"] = CSharpLanguage.Instance.StringFormatter.EvaluateFormattedString(stringToken);
            return astToken;
        }

        private AstNode TokenizeForwardSlash()
        {
            var start = Location;
            Read();

            char nextChar;
            Peek(out nextChar);

            AstNode node;
            switch (nextChar)
            {
                case '/':
                    node =  ReadSingleLineComment();
                    break;
                case '*':
                    node =  ReadCommentBlock();
                    break;
                case '=':
                    Read();
                    node = new CSharpAstToken(CSharpAstTokenCode.OP_DIV_ASSIGN, "/=");
                    break;
                default:
                    node = new CSharpAstToken(CSharpAstTokenCode.DIV, "/");
                    break;
            }
            var end = Location;
            node.Range = new TextRange(start, end);
            return node;
        }

        private AstNode ReadSingleLineComment()
        {
            // read second "/"
            Read();

            // get contents
            TextRange range;
            var contents = ReadCharacters(x => !"\r\n".Contains(x), out range);

            return new Comment(contents, CommentType.SingleLine);
        }

        private AstNode ReadCommentBlock()
        {
            // read "*"
            Read();

            // get contents
            TextRange range;
            char previousChar = '\0';
            var contents = ReadCharacters((x) =>
            {
                bool @continue = !(previousChar == '*' && x == '/');
                previousChar = x;
                return @continue;
            }, out range);

            if (previousChar == '/')
            {
                // read */
                contents = contents.Remove(contents.Length - 2);
                Read();
            }

            return new Comment(contents, CommentType.Block);
        }

        private CSharpAstTokenCode TokenizeCurrentSymbol()
        {
            char currentChar = Read();

            char nextChar;
            Peek(out nextChar);
            CSharpAstTokenCode code;
            
            switch (currentChar)
            {
                case '{':
                    return CSharpAstTokenCode.OPEN_BRACE;
                case '}':
                    return CSharpAstTokenCode.CLOSE_BRACE;
                case '(':
                    return CSharpAstTokenCode.OPEN_PARENS;
                case ')':
                    return CSharpAstTokenCode.CLOSE_PARENS;
                case '[':
                    return CSharpAstTokenCode.OPEN_BRACKET;
                case ']':
                    return CSharpAstTokenCode.CLOSE_BRACKET;
                case ',':
                    return CSharpAstTokenCode.COMMA;
                case '+':
                    switch (nextChar)
                    {
                        case '+':
                            code = CSharpAstTokenCode.OP_INC;
                            break;
                        case '=':
                            code = CSharpAstTokenCode.OP_ADD_ASSIGN;
                            break;
                        default:
                            return CSharpAstTokenCode.PLUS;
                    }
                    Read();
                    return code;
                case '-':
                    switch (nextChar)
                    {
                        case '>':
                            code = CSharpAstTokenCode.OP_PTR;
                            break;
                        case '-':
                            code = CSharpAstTokenCode.OP_DEC;
                            break;
                        case '=':
                            code = CSharpAstTokenCode.OP_SUB_ASSIGN;
                            break;
                        default:
                            return CSharpAstTokenCode.MINUS;
                    }
                    Read();
                    return code;
                case '*':
                    if (nextChar == '=')
                        code = CSharpAstTokenCode.OP_MULT_ASSIGN;
                    else
                        return CSharpAstTokenCode.STAR;
                    Read();
                    return code;
                case '%':
                    if (nextChar == '=')
                        code = CSharpAstTokenCode.OP_MOD_ASSIGN;
                    else
                        return CSharpAstTokenCode.PERCENT;
                    Read();
                    return code;
                case '<':
                    switch (nextChar)
                    {
                        case '=':
                            code = CSharpAstTokenCode.OP_LE;
                            break;
                        case '<':
                            Read();
                            Peek(out nextChar);
                            if (nextChar == '=')
                                code = CSharpAstTokenCode.OP_SHIFT_LEFT_ASSIGN;
                            else
                                return CSharpAstTokenCode.OP_SHIFT_LEFT;
                            break;
                        default:
                            return CSharpAstTokenCode.OP_LT;
                    }
                    Read();
                    return code;
                case '>':
                    switch (nextChar)
                    {
                        case '=':
                            code = CSharpAstTokenCode.OP_GE;
                            break;
                        case '>':
                            Read();
                            Peek(out nextChar);
                            if (nextChar == '=')
                                code =  CSharpAstTokenCode.OP_SHIFT_RIGHT_ASSIGN;
                            else
                                return CSharpAstTokenCode.OP_SHIFT_RIGHT;
                            break;
                        default:
                            return CSharpAstTokenCode.OP_GT;
                    }
                    Read();
                    return code;
                case '&':
                    switch (nextChar)
                    {
                        case '=':
                            code = CSharpAstTokenCode.OP_AND_ASSIGN;
                            break;
                        case '&':
                            code = CSharpAstTokenCode.OP_AND;
                            break;
                        default:
                            return CSharpAstTokenCode.BITWISE_AND;
                    }
                    Read();
                    return code;
                case '|':
                    switch (nextChar)
                    {
                        case '=':
                            code = CSharpAstTokenCode.OP_OR_ASSIGN;
                            break;
                        case '|':
                            code = CSharpAstTokenCode.OP_OR;
                            break;
                        default:
                            return CSharpAstTokenCode.BITWISE_OR;
                    }
                    Read();
                    return code;
                case '^':
                    if (nextChar == '=')
                        code = CSharpAstTokenCode.OP_XOR_ASSIGN;
                    else
                        return CSharpAstTokenCode.CARRET;
                    Read();
                    return code;
                case '!':
                    if (nextChar == '=')
                    {
                        Read();
                        return CSharpAstTokenCode.OP_NOTEQUALS;
                    }
                    return CSharpAstTokenCode.BANG;
                case '=':
                    switch (nextChar)
                    {
                        case '=':
                            code = CSharpAstTokenCode.OP_EQUALS;
                            break;
                        case '>':
                            code = CSharpAstTokenCode.ARROW;
                            break;
                        default:
                            return CSharpAstTokenCode.EQUALS;
                    }
                    Read();
                    return code;
                case ':':
                    if (nextChar == ':')
                        code = CSharpAstTokenCode.DOUBLE_COLON;
                    else
                        return CSharpAstTokenCode.COLON;
                    Read();
                    return code;
                case ';':
                    return CSharpAstTokenCode.SEMICOLON;
                case '.':
                    return CSharpAstTokenCode.DOT;
                case '?':
                    switch (nextChar)
                    {
                        case '?':
                            code = CSharpAstTokenCode.OP_COALESCING;
                            break;
                        case '.':
                            code = CSharpAstTokenCode.INTERR_OPERATOR;
                            break;
                        default:
                            return CSharpAstTokenCode.INTERR;
                    }
                    Read();
                    return code;
                case '~':
                    return CSharpAstTokenCode.TILDE;
            }

            return CSharpAstTokenCode.ERROR;
        }
    }
}
