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
using static AbstractCode.Ast.VisualBasic.VisualBasicAstTokenCode;

namespace AbstractCode.Ast.VisualBasic
{
    public class VisualBasicAstToken : AstToken
    {
        public static readonly Dictionary<string, VisualBasicAstTokenCode> KeywordMapping =
            new Dictionary<string, VisualBasicAstTokenCode>(StringComparer.OrdinalIgnoreCase)
            {
                ["and"] = BITWISE_AND,
                ["andalso"] = OP_ANDALSO,
                ["false"] = FALSE,
                ["if"] = IF,
                ["iff"] = IFF,
                ["me"] = ME,
                ["mod"] = MOD,
                ["mybase"] = MYBASE,
                ["not"] = NOT,
                ["or"] = BITWISE_OR,
                ["orelse"] = OP_ORELSE,
                ["true"] = TRUE,
            };

        public VisualBasicAstToken(VisualBasicAstTokenCode code, string value, TextRange range)
            : base(value, range)
        {
            Code = code;
        }

        public VisualBasicAstTokenCode Code
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
            var token = other as VisualBasicAstToken;
            return token != null && Code == token.Code;
        }
    }
}