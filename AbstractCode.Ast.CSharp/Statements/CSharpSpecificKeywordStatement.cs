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
using AbstractCode.Ast.Members;
using AbstractCode.Ast.Statements;
using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;
using TypeReference = AbstractCode.Ast.Types.TypeReference;

namespace AbstractCode.Ast.CSharp.Statements
{
    public abstract class CSharpSpecificKeywordStatement : Statement, ICSharpVisitable
    {
        public AstToken Keyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public AstToken Semicolon
        {
            get { return GetChildByTitle(AstNodeTitles.Semicolon); }
            set { SetChildByTitle(AstNodeTitles.Semicolon, value); }
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            this.AcceptCSharpVisitor(visitor);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return this.AcceptCSharpVisitor(visitor);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return this.AcceptCSharpVisitor(visitor, data);
        }

        public abstract void AcceptVisitor(ICSharpAstVisitor visitor);

        public abstract TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor);

        public abstract TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data);

    }

    public abstract class CSharpSpecificKeywordBodyStatement : Statement, ICSharpVisitable
    {
        public AstToken Keyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public BlockStatement Body
        {
            get { return GetChildByTitle(AstNodeTitles.Body); }
            set { SetChildByTitle(AstNodeTitles.Body, value); }
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            this.AcceptCSharpVisitor(visitor);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return this.AcceptCSharpVisitor(visitor);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return this.AcceptCSharpVisitor(visitor, data);
        }

        public abstract void AcceptVisitor(ICSharpAstVisitor visitor);

        public abstract TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor);

        public abstract TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data);
    }

    public class BreakStatement : CSharpSpecificKeywordStatement
    {
        public override void AcceptVisitor(ICSharpAstVisitor visitor)
        {
            visitor.VisitBreakStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor)
        {
            return visitor.VisitBreakStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitBreakStatement(this, data);
        }

        public override bool Match(AstNode other)
        {
            return other is BreakStatement;
        }
    }

    public class ContinueStatement : CSharpSpecificKeywordStatement
    {
        public override void AcceptVisitor(ICSharpAstVisitor visitor)
        {
            visitor.VisitContinueStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor)
        {
            return visitor.VisitContinueStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitContinueStatement(this, data);
        }

        public override bool Match(AstNode other)
        {
            return other is ContinueStatement;
        }
    }

    public class YieldBreakStatement : CSharpSpecificKeywordStatement
    {
        public static readonly AstNodeTitle<AstToken> BreakKeywordTitle = new AstNodeTitle<AstToken>("BreakKeyword");

        public AstToken BreakKeyword
        {
            get { return GetChildByTitle(BreakKeywordTitle); }
            set { SetChildByTitle(BreakKeywordTitle, value); }
        }

        public override void AcceptVisitor(ICSharpAstVisitor visitor)
        {
            visitor.VisitYieldBreakStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor)
        {
            return visitor.VisitYieldBreakStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitYieldBreakStatement(this, data);
        }

        public override bool Match(AstNode other)
        {
            return other is YieldBreakStatement;
        }
    }

    public class CheckedStatement : CSharpSpecificKeywordBodyStatement
    {
        public override void AcceptVisitor(ICSharpAstVisitor visitor)
        {
            visitor.VisitCheckedStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor)
        {
            return visitor.VisitCheckedStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitCheckedStatement(this, data);
        }

        public override bool Match(AstNode other)
        {
            var statement = other as CheckedStatement;
            return statement != null
                   && Body.MatchOrNull(statement.Body);
        }
    }

    public class UncheckedStatement : CSharpSpecificKeywordBodyStatement
    {
        public override void AcceptVisitor(ICSharpAstVisitor visitor)
        {
            visitor.VisitUncheckedStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor)
        {
            return visitor.VisitUncheckedStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitUncheckedStatement(this, data);
        }

        public override bool Match(AstNode other)
        {
            var statement = other as UncheckedStatement;
            return statement != null
                   && Body.MatchOrNull(statement.Body);
        }
    }

    public class UnsafeStatement : CSharpSpecificKeywordBodyStatement
    {
        public override void AcceptVisitor(ICSharpAstVisitor visitor)
        {
            visitor.VisistUnsafeStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor)
        {
            return visitor.VisistUnsafeStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisistUnsafeStatement(this, data);
        }

        public override bool Match(AstNode other)
        {
            var statement = other as UnsafeStatement;
            return statement != null
                   && Body.MatchOrNull(statement.Body);
        }
    }

    public class FixedStatement : CSharpSpecificKeywordStatement, IScopeProvider
    {
        public static readonly AstNodeTitle<VariableDeclarationStatement> VariableDeclarationTitle =
            new AstNodeTitle<VariableDeclarationStatement>("VariableDeclaration");

        private sealed class FixedStatementScope : AbstractScope<FixedStatement>
        {
            public FixedStatementScope(FixedStatement container)
                : base(container)
            {
            }

            public override ResolveResult ResolveIdentifier(string identifier)
            {
                var candidate = (from variable in Container.VariableDeclaration.GetVariableDefinitions()
                    where variable.Name == identifier
                    select variable).FirstOrDefault();

                return candidate != null 
                    ? new LocalResolveResult(candidate) 
                    : base.ResolveIdentifier(identifier);
            }
        }

        private IScope _scope;

        public FixedStatement()
        {
        }

        public FixedStatement(TypeReference type, VariableDeclarator declarator)
        {
            VariableDeclaration = new VariableDeclarationStatement(type, declarator);
        }

        public AstToken LeftParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.LeftParenthese); }
            set { SetChildByTitle(AstNodeTitles.LeftParenthese, value); }
        }

        public VariableDeclarationStatement VariableDeclaration
        {
            get { return GetChildByTitle(VariableDeclarationTitle); }
            set { SetChildByTitle(VariableDeclarationTitle, value); }
        }

        public Statement Body
        {
            get { return GetChildByTitle(AstNodeTitles.BodyStatement); }
            set { SetChildByTitle(AstNodeTitles.BodyStatement, value); }
        }

        public AstToken RightParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.RightParenthese); }
            set { SetChildByTitle(AstNodeTitles.RightParenthese, value); }
        }

        public override bool Match(AstNode other)
        {
            var statement = other as FixedStatement;
            return statement != null
                   && VariableDeclaration.MatchOrNull(statement.VariableDeclaration)
                   && Body.MatchOrNull(statement.Body);
        }


        public override void AcceptVisitor(ICSharpAstVisitor visitor)
        {
            visitor.VisitFixedStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor)
        {
            return visitor.VisitFixedStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitFixedStatement(this, data);
        }

        public IScope GetScope()
        {
            return _scope ?? (_scope = new FixedStatementScope(this));
        }
    }
}
