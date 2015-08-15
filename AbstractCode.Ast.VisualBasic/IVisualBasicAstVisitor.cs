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

namespace AbstractCode.Ast.VisualBasic
{
    public interface IVisualBasicAstVisitor : IAstVisitor
    {
        void VisitEndScopeBlockClause(Statements.EndScopeBlockClause clause);
        void VisitEndStatement(Statements.EndStatement statement);
        void VisitContinueStatement(Statements.ContinueStatement statement);
        void VisitExitStatement(Statements.ExitStatement statement);
        void VisitHandlesClause(Members.HandlesClause clause);
        void VisitImplementsClause(Members.ImplementsClause clause);
    }

    public interface IVisualBasicAstVisitor<TResult> : IAstVisitor
    {
        TResult VisitEndScopeBlockClause(Statements.EndScopeBlockClause clause);
        TResult VisitEndStatement(Statements.EndStatement statement);
        TResult VisitContinueStatement(Statements.ContinueStatement statement);
        TResult VisitExitStatement(Statements.ExitStatement statement);
        TResult VisitHandlesClause(Members.HandlesClause clause);
        TResult VisitImplementsClause(Members.ImplementsClause clause);
    }

    public interface IVisualBasicAstVisitor<TData, TResult> : IAstVisitor
    {
        TResult VisitEndScopeBlockClause(Statements.EndScopeBlockClause clause, TData data);
        TResult VisitEndStatement(Statements.EndStatement statement, TData data);
        TResult VisitContinueStatement(Statements.ContinueStatement statement, TData data);
        TResult VisitExitStatement(Statements.ExitStatement statement, TData data);
        TResult VisitHandlesClause(Members.HandlesClause clause, TData data);
        TResult VisitImplementsClause(Members.ImplementsClause clause, TData data);
    }
}