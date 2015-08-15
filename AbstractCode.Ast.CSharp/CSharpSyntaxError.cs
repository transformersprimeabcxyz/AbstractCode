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
using AbstractCode.Ast;

namespace AbstractCode.Ast.CSharp
{
    public class CSharpSyntaxError : SyntaxError
    {
        public CSharpSyntaxError(TextLocation location, CSharpSyntaxErrorCode errorCode)
        {
            if (errorCode == CSharpSyntaxErrorCode.ERRORS ||
                errorCode == CSharpSyntaxErrorCode.WARNINGS ||
                errorCode == CSharpSyntaxErrorCode.MESSAGES)
                throw new ArgumentException("errorCode");

            Location = location;
            CSharpErrorCode = errorCode;
            ErrorCode = (int)errorCode;

            if (ErrorCode > (int)CSharpSyntaxErrorCode.MESSAGES)
                Severity = MessageSeverity.Message;
            else if (ErrorCode > (int)CSharpSyntaxErrorCode.WARNINGS)
                Severity = MessageSeverity.Warning;
            else
                Severity = MessageSeverity.Error;

            Message = CSharpErrorCode.ToString();
        }

        public CSharpSyntaxErrorCode CSharpErrorCode
        {
            get;
            private set;
        }
    }

    public interface ICSharpSyntaxErrorMessageProvider
    {
        string GetErrorMessage(CSharpSyntaxErrorCode code);
    }

    public enum CSharpSyntaxErrorCode
    {
        ERRORS,

        Error_SyntaxError,

        // "expected X" errors.
        Error_ExpressionExpected,
        Error_IdentifierExpected,
        Error_StatementExpected,
        Error_EndOfStatementExpected,
        Error_SelfReferenceExpected,
        Error_TypeIdentifierExpected,
        Error_YieldReturnOrBreakExpected,
        Error_LiteralExpected,

        // "expected symbol" errors
        Error_OpeningParensOrBracketExpected,
        Error_ArraySpecifierExpected,
        Error_OpeningParensExpected,
        Error_ClosingParensExpected,
        Error_OpeningBraceExpected,
        Error_ClosingBraceExpected,
        Error_OpeningBracketExpected,
        Error_ClosingBracketExpected,
        Error_SemicolonExpected,
        Error_ColonExpected,
        Error_CommaExpected,
        Error_DotExpected,
        Error_StringIdentifierExpected,
        Error_LeftChevronExpected,
        Error_RightChevronExpected,

        // "expected <keyword>" errors.
        Error_KeywordInExpected,
        
        // Property / Event accessor errors.
        Error_AccessorExpected,
        Error_DuplicateAccessor,
        Error_InvalidAccessor,

        // Stray elements.
        Error_StrayMemberSelector,
        Error_StrayArrayBrackets,
        Error_StrayOperator,
        Error_StrayElseStatement,
        Error_StrayCatchClause,
        Error_StrayFinallyClause,
        Error_StrayLinqClause,

        // Misc
        Error_TooManyExpressions,
        Error_TooFewArguments,
        Error_TooManyArguments,

        WARNINGS,

        // TODO

        MESSAGES,

        // TODO
    }
}
