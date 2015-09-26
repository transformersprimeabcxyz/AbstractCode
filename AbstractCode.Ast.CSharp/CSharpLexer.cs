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
using static AbstractCode.Ast.CSharp.CSharpAstTokenCode;

namespace AbstractCode.Ast.CSharp
{

    public class CSharpLexer : Lexer
    {
        private const string _extraWordIndicators = "_@";

        public CSharpLexer(TextReader reader) 
            : base(reader)
        {
        }

        protected override void OnLineTerminated()
        {
            SpecialBag.SpecialNodes.Add(new CSharpAstToken(NEWLINE, "\r\n",
                new TextRange(Location, new TextLocation(Location.Line + 1, 1))));
            base.OnLineTerminated();
        }

        public override AstToken ReadNextToken()
        {
            while (true)
            {
                if (!MoveToNextToken())
                    return new CSharpAstToken(EOF, string.Empty, new TextRange(Location, Location));

                char currentChar;
                Peek(out currentChar);

                CSharpAstToken token;

                if (currentChar == '/')
                {
                    var node = TokenizeForwardSlash();
                    token = node as CSharpAstToken;
                    if (token == null)
                    {
                        SpecialBag.SpecialNodes.Add(node);
                        continue; 
                    }
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
                            case TRUE:
                                token.UserData["InterpretedValue"] = true;
                                break;
                            case FALSE:
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
            return new CSharpAstToken(IDENTIFIER, token, range);
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
            
            var astToken = new CSharpAstToken(LITERAL, token, range);
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

            var astToken = new CSharpAstToken(LITERAL, stringToken, new TextRange(startLocation, endLocation));
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
                    node = new CSharpAstToken(OP_DIV_ASSIGN, "/=");
                    break;
                default:
                    node = new CSharpAstToken(DIV, "/");
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

            var commentType = CommentType.SingleLine;
            char next;
            Peek(out next);
            if (next == '/')
            {
                Read();
                commentType = CommentType.Documentation;
            }

            // get contents
            TextRange range;
            var contents = ReadCharacters(x => !"\r\n".Contains(x), out range);

            return new Comment(contents, commentType);
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
                    return OPEN_BRACE;
                case '}':
                    return CLOSE_BRACE;
                case '(':
                    return OPEN_PARENS;
                case ')':
                    return CLOSE_PARENS;
                case '[':
                    return OPEN_BRACKET;
                case ']':
                    return CLOSE_BRACKET;
                case ',':
                    return COMMA;
                case '+':
                    switch (nextChar)
                    {
                        case '+':
                            code = OP_INC;
                            break;
                        case '=':
                            code = OP_ADD_ASSIGN;
                            break;
                        default:
                            return PLUS;
                    }
                    Read();
                    return code;
                case '-':
                    switch (nextChar)
                    {
                        case '>':
                            code = OP_PTR;
                            break;
                        case '-':
                            code = OP_DEC;
                            break;
                        case '=':
                            code = OP_SUB_ASSIGN;
                            break;
                        default:
                            return MINUS;
                    }
                    Read();
                    return code;
                case '*':
                    if (nextChar == '=')
                        code = OP_MULT_ASSIGN;
                    else
                        return STAR;
                    Read();
                    return code;
                case '%':
                    if (nextChar == '=')
                        code = OP_MOD_ASSIGN;
                    else
                        return PERCENT;
                    Read();
                    return code;
                case '<':
                    switch (nextChar)
                    {
                        case '=':
                            code = OP_LE;
                            break;
                        case '<':
                            Read();
                            Peek(out nextChar);
                            if (nextChar == '=')
                                code = OP_SHIFT_LEFT_ASSIGN;
                            else
                                return OP_SHIFT_LEFT;
                            break;
                        default:
                            return OP_LT;
                    }
                    Read();
                    return code;
                case '>':
                    switch (nextChar)
                    {
                        case '=':
                            code = OP_GE;
                            break;
                        case '>':
                            Read();
                            Peek(out nextChar);
                            if (nextChar == '=')
                                code =  OP_SHIFT_RIGHT_ASSIGN;
                            else
                                return OP_SHIFT_RIGHT;
                            break;
                        default:
                            return OP_GT;
                    }
                    Read();
                    return code;
                case '&':
                    switch (nextChar)
                    {
                        case '=':
                            code = OP_AND_ASSIGN;
                            break;
                        case '&':
                            code = OP_AND;
                            break;
                        default:
                            return BITWISE_AND;
                    }
                    Read();
                    return code;
                case '|':
                    switch (nextChar)
                    {
                        case '=':
                            code = OP_OR_ASSIGN;
                            break;
                        case '|':
                            code = OP_OR;
                            break;
                        default:
                            return BITWISE_OR;
                    }
                    Read();
                    return code;
                case '^':
                    if (nextChar == '=')
                        code = OP_XOR_ASSIGN;
                    else
                        return CARRET;
                    Read();
                    return code;
                case '!':
                    if (nextChar == '=')
                    {
                        Read();
                        return OP_NOTEQUALS;
                    }
                    return BANG;
                case '=':
                    switch (nextChar)
                    {
                        case '=':
                            code = OP_EQUALS;
                            break;
                        case '>':
                            code = ARROW;
                            break;
                        default:
                            return EQUALS;
                    }
                    Read();
                    return code;
                case ':':
                    if (nextChar == ':')
                        code = DOUBLE_COLON;
                    else
                        return COLON;
                    Read();
                    return code;
                case ';':
                    return SEMICOLON;
                case '.':
                    return DOT;
                case '?':
                    switch (nextChar)
                    {
                        case '?':
                            code = OP_COALESCING;
                            break;
                        case '.':
                            code = INTERR_OPERATOR;
                            break;
                        default:
                            return INTERR;
                    }
                    Read();
                    return code;
                case '~':
                    return TILDE;
            }

            return ERROR;
        }
    }
}
