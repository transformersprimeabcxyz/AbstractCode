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

using System.Collections.Generic;
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Members;
using AbstractCode.Symbols;
using TypeReference = AbstractCode.Ast.Types.TypeReference;

namespace AbstractCode.Ast.Statements
{
    public class VariableDeclarationStatement : Statement, IVariableDeclaratorProvider
    {
        private sealed class VariableDefinitionWrapper : VariableDefinition
        {
            private readonly VariableDeclarationStatement _declaration;
            private readonly VariableDeclarator _declarator;
            private TypeDefinition _variableType;

            public VariableDefinitionWrapper(VariableDeclarationStatement declaration, VariableDeclarator declarator)
            {
                _declaration = declaration;
                _declarator = declarator;
            }

            public override string Name
            {
                get { return _declarator.Identifier.Name; }
            }

            public override IScope GetDeclaringScope()
            {
                return _declaration.GetDeclaringScope();
            }

            public override TypeDefinition VariableType
            {
                get
                {
                    if (_variableType != null)
                        return _variableType;

                    var symbolsReference = _declaration.VariableType.GetSymbolsReference();
                    var result = symbolsReference.Resolve(GetDeclaringScope());
                    return _variableType = result.ScopeProvider as TypeDefinition;
                }
            }
        }

        public VariableDeclarationStatement()
        {
            Declarators = new AstNodeCollection<VariableDeclarator>(this, AstNodeTitles.Declarator);
        }

        public VariableDeclarationStatement(TypeReference variableType, VariableDeclarator declarator)
            : this()
        {
            VariableType = variableType;
            Declarators.Add(declarator);
        }

        public VariableDeclarationStatement(TypeReference variableType, string name)
            : this(variableType, new VariableDeclarator(name))
        {
        }

        public VariableDeclarationStatement(TypeReference variableType, string name, Expression value)
            : this(variableType, new VariableDeclarator(name, value))
        {
        }

        public VariableDeclarationStatement(TypeReference variableType, params VariableDeclarator[] declarators)
            : this()
        {
            VariableType = variableType;
            Declarators.AddRange(declarators);
        }

        public TypeReference VariableType
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }

        public AstNodeCollection<VariableDeclarator> Declarators
        {
            get;
        }

        public override bool Match(AstNode other)
        {
            var statement = other as VariableDeclarationStatement;
            return statement != null
                   && VariableType.MatchOrNull(statement.VariableType)
                   && Declarators.Match(statement.Declarators);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitVariableDeclarationStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitVariableDeclarationStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitVariableDeclarationStatement(this, data);
        }

        public virtual IEnumerable<VariableDefinition> GetVariableDefinitions()
        {
            foreach (var initializer in Declarators)
                yield return new VariableDefinitionWrapper(this, initializer);
        }

    }
}
