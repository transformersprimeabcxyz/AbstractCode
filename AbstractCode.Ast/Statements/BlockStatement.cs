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
using AbstractCode.Symbols;

namespace AbstractCode.Ast.Statements
{
    public class BlockStatement : Statement, ICodeBlockAstNode
    {
        private SimpleCodeBlockScope _scope;

        public BlockStatement()
        {
            Statements = new AstNodeCollection<Statement>(this, AstNodeTitles.Statement);
        }

        public AstNode StartScope
        {
            get { return GetChildByTitle(AstNodeTitles.StartScope); }
            set { SetChildByTitle(AstNodeTitles.StartScope, value); }
        }

        public AstNodeCollection<Statement> Statements
        {
            get;
            private set;
        }

        public AstNode EndScope
        {
            get { return GetChildByTitle(AstNodeTitles.EndScope); }
            set { SetChildByTitle(AstNodeTitles.EndScope, value); }
        }

        public VariableDeclarationStatement[] GetDeclaredVariables()
        {
            return GetChildren<VariableDeclarationStatement>().ToArray();
        }

        public virtual void Append(Statement statement)
        {
            AddChild(AstNodeTitles.Statement, statement);
        }

        public void Append(Expression expression)
        {
            Append(new ExpressionStatement(expression));
        }

        public override bool Match(AstNode other)
        {
            var statement = other as BlockStatement;
            return statement != null
                   && Statements.Match(statement.Statements);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitBlockStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitBlockStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitBlockStatement(this, data);
        }
        
        public IScope GetScope()
        {
            return _scope ?? (_scope = new SimpleCodeBlockScope(this));
        }
    }
}
