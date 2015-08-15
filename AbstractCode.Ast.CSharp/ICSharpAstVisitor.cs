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
using System.Threading.Tasks;
using AbstractCode.Ast;
using AbstractCode.Ast.CSharp.Expressions;
using AbstractCode.Ast.CSharp.Statements;

namespace AbstractCode.Ast.CSharp
{
    public interface ICSharpAstVisitor : IAstVisitor
    {
        void VisitUndocumentedCSharpKeywordExpression(UndocumentedCSharpKeywordExpression expression);
        void VisitCheckedExpression(CheckedExpression expression);
        void VisitDefaultExpression(DefaultExpression expression);
        void VisitUncheckedExpression(UncheckedExpression expression);
        void VisitSizeOfExpression(SizeOfExpression expression);
        void VisitStackAllocExpression(StackAllocExpression expression);
        void VisitBreakStatement(Statements.BreakStatement statement);
        void VisitCheckedStatement(Statements.CheckedStatement statement);
        void VisitContinueStatement(Statements.ContinueStatement statement);
        void VisitFixedStatement(FixedStatement statement);
        void VisitForeachLoopStatement(Statements.ForeachLoopStatement statement);
        void VisitForLoopStatement(Statements.ForLoopStatement statement);
        void VisitUncheckedStatement(Statements.UncheckedStatement statement);
        void VisistUnsafeStatement(Statements.UnsafeStatement statement);
        void VisitYieldBreakStatement(Statements.YieldBreakStatement statement);
        void VisitConstructorInitializer(Members.ConstructorInitializer initializer);
    }

    public interface ICSharpAstVisitor<TResult> : IAstVisitor<TResult>
    {
        TResult VisitUndocumentedCSharpKeywordExpression(UndocumentedCSharpKeywordExpression expression);
        TResult VisitCheckedExpression(CheckedExpression expression);
        TResult VisitDefaultExpression(DefaultExpression expression);
        TResult VisitUncheckedExpression(UncheckedExpression expression);
        TResult VisitSizeOfExpression(SizeOfExpression expression);
        TResult VisitStackAllocExpression(StackAllocExpression expression);
        TResult VisitBreakStatement(Statements.BreakStatement statement);
        TResult VisitCheckedStatement(Statements.CheckedStatement statement);
        TResult VisitContinueStatement(Statements.ContinueStatement statement);
        TResult VisitFixedStatement(FixedStatement statement);
        TResult VisitForeachLoopStatement(Statements.ForeachLoopStatement statement);
        TResult VisitForLoopStatement(Statements.ForLoopStatement statement);
        TResult VisitUncheckedStatement(Statements.UncheckedStatement statement);
        TResult VisistUnsafeStatement(Statements.UnsafeStatement statement);
        TResult VisitYieldBreakStatement(Statements.YieldBreakStatement statement);
        TResult VisitConstructorInitializer(Members.ConstructorInitializer initializer);
    }

    public interface ICSharpAstVisitor<TData, TResult> : IAstVisitor<TData, TResult>
    {
        TResult VisitUndocumentedCSharpKeywordExpression(UndocumentedCSharpKeywordExpression expression, TData data);
        TResult VisitCheckedExpression(CheckedExpression expression, TData data);
        TResult VisitDefaultExpression(DefaultExpression expression, TData data);
        TResult VisitUncheckedExpression(UncheckedExpression expression, TData data);
        TResult VisitSizeOfExpression(SizeOfExpression expression, TData data);
        TResult VisitStackAllocExpression(StackAllocExpression expression, TData data);
        TResult VisitBreakStatement(Statements.BreakStatement statement, TData data);
        TResult VisitCheckedStatement(Statements.CheckedStatement statement, TData data);
        TResult VisitContinueStatement(Statements.ContinueStatement statement, TData data);
        TResult VisitFixedStatement(FixedStatement statement, TData data);
        TResult VisitForeachLoopStatement(Statements.ForeachLoopStatement foreachLoopStatement, object data);
        TResult VisitForLoopStatement(Statements.ForLoopStatement statement, TData data);
        TResult VisitUncheckedStatement(Statements.UncheckedStatement statement, TData data);
        TResult VisistUnsafeStatement(Statements.UnsafeStatement statement, TData data);
        TResult VisitYieldBreakStatement(Statements.YieldBreakStatement statement, TData data);
        TResult VisitConstructorInitializer(Members.ConstructorInitializer initializer, TData data);
    }
}
