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
using AbstractCode.Ast.Parser;
using static AbstractCode.Ast.VisualBasic.VisualBasicAstTokenCode;

namespace AbstractCode.Ast.VisualBasic
{

    public class VisualBasicLexer : Lexer
    {
        private const string _extraWordIndicators = "_@";

        public VisualBasicLexer(TextReader reader) 
            : base(reader)
        {
        }

        public override AstToken ReadNextToken()
        {
            while (true)
            {
                if (!MoveToNextToken())
                    return new VisualBasicAstToken(EOF, string.Empty, new TextRange(Location, Location));

                char currentChar;
                Peek(out currentChar);

                VisualBasicAstToken token;

                if (currentChar == '\'')
                {
                    var node = ReadSingleLineComment();
                    token = node as VisualBasicAstToken;
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
                    VisualBasicAstTokenCode keywordCode;
                    if (VisualBasicAstToken.KeywordMapping.TryGetValue(token.Value, out keywordCode))
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

                    token = new VisualBasicAstToken(code, EndReadBuffer().ToString(), new TextRange(start, end));
                }

                return token;
            }
        }

        private static bool IsWordElement(char character)
        {
            return char.IsLetterOrDigit(character) || _extraWordIndicators.Contains(character);
        }

        protected VisualBasicAstToken ReadWordToken()
        {
            TextRange range;
            string token = ReadCharacters(IsWordElement, out range);
            return new VisualBasicAstToken(IDENTIFIER, token, range);
        }

        protected VisualBasicAstToken ReadNumberToken()
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
            
            var astToken = new VisualBasicAstToken(LITERAL, token, range);
            astToken.UserData["InterpretedValue"] = VisualBasicLanguage.Instance.NumberFormatter.EvaluateFormattedNumber(token);
            return astToken;
        }

        private VisualBasicAstToken ReadStringToken()
        {
            var startLocation = Location;
            var startCharacter = Read();

            TextRange range;

            bool escaped = false;

            string stringToken = startCharacter + ReadCharacters(x =>
            {
                return x == startCharacter;
                // TOOD: c suffix and double quotes inside string.
            }, out range);

            char character;
            if (Peek(out character) && character == startCharacter)
                stringToken += Read();
            var endLocation = Location;

            var astToken = new VisualBasicAstToken(LITERAL, stringToken, new TextRange(startLocation, endLocation));
            astToken.UserData["InterpretedValue"] = VisualBasicLanguage.Instance.StringFormatter.EvaluateFormattedString(stringToken);
            return astToken;
        }
        
        private AstNode ReadSingleLineComment()
        {
            // get contents
            TextRange range;
            var contents = ReadCharacters(x => !"\r\n".Contains(x), out range);

            var commentType = contents.StartsWith("'''") ? CommentType.Documentation : CommentType.SingleLine;
            if (commentType == CommentType.Documentation)
                contents = contents.Remove(0, 2);
            
            return new Comment(contents, commentType);
        }
        
        private VisualBasicAstTokenCode TokenizeCurrentSymbol()
        {
            char currentChar = Read();

            char nextChar;
            Peek(out nextChar);
            VisualBasicAstTokenCode code;
            
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
                case ',':
                    return COMMA;
                case '+':
                    if (nextChar == '=')
                        code = OP_ADD_ASSIGN;
                    else
                        return PLUS;
                    Read();
                    return code;
                case '-':
                    if (nextChar == '=')
                        code = OP_SUB_ASSIGN;
                    else
                        return MINUS;
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
                    return PERCENT;
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
                        case '>':
                            code = OP_NOTEQUALS;
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
                case '=':
                    return EQUALS;
                case ':':
                    return COLON;
                case '.':
                    return DOT;
            }

            return ERROR;
        }
    }
}
