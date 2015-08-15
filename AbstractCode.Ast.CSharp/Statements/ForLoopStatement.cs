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

using System.Linq;
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Statements;
using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;

namespace AbstractCode.Ast.CSharp.Statements
{
    public class ForLoopStatement : ConditionalLoopStatement, ICSharpVisitable, IScopeProvider
    {
        public class ForLoopStatementScope : AbstractScope<ForLoopStatement>
        {
            public ForLoopStatementScope(ForLoopStatement container)
                : base(container)
            {
            }

            public override ResolveResult ResolveIdentifier(string identifier)
            {
                var variable =
                    (from declarationStatement in Container.Initializers.OfType<VariableDeclarationStatement>()
                        from declaration in declarationStatement.GetVariableDefinitions()
                        where declaration.Name == identifier
                        select declaration).FirstOrDefault();
                
                return variable!=null
                    ? new LocalResolveResult(variable)
                    : base.ResolveIdentifier(identifier);
            }
        }

        public static readonly AstNodeTitle<Statement> InitializerTitle = new AstNodeTitle<Statement>("Initializer");
        public static readonly AstNodeTitle<Statement> IteratorTitle = new AstNodeTitle<Statement>("Iterator");
        private ForLoopStatementScope _scope;

        public ForLoopStatement()
        {
            Initializers = new AstNodeCollection<Statement>(this, InitializerTitle);
            Iterators = new AstNodeCollection<Statement>(this, IteratorTitle);
        }

        public ForLoopStatement(Statement initializer, Expression condition, Statement iterator)
            : this()
        {
            if (initializer != null)
                Initializers.Add(initializer);

            Condition = condition;

            if (iterator != null)
                Iterators.Add(iterator);
        }

        public AstToken ForKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public AstToken LeftParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.LeftParenthese); }
            set { SetChildByTitle(AstNodeTitles.LeftParenthese, value); }
        }

        public AstNodeCollection<Statement> Initializers
        {
            get;
        }

        public AstNodeCollection<Statement> Iterators
        {
            get;
        }

        public AstToken RightParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.RightParenthese); }
            set { SetChildByTitle(AstNodeTitles.RightParenthese, value); }
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        public override bool Match(AstNode other)
        {
            var statement = other as ForLoopStatement;
            return statement != null
                   && Initializers.Match(statement.Initializers)
                   && Condition.MatchOrNull(statement.Condition)
                   && Iterators.Match(statement.Iterators)
                   && Body.MatchOrNull(statement.Body);
        }

        public IScope GetScope()
        {
            return _scope ?? (_scope = new ForLoopStatementScope(this));
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
        
        public void AcceptVisitor(ICSharpAstVisitor visitor)
        {
            visitor.VisitForLoopStatement(this);
        }

        public TResult AcceptVisitor<TResult>(ICSharpAstVisitor<TResult> visitor)
        {
            return visitor.VisitForLoopStatement(this);
        }

        public TResult AcceptVisitor<TData, TResult>(ICSharpAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitForLoopStatement(this, data);
        }
    }
}
