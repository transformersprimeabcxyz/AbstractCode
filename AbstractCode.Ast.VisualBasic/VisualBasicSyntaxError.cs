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

namespace AbstractCode.Ast.VisualBasic
{
    public class VisualBasicSyntaxError : SyntaxError
    {
        public VisualBasicSyntaxError(TextLocation location, VisualBasicSyntaxErrorCode errorCode)
        {
            if (errorCode == VisualBasicSyntaxErrorCode.ERRORS ||
                errorCode == VisualBasicSyntaxErrorCode.WARNINGS ||
                errorCode == VisualBasicSyntaxErrorCode.MESSAGES)
                throw new ArgumentException("errorCode");

            Location = location;
            VisualBasicErrorCode = errorCode;
            ErrorCode = (int)errorCode;

            if (ErrorCode > (int)VisualBasicSyntaxErrorCode.MESSAGES)
                Severity = MessageSeverity.Message;
            else if (ErrorCode > (int)VisualBasicSyntaxErrorCode.WARNINGS)
                Severity = MessageSeverity.Warning;
            else
                Severity = MessageSeverity.Error;

            Message = VisualBasicErrorCode.ToString();
        }

        public VisualBasicSyntaxErrorCode VisualBasicErrorCode
        {
            get;
        }
    }

    public interface IVisualBasicErrorMessageProvider
    {
    }

    public enum VisualBasicSyntaxErrorCode
    {
        ERRORS,

        Error_SyntaxError,

        // "expected X" errors.
        Error_ExpressionExpected,
        Error_IdentifierExpected,
        Error_TypeIdentifierExpected,

        // "expected symbol" errors
        Error_OpeningParensExpected,
        Error_ClosingParensExpected,
        Error_OpeningBraceExpected,
        Error_ClosingBraceExpected,
        Error_ColonExpected,
        Error_CommaExpected,
        Error_DotExpected,
        Error_InitializerExpected,
        Error_ParameterExpected,
        Error_MemberReferenceExpected,
        Error_ImplementsOrHandlesClauseExpected,
        Error_ExitVariantExpected,
        Error_ContinueVariantExpected,
        Error_VariableExpected,

        // "expected <keyword>" errors.
        Error_KeywordEndExpected,
        Error_KeywordEndPropertyExpected,
        Error_KeywordEndTypeExpected,
        Error_KeywordFunctionOrSubExpected,
        Error_KeywordEndMethodExpected,
        Error_KeywordEndAccessorExpected,
        Error_KeywordEndNamespaceExpected,
        Error_KeywordEndWhileExpected,
        Error_KeywordWhileOrUntilExpected,
        Error_KeywordLoopExpected,
        Error_KeywordEndTryExpected,
        Error_KeywordAsExpected,
        Error_KeywordThenExpected,
        Error_KeywordEndIfExpected,
        Error_KeywordEndUsingExpected,

        // Property / Event accessor errors.
        Error_AccessorExpected,
        Error_DuplicateAccessor,
        Error_InvalidAccessor,

        // Stray elements.
        Error_StrayMemberSelector,
        Error_StrayOperator,
        Error_StrayElseStatement,
        Error_StrayElseIfStatement,
        Error_StrayCatchClause,
        Error_StrayFinallyClause,
        Error_StrayUntilClause,
        Error_StrayLinqClause,

        // Misc
        Error_TooManyExpressions,
        Error_TooFewArguments,
        Error_TooManyArguments,
        Error_SubReturnType,

        Error_NotSupported,

        WARNINGS,

        // TODO

        MESSAGES,

        // TODO
    }
}